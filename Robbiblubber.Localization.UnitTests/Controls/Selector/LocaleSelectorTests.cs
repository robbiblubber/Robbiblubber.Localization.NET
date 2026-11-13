namespace Robbiblubber.Localization.UnitTests.Controls.Selector;

using System.Reflection;
using System.Runtime.Versioning;

using NUnit.Framework;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Localization.UnitTests.Library;
using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class tests the <see cref="LocaleSelector"/> control.</summary>
[TestFixture][Apartment(ApartmentState.STA)][NonParallelizable][SupportedOSPlatform("windows")]
public class LocaleSelectorTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;

	/// <summary>Selector.</summary>
	private LocaleSelector _Selector = null!;

	/// <summary>Form containing the selector.</summary>
	private Form _Form = null!;

	/// <summary>Number of raised SelectedLocaleChanged events.</summary>
	private int _Changed = 0;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private properties                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the drop-down list of the selector.</summary>
	private ThemedComboBox _Combo
	{
		get { return (ThemedComboBox) _Selector.Controls[0]; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the image of the button of a button style.</summary>
	/// <param name="button">Button.</param>
	/// <returns>Returns the image.</returns>
	private static Image? _GetImage(Control button)
	{
		return (Image?) button.GetType().GetProperty("Image")!.GetValue(button);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test data and a form with a selector.</summary>
	[SetUp]
	public void SetUp()
	{
		Locale._Reset();
		_Files = new();
		_ControlTestData.WriteStandard(_Files);

		_Changed = 0;
		_Selector = new LocaleSelector();
		_Selector.SelectedLocaleChanged += (m, n) => { _Changed++; };
		_Form = _ControlTestData.CreateForm(_Selector);
	}


	/// <summary>Disposes the form and deletes the test data.</summary>
	[TearDown]
	public void TearDown()
	{
		_Selector.Dispose();
		_Form.Dispose();
		_Files.Dispose();
		Locale._Reset();
	}


	/// <summary>A new selector has the documented default values.</summary>
	[Test]
	public void SelectorHasDefaults()
	{
		Assert.That(_Selector.Style, Is.EqualTo(LocaleSelectorStyle.DROPDOWN));
		Assert.That(_Selector.AutoApply, Is.True);
		Assert.That(_Selector.EmptyText, Is.EqualTo("No locales"));
		Assert.That(_Selector.Controls[0], Is.InstanceOf<ThemedComboBox>());
	}


	/// <summary>The selector lists the locales and selects the current locale without raising an event.</summary>
	[Test]
	public void Fill_ListsLocales()
	{
		_ControlTestData.Show(_Form);

		Assert.That(_Combo.Items.Count, Is.EqualTo(Locale.Locales.Count));
		Assert.That(_Combo.Items.Cast<IListItem>().Select(m => m.Text), Is.EqualTo(Locale.Locales.Select(m => m.Name)));
		Assert.That(_Combo.Items.Cast<IListItem>().All(m => m.Image is not null), Is.True);
		Assert.That(_Selector.SelectedLocale, Is.EqualTo(_ControlTestData.Get("en")));
		Assert.That(_Combo.Text, Is.EqualTo("English"));
		Assert.That(_Changed, Is.Zero);
	}


	/// <summary>The selected locale is available before the control is shown.</summary>
	[Test]
	public void SelectedLocale_BeforeShown()
	{
		Assert.That(_Selector.SelectedLocale, Is.EqualTo(_ControlTestData.Get("en")));
		Assert.That(_Changed, Is.Zero);
	}


	/// <summary>Selecting a locale applies it as current locale.</summary>
	[Test]
	public void SelectedLocale_AutoApply()
	{
		_ControlTestData.Show(_Form);

		_Selector.SelectedLocale = _ControlTestData.Get("de");

		Assert.That(Locale.Current, Is.EqualTo(_ControlTestData.Get("de")));
		Assert.That(_Combo.Text, Is.EqualTo("Deutsch"));
		Assert.That(_Changed, Is.EqualTo(1));
	}


	/// <summary>Without automatic application, selecting a locale does not change the current locale.</summary>
	[Test]
	public void SelectedLocale_NoAutoApply()
	{
		_Selector.AutoApply = false;
		_ControlTestData.Show(_Form);

		_Selector.SelectedLocale = _ControlTestData.Get("de");

		Assert.That(Locale.Current, Is.EqualTo(_ControlTestData.Get("en")));
		Assert.That(_Selector.SelectedLocale, Is.EqualTo(_ControlTestData.Get("de")));
		Assert.That(_Changed, Is.EqualTo(1));
	}


	/// <summary>The selector follows the current locale when it is changed elsewhere.</summary>
	[Test]
	public void CurrentLocale_Followed()
	{
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");

		Assert.That(_Selector.SelectedLocale, Is.EqualTo(_ControlTestData.Get("de")));
		Assert.That(_Combo.Text, Is.EqualTo("Deutsch"));
		Assert.That(_Changed, Is.EqualTo(1));
	}


	/// <summary>Without automatic application, the selector keeps its selection when the current locale changes.</summary>
	[Test]
	public void CurrentLocale_NotFollowedWithoutAutoApply()
	{
		_Selector.AutoApply = false;
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");

		Assert.That(_Selector.SelectedLocale, Is.EqualTo(_ControlTestData.Get("en")));
		Assert.That(_Changed, Is.Zero);
	}


	/// <summary>Reloading the locales keeps the selection and updates the list.</summary>
	[Test]
	public void Reload_KeepsSelection()
	{
		_ControlTestData.Show(_Form);
		Locale.Current = _ControlTestData.Get("de");
		_Changed = 0;

		_Files.WriteLocale("fr", "Français");
		Locale.Reload();

		Assert.That(_Combo.Items.Count, Is.EqualTo(4));
		Assert.That(_Selector.SelectedLocale, Is.EqualTo(_ControlTestData.Get("de")));
		Assert.That(_Combo.Text, Is.EqualTo("Deutsch"));
		Assert.That(_Changed, Is.Zero);
	}


	/// <summary>A selected locale that is no longer available after a reload stays selected and listed until another locale
	///          is selected.</summary>
	[Test]
	public void Reload_RemovedLocaleStaysSelected()
	{
		_ControlTestData.Show(_Form);
		Locale.Current = _ControlTestData.Get("de");
		_Changed = 0;

		File.Delete(_Files.File("de.locale"));
		Locale.Reload();

		Assert.That(Locale.Locales["de"], Is.Null);
		Assert.That(_Combo.Items.Count, Is.EqualTo(3));
		Assert.That(_Selector.SelectedLocale!.Key, Is.EqualTo("de"));
		Assert.That(_Combo.Text, Is.EqualTo("Deutsch"));
		Assert.That(_Changed, Is.Zero);

		_Selector.SelectedLocale = _ControlTestData.Get("en");
		Application.DoEvents();

		Assert.That(_Combo.Items.Count, Is.EqualTo(2));
		Assert.That(_Combo.Text, Is.EqualTo("English"));
		Assert.That(_Changed, Is.EqualTo(1));
	}


	/// <summary>In the button style, a selected locale that is no longer available after a reload stays selected.</summary>
	[Test]
	public void Reload_RemovedLocaleStaysSelected_Button()
	{
		_Selector.Style = LocaleSelectorStyle.BUTTON;
		_ControlTestData.Show(_Form);
		Locale.Current = _ControlTestData.Get("de");

		File.Delete(_Files.File("de.locale"));
		Locale.Reload();

		Assert.That(_Selector.Controls[0].AccessibleName, Is.EqualTo("Deutsch"));
		Assert.That(_Selector.SelectedLocale!.Key, Is.EqualTo("de"));
	}


	/// <summary>Without locales, the selector shows the empty text.</summary>
	[Test]
	public void NoLocales_ShowsEmptyText()
	{
		Locale.Sources.Clear();
		_Selector.EmptyText = "Nothing";
		_ControlTestData.Show(_Form);

		Assert.That(_Selector.SelectedLocale, Is.Null);
		Assert.That(_Combo.Items.Count, Is.EqualTo(1));
		Assert.That(_Combo.Text, Is.EqualTo("Nothing"));
	}


	/// <summary>The button style shows a square button with the name of the selected locale and a menu with the
	///          locales.</summary>
	[Test]
	public void Button_Style()
	{
		_Selector.Style = LocaleSelectorStyle.BUTTON;
		_ControlTestData.Show(_Form);

		Control button = _Selector.Controls[0];

		Assert.That(_Selector.Controls.Count, Is.EqualTo(1));
		Assert.That(_Selector.Width, Is.EqualTo(_Selector.Height));
		Assert.That(button.AccessibleName, Is.EqualTo("English"));
		Assert.That(button.AccessibilityObject.Role, Is.EqualTo(AccessibleRole.ButtonDropDown));
		Assert.That(button.TabStop, Is.True);
		Assert.That(_GetImage(button), Is.Not.Null);

		_Selector.SelectedLocale = _ControlTestData.Get("de");
		Assert.That(button.AccessibleName, Is.EqualTo("Deutsch"));
		Assert.That(Locale.Current, Is.EqualTo(_ControlTestData.Get("de")));
	}


	/// <summary>Switching the style replaces the inner control and keeps the selection.</summary>
	[Test]
	public void Style_Switch()
	{
		_ControlTestData.Show(_Form);
		_Selector.SelectedLocale = _ControlTestData.Get("de");
		ThemedComboBox combo = _Combo;

		_Selector.Style = LocaleSelectorStyle.BUTTON;
		Assert.That(combo.IsDisposed, Is.True);
		Assert.That(_Selector.Controls[0], Is.Not.InstanceOf<ThemedComboBox>());

		_Selector.Style = LocaleSelectorStyle.DROPDOWN;
		Assert.That(_Combo.Text, Is.EqualTo("Deutsch"));
		Assert.That(_Selector.Height, Is.EqualTo(_Combo.Height));
	}


	/// <summary>The drop-down button style shows a button with an arrow box, so it is wider than high.</summary>
	[Test]
	public void DropDownButton_Style()
	{
		_Selector.Style = LocaleSelectorStyle.BUTTON;
		int size = _Selector.Height;

		_Selector.Style = LocaleSelectorStyle.DROPDOWN_BUTTON;
		_ControlTestData.Show(_Form);
		Control button = _Selector.Controls[0];

		Assert.That(_Selector.Height, Is.EqualTo(size));
		Assert.That(_Selector.Width, Is.GreaterThan(_Selector.Height));
		Assert.That(button.AccessibleName, Is.EqualTo("English"));
		Assert.That(_GetImage(button), Is.Not.Null);

		_Selector.Width = 200;
		Assert.That(_Selector.Width, Is.LessThan(200));
	}


	/// <summary>The default action of the button opens the menu, which is reported as expanded state.</summary>
	[Test]
	public void Button_DefaultActionOpensMenu()
	{
		_Selector.Style = LocaleSelectorStyle.DROPDOWN_BUTTON;
		_ControlTestData.Show(_Form);
		Control button = _Selector.Controls[0];
		ContextMenuStrip menu = (ContextMenuStrip) typeof(LocaleSelector).GetField("_Menu", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(_Selector)!;

		Assert.That(menu.Items.Count, Is.EqualTo(Locale.Locales.Count));
		Assert.That(button.AccessibilityObject.State.HasFlag(AccessibleStates.Collapsed), Is.True);

		button.AccessibilityObject.DoDefaultAction();
		try
		{
			Assert.That(menu.Visible, Is.True);
			Assert.That(button.AccessibilityObject.State.HasFlag(AccessibleStates.Expanded), Is.True);
		}
		finally { menu.Close(); }

		Assert.That(button.AccessibilityObject.State.HasFlag(AccessibleStates.Collapsed), Is.True);
	}


	/// <summary>A new selector uses the application theme without custom colors.</summary>
	[Test]
	public void Colors_Default()
	{
		Assert.That(_Selector.Colors.Theme, Is.Null);
		Assert.That(_Selector.Colors.IsDefault, Is.True);
		Assert.That(_Selector.ShouldSerializeColors(), Is.False);
		Assert.That(_Combo.Colors.IsDefault, Is.True);
	}


	/// <summary>The theme and custom colors are applied to the drop-down list, also after a style switch, and can be
	///          reset.</summary>
	[Test]
	public void Colors_AppliedToDropDownList()
	{
		_ControlTestData.Show(_Form);

		_Selector.Colors.Theme = Theme.DARK;
		_Selector.Colors.FocusColor = Color.Orange;

		Assert.That(_Selector.ShouldSerializeColors(), Is.True);
		Assert.That(_Combo.Colors.Theme, Is.SameAs(Theme.DARK));
		Assert.That(_Combo.Colors.FocusColor, Is.EqualTo(Color.Orange));
		Assert.That(_Combo.Colors.BackColor, Is.EqualTo(Color.Empty));

		_Selector.Style = LocaleSelectorStyle.BUTTON;
		_Selector.Style = LocaleSelectorStyle.DROPDOWN;
		Assert.That(_Combo.Colors.Theme, Is.SameAs(Theme.DARK));
		Assert.That(_Combo.Colors.FocusColor, Is.EqualTo(Color.Orange));

		_Selector.ResetColors();
		Assert.That(_Selector.Colors.IsDefault, Is.True);
		Assert.That(_Combo.Colors.IsDefault, Is.True);
	}


	/// <summary>A theme applied to the form reaches the drop-down list.</summary>
	[Test]
	public void Colors_AmbientTheme()
	{
		_ControlTestData.Show(_Form);
		ThemeManager.Apply(_Form, Theme.DARK);

		Assert.That(_Selector.Colors.EffectiveTheme, Is.SameAs(Theme.DARK));
		Assert.That(_Combo.Colors.EffectiveTheme, Is.SameAs(Theme.DARK));
		Assert.That(_Selector.Colors.Theme, Is.Null);

		ThemeManager.Apply(_Form, null);
		Assert.That(_Combo.Colors.EffectiveTheme, Is.SameAs(ThemeManager.Theme));
	}


	/// <summary>Changing the colors repaints the button styles without an exception.</summary>
	[Test]
	public void Colors_ButtonStyles()
	{
		foreach(LocaleSelectorStyle i in new[] { LocaleSelectorStyle.BUTTON, LocaleSelectorStyle.DROPDOWN_BUTTON })
		{
			_Selector.Style = i;
			_ControlTestData.Show(_Form);

			Assert.That(() =>
			{
				_Selector.Colors.Theme = Theme.DARK;
				_Selector.Colors.ButtonBackColor = Color.Gray;
				_Selector.Refresh();
				_Selector.Enabled = false;
				_Selector.Refresh();
				_Selector.Enabled = true;
				_Selector.ResetColors();
			}, Throws.Nothing);
		}
	}


	/// <summary>A disposed selector no longer follows the current locale.</summary>
	[Test]
	public void Dispose_StopsUpdates()
	{
		_ControlTestData.Show(_Form);
		_Selector.Dispose();

		Assert.That(() => Locale.Current = _ControlTestData.Get("de"), Throws.Nothing);
		Assert.That(_Changed, Is.Zero);
	}
}
