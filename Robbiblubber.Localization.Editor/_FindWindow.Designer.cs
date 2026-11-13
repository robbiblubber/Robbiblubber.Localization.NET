namespace Robbiblubber.Localization.Editor;

partial class _FindWindow
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_FindWindow));
		_Locale = new Robbiblubber.Localization.Controls.LocaleProvider(components);
		_ThemeProvider = new Robbiblubber.Spellbook.Controls.Themes.ThemeProvider(components);
		_LabelFind = new Label();
		_LabelReplace = new Label();
		_LabelScope = new Label();
		_RadioSymbol = new Robbiblubber.Spellbook.Controls.ThemedRadioButton();
		_RadioText = new Robbiblubber.Spellbook.Controls.ThemedRadioButton();
		_RadioBoth = new Robbiblubber.Spellbook.Controls.ThemedRadioButton();
		_CheckMatchCase = new Robbiblubber.Spellbook.Controls.ThemedCheckBox();
		_CheckWholeWord = new Robbiblubber.Spellbook.Controls.ThemedCheckBox();
		_CheckRegularExpression = new Robbiblubber.Spellbook.Controls.ThemedCheckBox();
		_CheckBackward = new Robbiblubber.Spellbook.Controls.ThemedCheckBox();
		_ButtonFindNext = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_ButtonReplace = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_ButtonReplaceAll = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_ButtonClose = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_Layout = new Robbiblubber.Spellbook.Controls.WindowLayoutProvider(components);
		_PanelFind = new Panel();
		_ComboFind = new ComboBox();
		_PanelReplace = new Panel();
		_ComboReplace = new ComboBox();
		_PanelOptions = new Panel();
		_FlowButtons = new FlowLayoutPanel();
		_StatusFind = new StatusStrip();
		_LabelStatus = new ToolStripStatusLabel();
		_PanelFind.SuspendLayout();
		_PanelReplace.SuspendLayout();
		_PanelOptions.SuspendLayout();
		_FlowButtons.SuspendLayout();
		_StatusFind.SuspendLayout();
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
		// _LabelFind
		//
		_LabelFind.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelFind, "led:find.what");
		_LabelFind.Location = new Point(12, 16);
		_LabelFind.Name = "_LabelFind";
		_LabelFind.Size = new Size(62, 15);
		_LabelFind.TabIndex = 0;
		_LabelFind.Text = "Fi&nd what:";
		//
		// _LabelReplace
		//
		_LabelReplace.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelReplace, "led:find.replacewith");
		_LabelReplace.Location = new Point(12, 7);
		_LabelReplace.Name = "_LabelReplace";
		_LabelReplace.Size = new Size(77, 15);
		_LabelReplace.TabIndex = 0;
		_LabelReplace.Text = "Re&place with:";
		//
		// _LabelScope
		//
		_LabelScope.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelScope, "led:find.lookin");
		_LabelScope.Location = new Point(12, 12);
		_LabelScope.Name = "_LabelScope";
		_LabelScope.Size = new Size(49, 15);
		_LabelScope.TabIndex = 0;
		_LabelScope.Text = "Look in:";
		//
		// _RadioSymbol
		//
		_RadioSymbol.AutoSize = true;
		_Locale.SetLocaleSymbol(_RadioSymbol, "led:find.symbol");
		_RadioSymbol.Location = new Point(124, 10);
		_RadioSymbol.Name = "_RadioSymbol";
		_RadioSymbol.Size = new Size(65, 19);
		_RadioSymbol.TabIndex = 1;
		_RadioSymbol.TabStop = true;
		_RadioSymbol.Text = "&Symbol";
		_RadioSymbol.UseVisualStyleBackColor = true;
		_RadioSymbol.CheckedChanged += _Option_Changed;
		//
		// _RadioText
		//
		_RadioText.AutoSize = true;
		_Locale.SetLocaleSymbol(_RadioText, "led:find.text");
		_RadioText.Location = new Point(208, 10);
		_RadioText.Name = "_RadioText";
		_RadioText.Size = new Size(46, 19);
		_RadioText.TabIndex = 2;
		_RadioText.TabStop = true;
		_RadioText.Text = "&Text";
		_RadioText.UseVisualStyleBackColor = true;
		_RadioText.CheckedChanged += _Option_Changed;
		//
		// _RadioBoth
		//
		_RadioBoth.AutoSize = true;
		_Locale.SetLocaleSymbol(_RadioBoth, "led:find.both");
		_RadioBoth.Location = new Point(292, 10);
		_RadioBoth.Name = "_RadioBoth";
		_RadioBoth.Size = new Size(50, 19);
		_RadioBoth.TabIndex = 3;
		_RadioBoth.TabStop = true;
		_RadioBoth.Text = "B&oth";
		_RadioBoth.UseVisualStyleBackColor = true;
		_RadioBoth.CheckedChanged += _Option_Changed;
		//
		// _CheckMatchCase
		//
		_CheckMatchCase.AutoSize = true;
		_Locale.SetLocaleSymbol(_CheckMatchCase, "led:find.matchcase");
		_CheckMatchCase.Location = new Point(12, 40);
		_CheckMatchCase.Name = "_CheckMatchCase";
		_CheckMatchCase.Size = new Size(86, 19);
		_CheckMatchCase.TabIndex = 4;
		_CheckMatchCase.Text = "Match &case";
		_CheckMatchCase.UseVisualStyleBackColor = true;
		_CheckMatchCase.CheckedChanged += _Option_Changed;
		//
		// _CheckWholeWord
		//
		_CheckWholeWord.AutoSize = true;
		_Locale.SetLocaleSymbol(_CheckWholeWord, "led:find.wholeword");
		_CheckWholeWord.Location = new Point(200, 40);
		_CheckWholeWord.Name = "_CheckWholeWord";
		_CheckWholeWord.Size = new Size(125, 19);
		_CheckWholeWord.TabIndex = 5;
		_CheckWholeWord.Text = "Match &whole word";
		_CheckWholeWord.UseVisualStyleBackColor = true;
		_CheckWholeWord.CheckedChanged += _Option_Changed;
		//
		// _CheckRegularExpression
		//
		_CheckRegularExpression.AutoSize = true;
		_Locale.SetLocaleSymbol(_CheckRegularExpression, "led:find.regex");
		_CheckRegularExpression.Location = new Point(12, 64);
		_CheckRegularExpression.Name = "_CheckRegularExpression";
		_CheckRegularExpression.Size = new Size(129, 19);
		_CheckRegularExpression.TabIndex = 6;
		_CheckRegularExpression.Text = "Regular e&xpressions";
		_CheckRegularExpression.UseVisualStyleBackColor = true;
		_CheckRegularExpression.CheckedChanged += _Option_Changed;
		//
		// _CheckBackward
		//
		_CheckBackward.AutoSize = true;
		_Locale.SetLocaleSymbol(_CheckBackward, "led:find.backward");
		_CheckBackward.Location = new Point(200, 64);
		_CheckBackward.Name = "_CheckBackward";
		_CheckBackward.Size = new Size(120, 19);
		_CheckBackward.TabIndex = 7;
		_CheckBackward.Text = "Search &backwards";
		_CheckBackward.UseVisualStyleBackColor = true;
		//
		// _ButtonFindNext
		//
		_Locale.SetLocaleSymbol(_ButtonFindNext, "led:find.next");
		_ButtonFindNext.Location = new Point(0, 10);
		_ButtonFindNext.Margin = new Padding(0, 0, 0, 6);
		_ButtonFindNext.Name = "_ButtonFindNext";
		_ButtonFindNext.Size = new Size(100, 26);
		_ButtonFindNext.TabIndex = 0;
		_ButtonFindNext.Text = "&Find Next";
		_ButtonFindNext.UseVisualStyleBackColor = true;
		_ButtonFindNext.Click += _ButtonFindNext_Click;
		//
		// _ButtonReplace
		//
		_Locale.SetLocaleSymbol(_ButtonReplace, "led:find.replace");
		_ButtonReplace.Location = new Point(0, 42);
		_ButtonReplace.Margin = new Padding(0, 0, 0, 6);
		_ButtonReplace.Name = "_ButtonReplace";
		_ButtonReplace.Size = new Size(100, 26);
		_ButtonReplace.TabIndex = 1;
		_ButtonReplace.Text = "&Replace";
		_ButtonReplace.UseVisualStyleBackColor = true;
		_ButtonReplace.Click += _ButtonReplace_Click;
		//
		// _ButtonReplaceAll
		//
		_Locale.SetLocaleSymbol(_ButtonReplaceAll, "led:find.replaceall");
		_ButtonReplaceAll.Location = new Point(0, 74);
		_ButtonReplaceAll.Margin = new Padding(0, 0, 0, 6);
		_ButtonReplaceAll.Name = "_ButtonReplaceAll";
		_ButtonReplaceAll.Size = new Size(100, 26);
		_ButtonReplaceAll.TabIndex = 2;
		_ButtonReplaceAll.Text = "Replace &All";
		_ButtonReplaceAll.UseVisualStyleBackColor = true;
		_ButtonReplaceAll.Click += _ButtonReplaceAll_Click;
		//
		// _ButtonClose
		//
		_Locale.SetLocaleSymbol(_ButtonClose, "led:find.close");
		_ButtonClose.Location = new Point(0, 106);
		_ButtonClose.Margin = new Padding(0, 0, 0, 6);
		_ButtonClose.Name = "_ButtonClose";
		_ButtonClose.Size = new Size(100, 26);
		_ButtonClose.TabIndex = 3;
		_ButtonClose.Text = "Close";
		_ButtonClose.UseVisualStyleBackColor = true;
		_ButtonClose.Click += _ButtonClose_Click;
		//
		// _Layout
		//
		_Layout.ContainerControl = this;
		_Layout.LayoutName = "find";
		//
		// _PanelFind
		//
		_PanelFind.Controls.Add(_LabelFind);
		_PanelFind.Controls.Add(_ComboFind);
		_PanelFind.Dock = DockStyle.Top;
		_PanelFind.Location = new Point(0, 0);
		_PanelFind.Name = "_PanelFind";
		_PanelFind.Size = new Size(388, 38);
		_PanelFind.TabIndex = 0;
		//
		// _ComboFind
		//
		_ComboFind.Anchor =  AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		_ComboFind.FormattingEnabled = true;
		_ComboFind.Location = new Point(124, 12);
		_ComboFind.Name = "_ComboFind";
		_ComboFind.Size = new Size(252, 23);
		_ComboFind.TabIndex = 1;
		_ComboFind.TextChanged += _ComboFind_TextChanged;
		//
		// _PanelReplace
		//
		_PanelReplace.Controls.Add(_LabelReplace);
		_PanelReplace.Controls.Add(_ComboReplace);
		_PanelReplace.Dock = DockStyle.Top;
		_PanelReplace.Location = new Point(0, 38);
		_PanelReplace.Name = "_PanelReplace";
		_PanelReplace.Size = new Size(388, 30);
		_PanelReplace.TabIndex = 1;
		//
		// _ComboReplace
		//
		_ComboReplace.Anchor =  AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		_ComboReplace.FormattingEnabled = true;
		_ComboReplace.Location = new Point(124, 3);
		_ComboReplace.Name = "_ComboReplace";
		_ComboReplace.Size = new Size(252, 23);
		_ComboReplace.TabIndex = 1;
		//
		// _PanelOptions
		//
		_PanelOptions.Controls.Add(_LabelScope);
		_PanelOptions.Controls.Add(_RadioSymbol);
		_PanelOptions.Controls.Add(_RadioText);
		_PanelOptions.Controls.Add(_RadioBoth);
		_PanelOptions.Controls.Add(_CheckMatchCase);
		_PanelOptions.Controls.Add(_CheckWholeWord);
		_PanelOptions.Controls.Add(_CheckRegularExpression);
		_PanelOptions.Controls.Add(_CheckBackward);
		_PanelOptions.Dock = DockStyle.Top;
		_PanelOptions.Location = new Point(0, 68);
		_PanelOptions.Name = "_PanelOptions";
		_PanelOptions.Size = new Size(388, 92);
		_PanelOptions.TabIndex = 2;
		//
		// _FlowButtons
		//
		_FlowButtons.Controls.Add(_ButtonFindNext);
		_FlowButtons.Controls.Add(_ButtonReplace);
		_FlowButtons.Controls.Add(_ButtonReplaceAll);
		_FlowButtons.Controls.Add(_ButtonClose);
		_FlowButtons.Dock = DockStyle.Right;
		_FlowButtons.FlowDirection = FlowDirection.TopDown;
		_FlowButtons.Location = new Point(388, 0);
		_FlowButtons.Name = "_FlowButtons";
		_FlowButtons.Padding = new Padding(0, 10, 12, 0);
		_FlowButtons.Size = new Size(112, 168);
		_FlowButtons.TabIndex = 3;
		_FlowButtons.WrapContents = false;
		//
		// _StatusFind
		//
		_StatusFind.Items.AddRange(new ToolStripItem[] { _LabelStatus });
		_StatusFind.Location = new Point(0, 168);
		_StatusFind.Name = "_StatusFind";
		_StatusFind.Size = new Size(500, 22);
		_StatusFind.SizingGrip = false;
		_StatusFind.TabIndex = 4;
		//
		// _LabelStatus
		//
		_LabelStatus.Name = "_LabelStatus";
		_LabelStatus.Size = new Size(485, 17);
		_LabelStatus.Spring = true;
		_LabelStatus.TextAlign = ContentAlignment.MiddleLeft;
		//
		// _FindWindow
		//
		AcceptButton = _ButtonFindNext;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = _ButtonClose;
		ClientSize = new Size(500, 190);
		Controls.Add(_PanelOptions);
		Controls.Add(_PanelReplace);
		Controls.Add(_PanelFind);
		Controls.Add(_FlowButtons);
		Controls.Add(_StatusFind);
		FormBorderStyle = FormBorderStyle.FixedDialog;
		Icon = (Icon) resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "_FindWindow";
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.Manual;
		Text = "Find";
		_PanelFind.ResumeLayout(false);
		_PanelFind.PerformLayout();
		_PanelReplace.ResumeLayout(false);
		_PanelReplace.PerformLayout();
		_PanelOptions.ResumeLayout(false);
		_PanelOptions.PerformLayout();
		_FlowButtons.ResumeLayout(false);
		_StatusFind.ResumeLayout(false);
		_StatusFind.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Robbiblubber.Localization.Controls.LocaleProvider _Locale;
	private Robbiblubber.Spellbook.Controls.Themes.ThemeProvider _ThemeProvider;
	private Robbiblubber.Spellbook.Controls.WindowLayoutProvider _Layout;
	private System.Windows.Forms.Panel _PanelFind;
	private System.Windows.Forms.Label _LabelFind;
	private System.Windows.Forms.ComboBox _ComboFind;
	private System.Windows.Forms.Panel _PanelReplace;
	private System.Windows.Forms.Label _LabelReplace;
	private System.Windows.Forms.ComboBox _ComboReplace;
	private System.Windows.Forms.Panel _PanelOptions;
	private System.Windows.Forms.Label _LabelScope;
	private Robbiblubber.Spellbook.Controls.ThemedRadioButton _RadioSymbol;
	private Robbiblubber.Spellbook.Controls.ThemedRadioButton _RadioText;
	private Robbiblubber.Spellbook.Controls.ThemedRadioButton _RadioBoth;
	private Robbiblubber.Spellbook.Controls.ThemedCheckBox _CheckMatchCase;
	private Robbiblubber.Spellbook.Controls.ThemedCheckBox _CheckWholeWord;
	private Robbiblubber.Spellbook.Controls.ThemedCheckBox _CheckRegularExpression;
	private Robbiblubber.Spellbook.Controls.ThemedCheckBox _CheckBackward;
	private System.Windows.Forms.FlowLayoutPanel _FlowButtons;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonFindNext;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonReplace;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonReplaceAll;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonClose;
	private System.Windows.Forms.StatusStrip _StatusFind;
	private System.Windows.Forms.ToolStripStatusLabel _LabelStatus;
}
