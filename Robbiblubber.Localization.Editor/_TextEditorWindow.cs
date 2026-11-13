namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;



/// <summary>This class implements the text editor, a dialog that edits a text of a language table with real line breaks and
///          tabs.</summary>
/// <remarks>The text is passed and returned in the single-line form shown in the table (see <see cref="_TextEscaping"/>). OK (also
///          Ctrl+Enter) takes the text over; Cancel and Escape discard it without asking. Enter inserts a line break, Tab a tab.</remarks>
[SupportedOSPlatform("windows")]
internal sealed partial class _TextEditorWindow: Form
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Symbol of the row.</summary>
	private readonly string _Symbol;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="symbol">Symbol of the row, shown in the title.</param>
	/// <param name="value">Text in its single-line form.</param>
	public _TextEditorWindow(string symbol, string value)
	{
		InitializeComponent();
		_Layout.ConfigFile = _Settings._LayoutFileName;

		_Symbol = symbol.Trim();
		_TextValue.Text = _TextEscaping._Unescape(value);
		_TextValue.Select(0, 0);
		_UpdateTitle();
		_Locale.Localized += (s, e) => _UpdateTitle();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the text in its single-line form.</summary>
	public string Value
	{
		get { return _TextEscaping._Escape(_TextValue.Text); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Sets the title, with the symbol if there is one.</summary>
	private void _UpdateTitle()
	{
		string title = "led:texteditor.title".LookUp("Text Editor");
		Text = ((_Symbol.Length == 0) ? title : (_Symbol + " - " + title));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Form                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Help requested (F1): shows the help page about the text editor.</summary>
	/// <param name="hevent">Event arguments.</param>
	protected override void OnHelpRequested(HelpEventArgs hevent)
	{
		_Help._Show(this, "tools");
		hevent.Handled = true;
	}


	/// <summary>Processes command keys: Ctrl+Enter closes the window with OK, Ctrl+A selects the whole text.</summary>
	/// <param name="msg">Message.</param>
	/// <param name="keyData">Keys.</param>
	/// <returns>Returns TRUE if the key has been processed, otherwise returns FALSE.</returns>
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		switch(keyData)
		{
			case (Keys.Control | Keys.Enter):
				DialogResult = DialogResult.OK;
				return true;
			case (Keys.Control | Keys.A):
				_TextValue.SelectAll();
				return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}
}
