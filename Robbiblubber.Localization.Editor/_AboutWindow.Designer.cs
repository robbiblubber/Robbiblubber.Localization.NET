namespace Robbiblubber.Localization.Editor;

partial class _AboutWindow
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_AboutWindow));
		_ButtonClose = new Button();
		_LabelRobbiblubber = new Label();
		_LabelVersion = new Label();
		_LabelImage = new Label();
		_Layout = new Robbiblubber.Spellbook.Controls.WindowLayoutProvider(components);
		SuspendLayout();
		//
		// _ButtonClose
		//
		_ButtonClose.Location = new Point(12, 340);
		_ButtonClose.Name = "_ButtonClose";
		_ButtonClose.Size = new Size(75, 23);
		_ButtonClose.TabIndex = 0;
		_ButtonClose.Text = "Close";
		_ButtonClose.UseVisualStyleBackColor = true;
		_ButtonClose.Click += _ButtonClose_Click;
		//
		// _LabelRobbiblubber
		//
		_LabelRobbiblubber.Image = (Image) resources.GetObject("_LabelRobbiblubber.Image");
		_LabelRobbiblubber.Location = new Point(346, 17);
		_LabelRobbiblubber.Name = "_LabelRobbiblubber";
		_LabelRobbiblubber.Size = new Size(198, 29);
		_LabelRobbiblubber.TabIndex = 5;
		//
		// _LabelVersion
		//
		_LabelVersion.AutoSize = true;
		_LabelVersion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
		_LabelVersion.Location = new Point(365, 250);
		_LabelVersion.Name = "_LabelVersion";
		_LabelVersion.Size = new Size(99, 21);
		_LabelVersion.TabIndex = 7;
		_LabelVersion.Text = "Version 0.0.0";
		//
		// _LabelImage
		//
		_LabelImage.Image = (Image) resources.GetObject("_LabelImage.Image");
		_LabelImage.Location = new Point(12, 42);
		_LabelImage.Name = "_LabelImage";
		_LabelImage.Size = new Size(536, 256);
		_LabelImage.TabIndex = 6;
		//
		// _Layout
		//
		_Layout.ContainerControl = this;
		_Layout.LayoutName = "about";
		//
		// _AboutWindow
		//
		AcceptButton = _ButtonClose;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		BackColor = Color.White;
		CancelButton = _ButtonClose;
		ClientSize = new Size(560, 320);
		Controls.Add(_LabelVersion);
		Controls.Add(_LabelImage);
		Controls.Add(_LabelRobbiblubber);
		Controls.Add(_ButtonClose);
		FormBorderStyle = FormBorderStyle.FixedSingle;
		Icon = (Icon) resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "_AboutWindow";
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterParent;
		Text = "Localization Editor";
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Button _ButtonClose;
	private Label _LabelRobbiblubber;
	private Label _LabelVersion;
	private Label _LabelImage;
	private Robbiblubber.Spellbook.Controls.WindowLayoutProvider _Layout;
}
