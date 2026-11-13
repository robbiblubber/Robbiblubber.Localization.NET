namespace Robbiblubber.Localization.UnitTests.Controls.ToolTips;

using System.Runtime.Versioning;

using NUnit.Framework;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Localization.UnitTests.Library;



/// <summary>This class tests the <see cref="LocalizableToolTip"/> component.</summary>
[TestFixture][Apartment(ApartmentState.STA)][NonParallelizable][SupportedOSPlatform("windows")]
public class LocalizableToolTipTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;

	/// <summary>Button with the default tool tip "Default".</summary>
	private Button _Button = null!;

	/// <summary>Form containing the button.</summary>
	private Form _Form = null!;

	/// <summary>Tool tip.</summary>
	private LocalizableToolTip _ToolTip = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test data, a form with a button and a tool tip.</summary>
	[SetUp]
	public void SetUp()
	{
		Locale._Reset();
		_Files = new();
		_ControlTestData.WriteStandard(_Files);

		_Button = new Button();
		_Form = _ControlTestData.CreateForm(_Button);
		_ToolTip = new LocalizableToolTip();
		_ToolTip.SetToolTip(_Button, "Default");
	}


	/// <summary>Disposes the form and the tool tip and deletes the test data.</summary>
	[TearDown]
	public void TearDown()
	{
		_ToolTip.Dispose();
		_Button.Dispose();
		_Form.Dispose();
		_Files.Dispose();
		Locale._Reset();
	}


	/// <summary>A new tool tip updates automatically.</summary>
	[Test]
	public void ToolTipHasDefaults()
	{
		Assert.That(_ToolTip.AutoUpdate, Is.True);
		Assert.That(_ToolTip.GetLocaleSymbol(_Button), Is.Empty);
	}


	/// <summary>Symbols are stored per control; NULL and empty remove the symbol.</summary>
	[Test]
	public void Symbols_GetAndSet()
	{
		_ToolTip.SetLocaleSymbol(_Button, "app:tip");
		Assert.That(_ToolTip.GetLocaleSymbol(_Button), Is.EqualTo("app:tip"));

		_ToolTip.SetLocaleSymbol(_Button, null);
		Assert.That(_ToolTip.GetLocaleSymbol(_Button), Is.Empty);

		Assert.That(() => _ToolTip.SetLocaleSymbol(null!, "app:tip"), Throws.ArgumentNullException);
	}


	/// <summary>The tool tip is localized when the control is shown, with the tool tip set in the designer as default
	///          text.</summary>
	[Test]
	public void Localize_WhenShown()
	{
		Button other = new();
		_Form.Controls.Add(other);
		_ToolTip.SetLocaleSymbol(other, "app:undefined");
		_ToolTip.SetToolTip(other, "Designer text");
		_ToolTip.SetLocaleSymbol(_Button, "app:tip");
		Locale.Current = _ControlTestData.Get("de");

		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Default"));

		_ControlTestData.Show(_Form);

		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Kurzinfo"));
		Assert.That(_ToolTip.GetToolTip(other), Is.EqualTo("Designer text"));
	}


	/// <summary>The tool tips follow the current locale.</summary>
	[Test]
	public void Localize_FollowsCurrentLocale()
	{
		_ToolTip.SetLocaleSymbol(_Button, "app:tip");
		_ControlTestData.Show(_Form);
		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Tool tip"));

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Kurzinfo"));
	}


	/// <summary>Without automatic update, the tool tips change only when the component localizes.</summary>
	[Test]
	public void Localize_AutoUpdateOff()
	{
		_ToolTip.AutoUpdate = false;
		_ToolTip.SetLocaleSymbol(_Button, "app:tip");
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Tool tip"));

		_ToolTip.Localize();
		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Kurzinfo"));
	}


	/// <summary>Removing the symbol restores the default text.</summary>
	[Test]
	public void Localize_RemoveSymbolRestoresDefault()
	{
		_ToolTip.SetLocaleSymbol(_Button, "app:tip");
		_ControlTestData.Show(_Form);

		_ToolTip.SetLocaleSymbol(_Button, string.Empty);
		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Default"));

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(_ToolTip.GetToolTip(_Button), Is.EqualTo("Default"));
	}


	/// <summary>Disposed controls are released.</summary>
	[Test]
	public void Localize_DisposedControlReleased()
	{
		_ToolTip.SetLocaleSymbol(_Button, "app:tip");
		_ControlTestData.Show(_Form);

		_Button.Dispose();

		Assert.That(_ToolTip.GetLocaleSymbol(_Button), Is.Empty);
		Assert.That(() => Locale.Current = _ControlTestData.Get("de"), Throws.Nothing);
	}


	/// <summary>A disposed tool tip no longer follows the current locale.</summary>
	[Test]
	public void Dispose_StopsUpdates()
	{
		_ToolTip.SetLocaleSymbol(_Button, "app:tip");
		_ControlTestData.Show(_Form);

		_ToolTip.Dispose();

		Assert.That(() => Locale.Current = _ControlTestData.Get("de"), Throws.Nothing);
	}
}
