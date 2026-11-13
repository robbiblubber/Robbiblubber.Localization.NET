namespace Robbiblubber.Localization.Editor;

partial class _LocaleDefinitionWindow
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_LocaleDefinitionWindow));
		_TableFields = new TableLayoutPanel();
		_LabelKey = new Label();
		_TextKey = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_LabelBase = new Label();
		_ComboBase = new ComboBox();
		_LabelName = new Label();
		_TextName = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_CheckDefault = new Robbiblubber.Spellbook.Controls.ThemedCheckBox();
		_CheckRightToLeft = new Robbiblubber.Spellbook.Controls.ThemedCheckBox();
		_LabelSmall = new Label();
		_FlowSmall = new FlowLayoutPanel();
		_IconSmall = new _IconBox();
		_CmenuIcon = new ContextMenuStrip(components);
		_CmenuIconCut = new ToolStripMenuItem();
		_CmenuIconCopy = new ToolStripMenuItem();
		_CmenuIconPaste = new ToolStripMenuItem();
		_CmenuIconDelete = new ToolStripMenuItem();
		_CmenuIconSep0 = new ToolStripSeparator();
		_CmenuIconLoad = new ToolStripMenuItem();
		_LabelSmallSize = new Label();
		_LabelLarge = new Label();
		_FlowLarge = new FlowLayoutPanel();
		_IconLarge = new _IconBox();
		_LabelLargeSize = new Label();
		_PanelContent.SuspendLayout();
		_TableFields.SuspendLayout();
		_FlowSmall.SuspendLayout();
		_CmenuIcon.SuspendLayout();
		_FlowLarge.SuspendLayout();
		SuspendLayout();
		//
		// _Layout
		//
		_Layout.LayoutName = "definition";
		//
		// _PanelContent
		//
		_PanelContent.Controls.Add(_TableFields);
		_PanelContent.Size = new Size(544, 305);
		//
		// _TableFields
		//
		_TableFields.AutoSize = true;
		_TableFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		_TableFields.ColumnCount = 2;
		_TableFields.ColumnStyles.Add(new ColumnStyle());
		_TableFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
		_TableFields.Controls.Add(_LabelKey, 0, 0);
		_TableFields.Controls.Add(_TextKey, 1, 0);
		_TableFields.Controls.Add(_LabelBase, 0, 1);
		_TableFields.Controls.Add(_ComboBase, 1, 1);
		_TableFields.Controls.Add(_LabelName, 0, 2);
		_TableFields.Controls.Add(_TextName, 1, 2);
		_TableFields.Controls.Add(_CheckDefault, 1, 3);
		_TableFields.Controls.Add(_CheckRightToLeft, 1, 4);
		_TableFields.Controls.Add(_LabelSmall, 0, 5);
		_TableFields.Controls.Add(_FlowSmall, 1, 5);
		_TableFields.Controls.Add(_LabelLarge, 0, 6);
		_TableFields.Controls.Add(_FlowLarge, 1, 6);
		_TableFields.Dock = DockStyle.Top;
		_TableFields.Location = new Point(0, 0);
		_TableFields.Name = "_TableFields";
		_TableFields.Padding = new Padding(6, 6, 6, 3);
		_TableFields.RowCount = 7;
		_TableFields.RowStyles.Add(new RowStyle());
		_TableFields.RowStyles.Add(new RowStyle());
		_TableFields.RowStyles.Add(new RowStyle());
		_TableFields.RowStyles.Add(new RowStyle());
		_TableFields.RowStyles.Add(new RowStyle());
		_TableFields.RowStyles.Add(new RowStyle());
		_TableFields.RowStyles.Add(new RowStyle());
		_TableFields.Size = new Size(544, 266);
		_TableFields.TabIndex = 0;
		//
		// _LabelKey
		//
		_LabelKey.Anchor = AnchorStyles.Left;
		_LabelKey.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelKey, "led:definition.key");
		_LabelKey.Location = new Point(9, 13);
		_LabelKey.Name = "_LabelKey";
		_LabelKey.Size = new Size(29, 15);
		_LabelKey.TabIndex = 0;
		_LabelKey.Text = "&Key:";
		//
		// _TextKey
		//
		_TextKey.ContextMenuStrip = _CmenuText;
		_TextKey.Dock = DockStyle.Fill;
		_TextKey.Location = new Point(80, 9);
		_TextKey.Name = "_TextKey";
		_TextKey.Size = new Size(455, 23);
		_TextKey.TabIndex = 1;
		_TextKey.TextChanged += _Field_Changed;
		//
		// _LabelBase
		//
		_LabelBase.Anchor = AnchorStyles.Left;
		_LabelBase.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelBase, "led:definition.base");
		_LabelBase.Location = new Point(9, 42);
		_LabelBase.Name = "_LabelBase";
		_LabelBase.Size = new Size(55, 15);
		_LabelBase.TabIndex = 2;
		_LabelBase.Text = "&Base key:";
		//
		// _ComboBase
		//
		_ComboBase.ContextMenuStrip = _CmenuText;
		_ComboBase.Dock = DockStyle.Fill;
		_ComboBase.Location = new Point(80, 38);
		_ComboBase.Name = "_ComboBase";
		_ComboBase.Size = new Size(455, 23);
		_ComboBase.TabIndex = 3;
		_ComboBase.TextChanged += _Field_Changed;
		//
		// _LabelName
		//
		_LabelName.Anchor = AnchorStyles.Left;
		_LabelName.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelName, "led:definition.name");
		_LabelName.Location = new Point(9, 71);
		_LabelName.Name = "_LabelName";
		_LabelName.Size = new Size(42, 15);
		_LabelName.TabIndex = 4;
		_LabelName.Text = "&Name:";
		//
		// _TextName
		//
		_TextName.ContextMenuStrip = _CmenuText;
		_TextName.Dock = DockStyle.Fill;
		_TextName.Location = new Point(80, 67);
		_TextName.Name = "_TextName";
		_TextName.Size = new Size(455, 23);
		_TextName.TabIndex = 5;
		_TextName.TextChanged += _Field_Changed;
		//
		// _CheckDefault
		//
		_CheckDefault.AutoSize = true;
		_Locale.SetLocaleSymbol(_CheckDefault, "led:definition.default");
		_CheckDefault.Location = new Point(80, 98);
		_CheckDefault.Margin = new Padding(3, 5, 3, 3);
		_CheckDefault.Name = "_CheckDefault";
		_CheckDefault.Size = new Size(98, 19);
		_CheckDefault.TabIndex = 6;
		_CheckDefault.Text = "&Default locale";
		_CheckDefault.UseVisualStyleBackColor = true;
		_CheckDefault.CheckedChanged += _Field_Changed;
		//
		// _CheckRightToLeft
		//
		_CheckRightToLeft.AutoSize = true;
		_Locale.SetLocaleSymbol(_CheckRightToLeft, "led:definition.rtl");
		_CheckRightToLeft.Location = new Point(80, 125);
		_CheckRightToLeft.Margin = new Padding(3, 5, 3, 3);
		_CheckRightToLeft.Name = "_CheckRightToLeft";
		_CheckRightToLeft.Size = new Size(88, 19);
		_CheckRightToLeft.TabIndex = 7;
		_CheckRightToLeft.Text = "&Right to left";
		_CheckRightToLeft.UseVisualStyleBackColor = true;
		_CheckRightToLeft.CheckedChanged += _Field_Changed;
		//
		// _LabelSmall
		//
		_LabelSmall.Anchor = AnchorStyles.Left;
		_LabelSmall.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelSmall, "led:definition.small");
		_LabelSmall.Location = new Point(9, 168);
		_LabelSmall.Name = "_LabelSmall";
		_LabelSmall.Size = new Size(65, 15);
		_LabelSmall.TabIndex = 8;
		_LabelSmall.Text = "&Small icon:";
		//
		// _FlowSmall
		//
		_FlowSmall.AutoSize = true;
		_FlowSmall.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		_FlowSmall.Controls.Add(_IconSmall);
		_FlowSmall.Controls.Add(_LabelSmallSize);
		_FlowSmall.Location = new Point(77, 147);
		_FlowSmall.Margin = new Padding(0);
		_FlowSmall.Name = "_FlowSmall";
		_FlowSmall.Size = new Size(67, 58);
		_FlowSmall.TabIndex = 9;
		_FlowSmall.WrapContents = false;
		//
		// _IconSmall
		//
		_IconSmall.AllowDrop = true;
		_IconSmall.BackColor = SystemColors.Window;
		_IconSmall.ContextMenuStrip = _CmenuIcon;
		_IconSmall.Location = new Point(3, 3);
		_IconSmall.Name = "_IconSmall";
		_IconSmall.Size = new Size(52, 52);
		_IconSmall.TabIndex = 0;
		_IconSmall.DataChanged += _Field_Changed;
		_IconSmall.LoadRequested += _Icon_LoadRequested;
		//
		// _CmenuIcon
		//
		_CmenuIcon.Items.AddRange(new ToolStripItem[] { _CmenuIconCut, _CmenuIconCopy, _CmenuIconPaste, _CmenuIconDelete, _CmenuIconSep0, _CmenuIconLoad });
		_CmenuIcon.Name = "_CmenuIcon";
		_CmenuIcon.Size = new Size(145, 120);
		//
		// _CmenuIconCut
		//
		_Edit.SetEditCommand(_CmenuIconCut, Spellbook.Controls.EditCommand.CUT);
		_Locale.SetLocaleSymbol(_CmenuIconCut, "led:menu.edit.cut");
		_CmenuIconCut.Name = "_CmenuIconCut";
		_CmenuIconCut.ShortcutKeyDisplayString = "Ctrl+X";
		_CmenuIconCut.Size = new Size(144, 22);
		_CmenuIconCut.Text = "Cu&t";
		//
		// _CmenuIconCopy
		//
		_Edit.SetEditCommand(_CmenuIconCopy, Spellbook.Controls.EditCommand.COPY);
		_Locale.SetLocaleSymbol(_CmenuIconCopy, "led:menu.edit.copy");
		_CmenuIconCopy.Name = "_CmenuIconCopy";
		_CmenuIconCopy.ShortcutKeyDisplayString = "Ctrl+C";
		_CmenuIconCopy.Size = new Size(144, 22);
		_CmenuIconCopy.Text = "&Copy";
		//
		// _CmenuIconPaste
		//
		_Edit.SetEditCommand(_CmenuIconPaste, Spellbook.Controls.EditCommand.PASTE);
		_Locale.SetLocaleSymbol(_CmenuIconPaste, "led:menu.edit.paste");
		_CmenuIconPaste.Name = "_CmenuIconPaste";
		_CmenuIconPaste.ShortcutKeyDisplayString = "Ctrl+V";
		_CmenuIconPaste.Size = new Size(144, 22);
		_CmenuIconPaste.Text = "&Paste";
		//
		// _CmenuIconDelete
		//
		_Edit.SetEditCommand(_CmenuIconDelete, Spellbook.Controls.EditCommand.DELETE);
		_Locale.SetLocaleSymbol(_CmenuIconDelete, "led:menu.edit.delete");
		_CmenuIconDelete.Name = "_CmenuIconDelete";
		_CmenuIconDelete.ShortcutKeyDisplayString = "Del";
		_CmenuIconDelete.Size = new Size(144, 22);
		_CmenuIconDelete.Text = "&Delete";
		//
		// _CmenuIconSep0
		//
		_CmenuIconSep0.Name = "_CmenuIconSep0";
		_CmenuIconSep0.Size = new Size(141, 6);
		//
		// _CmenuIconLoad
		//
		_Locale.SetLocaleSymbol(_CmenuIconLoad, "led:definition.icon.load");
		_CmenuIconLoad.Name = "_CmenuIconLoad";
		_CmenuIconLoad.Size = new Size(144, 22);
		_CmenuIconLoad.Text = "&Load...";
		_CmenuIconLoad.Click += _CmenuIconLoad_Click;
		//
		// _LabelSmallSize
		//
		_LabelSmallSize.Anchor = AnchorStyles.Left;
		_LabelSmallSize.AutoSize = true;
		_LabelSmallSize.ForeColor = SystemColors.GrayText;
		_LabelSmallSize.Location = new Point(64, 21);
		_LabelSmallSize.Margin = new Padding(6, 0, 3, 0);
		_LabelSmallSize.Name = "_LabelSmallSize";
		_LabelSmallSize.Size = new Size(0, 15);
		_LabelSmallSize.TabIndex = 1;
		//
		// _LabelLarge
		//
		_LabelLarge.Anchor = AnchorStyles.Left;
		_LabelLarge.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelLarge, "led:definition.large");
		_LabelLarge.Location = new Point(9, 226);
		_LabelLarge.Name = "_LabelLarge";
		_LabelLarge.Size = new Size(65, 15);
		_LabelLarge.TabIndex = 10;
		_LabelLarge.Text = "&Large icon:";
		//
		// _FlowLarge
		//
		_FlowLarge.AutoSize = true;
		_FlowLarge.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		_FlowLarge.Controls.Add(_IconLarge);
		_FlowLarge.Controls.Add(_LabelLargeSize);
		_FlowLarge.Location = new Point(77, 205);
		_FlowLarge.Margin = new Padding(0);
		_FlowLarge.Name = "_FlowLarge";
		_FlowLarge.Size = new Size(67, 58);
		_FlowLarge.TabIndex = 11;
		_FlowLarge.WrapContents = false;
		//
		// _IconLarge
		//
		_IconLarge.AllowDrop = true;
		_IconLarge.BackColor = SystemColors.Window;
		_IconLarge.ContextMenuStrip = _CmenuIcon;
		_IconLarge.Location = new Point(3, 3);
		_IconLarge.Name = "_IconLarge";
		_IconLarge.Size = new Size(52, 52);
		_IconLarge.TabIndex = 0;
		_IconLarge.DataChanged += _Field_Changed;
		_IconLarge.LoadRequested += _Icon_LoadRequested;
		//
		// _LabelLargeSize
		//
		_LabelLargeSize.Anchor = AnchorStyles.Left;
		_LabelLargeSize.AutoSize = true;
		_LabelLargeSize.ForeColor = SystemColors.GrayText;
		_LabelLargeSize.Location = new Point(64, 21);
		_LabelLargeSize.Margin = new Padding(6, 0, 3, 0);
		_LabelLargeSize.Name = "_LabelLargeSize";
		_LabelLargeSize.Size = new Size(0, 15);
		_LabelLargeSize.TabIndex = 1;
		//
		// _LocaleDefinitionWindow
		//
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(544, 351);
		Icon = (Icon) resources.GetObject("$this.Icon");
		MinimumSize = new Size(420, 390);
		Name = "_LocaleDefinitionWindow";
		Controls.SetChildIndex(_PanelContent, 0);
		_PanelContent.ResumeLayout(false);
		_PanelContent.PerformLayout();
		_TableFields.ResumeLayout(false);
		_TableFields.PerformLayout();
		_FlowSmall.ResumeLayout(false);
		_FlowSmall.PerformLayout();
		_CmenuIcon.ResumeLayout(false);
		_FlowLarge.ResumeLayout(false);
		_FlowLarge.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel _TableFields;
	private System.Windows.Forms.Label _LabelKey;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextKey;
	private System.Windows.Forms.Label _LabelBase;
	private System.Windows.Forms.ComboBox _ComboBase;
	private System.Windows.Forms.Label _LabelName;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextName;
	private Robbiblubber.Spellbook.Controls.ThemedCheckBox _CheckDefault;
	private Robbiblubber.Spellbook.Controls.ThemedCheckBox _CheckRightToLeft;
	private System.Windows.Forms.Label _LabelSmall;
	private System.Windows.Forms.FlowLayoutPanel _FlowSmall;
	private Robbiblubber.Localization.Editor._IconBox _IconSmall;
	private System.Windows.Forms.Label _LabelSmallSize;
	private System.Windows.Forms.Label _LabelLarge;
	private System.Windows.Forms.FlowLayoutPanel _FlowLarge;
	private Robbiblubber.Localization.Editor._IconBox _IconLarge;
	private System.Windows.Forms.Label _LabelLargeSize;
	private System.Windows.Forms.ContextMenuStrip _CmenuIcon;
	private System.Windows.Forms.ToolStripMenuItem _CmenuIconCut;
	private System.Windows.Forms.ToolStripMenuItem _CmenuIconCopy;
	private System.Windows.Forms.ToolStripMenuItem _CmenuIconPaste;
	private System.Windows.Forms.ToolStripMenuItem _CmenuIconDelete;
	private System.Windows.Forms.ToolStripSeparator _CmenuIconSep0;
	private System.Windows.Forms.ToolStripMenuItem _CmenuIconLoad;
}
