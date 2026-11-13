namespace Robbiblubber.Localization.UnitTests.Controls.Provider;

using System.ComponentModel;
using System.Runtime.Versioning;

using NUnit.Framework;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Localization.UnitTests.Library;



/// <summary>This class tests the <see cref="LocaleProvider"/> component.</summary>
[TestFixture][Apartment(ApartmentState.STA)][NonParallelizable][SupportedOSPlatform("windows")]
public class LocaleProviderTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;

	/// <summary>Label with the default text "Default".</summary>
	private Label _Label = null!;

	/// <summary>Form containing the label.</summary>
	private Form _Form = null!;

	/// <summary>Provider for the form.</summary>
	private LocaleProvider _Provider = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test data, a form with a label and a provider.</summary>
	[SetUp]
	public void SetUp()
	{
		Locale._Reset();
		_Files = new();
		_ControlTestData.WriteStandard(_Files);

		_Label = new Label() { Text = "Default" };
		_Form = _ControlTestData.CreateForm(_Label);
		_Provider = new LocaleProvider() { ContainerControl = _Form };
	}


	/// <summary>Disposes the form and the provider and deletes the test data.</summary>
	[TearDown]
	public void TearDown()
	{
		_Provider.Dispose();
		_Label.Dispose();
		_Form.Dispose();
		_Files.Dispose();
		Locale._Reset();
	}


	/// <summary>A new provider has the documented default values.</summary>
	[Test]
	public void ProviderHasDefaults()
	{
		using LocaleProvider provider = new();

		Assert.That(provider.ContainerControl, Is.Null);
		Assert.That(provider.AutoUpdate, Is.True);
		Assert.That(provider.LocalizeItems, Is.True);
		Assert.That(provider.ApplyRightToLeft, Is.False);
	}


	/// <summary>The constructors set the container and the container control.</summary>
	[Test]
	public void ConstructorsSetContainer()
	{
		using Container container = new();
		using LocaleProvider a = new(container);
		using LocaleProvider b = new(_Form);

		Assert.That(container.Components, Does.Contain(a));
		Assert.That(b.ContainerControl, Is.SameAs(_Form));
	}


	/// <summary>Controls, tool strip items, column headers and components with a text property can be extended.</summary>
	[Test]
	public void CanExtend()
	{
		using ToolStripButton button = new();
		using ColumnHeader column = new();
		using Component plain = new();
		using _TextComponent text = new();
		using LocaleProvider other = new();

		Assert.That(_Provider.CanExtend(_Label), Is.True);
		Assert.That(_Provider.CanExtend(_Form), Is.True);
		Assert.That(_Provider.CanExtend(button), Is.True);
		Assert.That(_Provider.CanExtend(column), Is.True);
		Assert.That(_Provider.CanExtend(text), Is.True);
		Assert.That(_Provider.CanExtend(plain), Is.False);
		Assert.That(_Provider.CanExtend(other), Is.False);
		Assert.That(_Provider.CanExtend("text"), Is.False);
	}


	/// <summary>Symbols are stored per component; NULL and empty remove the symbol.</summary>
	[Test]
	public void Symbols_GetAndSet()
	{
		Assert.That(_Provider.GetLocaleSymbol(_Label), Is.Empty);

		_Provider.SetLocaleSymbol(_Label, "app:save");
		_Provider.SetAccessibleNameLocaleSymbol(_Label, "app:name");
		_Provider.SetAccessibleDescriptionLocaleSymbol(_Label, "app:desc");

		Assert.That(_Provider.GetLocaleSymbol(_Label), Is.EqualTo("app:save"));
		Assert.That(_Provider.GetAccessibleNameLocaleSymbol(_Label), Is.EqualTo("app:name"));
		Assert.That(_Provider.GetAccessibleDescriptionLocaleSymbol(_Label), Is.EqualTo("app:desc"));

		_Provider.SetLocaleSymbol(_Label, null);
		_Provider.SetAccessibleNameLocaleSymbol(_Label, string.Empty);

		Assert.That(_Provider.GetLocaleSymbol(_Label), Is.Empty);
		Assert.That(_Provider.GetAccessibleNameLocaleSymbol(_Label), Is.Empty);
		Assert.That(_Provider.GetAccessibleDescriptionLocaleSymbol(_Label), Is.EqualTo("app:desc"));
	}


	/// <summary>NULL components are rejected.</summary>
	[Test]
	public void Symbols_NullComponent()
	{
		Assert.That(() => _Provider.GetLocaleSymbol(null!), Throws.ArgumentNullException);
		Assert.That(() => _Provider.SetLocaleSymbol(null!, "app:save"), Throws.ArgumentNullException);
		Assert.That(() => _Provider.GetAccessibleNameLocaleSymbol(null!), Throws.ArgumentNullException);
	}


	/// <summary>The texts are localized when the form is shown, not before.</summary>
	[Test]
	public void Localize_WhenShown()
	{
		Locale.Current = _ControlTestData.Get("de");
		_Provider.SetLocaleSymbol(_Label, "app:save");
		_Provider.SetLocaleSymbol(_Form, "app:open");

		Assert.That(_Label.Text, Is.EqualTo("Default"));

		_ControlTestData.Show(_Form);

		Assert.That(_Label.Text, Is.EqualTo("Speichern"));
		Assert.That(_Form.Text, Is.EqualTo("Öffnen"));
	}


	/// <summary>Symbols set in the designer before the text are applied with the text set afterwards as default text.</summary>
	[Test]
	public void Localize_SymbolBeforeText()
	{
		Label label = new();
		_Form.Controls.Add(label);

		_Provider.SetLocaleSymbol(label, "app:undefined");
		label.Text = "Designer text";
		_ControlTestData.Show(_Form);

		Assert.That(label.Text, Is.EqualTo("Designer text"));
	}


	/// <summary>The texts follow the current locale.</summary>
	[Test]
	public void Localize_FollowsCurrentLocale()
	{
		_Provider.SetLocaleSymbol(_Label, "app:save");
		_ControlTestData.Show(_Form);
		Assert.That(_Label.Text, Is.EqualTo("Save"));

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(_Label.Text, Is.EqualTo("Speichern"));

		Locale.Current = _ControlTestData.Get("en");
		Assert.That(_Label.Text, Is.EqualTo("Save"));
	}


	/// <summary>Without automatic update, the texts change only when the provider localizes.</summary>
	[Test]
	public void Localize_AutoUpdateOff()
	{
		_Provider.AutoUpdate = false;
		_Provider.SetLocaleSymbol(_Label, "app:save");
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(_Label.Text, Is.EqualTo("Save"));

		_Provider.Localize();
		Assert.That(_Label.Text, Is.EqualTo("Speichern"));
	}


	/// <summary>Undefined symbols display the default text.</summary>
	[Test]
	public void Localize_UndefinedSymbol()
	{
		_Provider.SetLocaleSymbol(_Label, "app:undefined");
		_ControlTestData.Show(_Form);

		Assert.That(_Label.Text, Is.EqualTo("Default"));

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(_Label.Text, Is.EqualTo("Default"));
	}


	/// <summary>Removing the symbol restores the default text.</summary>
	[Test]
	public void Localize_RemoveSymbolRestoresDefault()
	{
		_Provider.SetLocaleSymbol(_Label, "app:save");
		Locale.Current = _ControlTestData.Get("de");
		_ControlTestData.Show(_Form);
		Assert.That(_Label.Text, Is.EqualTo("Speichern"));

		_Provider.SetLocaleSymbol(_Label, string.Empty);
		Assert.That(_Label.Text, Is.EqualTo("Default"));

		Locale.Current = _ControlTestData.Get("en");
		Assert.That(_Label.Text, Is.EqualTo("Default"));
	}


	/// <summary>Symbols set after the first localization are applied immediately.</summary>
	[Test]
	public void Localize_SymbolAfterFirstLocalization()
	{
		_ControlTestData.Show(_Form);

		_Provider.SetLocaleSymbol(_Label, "app:save");
		Assert.That(_Label.Text, Is.EqualTo("Save"));

		_Provider.SetLocaleSymbol(_Label, "app:open");
		Assert.That(_Label.Text, Is.EqualTo("Open"));
	}


	/// <summary>Accessible names and descriptions are localized.</summary>
	[Test]
	public void Localize_Accessibility()
	{
		_Label.AccessibleName = "Default name";
		_Provider.SetAccessibleNameLocaleSymbol(_Label, "app:name");
		_Provider.SetAccessibleDescriptionLocaleSymbol(_Label, "app:desc");
		_ControlTestData.Show(_Form);

		Assert.That(_Label.AccessibleName, Is.EqualTo("Name"));
		Assert.That(_Label.AccessibleDescription, Is.EqualTo("Description"));
		Assert.That(_Label.Text, Is.EqualTo("Default"));

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(_Label.AccessibleName, Is.EqualTo("Name DE"));
		Assert.That(_Label.AccessibleDescription, Is.EqualTo("Beschreibung"));

		_Provider.SetAccessibleNameLocaleSymbol(_Label, null);
		Assert.That(_Label.AccessibleName, Is.EqualTo("Default name"));
	}


	/// <summary>Tool strip items, column headers and other components with a text property are localized.</summary>
	[Test]
	public void Localize_Components()
	{
		ToolStrip strip = new();
		ToolStripButton button = new("Button");
		strip.Items.Add(button);
		ListView list = new() { View = View.Details };
		ColumnHeader column = list.Columns.Add("Column default");
		using _TextComponent text = new() { Text = "Text default" };
		_Form.Controls.AddRange(new Control[] { strip, list });

		_Provider.SetLocaleSymbol(button, "app:save");
		_Provider.SetLocaleSymbol(column, "app:column");
		_Provider.SetLocaleSymbol(text, "app:open");
		Locale.Current = _ControlTestData.Get("de");
		_ControlTestData.Show(_Form);

		Assert.That(button.Text, Is.EqualTo("Speichern"));
		Assert.That(column.Text, Is.EqualTo("Spalte"));
		Assert.That(text.Text, Is.EqualTo("Öffnen"));
	}


	/// <summary>Disposed components are released.</summary>
	[Test]
	public void Localize_DisposedComponentReleased()
	{
		Label label = new() { Text = "Temporary" };
		_Form.Controls.Add(label);
		_Provider.SetLocaleSymbol(label, "app:save");
		_ControlTestData.Show(_Form);

		label.Dispose();

		Assert.That(_Provider.GetLocaleSymbol(label), Is.Empty);
		Assert.That(() => Locale.Current = _ControlTestData.Get("de"), Throws.Nothing);
	}


	/// <summary>A disposed provider no longer follows the current locale.</summary>
	[Test]
	public void Dispose_StopsUpdates()
	{
		_Provider.SetLocaleSymbol(_Label, "app:save");
		_ControlTestData.Show(_Form);

		_Provider.Dispose();
		Locale.Current = _ControlTestData.Get("de");

		Assert.That(_Label.Text, Is.EqualTo("Save"));
	}


	/// <summary>The Localized event is raised after each localization.</summary>
	[Test]
	public void Localized_IsRaised()
	{
		int count = 0;
		string? text = null;
		_Provider.SetLocaleSymbol(_Label, "app:save");
		_Provider.Localized += (m, n) => { count++; text = _Label.Text; };

		_ControlTestData.Show(_Form);
		Locale.Current = _ControlTestData.Get("de");

		Assert.That(count, Is.EqualTo(2));
		Assert.That(text, Is.EqualTo("Speichern"));
	}


	/// <summary>Localizable items in the controls of the container control are localized.</summary>
	[Test]
	public void LocalizeItems_On()
	{
		ListBox list = new();
		list.Items.Add(new LocalizableItem("app:item", "Item default"));
		Panel panel = new();
		panel.Controls.Add(list);
		_Form.Controls.Add(panel);
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");

		Assert.That(list.Items[0]!.ToString(), Is.EqualTo("Eintrag"));
	}


	/// <summary>Without item localization, localizable items are not localized.</summary>
	[Test]
	public void LocalizeItems_Off()
	{
		ListBox list = new();
		list.Items.Add(new LocalizableItem("app:item", "Item default"));
		_Form.Controls.Add(list);
		_Provider.LocalizeItems = false;
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");

		Assert.That(list.Items[0]!.ToString(), Is.EqualTo("Item"));
	}


	/// <summary>The reading direction of the current locale is applied to the form.</summary>
	[Test]
	public void ApplyRightToLeft_On()
	{
		_Provider.ApplyRightToLeft = true;
		_ControlTestData.Show(_Form);
		Assert.That(_Form.RightToLeft, Is.EqualTo(RightToLeft.No));

		Locale.Current = _ControlTestData.Get("ar");
		Assert.That(_Form.RightToLeft, Is.EqualTo(RightToLeft.Yes));
		Assert.That(_Form.RightToLeftLayout, Is.True);

		Locale.Current = _ControlTestData.Get("en");
		Assert.That(_Form.RightToLeft, Is.EqualTo(RightToLeft.No));
		Assert.That(_Form.RightToLeftLayout, Is.False);
	}


	/// <summary>Without applying the reading direction, the form is not changed.</summary>
	[Test]
	public void ApplyRightToLeft_Off()
	{
		_ControlTestData.Show(_Form);
		Locale.Current = _ControlTestData.Get("ar");

		Assert.That(_Form.RightToLeft, Is.EqualTo(RightToLeft.No));
	}


	/// <summary>A locale change on another thread is marshaled to the thread of the form.</summary>
	[Test]
	public void LocaleChanged_OtherThread()
	{
		_Provider.SetLocaleSymbol(_Label, "app:save");
		_ControlTestData.Show(_Form);

		Task.Run(() => { Locale.Current = _ControlTestData.Get("de"); }).Wait();
		Application.DoEvents();

		Assert.That(_Label.Text, Is.EqualTo("Speichern"));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [class] _TextComponent                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This class implements a component with a text property.</summary>
	private sealed class _TextComponent: Component
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public properties                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets or sets the text.</summary>
		[DefaultValue(null)]
		public string? Text
		{
			get; set;
		}
	}
}
