namespace Robbiblubber.Localization.Editor;

partial class _SettingsWindow
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_SettingsWindow));
		_Locale = new Robbiblubber.Localization.Controls.LocaleProvider(components);
		_ThemeProvider = new Robbiblubber.Spellbook.Controls.Themes.ThemeProvider(components);
		_PageEnvironment = new TabPage();
		_GroupTheme = new Robbiblubber.Spellbook.Controls.ThemedGroupBox();
		_LabelTheme = new Label();
		_SelectorTheme = new Robbiblubber.Spellbook.Controls.Themes.ThemeSelector();
		_GroupLanguages = new Robbiblubber.Spellbook.Controls.ThemedGroupBox();
		_LabelLanguage = new Label();
		_SelectorLanguage = new Robbiblubber.Localization.Controls.LocaleSelector();
		_ButtonOK = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_ButtonCancel = new Robbiblubber.Spellbook.Controls.ThemedButton();
		_TabMain = new Robbiblubber.Spellbook.Controls.ThemedTabControl();
		_PageEnvironment.SuspendLayout();
		_GroupTheme.SuspendLayout();
		_GroupLanguages.SuspendLayout();
		_TabMain.SuspendLayout();
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
		// _PageEnvironment
		//
		_PageEnvironment.Controls.Add(_GroupTheme);
		_PageEnvironment.Controls.Add(_GroupLanguages);
		_Locale.SetLocaleSymbol(_PageEnvironment, "led:settings.environment");
		_PageEnvironment.Location = new Point(4, 24);
		_PageEnvironment.Name = "_PageEnvironment";
		_PageEnvironment.Padding = new Padding(9);
		_PageEnvironment.Size = new Size(428, 270);
		_PageEnvironment.TabIndex = 0;
		_PageEnvironment.Text = "Environment";
		_PageEnvironment.UseVisualStyleBackColor = true;
		//
		// _GroupTheme
		//
		_GroupTheme.Controls.Add(_LabelTheme);
		_GroupTheme.Controls.Add(_SelectorTheme);
		_GroupTheme.Dock = DockStyle.Top;
		_Locale.SetLocaleSymbol(_GroupTheme, "led:settings.appearance");
		_GroupTheme.Location = new Point(9, 93);
		_GroupTheme.Name = "_GroupTheme";
		_GroupTheme.Padding = new Padding(9, 6, 9, 9);
		_GroupTheme.Size = new Size(410, 84);
		_GroupTheme.TabIndex = 1;
		_GroupTheme.TabStop = false;
		_GroupTheme.Text = "Appearance";
		//
		// _LabelTheme
		//
		_LabelTheme.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelTheme, "led:settings.theme");
		_LabelTheme.Location = new Point(12, 24);
		_LabelTheme.Name = "_LabelTheme";
		_LabelTheme.Size = new Size(47, 15);
		_LabelTheme.TabIndex = 0;
		_LabelTheme.Text = "&Theme:";
		//
		// _SelectorTheme
		//
		_SelectorTheme.Anchor =  AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		_SelectorTheme.Location = new Point(12, 44);
		_SelectorTheme.Name = "_SelectorTheme";
		_SelectorTheme.SaveOnEditorOk = true;
		_SelectorTheme.Size = new Size(386, 24);
		_SelectorTheme.TabIndex = 1;
		//
		// _GroupLanguages
		//
		_GroupLanguages.Controls.Add(_LabelLanguage);
		_GroupLanguages.Controls.Add(_SelectorLanguage);
		_GroupLanguages.Dock = DockStyle.Top;
		_Locale.SetLocaleSymbol(_GroupLanguages, "led:settings.languages");
		_GroupLanguages.Location = new Point(9, 9);
		_GroupLanguages.Name = "_GroupLanguages";
		_GroupLanguages.Padding = new Padding(9, 6, 9, 9);
		_GroupLanguages.Size = new Size(410, 84);
		_GroupLanguages.TabIndex = 0;
		_GroupLanguages.TabStop = false;
		_GroupLanguages.Text = "Languages";
		//
		// _LabelLanguage
		//
		_LabelLanguage.AutoSize = true;
		_Locale.SetLocaleSymbol(_LabelLanguage, "led:settings.uilanguage");
		_LabelLanguage.Location = new Point(12, 24);
		_LabelLanguage.Name = "_LabelLanguage";
		_LabelLanguage.Size = new Size(137, 15);
		_LabelLanguage.TabIndex = 0;
		_LabelLanguage.Text = "&User Interface Language:";
		//
		// _SelectorLanguage
		//
		_SelectorLanguage.Anchor =  AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		_SelectorLanguage.Location = new Point(12, 44);
		_SelectorLanguage.Name = "_SelectorLanguage";
		_SelectorLanguage.Size = new Size(386, 24);
		_SelectorLanguage.TabIndex = 1;
		//
		// _ButtonOK
		//
		_ButtonOK.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
		_ButtonOK.DialogResult = DialogResult.OK;
		_Locale.SetLocaleSymbol(_ButtonOK, "led:dialog.ok");
		_ButtonOK.Location = new Point(286, 322);
		_ButtonOK.Name = "_ButtonOK";
		_ButtonOK.Size = new Size(78, 26);
		_ButtonOK.TabIndex = 1;
		_ButtonOK.Text = "OK";
		_ButtonOK.UseVisualStyleBackColor = true;
		_ButtonOK.Click += _ButtonOK_Click;
		//
		// _ButtonCancel
		//
		_ButtonCancel.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
		_ButtonCancel.DialogResult = DialogResult.Cancel;
		_Locale.SetLocaleSymbol(_ButtonCancel, "led:dialog.cancel");
		_ButtonCancel.Location = new Point(370, 322);
		_ButtonCancel.Name = "_ButtonCancel";
		_ButtonCancel.Size = new Size(78, 26);
		_ButtonCancel.TabIndex = 2;
		_ButtonCancel.Text = "Cancel";
		_ButtonCancel.UseVisualStyleBackColor = true;
		//
		// _TabMain
		//
		_TabMain.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		_TabMain.Controls.Add(_PageEnvironment);
		_TabMain.Location = new Point(12, 12);
		_TabMain.Name = "_TabMain";
		_TabMain.SelectedIndex = 0;
		_TabMain.Size = new Size(436, 298);
		_TabMain.TabIndex = 0;
		//
		// _SettingsWindow
		//
		AcceptButton = _ButtonOK;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = _ButtonCancel;
		ClientSize = new Size(460, 360);
		Controls.Add(_TabMain);
		Controls.Add(_ButtonOK);
		Controls.Add(_ButtonCancel);
		FormBorderStyle = FormBorderStyle.FixedDialog;
		Icon = (Icon) resources.GetObject("$this.Icon");
		_Locale.SetLocaleSymbol(this, "led:settings.title");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "_SettingsWindow";
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterParent;
		Text = "Settings";
		_PageEnvironment.ResumeLayout(false);
		_GroupTheme.ResumeLayout(false);
		_GroupTheme.PerformLayout();
		_GroupLanguages.ResumeLayout(false);
		_GroupLanguages.PerformLayout();
		_TabMain.ResumeLayout(false);
		ResumeLayout(false);
	}

	#endregion

	private Robbiblubber.Localization.Controls.LocaleProvider _Locale;
	private Robbiblubber.Spellbook.Controls.Themes.ThemeProvider _ThemeProvider;
	private Robbiblubber.Spellbook.Controls.ThemedTabControl _TabMain;
	private System.Windows.Forms.TabPage _PageEnvironment;
	private Robbiblubber.Spellbook.Controls.ThemedGroupBox _GroupLanguages;
	private System.Windows.Forms.Label _LabelLanguage;
	private Robbiblubber.Localization.Controls.LocaleSelector _SelectorLanguage;
	private Robbiblubber.Spellbook.Controls.ThemedGroupBox _GroupTheme;
	private System.Windows.Forms.Label _LabelTheme;
	private Robbiblubber.Spellbook.Controls.Themes.ThemeSelector _SelectorTheme;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonOK;
	private Robbiblubber.Spellbook.Controls.ThemedButton _ButtonCancel;
}
