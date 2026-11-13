namespace Robbiblubber.Localization.Editor;

partial class _DocumentWindow
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_DocumentWindow));
		_Locale = new Robbiblubber.Localization.Controls.LocaleProvider(components);
		_ThemeProvider = new Robbiblubber.Spellbook.Controls.Themes.ThemeProvider(components);
		_MenuFile = new ToolStripMenuItem();
		_MenuFileNew = new ToolStripMenuItem();
		_MenuFileNewTable = new ToolStripMenuItem();
		_MenuFileNewDefinition = new ToolStripMenuItem();
		_MenuFileOpen = new ToolStripMenuItem();
		_MenuFileRecent = new ToolStripMenuItem();
		_MenuFileSep1 = new ToolStripSeparator();
		_MenuFileSave = new ToolStripMenuItem();
		_MenuFileSaveAs = new ToolStripMenuItem();
		_MenuFileSep2 = new ToolStripSeparator();
		_MenuFileClose = new ToolStripMenuItem();
		_MenuEdit = new ToolStripMenuItem();
		_MenuEditUndo = new ToolStripMenuItem();
		_MenuEditRedo = new ToolStripMenuItem();
		_MenuEditSep0 = new ToolStripSeparator();
		_MenuEditCut = new ToolStripMenuItem();
		_MenuEditCopy = new ToolStripMenuItem();
		_MenuEditPaste = new ToolStripMenuItem();
		_MenuEditDelete = new ToolStripMenuItem();
		_MenuEditSep1 = new ToolStripSeparator();
		_MenuEditSelectAll = new ToolStripMenuItem();
		_MenuEditSep2 = new ToolStripSeparator();
		_MenuEditFind = new ToolStripMenuItem();
		_MenuEditFindNext = new ToolStripMenuItem();
		_MenuEditFindPrevious = new ToolStripMenuItem();
		_MenuEditReplace = new ToolStripMenuItem();
		_MenuTools = new ToolStripMenuItem();
		_MenuToolsTextEditor = new ToolStripMenuItem();
		_MenuToolsSort = new ToolStripMenuItem();
		_MenuToolsImport = new ToolStripMenuItem();
		_MenuToolsSep1 = new ToolStripSeparator();
		_MenuToolsSettings = new ToolStripMenuItem();
		_MenuWindow = new ToolStripMenuItem();
		_MenuWindowNone = new ToolStripMenuItem();
		_CmenuTextUndo = new ToolStripMenuItem();
		_CmenuTextCut = new ToolStripMenuItem();
		_CmenuTextCopy = new ToolStripMenuItem();
		_CmenuTextPaste = new ToolStripMenuItem();
		_CmenuTextDelete = new ToolStripMenuItem();
		_CmenuTextSelectAll = new ToolStripMenuItem();
		_Edit = new Robbiblubber.Spellbook.Controls.EditCommandProvider(components);
		_Layout = new Robbiblubber.Spellbook.Controls.WindowLayoutProvider(components);
		_Recent = new _RecentFileList(components);
		_MenuMain = new MenuStrip();
		_CmenuText = new ContextMenuStrip(components);
		_CmenuTextSep0 = new ToolStripSeparator();
		_CmenuTextSep1 = new ToolStripSeparator();
		_PanelContent = new Panel();
		_StatusMain = new StatusStrip();
		_LabelInfo = new ToolStripStatusLabel();
		_LabelProblems = new ToolStripStatusLabel();
		_LabelMessage = new ToolStripStatusLabel();
		_LabelFile = new ToolStripStatusLabel();
		_MenuHelp = new ToolStripMenuItem();
		_MenuHelpShowHelp = new ToolStripMenuItem();
		_MenuHelpAbout = new ToolStripMenuItem();
		_MenuMain.SuspendLayout();
		_CmenuText.SuspendLayout();
		_StatusMain.SuspendLayout();
		SuspendLayout();
		//
		// _Locale
		//
		_Locale.ContainerControl = this;
		//
		// _ThemeProvider
		//
		_ThemeProvider.ContainerControl = this;
		_Locale.Localized += _Locale_Localized;
		//
		// _MenuFile
		//
		_MenuFile.DropDownItems.AddRange(new ToolStripItem[] { _MenuFileNew, _MenuFileOpen, _MenuFileRecent, _MenuFileSep1, _MenuFileSave, _MenuFileSaveAs, _MenuFileSep2, _MenuFileClose });
		_Locale.SetLocaleSymbol(_MenuFile, "led:menu.file");
		_MenuFile.Name = "_MenuFile";
		_MenuFile.Size = new Size(37, 20);
		_MenuFile.Text = "&File";
		//
		// _MenuFileNew
		//
		_MenuFileNew.DropDownItems.AddRange(new ToolStripItem[] { _MenuFileNewTable, _MenuFileNewDefinition });
		_MenuFileNew.Image = (Image) resources.GetObject("_MenuFileNew.Image");
		_Locale.SetLocaleSymbol(_MenuFileNew, "led:menu.file.new");
		_MenuFileNew.Name = "_MenuFileNew";
		_MenuFileNew.Size = new Size(180, 22);
		_MenuFileNew.Text = "&New";
		//
		// _MenuFileNewTable
		//
		_MenuFileNewTable.Image = (Image) resources.GetObject("_MenuFileNewTable.Image");
		_Locale.SetLocaleSymbol(_MenuFileNewTable, "led:menu.file.new.table");
		_MenuFileNewTable.Name = "_MenuFileNewTable";
		_MenuFileNewTable.ShortcutKeys =  Keys.Control | Keys.N;
		_MenuFileNewTable.Size = new Size(180, 22);
		_MenuFileNewTable.Text = "&Table";
		_MenuFileNewTable.Click += _MenuFileNewTable_Click;
		//
		// _MenuFileNewDefinition
		//
		_MenuFileNewDefinition.Image = (Image) resources.GetObject("_MenuFileNewDefinition.Image");
		_Locale.SetLocaleSymbol(_MenuFileNewDefinition, "led:menu.file.new.definition");
		_MenuFileNewDefinition.Name = "_MenuFileNewDefinition";
		_MenuFileNewDefinition.ShortcutKeys =  Keys.Control | Keys.Shift | Keys.N;
		_MenuFileNewDefinition.ShowShortcutKeys = false;
		_MenuFileNewDefinition.Size = new Size(180, 22);
		_MenuFileNewDefinition.Text = "&Definition";
		_MenuFileNewDefinition.Click += _MenuFileNewDefinition_Click;
		//
		// _MenuFileOpen
		//
		_MenuFileOpen.Image = (Image) resources.GetObject("_MenuFileOpen.Image");
		_Locale.SetLocaleSymbol(_MenuFileOpen, "led:menu.file.open");
		_MenuFileOpen.Name = "_MenuFileOpen";
		_MenuFileOpen.ShortcutKeys =  Keys.Control | Keys.O;
		_MenuFileOpen.Size = new Size(180, 22);
		_MenuFileOpen.Text = "&Open...";
		_MenuFileOpen.Click += _MenuFileOpen_Click;
		//
		// _MenuFileRecent
		//
		_Locale.SetLocaleSymbol(_MenuFileRecent, "led:menu.file.recent");
		_MenuFileRecent.Name = "_MenuFileRecent";
		_MenuFileRecent.Size = new Size(180, 22);
		_MenuFileRecent.Text = "&Recent Files";
		//
		// _MenuFileSep1
		//
		_MenuFileSep1.Name = "_MenuFileSep1";
		_MenuFileSep1.Size = new Size(177, 6);
		//
		// _MenuFileSave
		//
		_MenuFileSave.Image = (Image) resources.GetObject("_MenuFileSave.Image");
		_Locale.SetLocaleSymbol(_MenuFileSave, "led:menu.file.save");
		_MenuFileSave.Name = "_MenuFileSave";
		_MenuFileSave.ShortcutKeys =  Keys.Control | Keys.S;
		_MenuFileSave.Size = new Size(180, 22);
		_MenuFileSave.Text = "&Save";
		_MenuFileSave.Click += _MenuFileSave_Click;
		//
		// _MenuFileSaveAs
		//
		_MenuFileSaveAs.Image = (Image) resources.GetObject("_MenuFileSaveAs.Image");
		_Locale.SetLocaleSymbol(_MenuFileSaveAs, "led:menu.file.saveas");
		_MenuFileSaveAs.Name = "_MenuFileSaveAs";
		_MenuFileSaveAs.ShortcutKeys =  Keys.Control | Keys.Shift | Keys.S;
		_MenuFileSaveAs.ShowShortcutKeys = false;
		_MenuFileSaveAs.Size = new Size(180, 22);
		_MenuFileSaveAs.Text = "Save &As...";
		_MenuFileSaveAs.Click += _MenuFileSaveAs_Click;
		//
		// _MenuFileSep2
		//
		_MenuFileSep2.Name = "_MenuFileSep2";
		_MenuFileSep2.Size = new Size(177, 6);
		//
		// _MenuFileClose
		//
		_MenuFileClose.Image = (Image) resources.GetObject("_MenuFileClose.Image");
		_Locale.SetLocaleSymbol(_MenuFileClose, "led:menu.file.close");
		_MenuFileClose.Name = "_MenuFileClose";
		_MenuFileClose.ShortcutKeys =  Keys.Control | Keys.W;
		_MenuFileClose.Size = new Size(180, 22);
		_MenuFileClose.Text = "&Close";
		_MenuFileClose.Click += _MenuFileClose_Click;
		//
		// _MenuEdit
		//
		_MenuEdit.DropDownItems.AddRange(new ToolStripItem[] { _MenuEditUndo, _MenuEditRedo, _MenuEditSep0, _MenuEditFind, _MenuEditFindNext, _MenuEditFindPrevious, _MenuEditReplace, _MenuEditSep2, _MenuEditCut, _MenuEditCopy, _MenuEditPaste, _MenuEditSep1, _MenuEditDelete, _MenuEditSelectAll });
		_Locale.SetLocaleSymbol(_MenuEdit, "led:menu.edit");
		_MenuEdit.Name = "_MenuEdit";
		_MenuEdit.Size = new Size(39, 20);
		_MenuEdit.Text = "&Edit";
		_MenuEdit.DropDownClosed += _MenuEdit_DropDownClosed;
		_MenuEdit.DropDownOpening += _MenuEdit_DropDownOpening;
		//
		// _MenuEditUndo
		//
		_Edit.SetEditCommand(_MenuEditUndo, Spellbook.Controls.EditCommand.UNDO);
		_MenuEditUndo.Image = (Image) resources.GetObject("_MenuEditUndo.Image");
		_Locale.SetLocaleSymbol(_MenuEditUndo, "led:menu.edit.undo");
		_MenuEditUndo.Name = "_MenuEditUndo";
		_MenuEditUndo.ShortcutKeys =  Keys.Control | Keys.Z;
		_MenuEditUndo.Size = new Size(169, 22);
		_MenuEditUndo.Text = "&Undo";
		//
		// _MenuEditRedo
		//
		_Edit.SetEditCommand(_MenuEditRedo, Spellbook.Controls.EditCommand.REDO);
		_MenuEditRedo.Image = (Image) resources.GetObject("_MenuEditRedo.Image");
		_Locale.SetLocaleSymbol(_MenuEditRedo, "led:menu.edit.redo");
		_MenuEditRedo.Name = "_MenuEditRedo";
		_MenuEditRedo.ShortcutKeys =  Keys.Control | Keys.Y;
		_MenuEditRedo.Size = new Size(169, 22);
		_MenuEditRedo.Text = "&Redo";
		//
		// _MenuEditSep0
		//
		_MenuEditSep0.Name = "_MenuEditSep0";
		_MenuEditSep0.Size = new Size(166, 6);
		//
		// _MenuEditCut
		//
		_Edit.SetEditCommand(_MenuEditCut, Spellbook.Controls.EditCommand.CUT);
		_MenuEditCut.Image = (Image) resources.GetObject("_MenuEditCut.Image");
		_Locale.SetLocaleSymbol(_MenuEditCut, "led:menu.edit.cut");
		_MenuEditCut.Name = "_MenuEditCut";
		_MenuEditCut.ShortcutKeys =  Keys.Control | Keys.X;
		_MenuEditCut.Size = new Size(169, 22);
		_MenuEditCut.Text = "Cu&t";
		//
		// _MenuEditCopy
		//
		_Edit.SetEditCommand(_MenuEditCopy, Spellbook.Controls.EditCommand.COPY);
		_MenuEditCopy.Image = (Image) resources.GetObject("_MenuEditCopy.Image");
		_Locale.SetLocaleSymbol(_MenuEditCopy, "led:menu.edit.copy");
		_MenuEditCopy.Name = "_MenuEditCopy";
		_MenuEditCopy.ShortcutKeys =  Keys.Control | Keys.C;
		_MenuEditCopy.Size = new Size(169, 22);
		_MenuEditCopy.Text = "&Copy";
		//
		// _MenuEditPaste
		//
		_Edit.SetEditCommand(_MenuEditPaste, Spellbook.Controls.EditCommand.PASTE);
		_MenuEditPaste.Image = (Image) resources.GetObject("_MenuEditPaste.Image");
		_Locale.SetLocaleSymbol(_MenuEditPaste, "led:menu.edit.paste");
		_MenuEditPaste.Name = "_MenuEditPaste";
		_MenuEditPaste.ShortcutKeys =  Keys.Control | Keys.V;
		_MenuEditPaste.Size = new Size(169, 22);
		_MenuEditPaste.Text = "&Paste";
		//
		// _MenuEditDelete
		//
		_Edit.SetEditCommand(_MenuEditDelete, Spellbook.Controls.EditCommand.DELETE);
		_MenuEditDelete.Image = (Image) resources.GetObject("_MenuEditDelete.Image");
		_Locale.SetLocaleSymbol(_MenuEditDelete, "led:menu.edit.delete");
		_MenuEditDelete.Name = "_MenuEditDelete";
		_MenuEditDelete.ShortcutKeys = Keys.Delete;
		_MenuEditDelete.Size = new Size(169, 22);
		_MenuEditDelete.Text = "&Delete";
		//
		// _MenuEditSep1
		//
		_MenuEditSep1.Name = "_MenuEditSep1";
		_MenuEditSep1.Size = new Size(166, 6);
		//
		// _MenuEditSelectAll
		//
		_Edit.SetEditCommand(_MenuEditSelectAll, Spellbook.Controls.EditCommand.SELECT_ALL);
		_MenuEditSelectAll.Image = (Image) resources.GetObject("_MenuEditSelectAll.Image");
		_Locale.SetLocaleSymbol(_MenuEditSelectAll, "led:menu.edit.selectall");
		_MenuEditSelectAll.Name = "_MenuEditSelectAll";
		_MenuEditSelectAll.ShortcutKeys =  Keys.Control | Keys.A;
		_MenuEditSelectAll.Size = new Size(169, 22);
		_MenuEditSelectAll.Text = "Select &All";
		//
		// _MenuEditSep2
		//
		_MenuEditSep2.Name = "_MenuEditSep2";
		_MenuEditSep2.Size = new Size(166, 6);
		//
		// _MenuEditFind
		//
		_MenuEditFind.Image = (Image) resources.GetObject("_MenuEditFind.Image");
		_Locale.SetLocaleSymbol(_MenuEditFind, "led:menu.edit.find");
		_MenuEditFind.Name = "_MenuEditFind";
		_MenuEditFind.ShortcutKeys =  Keys.Control | Keys.F;
		_MenuEditFind.Size = new Size(169, 22);
		_MenuEditFind.Text = "&Find...";
		_MenuEditFind.Click += _MenuEditFind_Click;
		//
		// _MenuEditFindNext
		//
		_MenuEditFindNext.Image = (Image) resources.GetObject("_MenuEditFindNext.Image");
		_Locale.SetLocaleSymbol(_MenuEditFindNext, "led:menu.edit.findnext");
		_MenuEditFindNext.Name = "_MenuEditFindNext";
		_MenuEditFindNext.ShortcutKeys = Keys.F3;
		_MenuEditFindNext.Size = new Size(169, 22);
		_MenuEditFindNext.Text = "Find &Next";
		_MenuEditFindNext.Click += _MenuEditFindNext_Click;
		//
		// _MenuEditFindPrevious
		//
		_Locale.SetLocaleSymbol(_MenuEditFindPrevious, "led:menu.edit.findprevious");
		_MenuEditFindPrevious.Name = "_MenuEditFindPrevious";
		_MenuEditFindPrevious.ShortcutKeys =  Keys.Shift | Keys.F3;
		_MenuEditFindPrevious.ShowShortcutKeys = false;
		_MenuEditFindPrevious.Size = new Size(169, 22);
		_MenuEditFindPrevious.Text = "Find Pre&vious";
		_MenuEditFindPrevious.Click += _MenuEditFindPrevious_Click;
		//
		// _MenuEditReplace
		//
		_MenuEditReplace.Image = (Image) resources.GetObject("_MenuEditReplace.Image");
		_Locale.SetLocaleSymbol(_MenuEditReplace, "led:menu.edit.replace");
		_MenuEditReplace.Name = "_MenuEditReplace";
		_MenuEditReplace.ShortcutKeys =  Keys.Control | Keys.H;
		_MenuEditReplace.Size = new Size(169, 22);
		_MenuEditReplace.Text = "R&eplace...";
		_MenuEditReplace.Click += _MenuEditReplace_Click;
		//
		// _MenuTools
		//
		_MenuTools.DropDownItems.AddRange(new ToolStripItem[] { _MenuToolsTextEditor, _MenuToolsSort, _MenuToolsImport, _MenuToolsSep1, _MenuToolsSettings });
		_Locale.SetLocaleSymbol(_MenuTools, "led:menu.tools");
		_MenuTools.Name = "_MenuTools";
		_MenuTools.Size = new Size(47, 20);
		_MenuTools.Text = "&Tools";
		//
		// _MenuToolsTextEditor
		//
		_MenuToolsTextEditor.Image = (Image) resources.GetObject("_MenuToolsTextEditor.Image");
		_Locale.SetLocaleSymbol(_MenuToolsTextEditor, "led:menu.tools.texteditor");
		_MenuToolsTextEditor.Name = "_MenuToolsTextEditor";
		_MenuToolsTextEditor.ShortcutKeys =  Keys.Control | Keys.E;
		_MenuToolsTextEditor.Size = new Size(180, 22);
		_MenuToolsTextEditor.Text = "Te&xt Editor...";
		_MenuToolsTextEditor.Click += _MenuToolsTextEditor_Click;
		//
		// _MenuToolsSort
		//
		_MenuToolsSort.Image = (Image) resources.GetObject("_MenuToolsSort.Image");
		_Locale.SetLocaleSymbol(_MenuToolsSort, "led:menu.tools.sort");
		_MenuToolsSort.Name = "_MenuToolsSort";
		_MenuToolsSort.Size = new Size(180, 22);
		_MenuToolsSort.Text = "S&ort";
		_MenuToolsSort.Click += _MenuToolsSort_Click;
		//
		// _MenuToolsImport
		//
		_MenuToolsImport.Image = (Image) resources.GetObject("_MenuToolsImport.Image");
		_Locale.SetLocaleSymbol(_MenuToolsImport, "led:menu.tools.import");
		_MenuToolsImport.Name = "_MenuToolsImport";
		_MenuToolsImport.Size = new Size(180, 22);
		_MenuToolsImport.Text = "&Import...";
		_MenuToolsImport.Click += _MenuToolsImport_Click;
		//
		// _MenuToolsSep1
		//
		_MenuToolsSep1.Name = "_MenuToolsSep1";
		_MenuToolsSep1.Size = new Size(177, 6);
		//
		// _MenuToolsSettings
		//
		_MenuToolsSettings.Image = (Image) resources.GetObject("_MenuToolsSettings.Image");
		_Locale.SetLocaleSymbol(_MenuToolsSettings, "led:menu.tools.settings");
		_MenuToolsSettings.Name = "_MenuToolsSettings";
		_MenuToolsSettings.ShortcutKeys =  Keys.Control | Keys.Alt | Keys.S;
		_MenuToolsSettings.ShowShortcutKeys = false;
		_MenuToolsSettings.Size = new Size(180, 22);
		_MenuToolsSettings.Text = "&Settings...";
		_MenuToolsSettings.Click += _MenuToolsSettings_Click;
		//
		// _MenuWindow
		//
		_MenuWindow.DropDownItems.AddRange(new ToolStripItem[] { _MenuWindowNone });
		_Locale.SetLocaleSymbol(_MenuWindow, "led:menu.window");
		_MenuWindow.Name = "_MenuWindow";
		_MenuWindow.Size = new Size(63, 20);
		_MenuWindow.Text = "&Window";
		_MenuWindow.DropDownOpening += _MenuWindow_DropDownOpening;
		//
		// _MenuWindowNone
		//
		_MenuWindowNone.Enabled = false;
		_MenuWindowNone.Name = "_MenuWindowNone";
		_MenuWindowNone.Size = new Size(180, 22);
		_MenuWindowNone.Text = "(none)";
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
		// _Edit
		//
		_Edit.ContainerControl = this;
		//
		// _Layout
		//
		_Layout.ContainerControl = this;
		//
		// _Recent
		//
		_Recent.MenuItem = _MenuFileRecent;
		_Recent.ItemSelected += _Recent_ItemSelected;
		//
		// _MenuMain
		//
		_MenuMain.Items.AddRange(new ToolStripItem[] { _MenuFile, _MenuEdit, _MenuTools, _MenuWindow, _MenuHelp });
		_MenuMain.Location = new Point(0, 0);
		_MenuMain.Name = "_MenuMain";
		_MenuMain.Size = new Size(784, 24);
		_MenuMain.TabIndex = 0;
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
		// _PanelContent
		//
		_PanelContent.Dock = DockStyle.Fill;
		_PanelContent.Location = new Point(0, 24);
		_PanelContent.Name = "_PanelContent";
		_PanelContent.Size = new Size(784, 515);
		_PanelContent.TabIndex = 1;
		//
		// _StatusMain
		//
		_StatusMain.Items.AddRange(new ToolStripItem[] { _LabelInfo, _LabelProblems, _LabelMessage, _LabelFile });
		_StatusMain.Location = new Point(0, 539);
		_StatusMain.Name = "_StatusMain";
		_StatusMain.ShowItemToolTips = true;
		_StatusMain.Size = new Size(784, 22);
		_StatusMain.TabIndex = 2;
		//
		// _LabelInfo
		//
		_LabelInfo.Name = "_LabelInfo";
		_LabelInfo.Size = new Size(0, 17);
		//
		// _LabelProblems
		//
		_LabelProblems.ForeColor = Color.Firebrick;
		_LabelProblems.Name = "_LabelProblems";
		_LabelProblems.Size = new Size(0, 17);
		//
		// _LabelMessage
		//
		_LabelMessage.Name = "_LabelMessage";
		_LabelMessage.Size = new Size(0, 17);
		//
		// _LabelFile
		//
		_LabelFile.Name = "_LabelFile";
		_LabelFile.Size = new Size(769, 17);
		_LabelFile.Spring = true;
		_LabelFile.TextAlign = ContentAlignment.MiddleRight;
		//
		// _MenuHelp
		//
		_MenuHelp.DropDownItems.AddRange(new ToolStripItem[] { _MenuHelpShowHelp, _MenuHelpAbout });
		_Locale.SetLocaleSymbol(_MenuHelp, "led:menu.help");
		_MenuHelp.Name = "_MenuHelp";
		_MenuHelp.Size = new Size(44, 20);
		_MenuHelp.Text = "&Help";
		//
		// _MenuHelpShowHelp
		//
		_MenuHelpShowHelp.Image = (Image) resources.GetObject("_MenuHelpShowHelp.Image");
		_Locale.SetLocaleSymbol(_MenuHelpShowHelp, "led:menu.help.showhelp");
		_MenuHelpShowHelp.Name = "_MenuHelpShowHelp";
		_MenuHelpShowHelp.ShortcutKeys = Keys.F1;
		_MenuHelpShowHelp.Size = new Size(180, 22);
		_MenuHelpShowHelp.Text = "Show &Help...";
		_MenuHelpShowHelp.Click += _MenuHelpShowHelp_Click;
		//
		// _MenuHelpAbout
		//
		_MenuHelpAbout.Image = (Image) resources.GetObject("_MenuHelpAbout.Image");
		_Locale.SetLocaleSymbol(_MenuHelpAbout, "led:menu.help.about");
		_MenuHelpAbout.Name = "_MenuHelpAbout";
		_MenuHelpAbout.Size = new Size(180, 22);
		_MenuHelpAbout.Text = "&About...";
		_MenuHelpAbout.Click += _MenuHelpAbout_Click;
		//
		// _DocumentWindow
		//
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(784, 561);
		Controls.Add(_PanelContent);
		Controls.Add(_StatusMain);
		Controls.Add(_MenuMain);
		Icon = (Icon) resources.GetObject("$this.Icon");
		MainMenuStrip = _MenuMain;
		MinimumSize = new Size(480, 320);
		Name = "_DocumentWindow";
		Text = "Localization Editor";
		FormClosing += _DocumentWindow_FormClosing;
		Load += _DocumentWindow_Load;
		_MenuMain.ResumeLayout(false);
		_MenuMain.PerformLayout();
		_CmenuText.ResumeLayout(false);
		_StatusMain.ResumeLayout(false);
		_StatusMain.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	protected Robbiblubber.Localization.Controls.LocaleProvider _Locale;
	private Robbiblubber.Spellbook.Controls.Themes.ThemeProvider _ThemeProvider;
	protected Robbiblubber.Spellbook.Controls.EditCommandProvider _Edit;
	protected Robbiblubber.Spellbook.Controls.WindowLayoutProvider _Layout;
	private Robbiblubber.Localization.Editor._RecentFileList _Recent;
	private System.Windows.Forms.MenuStrip _MenuMain;
	private System.Windows.Forms.ToolStripMenuItem _MenuFile;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileNew;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileNewTable;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileNewDefinition;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileOpen;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileRecent;
	private System.Windows.Forms.ToolStripSeparator _MenuFileSep1;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileSave;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileSaveAs;
	private System.Windows.Forms.ToolStripSeparator _MenuFileSep2;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileClose;
	private System.Windows.Forms.ToolStripMenuItem _MenuEdit;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditUndo;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditRedo;
	private System.Windows.Forms.ToolStripSeparator _MenuEditSep0;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditCut;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditCopy;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditPaste;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditDelete;
	private System.Windows.Forms.ToolStripSeparator _MenuEditSep1;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditSelectAll;
	private System.Windows.Forms.ToolStripSeparator _MenuEditSep2;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditFind;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditFindNext;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditFindPrevious;
	private System.Windows.Forms.ToolStripMenuItem _MenuEditReplace;
	private System.Windows.Forms.ToolStripMenuItem _MenuTools;
	private System.Windows.Forms.ToolStripMenuItem _MenuToolsTextEditor;
	private System.Windows.Forms.ToolStripMenuItem _MenuToolsSort;
	private System.Windows.Forms.ToolStripMenuItem _MenuToolsImport;
	private System.Windows.Forms.ToolStripSeparator _MenuToolsSep1;
	private System.Windows.Forms.ToolStripMenuItem _MenuToolsSettings;
	private System.Windows.Forms.ToolStripMenuItem _MenuWindow;
	private System.Windows.Forms.ToolStripMenuItem _MenuWindowNone;
	protected System.Windows.Forms.ContextMenuStrip _CmenuText;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextUndo;
	private System.Windows.Forms.ToolStripSeparator _CmenuTextSep0;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextCut;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextCopy;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextPaste;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextDelete;
	private System.Windows.Forms.ToolStripSeparator _CmenuTextSep1;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextSelectAll;
	protected System.Windows.Forms.Panel _PanelContent;
	private System.Windows.Forms.StatusStrip _StatusMain;
	protected System.Windows.Forms.ToolStripStatusLabel _LabelInfo;
	protected System.Windows.Forms.ToolStripStatusLabel _LabelProblems;
	private System.Windows.Forms.ToolStripStatusLabel _LabelMessage;
	private System.Windows.Forms.ToolStripStatusLabel _LabelFile;
	private ToolStripMenuItem _MenuHelp;
	private ToolStripMenuItem _MenuHelpShowHelp;
	private ToolStripMenuItem _MenuHelpAbout;
}
