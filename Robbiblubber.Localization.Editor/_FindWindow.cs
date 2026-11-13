namespace Robbiblubber.Localization.Editor;

using System.Media;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

using Robbiblubber.Spellbook.Controls;



/// <summary>This class implements the window for finding and replacing text in language tables.</summary>
/// <remarks><para>There is one window per process. It works on the active document window and is owned by it, so it stays above the
///          editor windows. If that window is closed while the find window is shown, the find window appears again with the next
///          document window that is activated. Closing the window only hides it.</para>
///          <para>The search options and the recent search texts and replacements are kept in the settings. Find Next (F3) and Find
///          Previous (Shift+F3) also work while the window is not shown; they use the latest search text and report in the status bar
///          of the document window.</para></remarks>
[SupportedOSPlatform("windows")]
internal sealed partial class _FindWindow: Form
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Instance of the window, NULL if none has been created.</summary>
	private static _FindWindow? _Instance = null;

	/// <summary>Determines if the window is shown again when the next document window is activated.</summary>
	private static bool _ReopenPending = false;

	/// <summary>Determines if the window that is shown again is in replace mode.</summary>
	private static bool _ReopenReplace = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Document window the window works on.</summary>
	private _DocumentWindow? _Target = null;

	/// <summary>Determines if the window is in replace mode.</summary>
	private bool _ReplaceMode = false;

	/// <summary>Determines if the window is activated when it is shown.</summary>
	private bool _Activating = true;

	/// <summary>Determines if the window was shown when its owner started closing.</summary>
	private bool _ShownOnClose = false;

	/// <summary>Determines if the window is being initialized; option changes are not taken over then.</summary>
	private bool _Initializing = true;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	private _FindWindow()
	{
		InitializeComponent();
		_Layout.ConfigFile = _Settings._LayoutFileName;

		_RadioSymbol.Checked = (_Settings._FindScope == _SearchScope.SYMBOL);
		_RadioText.Checked = (_Settings._FindScope == _SearchScope.TEXT);
		_RadioBoth.Checked = (_Settings._FindScope == _SearchScope.BOTH);
		_CheckMatchCase.Checked = _Settings._FindMatchCase;
		_CheckWholeWord.Checked = _Settings._FindWholeWord;
		_CheckRegularExpression.Checked = _Settings._FindRegularExpression;

		_FillHistory();
		_ComboFind.Text = (_Settings._FindHistory.FirstOrDefault() ?? string.Empty);
		_Initializing = false;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Shows the window for a document window.</summary>
	/// <param name="target">Document window.</param>
	/// <param name="replace">Determines if the window is shown in replace mode.</param>
	/// <remarks>The selected text or the value of the current cell of the table is suggested as search text.</remarks>
	internal static void _Show(_DocumentWindow target, bool replace)
	{
		bool created = ((_Instance is null) || _Instance.IsDisposed);
		if(created) { _Instance = new _FindWindow(); }

		_FindWindow w = _Instance!;
		_ReopenPending = false;

		string? suggestion = (target as _LanguageTableWindow)?._GetSearchSuggestion();
		w._SetTarget(target);
		w._SetMode(replace);
		if(suggestion is not null) { w._ComboFind.Text = suggestion; }

		if(w.Visible)
		{
			w.Activate();
		}
		else
		{
			if(created) { w._PlaceNear(target); }
			w._Activating = true;
			w.Show(target);
		}

		w._ComboFind.Focus();
		w._ComboFind.SelectAll();
	}


	/// <summary>Finds the next cell that contains the search text.</summary>
	/// <param name="target">Document window.</param>
	/// <param name="backward">Determines if the table is searched backwards.</param>
	/// <remarks>If there is no search text yet, the window is shown.</remarks>
	internal static void _FindNext(_DocumentWindow target, bool backward)
	{
		if(target is not _LanguageTableWindow table) return;

		_FindWindow? w = _Visible;
		string text = ((w is null) ? (_Settings._FindHistory.FirstOrDefault() ?? string.Empty) : w._ComboFind.Text);

		if(text.Length == 0)
		{
			_Show(target, false);
			return;
		}
		_Search(table, text, backward, w);
	}


	/// <summary>Lets the window work on a document window that has been activated.</summary>
	/// <param name="window">Document window.</param>
	/// <remarks>If the window has been closed together with its owner, it is shown again, without taking the focus.</remarks>
	internal static void _Attach(_DocumentWindow window)
	{
		if(_Visible is _FindWindow w)
		{
			w._SetTarget(window);
			return;
		}
		if(!_ReopenPending) return;

		_ReopenPending = false;
		_Instance = w = new _FindWindow();
		w._SetTarget(window);
		w._SetMode(_ReopenReplace);
		w._Activating = false;
		w.Show(window);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static properties                                                                                        //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the window if it is shown, otherwise NULL.</summary>
	private static _FindWindow? _Visible
	{
		get { return (((_Instance is not null) && (!_Instance.IsDisposed) && _Instance.Visible) ? _Instance : null); }
	}


	/// <summary>Gets the message shown when a regular expression takes too long.</summary>
	private static string _TimeoutMessage
	{
		get { return "led:find.timeout".LookUp("The search took too long and has been cancelled."); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a search with the options from the settings.</summary>
	/// <param name="text">Search text.</param>
	/// <param name="error">Returns the error message if the search text is not a valid regular expression.</param>
	/// <returns>Returns the search, NULL if the search text is not valid.</returns>
	private static _TableSearch? _CreateSearch(string text, out string error)
	{
		error = string.Empty;
		try
		{
			return new _TableSearch(text, _Settings._FindScope, _Settings._FindMatchCase, _Settings._FindWholeWord, _Settings._FindRegularExpression);
		}
		catch(ArgumentException ex)
		{
			error = "led:find.badregex".LookUp("The regular expression is not valid: $ERROR", ("ERROR", ex.Message));
			return null;
		}
	}


	/// <summary>Searches a table and reports the result.</summary>
	/// <param name="table">Table window.</param>
	/// <param name="text">Search text.</param>
	/// <param name="backward">Determines if the table is searched backwards.</param>
	/// <param name="w">Find window that reports the result, NULL to report it in the status bar of the table window.</param>
	private static void _Search(_LanguageTableWindow table, string text, bool backward, _FindWindow? w)
	{
		_Settings._AddToHistory(_Settings._FindHistory, text);
		w?._FillHistory();
		_Settings._Save();

		if(_CreateSearch(text, out string error) is not _TableSearch search)
		{
			_Report(table, w, error, true);
			return;
		}

		try
		{
			switch(table._Find(search, backward))
			{
				case null:
					_Report(table, w, "led:find.notfound".LookUp("“$TEXT” was not found.", ("TEXT", text)), true); break;
				case _TableMatch m when m.Wrapped:
					_Report(table, w, (backward ? "led:find.wrapped.start".LookUp("Passed the beginning of the table; continued at the end.")
												: "led:find.wrapped.end".LookUp("Passed the end of the table; continued at the beginning.")), false);
					break;
				default:
					_Report(table, w, string.Empty, false); break;
			}
		}
		catch(RegexMatchTimeoutException) { _Report(table, w, _TimeoutMessage, true); }
	}


	/// <summary>Shows a message in the find window or, if it is not shown, in the status bar of the table window.</summary>
	/// <param name="table">Table window.</param>
	/// <param name="w">Find window, NULL if it is not shown.</param>
	/// <param name="text">Message.</param>
	/// <param name="beep">Determines if a sound is played.</param>
	private static void _Report(_DocumentWindow table, _FindWindow? w, string text, bool beep)
	{
		if(w is null)
		{
			table._ShowMessage(text);
		}
		else { w._LabelStatus.Text = text; }

		if(beep) { SystemSounds.Beep.Play(); }
	}


	/// <summary>Fills a combo box with a history list, keeping its text.</summary>
	/// <param name="combo">Combo box.</param>
	/// <param name="history">History list.</param>
	private static void _FillCombo(ComboBox combo, List<string> history)
	{
		string text = combo.Text;

		combo.BeginUpdate();
		combo.Items.Clear();
		combo.Items.AddRange(history.ToArray<object>());
		combo.EndUpdate();

		if(combo.Text != text) { combo.Text = text; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Sets the document window the window works on.</summary>
	/// <param name="window">Document window.</param>
	private void _SetTarget(_DocumentWindow window)
	{
		_Target = window;
		if(Owner != window) { Owner = window; }

		_LabelStatus.Text = (window._CanSearch ? string.Empty : "led:find.notable".LookUp("Only language tables can be searched."));
		_UpdateButtons();
	}


	/// <summary>Switches between find mode and replace mode.</summary>
	/// <param name="replace">Determines if the window is in replace mode.</param>
	private void _SetMode(bool replace)
	{
		_ReplaceMode = replace;
		_PanelReplace.Visible = replace;
		_ButtonReplace.Visible = replace;
		_ButtonReplaceAll.Visible = replace;

		_UpdateHeight();
		_UpdateTitle();
	}


	/// <summary>Adjusts the height of the window to the mode.</summary>
	private void _UpdateHeight()
	{
		ClientSize = new Size(ClientSize.Width, (_PanelFind.Height + (_ReplaceMode ? _PanelReplace.Height : 0) + _PanelOptions.Height + _StatusFind.Height));
	}


	/// <summary>Sets the title for the mode.</summary>
	private void _UpdateTitle()
	{
		Text = (_ReplaceMode ? "led:find.title.replace".LookUp("Replace") : "led:find.title".LookUp("Find"));
	}


	/// <summary>Enables the buttons if there is a search text and the window works on a table.</summary>
	private void _UpdateButtons()
	{
		bool enabled = ((_Target is _LanguageTableWindow { IsDisposed: false }) && (_ComboFind.Text.Length > 0));

		_ButtonFindNext.Enabled = enabled;
		_ButtonReplace.Enabled = enabled;
		_ButtonReplaceAll.Enabled = enabled;
	}


	/// <summary>Fills the combo boxes with the recent search texts and replacements.</summary>
	private void _FillHistory()
	{
		_FillCombo(_ComboFind, _Settings._FindHistory);
		_FillCombo(_ComboReplace, _Settings._ReplaceHistory);
	}


	/// <summary>Takes over the replacement into the history.</summary>
	/// <returns>Returns the replacement.</returns>
	private string _TakeReplacement()
	{
		string rval = _ComboReplace.Text;
		if(rval.Length > 0)
		{
			_Settings._AddToHistory(_Settings._ReplaceHistory, rval);
			_FillHistory();
		}
		return rval;
	}


	/// <summary>Places the window at the upper right of a document window.</summary>
	/// <param name="window">Document window.</param>
	/// <remarks>The position is used when the layout provider has no saved position.</remarks>
	private void _PlaceNear(Form window)
	{
		int margin = LogicalToDeviceUnits(24);
		Location = new Point(Math.Max(window.Left, (window.Right - Width - margin)), (window.Top + LogicalToDeviceUnits(80)));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Form                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets if the window is shown without being activated.</summary>
	protected override bool ShowWithoutActivation
	{
		get { return (!_Activating); }
	}


	/// <summary>Window loaded: the height is adjusted to the mode, after the layout provider has restored the saved size.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		_UpdateHeight();
	}


	/// <summary>Help requested (F1): shows the help page about finding and replacing.</summary>
	/// <param name="hevent">Event arguments.</param>
	protected override void OnHelpRequested(HelpEventArgs hevent)
	{
		_Help._Show(this, "find");
		hevent.Handled = true;
	}


	/// <summary>Processes command keys: F3 and Shift+F3 search, Ctrl+F and Ctrl+H switch the mode.</summary>
	/// <param name="msg">Message.</param>
	/// <param name="keyData">Keys.</param>
	/// <returns>Returns TRUE if the key has been processed, otherwise returns FALSE.</returns>
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		switch(keyData)
		{
			case Keys.F3:
			case (Keys.Shift | Keys.F3):
				if(_Target is not null) { _FindNext(_Target, (keyData != Keys.F3)); }
				return true;
			case (Keys.Control | Keys.F):
			case (Keys.Control | Keys.H):
				_SetMode(keyData == (Keys.Control | Keys.H));
				_ComboFind.Focus();
				_ComboFind.SelectAll();
				return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}


	/// <summary>Window closing: closing by the user only hides the window.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		base.OnFormClosing(e);

		if(e.CloseReason == CloseReason.UserClosing)
		{
			e.Cancel = true;
			Hide();
			_Settings._Save();
			return;
		}
		_ShownOnClose = Visible;
	}


	/// <summary>Window closed: if it has been closed together with its owner, it is shown again with the next document window.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		if((e.CloseReason == CloseReason.FormOwnerClosing) && _ShownOnClose)
		{
			_ReopenPending = true;
			_ReopenReplace = _ReplaceMode;
		}
		_Settings._Save();

		base.OnFormClosed(e);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Localized texts applied.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Locale_Localized(object? sender, EventArgs e)
	{
		_UpdateTitle();
	}


	/// <summary>Search text changed.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ComboFind_TextChanged(object? sender, EventArgs e)
	{
		_UpdateButtons();
	}


	/// <summary>Search option changed.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Option_Changed(object? sender, EventArgs e)
	{
		if(_Initializing) return;

		_Settings._FindScope = (_RadioSymbol.Checked ? _SearchScope.SYMBOL : (_RadioText.Checked ? _SearchScope.TEXT : _SearchScope.BOTH));
		_Settings._FindMatchCase = _CheckMatchCase.Checked;
		_Settings._FindWholeWord = _CheckWholeWord.Checked;
		_Settings._FindRegularExpression = _CheckRegularExpression.Checked;
	}


	/// <summary>Find Next clicked.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonFindNext_Click(object? sender, EventArgs e)
	{
		if(_Target is _LanguageTableWindow table) { _Search(table, _ComboFind.Text, _CheckBackward.Checked, this); }
	}


	/// <summary>Replace clicked: replaces the search text in the cell found last and finds the next cell.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonReplace_Click(object? sender, EventArgs e)
	{
		if(_Target is not _LanguageTableWindow table) return;

		string replacement = _TakeReplacement();
		if(_CreateSearch(_ComboFind.Text, out string error) is not _TableSearch search)
		{
			_Report(table, this, error, true);
			return;
		}

		try
		{
			table._ReplaceCurrent(search, replacement);
		}
		catch(RegexMatchTimeoutException)
		{
			_Report(table, this, _TimeoutMessage, true);
			return;
		}
		_Search(table, _ComboFind.Text, _CheckBackward.Checked, this);
	}


	/// <summary>Replace All clicked: replaces all occurrences after asking the user.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonReplaceAll_Click(object? sender, EventArgs e)
	{
		if(_Target is not _LanguageTableWindow table) return;

		string text = _ComboFind.Text;
		string replacement = _TakeReplacement();
		_Settings._AddToHistory(_Settings._FindHistory, text);
		_FillHistory();
		_Settings._Save();

		if(_CreateSearch(text, out string error) is not _TableSearch search)
		{
			_Report(table, this, error, true);
			return;
		}

		try
		{
			(int count, int rows) = table._CountMatches(search);
			if(count == 0)
			{
				_Report(table, this, "led:find.notfound".LookUp("“$TEXT” was not found.", ("TEXT", text)), true);
				return;
			}

			if(ThemedMessageBox.Show(this, "led:find.replaceall.confirm".LookUp("Replace $COUNT occurrences in $ROWS rows?\n\nThis cannot be undone.", ("COUNT", count), ("ROWS", rows)),
									 Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) { return; }

			_Report(table, this, "led:find.replaced".LookUp("Occurrences replaced: $COUNT", ("COUNT", table._ReplaceAll(search, replacement))), false);
		}
		catch(RegexMatchTimeoutException) { _Report(table, this, _TimeoutMessage, true); }
	}


	/// <summary>Close clicked.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonClose_Click(object? sender, EventArgs e)
	{
		Close();
	}
}
