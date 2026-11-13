namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook;
using Robbiblubber.Spellbook.Controls;



/// <summary>This class implements the about window (Help > About).</summary>
/// <remarks>Escape and Enter close the window. The first time it opens centered over the current window, afterwards at the
///          position it was closed at.</remarks>
[SupportedOSPlatform("windows")]
internal sealed partial class _AboutWindow: Form
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	public _AboutWindow()
	{
		InitializeComponent();
		_Layout.ConfigFile = _Settings._LayoutFileName;

		_LabelVersion.Text = "led:about.version".LookUp("Version $VERSION", ("VERSION", VersionOp.ApplicationVersion.ToVersionString()));
		_ButtonClose.Text = "led:dialog.close".LookUp("Close");
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Form                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Help requested (F1): shows the start page of the help.</summary>
	/// <param name="hevent">Event arguments.</param>
	protected override void OnHelpRequested(HelpEventArgs hevent)
	{
		_Help._Show(this, _Help._START_PAGE);
		hevent.Handled = true;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Close clicked.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonClose_Click(object? sender, EventArgs e)
	{
		Close();
	}
}
