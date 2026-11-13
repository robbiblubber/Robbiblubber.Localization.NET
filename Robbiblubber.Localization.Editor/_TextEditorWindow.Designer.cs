namespace Robbiblubber.Localization.Editor;

partial class _TextEditorWindow
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if(disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_TextEditorWindow));
		_Locale = new Robbiblubber.Localization.Controls.LocaleProvider(components);
		_ThemeProvider = new Robbiblubber.Spellbook.Controls.Themes.ThemeProvider(components);
		_CmenuTextUndo = new ToolStripMenuItem();
		_CmenuTextCut = new ToolStripMenuItem();
		_CmenuTextCopy = new ToolStripMenuItem();
		_CmenuTextPaste = new ToolStripMenuItem();
		_CmenuTextDelete = new ToolStripMenuItem();
		_CmenuTextSelectAll = new ToolStripMenuItem();
		_ButtonOK = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_ButtonCancel = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_Edit = new Robbiblubber.Spellbook.Controls.EditCommandProvider(components);
		_Layout = new Robbiblubber.Spellbook.Controls.WindowLayoutProvider(components);
		_TextValue = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_CmenuText = new ContextMenuStrip(components);
		_CmenuTextSep0 = new ToolStripSeparator();
		_CmenuTextSep1 = new ToolStripSeparator();
		_PanelButtons = new Panel();
		_CmenuText.SuspendLayout();
		_PanelButtons.SuspendLayout();
		SuspendLayout();
		//
		// _Locale
		//
		_Locale.ContainerControl = this;
		//
		// _ThemeProvider
		//
		_ThemeProvider.ContainerControl = this;
		//
		// _CmenuTextUndo
		//
		_Edit.SetEditCommand(_CmenuTextUndo, Spellbook.Controls.EditCommand.UNDO);
		_Locale.SetLocaleSymbol(_CmenuTextUndo, "led:menu.edit.undo");
		_CmenuTextUndo.Name = "_CmenuTextUndo";
		_CmenuTextUndo.ShortcutKeyDisplayString = "Ctrl+Z";
		_CmenuTextUndo.Size = new Size(164, 22);
		_CmenuTextUndo.Text = "&Undo";
		//
		// _CmenuTextCut
		//
		_Edit.SetEditCommand(_CmenuTextCut, Spellbook.Controls.EditCommand.CUT);
		_Locale.SetLocaleSymbol(_CmenuTextCut, "led:menu.edit.cut");
		_CmenuTextCut.Name = "_CmenuTextCut";
		_CmenuTextCut.ShortcutKeyDisplayString = "Ctrl+X";
		_CmenuTextCut.Size = new Size(164, 22);
		_CmenuTextCut.Text = "Cu&t";
		//
		// _CmenuTextCopy
		//
		_Edit.SetEditCommand(_CmenuTextCopy, Spellbook.Controls.EditCommand.COPY);
		_Locale.SetLocaleSymbol(_CmenuTextCopy, "led:menu.edit.copy");
		_CmenuTextCopy.Name = "_CmenuTextCopy";
		_CmenuTextCopy.ShortcutKeyDisplayString = "Ctrl+C";
		_CmenuTextCopy.Size = new Size(164, 22);
		_CmenuTextCopy.Text = "&Copy";
		//
		// _CmenuTextPaste
		//
		_Edit.SetEditCommand(_CmenuTextPaste, Spellbook.Controls.EditCommand.PASTE);
		_Locale.SetLocaleSymbol(_CmenuTextPaste, "led:menu.edit.paste");
		_CmenuTextPaste.Name = "_CmenuTextPaste";
		_CmenuTextPaste.ShortcutKeyDisplayString = "Ctrl+V";
		_CmenuTextPaste.Size = new Size(164, 22);
		_CmenuTextPaste.Text = "&Paste";
		//
		// _CmenuTextDelete
		//
		_Edit.SetEditCommand(_CmenuTextDelete, Spellbook.Controls.EditCommand.DELETE);
		_Locale.SetLocaleSymbol(_CmenuTextDelete, "led:menu.edit.delete");
		_CmenuTextDelete.Name = "_CmenuTextDelete";
		_CmenuTextDelete.ShortcutKeyDisplayString = "Del";
		_CmenuTextDelete.Size = new Size(164, 22);
		_CmenuTextDelete.Text = "&Delete";
		//
		// _CmenuTextSelectAll
		//
		_Edit.SetEditCommand(_CmenuTextSelectAll, Spellbook.Controls.EditCommand.SELECT_ALL);
		_Locale.SetLocaleSymbol(_CmenuTextSelectAll, "led:menu.edit.selectall");
		_CmenuTextSelectAll.Name = "_CmenuTextSelectAll";
		_CmenuTextSelectAll.ShortcutKeyDisplayString = "Ctrl+A";
		_CmenuTextSelectAll.Size = new Size(164, 22);
		_CmenuTextSelectAll.Text = "Select &All";
		//
		// _ButtonOK
		//
		_ButtonOK.Anchor =  AnchorStyles.Top | AnchorStyles.Right;
		_ButtonOK.DialogResult = DialogResult.OK;
		_Locale.SetLocaleSymbol(_ButtonOK, "led:dialog.ok");
		_ButtonOK.Location = new Point(386, 10);
		_ButtonOK.Name = "_ButtonOK";
		_ButtonOK.Size = new Size(78, 26);
		_ButtonOK.TabIndex = 0;
		_ButtonOK.Text = "OK";
		_ButtonOK.UseVisualStyleBackColor = true;
		//
		// _ButtonCancel
		//
		_ButtonCancel.Anchor =  AnchorStyles.Top | AnchorStyles.Right;
		_ButtonCancel.DialogResult = DialogResult.Cancel;
		_Locale.SetLocaleSymbol(_ButtonCancel, "led:dialog.cancel");
		_ButtonCancel.Location = new Point(470, 10);
		_ButtonCancel.Name = "_ButtonCancel";
		_ButtonCancel.Size = new Size(78, 26);
		_ButtonCancel.TabIndex = 1;
		_ButtonCancel.Text = "Cancel";
		_ButtonCancel.UseVisualStyleBackColor = true;
		//
		// _Edit
		//
		_Edit.ContainerControl = this;
		//
		// _Layout
		//
		_Layout.ContainerControl = this;
		_Layout.LayoutName = "texteditor";
		//
		// _TextValue
		//
		_TextValue.AcceptsReturn = true;
		_TextValue.AcceptsTab = true;
		_TextValue.ContextMenuStrip = _CmenuText;
		_TextValue.Dock = DockStyle.Fill;
		_TextValue.Location = new Point(0, 0);
		_TextValue.Multiline = true;
		_TextValue.Name = "_TextValue";
		_TextValue.ScrollBars = ScrollBars.Vertical;
		_TextValue.Size = new Size(560, 302);
		_TextValue.TabIndex = 0;
		//
		// _CmenuText
		//
		_CmenuText.Items.AddRange(new ToolStripItem[] { _CmenuTextUndo, _CmenuTextSep0, _CmenuTextCut, _CmenuTextCopy, _CmenuTextPaste, _CmenuTextDelete, _CmenuTextSep1, _CmenuTextSelectAll });
		_CmenuText.Name = "_CmenuText";
		_CmenuText.Size = new Size(165, 148);
		//
		// _CmenuTextSep0
		//
		_CmenuTextSep0.Name = "_CmenuTextSep0";
		_CmenuTextSep0.Size = new Size(161, 6);
		//
		// _CmenuTextSep1
		//
		_CmenuTextSep1.Name = "_CmenuTextSep1";
		_CmenuTextSep1.Size = new Size(161, 6);
		//
		// _PanelButtons
		//
		_PanelButtons.Controls.Add(_ButtonOK);
		_PanelButtons.Controls.Add(_ButtonCancel);
		_PanelButtons.Dock = DockStyle.Bottom;
		_PanelButtons.Location = new Point(0, 302);
		_PanelButtons.Name = "_PanelButtons";
		_PanelButtons.Size = new Size(560, 46);
		_PanelButtons.TabIndex = 1;
		//
		// _TextEditorWindow
		//
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = _ButtonCancel;
		ClientSize = new Size(560, 348);
		Controls.Add(_TextValue);
		Controls.Add(_PanelButtons);
		Icon = (Icon) resources.GetObject("$this.Icon");
		MinimizeBox = false;
		MinimumSize = new Size(320, 200);
		Name = "_TextEditorWindow";
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterParent;
		Text = "Text Editor";
		_CmenuText.ResumeLayout(false);
		_PanelButtons.ResumeLayout(false);
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Robbiblubber.Localization.Controls.LocaleProvider _Locale;
	private Robbiblubber.Spellbook.Controls.Themes.ThemeProvider _ThemeProvider;
	private Robbiblubber.Spellbook.Controls.EditCommandProvider _Edit;
	private Robbiblubber.Spellbook.Controls.WindowLayoutProvider _Layout;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextValue;
	private System.Windows.Forms.ContextMenuStrip _CmenuText;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextUndo;
	private System.Windows.Forms.ToolStripSeparator _CmenuTextSep0;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextCut;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextCopy;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextPaste;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextDelete;
	private System.Windows.Forms.ToolStripSeparator _CmenuTextSep1;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextSelectAll;
	private System.Windows.Forms.Panel _PanelButtons;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonOK;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonCancel;
}
