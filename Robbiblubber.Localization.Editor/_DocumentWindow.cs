namespace Robbiblubber.Localization.Editor;

using System.ComponentModel;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class is the base class of the document windows.</summary>
/// <remarks><para>The class provides what all windows share: the menus, the recent files, the status bar, the title, saving, the
///          question for unsaved changes when the window is closed, the theme, and the position of new windows. Derived windows put their
///          contents into <see cref="_PanelContent"/> and override the members of the section "protected virtual members".</para>
///          <para>The class is not abstract, so that the windows can be edited in the designer.</para></remarks>
[SupportedOSPlatform("windows")]
internal partial class _DocumentWindow: Form
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Offset of a new window from the window it was opened from, in logical pixels.</summary>
	private const int _CASCADE_OFFSET = 26;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Window the window was opened from.</summary>
	private Form? _Opener;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// protected members                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Determines if the window is being initialized or the fields are being filled; changes are not taken over then.</summary>
	protected bool _Binding = true;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	/// <remarks>This constructor is used by the designer.</remarks>
	public _DocumentWindow(): this(null)
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="opener">Window the new window is opened from, NULL if none.</param>
	/// <remarks>In the designer of a derived window, only the components are created; the settings files are not used.</remarks>
	public _DocumentWindow(Form? opener)
	{
		InitializeComponent();
		if(LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

		Icon = _Program._Icon;
		_Opener = opener;
		_Layout.ConfigFile = _Settings._LayoutFileName;
		_Recent.ConfigFile = _Settings._RecentFileName;

		ThemeManager.ThemeChanged += _ThemeManager_ThemeChanged;
		_UpdateThemeColors();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal properties                                                                                              //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the document.</summary>
	internal _EditorDocument _Document
	{
		get; private set;
	} = null!;


	/// <summary>Gets the short title of the window: the file name, marked with <c>*</c> if there are unsaved changes.</summary>
	internal string _Caption
	{
		get { return _DocumentName + (_Document.IsDirty ? "*" : string.Empty); }
	}


	/// <summary>Gets if the window offers the table tools: text editor, sort and import.</summary>
	/// <remarks>The tool commands are disabled in windows without table tools.</remarks>
	internal virtual bool _HasTableTools
	{
		get { return false; }
	}


	/// <summary>Gets if the document can be searched.</summary>
	/// <remarks>The find commands are disabled in windows that cannot be searched.</remarks>
	internal virtual bool _CanSearch
	{
		get { return false; }
	}


	/// <summary>Gets the name of the help page for the window.</summary>
	/// <remarks>Help > Show Help (F1) opens this page.</remarks>
	internal virtual string _HelpPage
	{
		get { return _Help._START_PAGE; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal methods                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Shows a document in this window.</summary>
	/// <param name="document">Document.</param>
	/// <remarks>Derived windows fill their fields with <see cref="_Binding"/> set and call the base method last.</remarks>
	internal virtual void _SetDocument(_EditorDocument document)
	{
		_Document = document;
		_Binding = false;
		_Update();
	}


	/// <summary>Ends the editing of a field and takes over the values of all fields.</summary>
	/// <returns>Returns TRUE if the values could be taken over, otherwise returns FALSE.</returns>
	internal virtual bool _Commit()
	{
		return true;
	}


	/// <summary>Shows a message in the status bar.</summary>
	/// <param name="text">Message; an empty string removes the message.</param>
	/// <remarks>The message is removed with the next change.</remarks>
	internal void _ShowMessage(string text)
	{
		_LabelMessage.Text = text;
	}


	/// <summary>Returns if a document can be shown in this window.</summary>
	/// <param name="document">Document.</param>
	/// <returns>Returns TRUE if the window shows documents of this type, otherwise returns FALSE.</returns>
	internal virtual bool _CanShow(_EditorDocument document)
	{
		return false;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// protected virtual members                                                                                        //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the filter of the Save As dialog.</summary>
	protected virtual string _SaveFilter
	{
		get { return "led:filter.all".LookUp("All Files") + " (*.*)|*.*"; }
	}


	/// <summary>Returns the filter index of the Save As dialog for a file.</summary>
	/// <param name="fileName">File name, NULL for a new document.</param>
	/// <returns>Returns the filter index, starting at 1.</returns>
	protected virtual int _GetFilterIndex(string? fileName)
	{
		return 1;
	}


	/// <summary>Suggests a file name for a new document.</summary>
	/// <returns>Returns the file name.</returns>
	protected virtual string _SuggestFileName()
	{
		return string.Empty;
	}


	/// <summary>Checks the document before it is saved and asks the user if there are problems.</summary>
	/// <returns>Returns TRUE if the document may be saved, otherwise returns FALSE.</returns>
	protected virtual bool _CheckBeforeSave()
	{
		return true;
	}


	/// <summary>Updates the contents of the window and the status bar after a change.</summary>
	protected virtual void _UpdateContents()
	{}


	/// <summary>Edits the current text in the text editor (Tools > Text Editor).</summary>
	protected virtual void _EditText()
	{}


	/// <summary>Sorts the document (Tools > Sort).</summary>
	protected virtual void _Sort()
	{}


	/// <summary>Imports another document (Tools > Import).</summary>
	protected virtual void _Import()
	{}


	/// <summary>Applies the localized texts that are not set by the locale provider.</summary>
	/// <remarks>Derived windows call the base method last; it updates the window.</remarks>
	protected virtual void _Localize()
	{
		_Recent.EmptyText = "led:recent.empty".LookUp("(empty)");
		_Recent.ClearText = "led:recent.clear".LookUp("&Clear List");
		_Recent.OpenText = "led:recent.open".LookUp("&Open");
		_Recent.PinText = "led:recent.pin".LookUp("&Pin");
		_Recent.UnpinText = "led:recent.unpin".LookUp("&Unpin");
		_Recent.RemoveText = "led:recent.remove".LookUp("&Remove from List");

		_Update();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// protected methods                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Updates the title, the status bar and the contents of the window.</summary>
	protected void _Update()
	{
		if(_Binding) return;

		_UpdateContents();
		_LabelMessage.Text = string.Empty;

		string title = _Caption + " - " + "led:app.name".LookUp("Localization Editor");
		if(Text != title) { Text = title; }
		_LabelFile.Text = (_Document.FileName ?? string.Empty);
	}


	/// <summary>Asks the user whether to save anyway.</summary>
	/// <param name="warnings">Warnings.</param>
	/// <returns>Returns TRUE if there are no warnings or the user wants to save, otherwise returns FALSE.</returns>
	protected bool _ConfirmSave(List<string> warnings)
	{
		if(warnings.Count == 0) { return true; }

		return (ThemedMessageBox.Show(this, string.Join("\n\n", warnings) + "\n\n" + "led:save.anyway".LookUp("Save anyway?"), "led:save.caption".LookUp("Save"),
									  MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
	}


	/// <summary>Shows an error that prevents saving.</summary>
	/// <param name="errors">Errors.</param>
	/// <returns>Returns TRUE if there are no errors, otherwise shows them and returns FALSE.</returns>
	protected bool _CheckErrors(List<string> errors)
	{
		if(errors.Count == 0) { return true; }

		ThemedMessageBox.Show(this, string.Join("\n\n", errors), "led:save.caption".LookUp("Save"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		return false;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private properties                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the file name of the document without path, or "Untitled" for a new document.</summary>
	private string _DocumentName
	{
		get { return ((_Document.FileName == null) ? "led:table.untitled".LookUp("Untitled") : Path.GetFileName(_Document.FileName)); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Saves the document.</summary>
	/// <param name="saveAs">Determines if the user is asked for a file name.</param>
	/// <returns>Returns TRUE if the document has been saved, otherwise returns FALSE.</returns>
	private bool _Save(bool saveAs)
	{
		if((!_Commit()) || (!_CheckBeforeSave())) { return false; }

		string? fileName = _Document.FileName;
		if(saveAs || (fileName == null))
		{
			using SaveFileDialog d = new()
			{
				Filter = _SaveFilter,
				FileName = ((fileName == null) ? _SuggestFileName() : Path.GetFileName(fileName)),
				InitialDirectory = ((fileName == null) ? string.Empty : (Path.GetDirectoryName(fileName) ?? string.Empty)),
				FilterIndex = _GetFilterIndex(fileName),
				OverwritePrompt = true
			};
			if(d.ShowDialog(this) != DialogResult.OK) { return false; }
			fileName = d.FileName;

			if(_Program._DocumentWindows.Any(m => (m != this) && string.Equals(m._Document.FileName, Path.GetFullPath(fileName), StringComparison.OrdinalIgnoreCase)))
			{
				ThemedMessageBox.Show(this, "led:save.open".LookUp("The file is open in another window. Close it there first."), "led:save.caption".LookUp("Save"),
									  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
		}

		try
		{
			_Document.Save(fileName);
		}
		catch(Exception ex)
		{
			ThemedMessageBox.Show(this, "led:save.failed".LookUp("The file could not be saved.\n\n$ERROR", ("ERROR", ex.Message)), "led:save.caption".LookUp("Save"),
								  MessageBoxButtons.OK, MessageBoxIcon.Error);
			return false;
		}

		_Program._RecentFiles.Add(_Document.FileName!);
		_Update();
		return true;
	}


	/// <summary>Sets the colors that the window chooses itself to the colors of the theme: problems are shown in the error color.</summary>
	private void _UpdateThemeColors()
	{
		_LabelProblems.ForeColor = ThemeManager.Theme[ThemeColor.ERROR];
	}


	/// <summary>Moves the window so that it does not cover the window it was opened from or another window exactly.</summary>
	/// <remarks>A window opened from a window of the same type is placed next to it. A window of another type keeps the position
	///          restored by the layout provider, so table and definition windows each remember their own position.</remarks>
	private void _Cascade()
	{
		if(WindowState != FormWindowState.Normal) return;

		int offset = LogicalToDeviceUnits(_CASCADE_OFFSET);
		if((_Opener != null) && (!_Opener.IsDisposed) && (_Opener.GetType() == GetType()) && (_Opener.WindowState == FormWindowState.Normal))
		{
			Location = new Point(_Opener.Left + offset, _Opener.Top + offset);
		}

		Point[] taken = _Program._Context.Forms.Where(m => (m != this) && m.Visible).Select(m => m.Location).ToArray();
		for(int i = 0; (i < 20) && taken.Contains(Location); i++) { Location = new Point(Left + offset, Top + offset); }

		Rectangle area = Screen.FromControl(this).WorkingArea;
		if((Right > area.Right) || (Bottom > area.Bottom)) { Location = new Point(area.Left + offset, area.Top + offset); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Form                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Window activated: the find window works on this window now.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnActivated(EventArgs e)
	{
		base.OnActivated(e);
		if(!DesignMode) { _FindWindow._Attach(this); }
	}


	/// <summary>Window closed: the window stops following the theme.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		ThemeManager.ThemeChanged -= _ThemeManager_ThemeChanged;
		base.OnFormClosed(e);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Theme of the application changed.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ThemeManager_ThemeChanged(object? sender, EventArgs e)
	{
		_UpdateThemeColors();
	}


	/// <summary>Window loaded.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _DocumentWindow_Load(object? sender, EventArgs e)
	{
		if(DesignMode) return;

		_Cascade();
		_Localize();

		foreach(ToolStripItem i in new ToolStripItem[] { _MenuEditFind, _MenuEditFindNext, _MenuEditFindPrevious, _MenuEditReplace }) { i.Enabled = _CanSearch; }
		foreach(ToolStripItem i in new ToolStripItem[] { _MenuToolsTextEditor, _MenuToolsSort, _MenuToolsImport }) { i.Enabled = _HasTableTools; }
	}


	/// <summary>Window closing.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _DocumentWindow_FormClosing(object? sender, FormClosingEventArgs e)
	{
		if(DesignMode) return;

		_Commit();
		if(!_Document.IsDirty) return;

		MultiFormApplicationContext.ActivateForm(this);
		switch(ThemedMessageBox.Show(this, "led:close.unsaved".LookUp("$NAME has unsaved changes. Do you want to save them?", ("NAME", _DocumentName)),
									 "led:app.name".LookUp("Localization Editor"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
		{
			case DialogResult.Yes:
				e.Cancel = (!_Save(false)); break;
			case DialogResult.Cancel:
				e.Cancel = true; break;
		}
	}


	/// <summary>Localized texts applied.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Locale_Localized(object? sender, EventArgs e)
	{
		if(_Binding) return;
		_Localize();
	}


	/// <summary>File > New > Table.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuFileNewTable_Click(object? sender, EventArgs e)
	{
		_Program._NewTable(this);
	}


	/// <summary>File > New > Definition.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuFileNewDefinition_Click(object? sender, EventArgs e)
	{
		_Program._NewDefinition(this);
	}


	/// <summary>File > Open.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuFileOpen_Click(object? sender, EventArgs e)
	{
		using OpenFileDialog d = new()
		{
			Filter = "led:filter.supported".LookUp("All Supported Files") + " (*.ltab, *.cltab, *.locale)|*.ltab;*.cltab;*.locale|" +
					 "led:filter.tables".LookUp("Language Tables") + " (*.ltab, *.cltab)|*.ltab;*.cltab|" +
					 "led:filter.locale".LookUp("Locale Definitions") + " (*.locale)|*.locale|" +
					 "led:filter.all".LookUp("All Files") + " (*.*)|*.*",
			Multiselect = true
		};
		if(_Document.FileName != null) { d.InitialDirectory = (Path.GetDirectoryName(_Document.FileName) ?? string.Empty); }

		if(d.ShowDialog(this) == DialogResult.OK) { _Program._Open(d.FileNames, this); }
	}


	/// <summary>Recent file selected.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Recent_ItemSelected(object? sender, RecentItemEventArgs e)
	{
		if(e.Available == false)
		{
			e.Remove = (ThemedMessageBox.Show(this, "led:recent.missing".LookUp("$FILE does not exist any more.\n\nRemove it from the list?", ("FILE", e.Item.Key)),
											  "led:open.caption".LookUp("Open"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
			return;
		}
		_Program._Open([e.Item.Key], this);
	}


	/// <summary>File > Save.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuFileSave_Click(object? sender, EventArgs e)
	{
		_Save(false);
	}


	/// <summary>File > Save As.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuFileSaveAs_Click(object? sender, EventArgs e)
	{
		_Save(true);
	}


	/// <summary>File > Close.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuFileClose_Click(object? sender, EventArgs e)
	{
		Close();
	}


	/// <summary>Edit menu opening: Find Previous is hidden; it is only available through its shortcut.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuEdit_DropDownOpening(object? sender, EventArgs e)
	{
		_MenuEditFindPrevious.Visible = false;
	}


	/// <summary>Edit menu closed: Find Previous is shown again, so that its shortcut works.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuEdit_DropDownClosed(object? sender, EventArgs e)
	{
		_MenuEditFindPrevious.Visible = true;
	}


	/// <summary>Edit > Find.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuEditFind_Click(object? sender, EventArgs e)
	{
		_FindWindow._Show(this, false);
	}


	/// <summary>Edit > Find Next.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuEditFindNext_Click(object? sender, EventArgs e)
	{
		_FindWindow._FindNext(this, false);
	}


	/// <summary>Edit > Find Previous.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuEditFindPrevious_Click(object? sender, EventArgs e)
	{
		_FindWindow._FindNext(this, true);
	}


	/// <summary>Edit > Replace.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuEditReplace_Click(object? sender, EventArgs e)
	{
		_FindWindow._Show(this, true);
	}


	/// <summary>Tools > Text Editor.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuToolsTextEditor_Click(object? sender, EventArgs e)
	{
		_EditText();
	}


	/// <summary>Tools > Sort.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuToolsSort_Click(object? sender, EventArgs e)
	{
		_Sort();
	}


	/// <summary>Tools > Import.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuToolsImport_Click(object? sender, EventArgs e)
	{
		_Import();
	}


	/// <summary>Tools > Settings.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuToolsSettings_Click(object? sender, EventArgs e)
	{
		using _SettingsWindow w = new();
		w.ShowDialog(this);
	}


	/// <summary>Help > Show Help.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuHelpShowHelp_Click(object? sender, EventArgs e)
	{
		_Help._Show(this, _HelpPage);
	}


	/// <summary>Help > About.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuHelpAbout_Click(object? sender, EventArgs e)
	{
		using _AboutWindow w = new();
		w.ShowDialog(this);
	}


	/// <summary>Window menu opening.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuWindow_DropDownOpening(object? sender, EventArgs e)
	{
		_MenuWindow.DropDownItems.Clear();

		int n = 1;
		foreach(Form i in _Program._Context.Forms)
		{
			string text = ((n < 10) ? ("&" + n + " ") : string.Empty) + ((i is _DocumentWindow w) ? w._Caption : i.Text).Replace("&", "&&");
			ToolStripMenuItem m = new(text) { Tag = i, Checked = (i == this) };
			m.Click += _MenuWindowItem_Click;
			_MenuWindow.DropDownItems.Add(m);
			n++;
		}
	}


	/// <summary>Window selected.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuWindowItem_Click(object? sender, EventArgs e)
	{
		if((sender as ToolStripMenuItem)?.Tag is Form f) { MultiFormApplicationContext.ActivateForm(f); }
	}
}
