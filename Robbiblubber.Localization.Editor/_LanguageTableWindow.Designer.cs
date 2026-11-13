namespace Robbiblubber.Localization.Editor;

partial class _LanguageTableWindow
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_LanguageTableWindow));
		_TableHeader = new TableLayoutPanel();
		_LabelKey = new Label();
		_TextKey = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_LabelLocale = new Label();
		_TextLocale = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_LabelName = new Label();
		_TextName = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_LabelVersion = new Label();
		_TextVersion = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_LabelPrefix = new Label();
		_TextPrefix = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_LabelAuthor = new Label();
		_TextAuthor = new Robbiblubber.Spellbook.Controls.ThemedTextBox();
		_GridData = new DataGridView();
		_CmenuGrid = new ContextMenuStrip(components);
		_CmenuCut = new ToolStripMenuItem();
		_CmenuCopy = new ToolStripMenuItem();
		_CmenuPaste = new ToolStripMenuItem();
		_CmenuDelete = new ToolStripMenuItem();
		_CmenuSep1 = new ToolStripSeparator();
		_CmenuSelectAll = new ToolStripMenuItem();
		_CmenuSep2 = new ToolStripSeparator();
		_CmenuTextEditor = new ToolStripMenuItem();
		_PanelContent.SuspendLayout();
		_TableHeader.SuspendLayout();
		((System.ComponentModel.ISupportInitialize) _GridData).BeginInit();
		_CmenuGrid.SuspendLayout();
		SuspendLayout();
		//
		// _Layout
		//
		_Layout.LayoutName = "table";
		//
		// _PanelContent
		//
		_PanelContent.Controls.Add(_GridData);
		_PanelContent.Controls.Add(_TableHeader);
		//
		// _TableHeader
		//
		_TableHeader.AutoSize = true;
		_TableHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		_TableHeader.ColumnCount = 4;
		_TableHeader.ColumnStyles.Add(new ColumnStyle());
		_TableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
		_TableHeader.ColumnStyles.Add(new ColumnStyle());
		_TableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
		_TableHeader.Controls.Add(_LabelKey, 0, 0);
		_TableHeader.Controls.Add(_TextKey, 1, 0);
		_TableHeader.Controls.Add(_LabelLocale, 2, 0);
		_TableHeader.Controls.Add(_TextLocale, 3, 0);
		_TableHeader.Controls.Add(_LabelName, 0, 1);
		_TableHeader.Controls.Add(_TextName, 1, 1);
		_TableHeader.Controls.Add(_LabelVersion, 2, 1);
		_TableHeader.Controls.Add(_TextVersion, 3, 1);
		_TableHeader.Controls.Add(_LabelPrefix, 0, 2);
		_TableHeader.Controls.Add(_TextPrefix, 1, 2);
		_TableHeader.Controls.Add(_LabelAuthor, 2, 2);
		_TableHeader.Controls.Add(_TextAuthor, 3, 2);
		_TableHeader.Dock = DockStyle.Top;
		_TableHeader.Location = new Point(0, 0);
		_TableHeader.Name = "_TableHeader";
		_TableHeader.Padding = new Padding(6, 6, 6, 3);
		_TableHeader.RowCount = 3;
		_TableHeader.RowStyles.Add(new RowStyle());
		_TableHeader.RowStyles.Add(new RowStyle());
		_TableHeader.RowStyles.Add(new RowStyle());
		_TableHeader.Size = new Size(784, 96);
		_TableHeader.TabIndex = 0;
		//
		// _LabelKey
		//
		_LabelKey.Anchor = AnchorStyles.Left;
		_LabelKey.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelKey, "led:header.key");
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
		_TextKey.Location = new Point(65, 9);
		_TextKey.Name = "_TextKey";
		_TextKey.Size = new Size(320, 23);
		_TextKey.TabIndex = 1;
		_TextKey.TextChanged += _TextHeader_TextChanged;
		//
		// _LabelLocale
		//
		_LabelLocale.Anchor = AnchorStyles.Left;
		_LabelLocale.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelLocale, "led:header.locale");
		_LabelLocale.Location = new Point(400, 13);
		_LabelLocale.Margin = new Padding(12, 0, 3, 0);
		_LabelLocale.Name = "_LabelLocale";
		_LabelLocale.Size = new Size(44, 15);
		_LabelLocale.TabIndex = 2;
		_LabelLocale.Text = "&Locale:";
		//
		// _TextLocale
		//
		_TextLocale.ContextMenuStrip = _CmenuText;
		_TextLocale.Dock = DockStyle.Fill;
		_TextLocale.Location = new Point(454, 9);
		_TextLocale.Name = "_TextLocale";
		_TextLocale.Size = new Size(321, 23);
		_TextLocale.TabIndex = 3;
		_TextLocale.TextChanged += _TextHeader_TextChanged;
		//
		// _LabelName
		//
		_LabelName.Anchor = AnchorStyles.Left;
		_LabelName.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelName, "led:header.name");
		_LabelName.Location = new Point(9, 42);
		_LabelName.Name = "_LabelName";
		_LabelName.Size = new Size(42, 15);
		_LabelName.TabIndex = 4;
		_LabelName.Text = "&Name:";
		//
		// _TextName
		//
		_TextName.ContextMenuStrip = _CmenuText;
		_TextName.Dock = DockStyle.Fill;
		_TextName.Location = new Point(65, 38);
		_TextName.Name = "_TextName";
		_TextName.Size = new Size(320, 23);
		_TextName.TabIndex = 5;
		_TextName.TextChanged += _TextHeader_TextChanged;
		//
		// _LabelVersion
		//
		_LabelVersion.Anchor = AnchorStyles.Left;
		_LabelVersion.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelVersion, "led:header.version");
		_LabelVersion.Location = new Point(400, 42);
		_LabelVersion.Margin = new Padding(12, 0, 3, 0);
		_LabelVersion.Name = "_LabelVersion";
		_LabelVersion.Size = new Size(48, 15);
		_LabelVersion.TabIndex = 6;
		_LabelVersion.Text = "&Version:";
		//
		// _TextVersion
		//
		_TextVersion.ContextMenuStrip = _CmenuText;
		_TextVersion.Dock = DockStyle.Fill;
		_TextVersion.Location = new Point(454, 38);
		_TextVersion.Name = "_TextVersion";
		_TextVersion.Size = new Size(321, 23);
		_TextVersion.TabIndex = 7;
		_TextVersion.TextChanged += _TextHeader_TextChanged;
		//
		// _LabelPrefix
		//
		_LabelPrefix.Anchor = AnchorStyles.Left;
		_LabelPrefix.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelPrefix, "led:header.prefix");
		_LabelPrefix.Location = new Point(9, 71);
		_LabelPrefix.Name = "_LabelPrefix";
		_LabelPrefix.Size = new Size(50, 15);
		_LabelPrefix.TabIndex = 8;
		_LabelPrefix.Text = "&Prefixes:";
		//
		// _TextPrefix
		//
		_TextPrefix.ContextMenuStrip = _CmenuText;
		_TextPrefix.Dock = DockStyle.Fill;
		_TextPrefix.Location = new Point(65, 67);
		_TextPrefix.Name = "_TextPrefix";
		_TextPrefix.Size = new Size(320, 23);
		_TextPrefix.TabIndex = 9;
		_TextPrefix.TextChanged += _TextHeader_TextChanged;
		//
		// _LabelAuthor
		//
		_LabelAuthor.Anchor = AnchorStyles.Left;
		_LabelAuthor.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelAuthor, "led:header.author");
		_LabelAuthor.Location = new Point(400, 71);
		_LabelAuthor.Margin = new Padding(12, 0, 3, 0);
		_LabelAuthor.Name = "_LabelAuthor";
		_LabelAuthor.Size = new Size(47, 15);
		_LabelAuthor.TabIndex = 10;
		_LabelAuthor.Text = "&Author:";
		//
		// _TextAuthor
		//
		_TextAuthor.ContextMenuStrip = _CmenuText;
		_TextAuthor.Dock = DockStyle.Fill;
		_TextAuthor.Location = new Point(454, 67);
		_TextAuthor.Name = "_TextAuthor";
		_TextAuthor.Size = new Size(321, 23);
		_TextAuthor.TabIndex = 11;
		_TextAuthor.TextChanged += _TextHeader_TextChanged;
		//
		// _GridData
		//
		_GridData.AllowUserToResizeRows = false;
		_GridData.BackgroundColor = SystemColors.Window;
		_GridData.BorderStyle = BorderStyle.None;
		_GridData.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
		_GridData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		_GridData.ContextMenuStrip = _CmenuGrid;
		_GridData.Dock = DockStyle.Fill;
		_GridData.Location = new Point(0, 96);
		_GridData.Name = "_GridData";
		_GridData.RowHeadersWidth = 48;
		_Layout.SetSaveLayout(_GridData, true);
		_GridData.Size = new Size(784, 419);
		_GridData.TabIndex = 1;
		_GridData.CellDoubleClick += _GridData_CellDoubleClick;
		_GridData.CellMouseDown += _GridData_CellMouseDown;
		_GridData.CellValueChanged += _GridData_Changed;
		_GridData.DataError += _GridData_DataError;
		_GridData.EditingControlShowing += _GridData_EditingControlShowing;
		_GridData.RowErrorTextNeeded += _GridData_RowErrorTextNeeded;
		_GridData.RowsRemoved += _GridData_Changed;
		//
		// _CmenuGrid
		//
		_CmenuGrid.Items.AddRange(new ToolStripItem[] { _CmenuCut, _CmenuCopy, _CmenuPaste, _CmenuDelete, _CmenuSep1, _CmenuSelectAll, _CmenuSep2, _CmenuTextEditor });
		_CmenuGrid.Name = "_CmenuGrid";
		_CmenuGrid.Size = new Size(179, 148);
		//
		// _CmenuCut
		//
		_Edit.SetEditCommand(_CmenuCut, Spellbook.Controls.EditCommand.CUT);
		_Locale.SetLocaleSymbol(_CmenuCut, "led:menu.edit.cut");
		_CmenuCut.Name = "_CmenuCut";
		_CmenuCut.ShortcutKeyDisplayString = "Ctrl+X";
		_CmenuCut.Size = new Size(178, 22);
		_CmenuCut.Text = "Cu&t";
		//
		// _CmenuCopy
		//
		_Edit.SetEditCommand(_CmenuCopy, Spellbook.Controls.EditCommand.COPY);
		_Locale.SetLocaleSymbol(_CmenuCopy, "led:menu.edit.copy");
		_CmenuCopy.Name = "_CmenuCopy";
		_CmenuCopy.ShortcutKeyDisplayString = "Ctrl+C";
		_CmenuCopy.Size = new Size(178, 22);
		_CmenuCopy.Text = "&Copy";
		//
		// _CmenuPaste
		//
		_Edit.SetEditCommand(_CmenuPaste, Spellbook.Controls.EditCommand.PASTE);
		_Locale.SetLocaleSymbol(_CmenuPaste, "led:menu.edit.paste");
		_CmenuPaste.Name = "_CmenuPaste";
		_CmenuPaste.ShortcutKeyDisplayString = "Ctrl+V";
		_CmenuPaste.Size = new Size(178, 22);
		_CmenuPaste.Text = "&Paste";
		//
		// _CmenuDelete
		//
		_Edit.SetEditCommand(_CmenuDelete, Spellbook.Controls.EditCommand.DELETE);
		_Locale.SetLocaleSymbol(_CmenuDelete, "led:menu.edit.delete");
		_CmenuDelete.Name = "_CmenuDelete";
		_CmenuDelete.ShortcutKeyDisplayString = "Del";
		_CmenuDelete.Size = new Size(178, 22);
		_CmenuDelete.Text = "&Delete";
		//
		// _CmenuSep1
		//
		_CmenuSep1.Name = "_CmenuSep1";
		_CmenuSep1.Size = new Size(175, 6);
		//
		// _CmenuSelectAll
		//
		_Edit.SetEditCommand(_CmenuSelectAll, Spellbook.Controls.EditCommand.SELECT_ALL);
		_Locale.SetLocaleSymbol(_CmenuSelectAll, "led:menu.edit.selectall");
		_CmenuSelectAll.Name = "_CmenuSelectAll";
		_CmenuSelectAll.ShortcutKeyDisplayString = "Ctrl+A";
		_CmenuSelectAll.Size = new Size(178, 22);
		_CmenuSelectAll.Text = "Select &All";
		//
		// _CmenuSep2
		//
		_CmenuSep2.Name = "_CmenuSep2";
		_CmenuSep2.Size = new Size(175, 6);
		//
		// _CmenuTextEditor
		//
		_Locale.SetLocaleSymbol(_CmenuTextEditor, "led:menu.tools.texteditor");
		_CmenuTextEditor.Name = "_CmenuTextEditor";
		_CmenuTextEditor.ShortcutKeyDisplayString = "Ctrl+E";
		_CmenuTextEditor.Size = new Size(178, 22);
		_CmenuTextEditor.Text = "Te&xt Editor...";
		_CmenuTextEditor.Click += _CmenuTextEditor_Click;
		//
		// _LanguageTableWindow
		//
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(784, 561);
		Icon = (Icon) resources.GetObject("$this.Icon");
		Name = "_LanguageTableWindow";
		Controls.SetChildIndex(_PanelContent, 0);
		_PanelContent.ResumeLayout(false);
		_PanelContent.PerformLayout();
		_TableHeader.ResumeLayout(false);
		_TableHeader.PerformLayout();
		((System.ComponentModel.ISupportInitialize) _GridData).EndInit();
		_CmenuGrid.ResumeLayout(false);
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel _TableHeader;
	private System.Windows.Forms.Label _LabelKey;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextKey;
	private System.Windows.Forms.Label _LabelLocale;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextLocale;
	private System.Windows.Forms.Label _LabelName;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextName;
	private System.Windows.Forms.Label _LabelVersion;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextVersion;
	private System.Windows.Forms.Label _LabelPrefix;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextPrefix;
	private System.Windows.Forms.Label _LabelAuthor;
	private Robbiblubber.Spellbook.Controls.ThemedTextBox _TextAuthor;
	private System.Windows.Forms.DataGridView _GridData;
	private System.Windows.Forms.ContextMenuStrip _CmenuGrid;
	private System.Windows.Forms.ToolStripMenuItem _CmenuCut;
	private System.Windows.Forms.ToolStripMenuItem _CmenuCopy;
	private System.Windows.Forms.ToolStripMenuItem _CmenuPaste;
	private System.Windows.Forms.ToolStripMenuItem _CmenuDelete;
	private System.Windows.Forms.ToolStripSeparator _CmenuSep1;
	private System.Windows.Forms.ToolStripMenuItem _CmenuSelectAll;
	private System.Windows.Forms.ToolStripSeparator _CmenuSep2;
	private System.Windows.Forms.ToolStripMenuItem _CmenuTextEditor;
}
