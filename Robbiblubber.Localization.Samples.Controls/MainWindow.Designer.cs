namespace Robbiblubber.Localization.Samples.Controls;

partial class MainWindow
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
		this.components = new System.ComponentModel.Container();
		this._Provider = new Robbiblubber.Localization.Controls.LocaleProvider(this.components);
		this._ToolTip = new Robbiblubber.Localization.Controls.LocalizableToolTip(this.components);
		this._MenuMain = new System.Windows.Forms.MenuStrip();
		this._MenuFile = new System.Windows.Forms.ToolStripMenuItem();
		this._MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
		this._MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
		this._MenuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
		this._ToolMain = new System.Windows.Forms.ToolStrip();
		this._ToolSave = new System.Windows.Forms.ToolStripButton();
		this._ToolOpen = new System.Windows.Forms.ToolStripButton();
		this._LabelLocale = new System.Windows.Forms.Label();
		this._SelectorList = new Robbiblubber.Localization.Controls.LocaleSelector();
		this._SelectorButton = new Robbiblubber.Localization.Controls.LocaleSelector();
		this._SelectorDropDown = new Robbiblubber.Localization.Controls.LocaleSelector();
		this._SelectorArrow = new Robbiblubber.Localization.Controls.LocaleSelector();
		this._GroupControls = new System.Windows.Forms.GroupBox();
		this._LabelName = new System.Windows.Forms.Label();
		this._TextName = new System.Windows.Forms.TextBox();
		this._CheckNewsletter = new System.Windows.Forms.CheckBox();
		this._ButtonSend = new System.Windows.Forms.Button();
		this._LabelMissing = new System.Windows.Forms.Label();
		this._LabelList = new System.Windows.Forms.Label();
		this._ListColors = new System.Windows.Forms.ListBox();
		this._LabelCombo = new System.Windows.Forms.Label();
		this._ComboColors = new System.Windows.Forms.ComboBox();
		this._LabelThemedCombo = new System.Windows.Forms.Label();
		this._ThemedComboColors = new Robbiblubber.Spellbook.Controls.ThemedComboBox();
		this._LabelListView = new System.Windows.Forms.Label();
		this._ListProducts = new System.Windows.Forms.ListView();
		this._ColumnProduct = new System.Windows.Forms.ColumnHeader();
		this._ColumnPrice = new System.Windows.Forms.ColumnHeader();
		this._LabelTree = new System.Windows.Forms.Label();
		this._TreeFood = new System.Windows.Forms.TreeView();
		this._LabelStatus = new System.Windows.Forms.Label();
		this._GroupOptions = new System.Windows.Forms.GroupBox();
		this._CheckAutoUpdate = new System.Windows.Forms.CheckBox();
		this._CheckLocalizeItems = new System.Windows.Forms.CheckBox();
		this._CheckRightToLeft = new System.Windows.Forms.CheckBox();
		this._CheckToolTipAutoUpdate = new System.Windows.Forms.CheckBox();
		this._CheckAutoApply = new System.Windows.Forms.CheckBox();
		this._CheckDarkSelectors = new System.Windows.Forms.CheckBox();
		this._CheckPreferFallback = new System.Windows.Forms.CheckBox();
		this._ButtonLocalize = new System.Windows.Forms.Button();
		this._ButtonReload = new System.Windows.Forms.Button();
		this._ButtonDefault = new System.Windows.Forms.Button();
		this._ButtonThread = new System.Windows.Forms.Button();
		this._ButtonIssues = new System.Windows.Forms.Button();
		this._LabelLog = new System.Windows.Forms.Label();
		this._ButtonClearLog = new System.Windows.Forms.Button();
		this._ListLog = new System.Windows.Forms.ListBox();
		this._MenuMain.SuspendLayout();
		this._ToolMain.SuspendLayout();
		this._GroupControls.SuspendLayout();
		this._GroupOptions.SuspendLayout();
		this.SuspendLayout();
		//
		// _Provider
		//
		this._Provider.ContainerControl = this;
		this._Provider.Localized += new System.EventHandler(this._Provider_Localized);
		//
		// _MenuMain
		//
		this._MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this._MenuFile, this._MenuHelp });
		this._MenuMain.Location = new System.Drawing.Point(0, 0);
		this._MenuMain.Name = "_MenuMain";
		this._MenuMain.Size = new System.Drawing.Size(900, 24);
		this._MenuMain.TabIndex = 0;
		//
		// _MenuFile
		//
		this._MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this._MenuFileExit });
		this._Provider.SetLocaleSymbol(this._MenuFile, "sample:menu.file");
		this._MenuFile.Name = "_MenuFile";
		this._MenuFile.Size = new System.Drawing.Size(37, 20);
		this._MenuFile.Text = "&File";
		//
		// _MenuFileExit
		//
		this._Provider.SetLocaleSymbol(this._MenuFileExit, "sample:menu.file.exit");
		this._MenuFileExit.Name = "_MenuFileExit";
		this._MenuFileExit.Size = new System.Drawing.Size(180, 22);
		this._MenuFileExit.Text = "E&xit";
		this._MenuFileExit.Click += new System.EventHandler(this._MenuFileExit_Click);
		//
		// _MenuHelp
		//
		this._MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this._MenuHelpAbout });
		this._Provider.SetLocaleSymbol(this._MenuHelp, "sample:menu.help");
		this._MenuHelp.Name = "_MenuHelp";
		this._MenuHelp.Size = new System.Drawing.Size(44, 20);
		this._MenuHelp.Text = "&Help";
		//
		// _MenuHelpAbout
		//
		this._Provider.SetLocaleSymbol(this._MenuHelpAbout, "sample:menu.help.about");
		this._MenuHelpAbout.Name = "_MenuHelpAbout";
		this._MenuHelpAbout.Size = new System.Drawing.Size(180, 22);
		this._MenuHelpAbout.Text = "&About";
		this._MenuHelpAbout.Click += new System.EventHandler(this._MenuHelpAbout_Click);
		//
		// _ToolMain
		//
		this._ToolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this._ToolSave, this._ToolOpen });
		this._ToolMain.Location = new System.Drawing.Point(0, 24);
		this._ToolMain.Name = "_ToolMain";
		this._ToolMain.Size = new System.Drawing.Size(900, 25);
		this._ToolMain.TabIndex = 1;
		//
		// _ToolSave
		//
		this._ToolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this._Provider.SetLocaleSymbol(this._ToolSave, "sample:tool.save");
		this._ToolSave.Name = "_ToolSave";
		this._ToolSave.Size = new System.Drawing.Size(35, 22);
		this._ToolSave.Text = "Save";
		this._ToolSave.Click += new System.EventHandler(this._ToolSave_Click);
		//
		// _ToolOpen
		//
		this._ToolOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this._Provider.SetLocaleSymbol(this._ToolOpen, "sample:tool.open");
		this._ToolOpen.Name = "_ToolOpen";
		this._ToolOpen.Size = new System.Drawing.Size(40, 22);
		this._ToolOpen.Text = "Open";
		this._ToolOpen.Click += new System.EventHandler(this._ToolOpen_Click);
		//
		// _LabelLocale
		//
		this._LabelLocale.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelLocale, "sample:label.locale");
		this._LabelLocale.Location = new System.Drawing.Point(12, 62);
		this._LabelLocale.Name = "_LabelLocale";
		this._LabelLocale.Size = new System.Drawing.Size(62, 15);
		this._LabelLocale.TabIndex = 2;
		this._LabelLocale.Text = "Language:";
		//
		// _SelectorList
		//
		this._SelectorList.Location = new System.Drawing.Point(142, 58);
		this._SelectorList.Name = "_SelectorList";
		this._SelectorList.Size = new System.Drawing.Size(180, 24);
		this._SelectorList.TabIndex = 3;
		this._SelectorList.SelectedLocaleChanged += new System.EventHandler(this._Selector_SelectedLocaleChanged);
		//
		// _SelectorButton
		//
		this._SelectorButton.Location = new System.Drawing.Point(334, 57);
		this._SelectorButton.Name = "_SelectorButton";
		this._SelectorButton.Size = new System.Drawing.Size(26, 26);
		this._SelectorButton.Style = Robbiblubber.Localization.Controls.LocaleSelectorStyle.BUTTON;
		this._SelectorButton.TabIndex = 4;
		this._SelectorButton.SelectedLocaleChanged += new System.EventHandler(this._Selector_SelectedLocaleChanged);
		//
		// _SelectorDropDown
		//
		this._SelectorDropDown.Location = new System.Drawing.Point(372, 57);
		this._SelectorDropDown.Name = "_SelectorDropDown";
		this._SelectorDropDown.Size = new System.Drawing.Size(42, 26);
		this._SelectorDropDown.Style = Robbiblubber.Localization.Controls.LocaleSelectorStyle.DROPDOWN_BUTTON;
		this._SelectorDropDown.TabIndex = 4;
		this._SelectorDropDown.SelectedLocaleChanged += new System.EventHandler(this._Selector_SelectedLocaleChanged);
		//
		// _SelectorArrow
		//
		this._SelectorArrow.Location = new System.Drawing.Point(426, 57);
		this._SelectorArrow.Name = "_SelectorArrow";
		this._SelectorArrow.Size = new System.Drawing.Size(38, 26);
		this._SelectorArrow.Style = Robbiblubber.Localization.Controls.LocaleSelectorStyle.ARROW_BUTTON;
		this._SelectorArrow.TabIndex = 4;
		this._SelectorArrow.SelectedLocaleChanged += new System.EventHandler(this._Selector_SelectedLocaleChanged);
		//
		// _GroupControls
		//
		this._GroupControls.Controls.Add(this._LabelName);
		this._GroupControls.Controls.Add(this._TextName);
		this._GroupControls.Controls.Add(this._CheckNewsletter);
		this._GroupControls.Controls.Add(this._ButtonSend);
		this._GroupControls.Controls.Add(this._LabelMissing);
		this._GroupControls.Controls.Add(this._LabelList);
		this._GroupControls.Controls.Add(this._ListColors);
		this._GroupControls.Controls.Add(this._LabelCombo);
		this._GroupControls.Controls.Add(this._ComboColors);
		this._GroupControls.Controls.Add(this._LabelThemedCombo);
		this._GroupControls.Controls.Add(this._ThemedComboColors);
		this._GroupControls.Controls.Add(this._LabelListView);
		this._GroupControls.Controls.Add(this._ListProducts);
		this._GroupControls.Controls.Add(this._LabelTree);
		this._GroupControls.Controls.Add(this._TreeFood);
		this._GroupControls.Controls.Add(this._LabelStatus);
		this._Provider.SetLocaleSymbol(this._GroupControls, "sample:group.controls");
		this._GroupControls.Location = new System.Drawing.Point(12, 92);
		this._GroupControls.Name = "_GroupControls";
		this._GroupControls.Size = new System.Drawing.Size(560, 430);
		this._GroupControls.TabIndex = 5;
		this._GroupControls.TabStop = false;
		this._GroupControls.Text = "Localized controls";
		//
		// _LabelName
		//
		this._LabelName.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelName, "sample:label.name");
		this._LabelName.Location = new System.Drawing.Point(12, 29);
		this._LabelName.Name = "_LabelName";
		this._LabelName.Size = new System.Drawing.Size(42, 15);
		this._LabelName.TabIndex = 0;
		this._LabelName.Text = "Name:";
		//
		// _TextName
		//
		this._Provider.SetAccessibleDescriptionLocaleSymbol(this._TextName, "sample:text.name.description");
		this._TextName.AccessibleDescription = "Enter the name of the customer.";
		this._Provider.SetAccessibleNameLocaleSymbol(this._TextName, "sample:text.name.name");
		this._TextName.AccessibleName = "Customer name";
		this._TextName.Location = new System.Drawing.Point(130, 26);
		this._TextName.Name = "_TextName";
		this._TextName.Size = new System.Drawing.Size(200, 23);
		this._TextName.TabIndex = 1;
		//
		// _CheckNewsletter
		//
		this._CheckNewsletter.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._CheckNewsletter, "sample:check.newsletter");
		this._CheckNewsletter.Location = new System.Drawing.Point(130, 55);
		this._CheckNewsletter.Name = "_CheckNewsletter";
		this._CheckNewsletter.Size = new System.Drawing.Size(155, 19);
		this._CheckNewsletter.TabIndex = 2;
		this._CheckNewsletter.Text = "Subscribe to newsletter";
		this._CheckNewsletter.UseVisualStyleBackColor = true;
		//
		// _ButtonSend
		//
		this._Provider.SetLocaleSymbol(this._ButtonSend, "sample:button.send");
		this._ButtonSend.Location = new System.Drawing.Point(130, 80);
		this._ButtonSend.Name = "_ButtonSend";
		this._ButtonSend.Size = new System.Drawing.Size(100, 27);
		this._ButtonSend.TabIndex = 3;
		this._ButtonSend.Text = "Send";
		this._ToolTip.SetLocaleSymbol(this._ButtonSend, "sample:button.send.tip");
		this._ToolTip.SetToolTip(this._ButtonSend, "Sends the form.");
		this._ButtonSend.UseVisualStyleBackColor = true;
		this._ButtonSend.Click += new System.EventHandler(this._ButtonSend_Click);
		//
		// _LabelMissing
		//
		this._LabelMissing.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelMissing, "sample:label.missing");
		this._LabelMissing.Location = new System.Drawing.Point(130, 114);
		this._LabelMissing.Name = "_LabelMissing";
		this._LabelMissing.Size = new System.Drawing.Size(232, 15);
		this._LabelMissing.TabIndex = 4;
		this._LabelMissing.Text = "This text has no translation (default text).";
		//
		// _LabelList
		//
		this._LabelList.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelList, "sample:label.list");
		this._LabelList.Location = new System.Drawing.Point(12, 143);
		this._LabelList.Name = "_LabelList";
		this._LabelList.Size = new System.Drawing.Size(53, 15);
		this._LabelList.TabIndex = 5;
		this._LabelList.Text = "List box:";
		//
		// _ListColors
		//
		this._ListColors.FormattingEnabled = true;
		this._ListColors.ItemHeight = 15;
		this._ListColors.Location = new System.Drawing.Point(130, 140);
		this._ListColors.Name = "_ListColors";
		this._ListColors.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
		this._ListColors.Size = new System.Drawing.Size(150, 64);
		this._ListColors.TabIndex = 6;
		this._ListColors.SelectedIndexChanged += new System.EventHandler(this._ListColors_SelectedIndexChanged);
		//
		// _LabelCombo
		//
		this._LabelCombo.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelCombo, "sample:label.combo");
		this._LabelCombo.Location = new System.Drawing.Point(12, 215);
		this._LabelCombo.Name = "_LabelCombo";
		this._LabelCombo.Size = new System.Drawing.Size(70, 15);
		this._LabelCombo.TabIndex = 7;
		this._LabelCombo.Text = "Combo box:";
		//
		// _ComboColors
		//
		this._ComboColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this._ComboColors.FormattingEnabled = true;
		this._ComboColors.Location = new System.Drawing.Point(130, 212);
		this._ComboColors.Name = "_ComboColors";
		this._ComboColors.Size = new System.Drawing.Size(150, 23);
		this._ComboColors.TabIndex = 8;
		//
		// _LabelThemedCombo
		//
		this._LabelThemedCombo.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelThemedCombo, "sample:label.themedcombo");
		this._LabelThemedCombo.Location = new System.Drawing.Point(12, 246);
		this._LabelThemedCombo.Name = "_LabelThemedCombo";
		this._LabelThemedCombo.Size = new System.Drawing.Size(96, 15);
		this._LabelThemedCombo.TabIndex = 9;
		this._LabelThemedCombo.Text = "Themed combo box:";
		//
		// _ThemedComboColors
		//
		this._ThemedComboColors.Location = new System.Drawing.Point(130, 242);
		this._ThemedComboColors.Name = "_ThemedComboColors";
		this._ThemedComboColors.Size = new System.Drawing.Size(150, 24);
		this._ThemedComboColors.TabIndex = 10;
		//
		// _LabelListView
		//
		this._LabelListView.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelListView, "sample:label.listview");
		this._LabelListView.Location = new System.Drawing.Point(300, 140);
		this._LabelListView.Name = "_LabelListView";
		this._LabelListView.Size = new System.Drawing.Size(56, 15);
		this._LabelListView.TabIndex = 11;
		this._LabelListView.Text = "List view:";
		//
		// _ListProducts
		//
		this._ListProducts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this._ColumnProduct, this._ColumnPrice });
		this._ListProducts.FullRowSelect = true;
		this._ListProducts.Location = new System.Drawing.Point(300, 160);
		this._ListProducts.Name = "_ListProducts";
		this._ListProducts.Size = new System.Drawing.Size(245, 106);
		this._ListProducts.TabIndex = 12;
		this._ListProducts.UseCompatibleStateImageBehavior = false;
		this._ListProducts.View = System.Windows.Forms.View.Details;
		//
		// _ColumnProduct
		//
		this._Provider.SetLocaleSymbol(this._ColumnProduct, "sample:column.product");
		this._ColumnProduct.Text = "Product";
		this._ColumnProduct.Width = 140;
		//
		// _ColumnPrice
		//
		this._Provider.SetLocaleSymbol(this._ColumnPrice, "sample:column.price");
		this._ColumnPrice.Text = "Price";
		this._ColumnPrice.Width = 80;
		//
		// _LabelTree
		//
		this._LabelTree.AutoSize = true;
		this._Provider.SetLocaleSymbol(this._LabelTree, "sample:label.tree");
		this._LabelTree.Location = new System.Drawing.Point(300, 278);
		this._LabelTree.Name = "_LabelTree";
		this._LabelTree.Size = new System.Drawing.Size(59, 15);
		this._LabelTree.TabIndex = 13;
		this._LabelTree.Text = "Tree view:";
		//
		// _TreeFood
		//
		this._TreeFood.Location = new System.Drawing.Point(300, 298);
		this._TreeFood.Name = "_TreeFood";
		this._TreeFood.Size = new System.Drawing.Size(245, 116);
		this._TreeFood.TabIndex = 14;
		//
		// _LabelStatus
		//
		this._LabelStatus.AutoSize = true;
		this._LabelStatus.Location = new System.Drawing.Point(12, 399);
		this._LabelStatus.Name = "_LabelStatus";
		this._LabelStatus.Size = new System.Drawing.Size(39, 15);
		this._LabelStatus.TabIndex = 15;
		this._LabelStatus.Text = "Status";
		//
		// _GroupOptions
		//
		this._GroupOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this._GroupOptions.Controls.Add(this._CheckAutoUpdate);
		this._GroupOptions.Controls.Add(this._CheckLocalizeItems);
		this._GroupOptions.Controls.Add(this._CheckRightToLeft);
		this._GroupOptions.Controls.Add(this._CheckToolTipAutoUpdate);
		this._GroupOptions.Controls.Add(this._CheckAutoApply);
		this._GroupOptions.Controls.Add(this._CheckDarkSelectors);
		this._GroupOptions.Controls.Add(this._CheckPreferFallback);
		this._GroupOptions.Controls.Add(this._ButtonLocalize);
		this._GroupOptions.Controls.Add(this._ButtonReload);
		this._GroupOptions.Controls.Add(this._ButtonDefault);
		this._GroupOptions.Controls.Add(this._ButtonThread);
		this._GroupOptions.Controls.Add(this._ButtonIssues);
		this._GroupOptions.Location = new System.Drawing.Point(584, 92);
		this._GroupOptions.Name = "_GroupOptions";
		this._GroupOptions.Size = new System.Drawing.Size(304, 430);
		this._GroupOptions.TabIndex = 6;
		this._GroupOptions.TabStop = false;
		this._GroupOptions.Text = "Options (not localized)";
		//
		// _CheckAutoUpdate
		//
		this._CheckAutoUpdate.AutoSize = true;
		this._CheckAutoUpdate.Checked = true;
		this._CheckAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
		this._CheckAutoUpdate.Location = new System.Drawing.Point(12, 26);
		this._CheckAutoUpdate.Name = "_CheckAutoUpdate";
		this._CheckAutoUpdate.Size = new System.Drawing.Size(150, 19);
		this._CheckAutoUpdate.TabIndex = 0;
		this._CheckAutoUpdate.Text = "Provider: AutoUpdate";
		this._CheckAutoUpdate.UseVisualStyleBackColor = true;
		this._CheckAutoUpdate.CheckedChanged += new System.EventHandler(this._CheckAutoUpdate_CheckedChanged);
		//
		// _CheckLocalizeItems
		//
		this._CheckLocalizeItems.AutoSize = true;
		this._CheckLocalizeItems.Checked = true;
		this._CheckLocalizeItems.CheckState = System.Windows.Forms.CheckState.Checked;
		this._CheckLocalizeItems.Location = new System.Drawing.Point(12, 51);
		this._CheckLocalizeItems.Name = "_CheckLocalizeItems";
		this._CheckLocalizeItems.Size = new System.Drawing.Size(155, 19);
		this._CheckLocalizeItems.TabIndex = 1;
		this._CheckLocalizeItems.Text = "Provider: LocalizeItems";
		this._CheckLocalizeItems.UseVisualStyleBackColor = true;
		this._CheckLocalizeItems.CheckedChanged += new System.EventHandler(this._CheckLocalizeItems_CheckedChanged);
		//
		// _CheckRightToLeft
		//
		this._CheckRightToLeft.AutoSize = true;
		this._CheckRightToLeft.Location = new System.Drawing.Point(12, 76);
		this._CheckRightToLeft.Name = "_CheckRightToLeft";
		this._CheckRightToLeft.Size = new System.Drawing.Size(175, 19);
		this._CheckRightToLeft.TabIndex = 2;
		this._CheckRightToLeft.Text = "Provider: ApplyRightToLeft";
		this._CheckRightToLeft.UseVisualStyleBackColor = true;
		this._CheckRightToLeft.CheckedChanged += new System.EventHandler(this._CheckRightToLeft_CheckedChanged);
		//
		// _CheckToolTipAutoUpdate
		//
		this._CheckToolTipAutoUpdate.AutoSize = true;
		this._CheckToolTipAutoUpdate.Checked = true;
		this._CheckToolTipAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
		this._CheckToolTipAutoUpdate.Location = new System.Drawing.Point(12, 101);
		this._CheckToolTipAutoUpdate.Name = "_CheckToolTipAutoUpdate";
		this._CheckToolTipAutoUpdate.Size = new System.Drawing.Size(150, 19);
		this._CheckToolTipAutoUpdate.TabIndex = 3;
		this._CheckToolTipAutoUpdate.Text = "Tool tip: AutoUpdate";
		this._CheckToolTipAutoUpdate.UseVisualStyleBackColor = true;
		this._CheckToolTipAutoUpdate.CheckedChanged += new System.EventHandler(this._CheckToolTipAutoUpdate_CheckedChanged);
		//
		// _CheckAutoApply
		//
		this._CheckAutoApply.AutoSize = true;
		this._CheckAutoApply.Checked = true;
		this._CheckAutoApply.CheckState = System.Windows.Forms.CheckState.Checked;
		this._CheckAutoApply.Location = new System.Drawing.Point(12, 126);
		this._CheckAutoApply.Name = "_CheckAutoApply";
		this._CheckAutoApply.Size = new System.Drawing.Size(150, 19);
		this._CheckAutoApply.TabIndex = 4;
		this._CheckAutoApply.Text = "Selectors: AutoApply";
		this._CheckAutoApply.UseVisualStyleBackColor = true;
		this._CheckAutoApply.CheckedChanged += new System.EventHandler(this._CheckAutoApply_CheckedChanged);
		//
		// _CheckDarkSelectors
		//
		this._CheckDarkSelectors.AutoSize = true;
		this._CheckDarkSelectors.Location = new System.Drawing.Point(170, 126);
		this._CheckDarkSelectors.Name = "_CheckDarkSelectors";
		this._CheckDarkSelectors.Size = new System.Drawing.Size(110, 19);
		this._CheckDarkSelectors.TabIndex = 4;
		this._CheckDarkSelectors.Text = "Selectors: dark";
		this._CheckDarkSelectors.UseVisualStyleBackColor = true;
		this._CheckDarkSelectors.CheckedChanged += new System.EventHandler(this._CheckDarkSelectors_CheckedChanged);
		//
		// _CheckPreferFallback
		//
		this._CheckPreferFallback.AutoSize = true;
		this._CheckPreferFallback.Location = new System.Drawing.Point(12, 151);
		this._CheckPreferFallback.Name = "_CheckPreferFallback";
		this._CheckPreferFallback.Size = new System.Drawing.Size(160, 19);
		this._CheckPreferFallback.TabIndex = 5;
		this._CheckPreferFallback.Text = "Locale.PreferFallback";
		this._CheckPreferFallback.UseVisualStyleBackColor = true;
		this._CheckPreferFallback.CheckedChanged += new System.EventHandler(this._CheckPreferFallback_CheckedChanged);
		//
		// _ButtonLocalize
		//
		this._ButtonLocalize.Location = new System.Drawing.Point(12, 190);
		this._ButtonLocalize.Name = "_ButtonLocalize";
		this._ButtonLocalize.Size = new System.Drawing.Size(280, 27);
		this._ButtonLocalize.TabIndex = 6;
		this._ButtonLocalize.Text = "Localize now";
		this._ButtonLocalize.UseVisualStyleBackColor = true;
		this._ButtonLocalize.Click += new System.EventHandler(this._ButtonLocalize_Click);
		//
		// _ButtonReload
		//
		this._ButtonReload.Location = new System.Drawing.Point(12, 223);
		this._ButtonReload.Name = "_ButtonReload";
		this._ButtonReload.Size = new System.Drawing.Size(280, 27);
		this._ButtonReload.TabIndex = 7;
		this._ButtonReload.Text = "Reload locales";
		this._ButtonReload.UseVisualStyleBackColor = true;
		this._ButtonReload.Click += new System.EventHandler(this._ButtonReload_Click);
		//
		// _ButtonDefault
		//
		this._ButtonDefault.Location = new System.Drawing.Point(12, 256);
		this._ButtonDefault.Name = "_ButtonDefault";
		this._ButtonDefault.Size = new System.Drawing.Size(280, 27);
		this._ButtonDefault.TabIndex = 8;
		this._ButtonDefault.Text = "Select default locale";
		this._ButtonDefault.UseVisualStyleBackColor = true;
		this._ButtonDefault.Click += new System.EventHandler(this._ButtonDefault_Click);
		//
		// _ButtonThread
		//
		this._ButtonThread.Location = new System.Drawing.Point(12, 289);
		this._ButtonThread.Name = "_ButtonThread";
		this._ButtonThread.Size = new System.Drawing.Size(280, 27);
		this._ButtonThread.TabIndex = 9;
		this._ButtonThread.Text = "Next locale from background thread";
		this._ButtonThread.UseVisualStyleBackColor = true;
		this._ButtonThread.Click += new System.EventHandler(this._ButtonThread_Click);
		//
		// _ButtonIssues
		//
		this._ButtonIssues.Location = new System.Drawing.Point(12, 322);
		this._ButtonIssues.Name = "_ButtonIssues";
		this._ButtonIssues.Size = new System.Drawing.Size(280, 27);
		this._ButtonIssues.TabIndex = 10;
		this._ButtonIssues.Text = "Log locales and issues";
		this._ButtonIssues.UseVisualStyleBackColor = true;
		this._ButtonIssues.Click += new System.EventHandler(this._ButtonIssues_Click);
		//
		// _LabelLog
		//
		this._LabelLog.AutoSize = true;
		this._LabelLog.Location = new System.Drawing.Point(12, 535);
		this._LabelLog.Name = "_LabelLog";
		this._LabelLog.Size = new System.Drawing.Size(27, 15);
		this._LabelLog.TabIndex = 7;
		this._LabelLog.Text = "Log";
		//
		// _ButtonClearLog
		//
		this._ButtonClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		this._ButtonClearLog.Location = new System.Drawing.Point(788, 529);
		this._ButtonClearLog.Name = "_ButtonClearLog";
		this._ButtonClearLog.Size = new System.Drawing.Size(100, 27);
		this._ButtonClearLog.TabIndex = 8;
		this._ButtonClearLog.Text = "Clear log";
		this._ButtonClearLog.UseVisualStyleBackColor = true;
		this._ButtonClearLog.Click += new System.EventHandler(this._ButtonClearLog_Click);
		//
		// _ListLog
		//
		this._ListLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this._ListLog.Font = new System.Drawing.Font("Consolas", 9F);
		this._ListLog.FormattingEnabled = true;
		this._ListLog.HorizontalScrollbar = true;
		this._ListLog.IntegralHeight = false;
		this._ListLog.ItemHeight = 14;
		this._ListLog.Location = new System.Drawing.Point(12, 560);
		this._ListLog.Name = "_ListLog";
		this._ListLog.Size = new System.Drawing.Size(876, 128);
		this._ListLog.TabIndex = 9;
		//
		// MainWindow
		//
		this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(900, 700);
		this.Controls.Add(this._ListLog);
		this.Controls.Add(this._ButtonClearLog);
		this.Controls.Add(this._LabelLog);
		this.Controls.Add(this._GroupOptions);
		this.Controls.Add(this._GroupControls);
		this.Controls.Add(this._SelectorArrow);
		this.Controls.Add(this._SelectorDropDown);
		this.Controls.Add(this._SelectorButton);
		this.Controls.Add(this._SelectorList);
		this.Controls.Add(this._LabelLocale);
		this.Controls.Add(this._ToolMain);
		this.Controls.Add(this._MenuMain);
		this._Provider.SetLocaleSymbol(this, "sample:title");
		this.MainMenuStrip = this._MenuMain;
		this.MinimumSize = new System.Drawing.Size(916, 600);
		this.Name = "MainWindow";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Localization Controls Sample";
		this._MenuMain.ResumeLayout(false);
		this._MenuMain.PerformLayout();
		this._ToolMain.ResumeLayout(false);
		this._ToolMain.PerformLayout();
		this._GroupControls.ResumeLayout(false);
		this._GroupControls.PerformLayout();
		this._GroupOptions.ResumeLayout(false);
		this._GroupOptions.PerformLayout();
		this.ResumeLayout(false);
		this.PerformLayout();
	}

	#endregion

	private Robbiblubber.Localization.Controls.LocaleProvider _Provider;
	private Robbiblubber.Localization.Controls.LocalizableToolTip _ToolTip;
	private System.Windows.Forms.MenuStrip _MenuMain;
	private System.Windows.Forms.ToolStripMenuItem _MenuFile;
	private System.Windows.Forms.ToolStripMenuItem _MenuFileExit;
	private System.Windows.Forms.ToolStripMenuItem _MenuHelp;
	private System.Windows.Forms.ToolStripMenuItem _MenuHelpAbout;
	private System.Windows.Forms.ToolStrip _ToolMain;
	private System.Windows.Forms.ToolStripButton _ToolSave;
	private System.Windows.Forms.ToolStripButton _ToolOpen;
	private System.Windows.Forms.Label _LabelLocale;
	private Robbiblubber.Localization.Controls.LocaleSelector _SelectorList;
	private Robbiblubber.Localization.Controls.LocaleSelector _SelectorButton;
	private Robbiblubber.Localization.Controls.LocaleSelector _SelectorDropDown;
	private Robbiblubber.Localization.Controls.LocaleSelector _SelectorArrow;
	private System.Windows.Forms.GroupBox _GroupControls;
	private System.Windows.Forms.Label _LabelName;
	private System.Windows.Forms.TextBox _TextName;
	private System.Windows.Forms.CheckBox _CheckNewsletter;
	private System.Windows.Forms.Button _ButtonSend;
	private System.Windows.Forms.Label _LabelMissing;
	private System.Windows.Forms.Label _LabelList;
	private System.Windows.Forms.ListBox _ListColors;
	private System.Windows.Forms.Label _LabelCombo;
	private System.Windows.Forms.ComboBox _ComboColors;
	private System.Windows.Forms.Label _LabelThemedCombo;
	private Robbiblubber.Spellbook.Controls.ThemedComboBox _ThemedComboColors;
	private System.Windows.Forms.Label _LabelListView;
	private System.Windows.Forms.ListView _ListProducts;
	private System.Windows.Forms.ColumnHeader _ColumnProduct;
	private System.Windows.Forms.ColumnHeader _ColumnPrice;
	private System.Windows.Forms.Label _LabelTree;
	private System.Windows.Forms.TreeView _TreeFood;
	private System.Windows.Forms.Label _LabelStatus;
	private System.Windows.Forms.GroupBox _GroupOptions;
	private System.Windows.Forms.CheckBox _CheckAutoUpdate;
	private System.Windows.Forms.CheckBox _CheckLocalizeItems;
	private System.Windows.Forms.CheckBox _CheckRightToLeft;
	private System.Windows.Forms.CheckBox _CheckToolTipAutoUpdate;
	private System.Windows.Forms.CheckBox _CheckAutoApply;
	private System.Windows.Forms.CheckBox _CheckDarkSelectors;
	private System.Windows.Forms.CheckBox _CheckPreferFallback;
	private System.Windows.Forms.Button _ButtonLocalize;
	private System.Windows.Forms.Button _ButtonReload;
	private System.Windows.Forms.Button _ButtonDefault;
	private System.Windows.Forms.Button _ButtonThread;
	private System.Windows.Forms.Button _ButtonIssues;
	private System.Windows.Forms.Label _LabelLog;
	private System.Windows.Forms.Button _ButtonClearLog;
	private System.Windows.Forms.ListBox _ListLog;
}
