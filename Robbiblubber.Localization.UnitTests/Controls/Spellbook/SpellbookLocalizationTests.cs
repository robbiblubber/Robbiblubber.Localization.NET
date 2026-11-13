namespace Robbiblubber.Localization.UnitTests.Controls.Spellbook;

using System.Runtime.Versioning;

using NUnit.Framework;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Localization.UnitTests.Library;
using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class tests <see cref="SpellbookLocalization"/>.</summary>
[TestFixture][NonParallelizable][SupportedOSPlatform("windows")]
public class SpellbookLocalizationTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static properties                                                                                        //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the directory of the language tables in the output directory.</summary>
	private static string _LocaleDirectory
	{
		get { return Path.Combine(AppContext.BaseDirectory, "locale"); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test data.</summary>
	[SetUp]
	public void SetUp()
	{
		Locale._Reset();
		_Files = new();
		_ControlTestData.WriteStandard(_Files);
		Locale.Sources.Set(new DirectoryLocalizationSource(_LocaleDirectory), new DirectoryLocalizationSource(_Files.Path));
	}


	/// <summary>Ends the localization and deletes the test data.</summary>
	[TearDown]
	public void TearDown()
	{
		SpellbookLocalization.Disable();
		ThemedMessageBoxTexts.Yes = null;
		_Files.Dispose();
		Locale._Reset();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// test methods                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>The language tables are copied to the directory locale in the output directory.</summary>
	[Test]
	public void CopiesTables()
	{
		foreach(string i in new[] { "msgbox", "themeeditor", "colordialog", "themeselector" })
		{
			foreach(string k in new[] { "de", "en", "fr", "id" })
			{
				Assert.That(File.Exists(Path.Combine(_LocaleDirectory, $"spellbook.{i}.{k}.ltab")), Is.True, $"{i}.{k}");
			}
		}
	}


	/// <summary>The dialogs show the texts of the current locale from the tables of the library.</summary>
	[Test]
	public void LocalizesDialogs()
	{
		SpellbookLocalization.Enable();
		Locale.Current = _ControlTestData.Get("de");

		Assert.That(SpellbookLocalization.Enabled, Is.True);
		Assert.That(ThemedMessageBoxTexts.Yes, Is.EqualTo("&Ja"));
		Assert.That(ThemeEditorTexts.ResetColor, Is.EqualTo("Farbe &zurücksetzen"));
		Assert.That(ThemeEditorTexts.CopyName, Is.EqualTo("{0} (Kopie)"));
		Assert.That(ThemedColorDialogTexts.Hue, Is.EqualTo("Farb&ton:"));
		Assert.That(ThemeSelectorTexts.Dark, Is.EqualTo("Dunkel"));
		Assert.That(ThemeSelectorTexts.GetName(Theme.LIGHT), Is.EqualTo("Hell"));

		Locale.Current = _ControlTestData.Get("en");
		Assert.That(ThemedMessageBoxTexts.Yes, Is.EqualTo("&Yes"));
	}


	/// <summary>Every text of the dialogs is defined in every table of the library.</summary>
	[Test]
	public void DefinesAllTexts()
	{
		SpellbookLocalization.Enable();
		string[] symbols = typeof(ThemedMessageBoxTexts).GetProperties().Select(m => "spellbook:msgbox." + m.Name.ToLowerInvariant())
						   .Concat(typeof(ThemeEditorTexts).GetProperties().Where(m => m.PropertyType == typeof(string)).Select(m => "spellbook:themeeditor." + m.Name.ToLowerInvariant()))
						   .Concat(typeof(ThemedColorDialogTexts).GetProperties().Where(m => m.PropertyType == typeof(string)).Select(m => "spellbook:colordialog." + m.Name.ToLowerInvariant()))
						   .Concat(typeof(ThemeSelectorTexts).GetProperties().Where(m => m.PropertyType == typeof(string)).Select(m => "spellbook:themeselector." + m.Name.ToLowerInvariant()))
						   .Concat(Enum.GetValues<ThemeColor>().Select(m => "spellbook:themeeditor.role." + m.ToString().ToLowerInvariant()))
						   .ToArray();

		foreach(string i in new[] { "de", "en" })
		{
			Locale.Current = _ControlTestData.Get(i);
			Assert.That(symbols.Where(m => Locale.Current!.GetText(m) is null), Is.Empty, i);
		}
	}


	/// <summary>Tables of a later source override the texts of the library.</summary>
	[Test]
	public void ApplicationTablesOverride()
	{
		_Files.WriteTable("spellbook.de.ltab", "de", "spellbook", ("msgbox.yes", "&Jawohl"));
		SpellbookLocalization.Enable();
		Locale.Current = _ControlTestData.Get("de");

		Assert.That(ThemedMessageBoxTexts.Yes, Is.EqualTo("&Jawohl"));
		Assert.That(ThemedMessageBoxTexts.No, Is.EqualTo("&Nein"));
	}


	/// <summary>Explicitly set texts win over the localization.</summary>
	[Test]
	public void ExplicitTextsWin()
	{
		SpellbookLocalization.Enable();
		Locale.Current = _ControlTestData.Get("de");
		ThemedMessageBoxTexts.Yes = "&Sicher";

		Assert.That(ThemedMessageBoxTexts.Yes, Is.EqualTo("&Sicher"));
	}


	/// <summary>Disable ends the localization.</summary>
	[Test]
	public void DisableEndsLocalization()
	{
		SpellbookLocalization.Enable();
		SpellbookLocalization.Disable();
		Locale.Current = _ControlTestData.Get("de");

		Assert.That(SpellbookLocalization.Enabled, Is.False);
		Assert.That(ControlTexts.LookUp, Is.Null);
		Assert.That(ThemedMessageBoxTexts.Yes, Is.EqualTo("&Yes"));
	}
}
