namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook;
using Robbiblubber.Spellbook.Controls;



/// <summary>This class implements the splash window that is shown while the editor starts.</summary>
/// <remarks>The window is shown on top before the first document window. When that window is there, the splash window moves
///          behind it and closes a few seconds later (see <see cref="_Program"/>). It never takes the focus.</remarks>
[SupportedOSPlatform("windows")]
internal sealed partial class _SplashWindow: Form
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	public _SplashWindow()
	{
		InitializeComponent();

		_LabelVersion.Text = "led:about.version".LookUp("Version $VERSION", ("VERSION", VersionOp.ApplicationVersion.ToVersionString()));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Form                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <inheritdoc/>
	protected override bool ShowWithoutActivation
	{
		get { return true; }
	}
}
