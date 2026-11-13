namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Spellbook;
using Robbiblubber.Spellbook.Controls;



/// <summary>This class contains the main entry point of the application.</summary>
/// <remarks>Only one process runs per user: further starts pass their files to the running process and end. Every document has its
///          own window; the application ends when the last window is closed.</remarks>
[SupportedOSPlatform("windows")]
internal static class _Program
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Time in milliseconds the splash window stays open (behind the windows) after the first document window has been
	///          shown.</summary>
	private const int _SPLASH_TIME = 3000;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Splash window, NULL if it is closed.</summary>
	private static _SplashWindow? _Splash = null;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static properties                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the application context.</summary>
	internal static MultiFormApplicationContext _Context
	{
		get; private set;
	} = null!;


	/// <summary>Gets the application icon.</summary>
	internal static Icon? _Icon
	{
		get; private set;
	}


	/// <summary>Gets the list of recently used files.</summary>
	/// <remarks>The windows show the list in their menus with their own list components, which read the same file.</remarks>
	internal static RecentFileList _RecentFiles
	{
		get; private set;
	} = null!;


	/// <summary>Gets the document windows.</summary>
	internal static IEnumerable<_DocumentWindow> _DocumentWindows
	{
		get { return _Context.Forms.OfType<_DocumentWindow>(); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// application entry point                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>The main entry point of the application.</summary>
	/// <param name="args">Command line arguments: files to open.</param>
	[STAThread]
	private static void Main(string[] args)
	{
		PathOp.Initialize("robbiblubber.org/led");

		using SingleInstance instance = new("robbiblubber.org/led");
		if(!instance.IsFirstInstance)
		{
			if(MultiFormApplicationContext.Forward(instance, args)) { return; }
		}

		ApplicationConfiguration.Initialize();
		_Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

		Locale.Sources.Set(new DirectoryLocalizationSource(Path.Combine(PathOp.ApplicationDirectory, "locale")));
		SpellbookLocalization.Enable();
		_DarkImages._Enable();
		_Settings._Load();
		_RecentFiles = new RecentFileList() { ConfigFile = _Settings._RecentFileName };

		_Splash = new _SplashWindow();
		_Splash.Show();
		_Splash.Update();

		_Context = new MultiFormApplicationContext(instance);
		_Context.ArgumentsReceived += _Context_ArgumentsReceived;

		_Open(args, null);
		if(_Context.Forms.Count == 0) { _NewTable(null); }
		_LowerSplash();

		System.Windows.Forms.Timer splashTimer = new() { Interval = _SPLASH_TIME };
		splashTimer.Tick += (s, e) => { splashTimer.Dispose(); _CloseSplash(); };
		splashTimer.Start();

		Application.Run(_Context);
		_Context.Dispose();
		_RecentFiles.Dispose();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Opens a new window with an empty table.</summary>
	/// <param name="owner">Window the command was given in, NULL if none.</param>
	internal static void _NewTable(Form? owner)
	{
		_Context.ShowForm(new _LanguageTableWindow(new _TableDocument(), owner));
	}


	/// <summary>Opens a new window with an empty locale definition.</summary>
	/// <param name="owner">Window the command was given in, NULL if none.</param>
	internal static void _NewDefinition(Form? owner)
	{
		_Context.ShowForm(new _LocaleDefinitionWindow(new _LocaleDefinitionDocument(), owner));
	}


	/// <summary>Opens files.</summary>
	/// <param name="files">Full file names.</param>
	/// <param name="owner">Window the command was given in, NULL if none. An untouched new document in this window is replaced by the
	///                     first file of the same type.</param>
	/// <remarks>Language tables and locale definitions are recognized by their content. A file that is already open is shown in its
	///          window and reloaded; if it has unsaved changes there, the user is asked first. The user is warned about entries that
	///          occur more than once in a table file.</remarks>
	internal static void _Open(IEnumerable<string> files, _DocumentWindow? owner)
	{
		List<string> failed = new();

		foreach(string i in files)
		{
			string path;
			try
			{
				path = Path.GetFullPath(i);
			}
			catch(Exception) { failed.Add(i); continue; }

			_EditorDocument doc;
			try
			{
				doc = _EditorDocument.Open(path);
			}
			catch(Exception ex)
			{
				failed.Add(path + "\n    " + ((ex is FormatException) ? "led:open.format".LookUp("The file is neither a language table nor a locale definition.") : ex.Message));
				continue;
			}

			_RecentFiles.Add(path);
			_DocumentWindow? open = _DocumentWindows.FirstOrDefault(m => string.Equals(m._Document.FileName, path, StringComparison.OrdinalIgnoreCase));
			if((open != null) && open._CanShow(doc))
			{
				MultiFormApplicationContext.ActivateForm(open);
				if((!open._Commit()) || open._Document.IsDirty)
				{
					if(ThemedMessageBox.Show(open, "led:open.reload".LookUp("$NAME is already open and has unsaved changes.\n\nReload the file and discard the changes?", ("NAME", Path.GetFileName(path))),
											 "led:open.caption".LookUp("Open"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) { continue; }
				}
				open._SetDocument(doc);
				_ReportDuplicates(doc, open);
				continue;
			}

			if((owner != null) && (!owner.IsDisposed) && owner._CanShow(doc) && owner._Commit() && owner._Document.IsUntouched)
			{
				owner._SetDocument(doc);
				MultiFormApplicationContext.ActivateForm(owner);
				_ReportDuplicates(doc, owner);
				owner = null;
			}
			else
			{
				_DocumentWindow window = _CreateWindow(doc, owner);
				_Context.ShowForm(window);
				_ReportDuplicates(doc, window);
			}
		}

		if(failed.Count > 0)
		{
			_CloseSplash();
			ThemedMessageBox.Show("led:open.failed".LookUp("The following files could not be opened:\n\n$FILES", ("FILES", string.Join("\n", failed))),
								  "led:open.caption".LookUp("Open"), MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates the window for a document.</summary>
	/// <param name="document">Document.</param>
	/// <param name="opener">Window the new window is opened from, NULL if none.</param>
	/// <returns>Returns the window.</returns>
	private static _DocumentWindow _CreateWindow(_EditorDocument document, Form? opener)
	{
		return document switch
		{
			_LocaleDefinitionDocument d => new _LocaleDefinitionWindow(d, opener),
			_ => new _LanguageTableWindow((_TableDocument) document, opener)
		};
	}


	/// <summary>Puts the splash window behind the document windows, so that it does not cover them until it closes.</summary>
	private static void _LowerSplash()
	{
		if(_Splash is null) return;

		_Splash.TopMost = false;
		if(_Context.Forms.LastOrDefault() is Form last) { MultiFormApplicationContext.ActivateForm(last); }
	}


	/// <summary>Closes the splash window if it is open.</summary>
	/// <remarks>The window is closed before messages are shown, so that it does not cover them.</remarks>
	private static void _CloseSplash()
	{
		_Splash?.Close();
		_Splash?.Dispose();
		_Splash = null;
	}


	/// <summary>Warns the user if a table file contains entries more than once.</summary>
	/// <param name="document">Opened document.</param>
	/// <param name="window">Window that shows the document.</param>
	private static void _ReportDuplicates(_EditorDocument document, _DocumentWindow window)
	{
		if((document is not _TableDocument table) || (table.FileDuplicates.Count == 0)) return;

		_CloseSplash();

		string symbols = string.Join("\n", table.FileDuplicates.Take(10)) + ((table.FileDuplicates.Count > 10) ? "\n…" : string.Empty);
		ThemedMessageBox.Show(window, "led:open.duplicates".LookUp("$NAME contains entries more than once. Only the last value of each is shown and saved:\n\n$SYMBOLS",
																	("NAME", Path.GetFileName(table.FileName)), ("SYMBOLS", symbols)),
							  "led:open.caption".LookUp("Open"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
	}


	/// <summary>Returns the full path of a file passed by a further start.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="path">Path.</param>
	/// <returns>Returns the full path, or the path itself if it is invalid.</returns>
	private static string _GetFullPath(SingleInstanceEventArgs e, string path)
	{
		try
		{
			return e.GetFullPath(path);
		}
		catch(Exception) { return path; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Files passed by a further start.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private static void _Context_ArgumentsReceived(object? sender, SingleInstanceEventArgs e)
	{
		if(e.Arguments.Length == 0)
		{
			_NewTable(null);
		}
		else { _Open(e.Arguments.Select(m => _GetFullPath(e, m)), null); }
	}
}
