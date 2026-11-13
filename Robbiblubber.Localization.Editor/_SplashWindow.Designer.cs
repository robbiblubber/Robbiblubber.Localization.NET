namespace Robbiblubber.Localization.Editor;

partial class _SplashWindow
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_SplashWindow));
		_LabelImage = new Label();
		_LabelRobbiblubber = new Label();
		_LabelVersion = new Label();
		SuspendLayout();
		//
		// _LabelImage
		//
		_LabelImage.Image = (Image) resources.GetObject("_LabelImage.Image");
		_LabelImage.Location = new Point(12, 42);
		_LabelImage.Name = "_LabelImage";
		_LabelImage.Size = new Size(536, 256);
		_LabelImage.TabIndex = 0;
		//
		// _LabelRobbiblubber
		//
		_LabelRobbiblubber.Image = (Image) resources.GetObject("_LabelRobbiblubber.Image");
		_LabelRobbiblubber.Location = new Point(346, 17);
		_LabelRobbiblubber.Name = "_LabelRobbiblubber";
		_LabelRobbiblubber.Size = new Size(198, 29);
		_LabelRobbiblubber.TabIndex = 1;
		//
		// _LabelVersion
		//
		_LabelVersion.AutoSize = true;
		_LabelVersion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
		_LabelVersion.Location = new Point(365, 250);
		_LabelVersion.Name = "_LabelVersion";
		_LabelVersion.Size = new Size(99, 21);
		_LabelVersion.TabIndex = 3;
		_LabelVersion.Text = "Version 0.0.0";
		//
		// _SplashWindow
		//
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		BackColor = Color.White;
		ClientSize = new Size(560, 320);
		Controls.Add(_LabelVersion);
		Controls.Add(_LabelRobbiblubber);
		Controls.Add(_LabelImage);
		FormBorderStyle = FormBorderStyle.None;
		Icon = (Icon) resources.GetObject("$this.Icon");
		Name = "_SplashWindow";
		Opacity = 0.8D;
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "_SplashWindow";
		TopMost = true;
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Label _LabelImage;
	private Label _LabelRobbiblubber;
	private Label _LabelVersion;
}
