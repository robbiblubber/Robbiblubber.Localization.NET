namespace Robbiblubber.Localization.UnitTests.Library.Lookup;

using System.Collections.Concurrent;

using NUnit.Framework;



/// <summary>This class tests the localization services of <see cref="Locale"/>.</summary>
[TestFixture][NonParallelizable]
public class LocaleLookupTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;

	/// <summary>Number of raised locale changed events.</summary>
	private int _Changed = 0;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Writes the standard test data and uses the test directory as only source.</summary>
	private void _Standard()
	{
		_Files.WriteLocale("en", "English", isDefault: true);
		_Files.WriteLocale("de", "Deutsch");
		_Files.WriteLocale("de-AT", "Österreichisch", "de");
		_Files.WriteTable("app.en.ltab", "en", "app", ("save", "Save"), ("open", "Open"), ("only.en", "English only"));
		_Files.WriteTable("app.de.ltab", "de", "app", ("save", "Speichern"), ("open", "Öffnen"));
		_Files.WriteTable("app.de-at.ltab", "de-AT", "app", ("open", "Aufmachen"));
		_Files.WriteTable("own.de.ltab", "de", null, ("title", "Titel"));

		Locale.Sources.Set(new DirectoryLocalizationSource(_Files.Path));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test directory and resets the localization services.</summary>
	[SetUp]
	public void SetUp()
	{
		_Files = new();
		_Changed = 0;
		Locale._Reset();
		Locale.LocaleChanged += _Locale_Changed;
	}


	/// <summary>Deletes the test directory and resets the localization services.</summary>
	[TearDown]
	public void TearDown()
	{
		Locale.LocaleChanged -= _Locale_Changed;
		Locale._Reset();
		_Files.Dispose();
	}


	/// <summary>The locales are ordered by name; the default locale is current and fallback.</summary>
	[Test]
	public void Locales_OrderAndDefault()
	{
		_Standard();

		Assert.That(Locale.Locales.Keys, Is.EqualTo(new[] { "de", "en", "de-AT" }));
		Assert.That(Locale.Locales.Default!.Key, Is.EqualTo("en"));
		Assert.That(Locale.Current!.Key, Is.EqualTo("en"));
		Assert.That(Locale.Fallback!.Key, Is.EqualTo("en"));
		Assert.That(Locale.Issues, Is.Empty);
	}


	/// <summary>The collection finds locales by exact key regardless of case, by index and by locale.</summary>
	[Test]
	public void Locales_Access()
	{
		_Standard();
		LocaleCollection c = Locale.Locales;

		Assert.That(c["DE-at"]!.Name, Is.EqualTo("Österreichisch"));
		Assert.That(c["d"], Is.Null);
		Assert.That(c[null], Is.Null);
		Assert.That(c[1].Key, Is.EqualTo("en"));
		Assert.That(() => c[3], Throws.TypeOf<ArgumentOutOfRangeException>());
		Assert.That(c.IndexOf(c["de-at"]), Is.EqualTo(2));
		Assert.That(c.IndexOf(null), Is.EqualTo(-1));
		Assert.That(c.Contains("EN"), Is.True);
		Assert.That(c.Count, Is.EqualTo(3));
		Assert.That(c.Select(m => m.Key), Is.EqualTo(c.Keys));
	}


	/// <summary>Without a locale marked as default, the first locale is the default.</summary>
	[Test]
	public void Locales_DefaultWithoutMark()
	{
		_Files.WriteLocale("fr", "Français");
		_Files.WriteLocale("de", "Deutsch");
		Locale.Sources.Set(new DirectoryLocalizationSource(_Files.Path));

		Assert.That(Locale.Current!.Key, Is.EqualTo("de"));
	}


	/// <summary>Lookups use the current locale, the prefix and tables without prefix.</summary>
	[Test]
	public void LookUp_CurrentLocale()
	{
		_Standard();
		Locale.Current = Locale.Locales["de"];

		Assert.That(Locale.LookUp("app:save", "x"), Is.EqualTo("Speichern"));
		Assert.That("app:open".LookUp("x"), Is.EqualTo("Öffnen"));
		Assert.That(Locale.LookUp("title", "x"), Is.EqualTo("Titel"));
		Assert.That(Locale.LookUp("save", "x"), Is.EqualTo("x"));
		Assert.That(Locale.Current!.GetText("app:save"), Is.EqualTo("Speichern"));
		Assert.That(Locale.Locales["en"]!.GetText("app:save"), Is.EqualTo("Save"));
		Assert.That(Locale.Locales["en"]!.GetText("title"), Is.Null);
	}


	/// <summary>A locale overrides texts of its base locale and inherits the others.</summary>
	[Test]
	public void LookUp_BaseLocale()
	{
		_Standard();
		Locale.Current = Locale.Locales["de-at"];

		Assert.That(Locale.LookUp("app:open", "x"), Is.EqualTo("Aufmachen"));
		Assert.That(Locale.LookUp("app:save", "x"), Is.EqualTo("Speichern"));
		Assert.That(Locale.LookUp("title", "x"), Is.EqualTo("Titel"));
		Assert.That(Locale.Issues, Is.Empty);
	}


	/// <summary>The fallback locale is used without default text, or when the fallback is preferred.</summary>
	[Test]
	public void LookUp_Fallback()
	{
		_Standard();
		Locale.Current = Locale.Locales["de"];

		Assert.That(Locale.LookUp("app:only.en"), Is.EqualTo("English only"));
		Assert.That(Locale.LookUp("app:only.en", "Default"), Is.EqualTo("Default"));

		Locale.PreferFallback = true;
		Assert.That(Locale.LookUp("app:only.en", "Default"), Is.EqualTo("English only"));

		Assert.That(Locale.LookUp("app:missing"), Is.Empty);
		Assert.That(Locale.LookUp(null, "Default"), Is.EqualTo("Default"));
	}


	/// <summary>TryLookUp finds texts of the current and fallback locale.</summary>
	[Test]
	public void TryLookUp()
	{
		_Standard();
		Locale.Current = Locale.Locales["de"];

		Assert.That(Locale.TryLookUp("app:save", out string? t), Is.True);
		Assert.That(t, Is.EqualTo("Speichern"));
		Assert.That(Locale.TryLookUp("app:only.en", out t), Is.True);
		Assert.That(t, Is.EqualTo("English only"));
		Assert.That(Locale.TryLookUp("app:missing", out t), Is.False);
		Assert.That(t, Is.Null);
		Assert.That(Locale.TryLookUp(null, out _), Is.False);
	}


	/// <summary>Placeholders are replaced by the longest name, values are not scanned again.</summary>
	[Test]
	public void LookUp_Placeholders()
	{
		_Standard();

		Assert.That(Locale.LookUp("missing", "$NAME has $NAMES; $X $$", ("NAME", "A$NAMES"), ("NAMES", 3)),
					Is.EqualTo("A$NAMES has 3; $X $$"));
		Assert.That("missing".LookUp("$N items", ("N", 2.5)), Is.EqualTo("2.5 items"));
		Assert.That(Locale.LookUp("missing", "no placeholders"), Is.EqualTo("no placeholders"));
	}


	/// <summary>Changing the current locale raises the event only for real changes.</summary>
	[Test]
	public void Current_RaisesEventOnChange()
	{
		_Standard();

		Locale.Current = Locale.Locales["en"];
		Assert.That(_Changed, Is.Zero);

		Locale.Current = Locale.Locales["de"];
		Assert.That(_Changed, Is.EqualTo(1));

		Locale.Current = Locale.Locales["DE"];
		Assert.That(_Changed, Is.EqualTo(1));

		Locale.Current = null;
		Assert.That(_Changed, Is.EqualTo(2));
		Assert.That(Locale.Current!.Key, Is.EqualTo("en"));

		Locale.Fallback = Locale.Locales["de"];
		Assert.That(_Changed, Is.EqualTo(3));
		Assert.That(Locale.Fallback!.Key, Is.EqualTo("de"));
	}


	/// <summary>A locale that is not available can be selected; its texts come from tables with its key.</summary>
	[Test]
	public void Current_UnknownLocale()
	{
		_Standard();
		_Files.WriteTable("app.fr.ltab", "fr", "app", ("save", "Enregistrer"));
		Locale.Reload();

		Locale.Current = new Robbiblubber.Localization.Definition.LocaleDefinition() { Key = "fr" }.ToLocale();

		Assert.That(Locale.Current!.Key, Is.EqualTo("fr"));
		Assert.That(Locale.LookUp("app:save", "x"), Is.EqualTo("Enregistrer"));
	}


	/// <summary>The current locale stays selected after reloading; new files are found.</summary>
	[Test]
	public void Reload_KeepsSelection()
	{
		_Standard();
		Locale.Current = Locale.Locales["de"];
		_Files.WriteTable("more.de.ltab", "de", "app", ("new", "Neu"));

		Assert.That(Locale.LookUp("app:new", "x"), Is.EqualTo("x"));

		Locale.Reload();

		Assert.That(_Changed, Is.EqualTo(2));
		Assert.That(Locale.Current!.Key, Is.EqualTo("de"));
		Assert.That(Locale.LookUp("app:new", "x"), Is.EqualTo("Neu"));
	}


	/// <summary>Changing the sources after loading raises the event and reloads.</summary>
	[Test]
	public void Sources_ChangeReloads()
	{
		_Standard();
		Assert.That(Locale.Locales, Has.Count.EqualTo(3));

		using(_TestFiles other = new())
		{
			other.WriteLocale("it", "Italiano");
			Locale.Sources.Add(new DirectoryLocalizationSource(other.Path));

			Assert.That(_Changed, Is.EqualTo(1));
			Assert.That(Locale.Locales.Keys, Does.Contain("it"));
		}

		Assert.That(() => Locale.Sources.Add(null!), Throws.ArgumentNullException);
	}


	/// <summary>Duplicate symbols on the same level and unreadable files are reported; the rest still works.</summary>
	[Test]
	public void Issues_DuplicatesAndBrokenFiles()
	{
		_Standard();
		_Files.WriteTable("zz.de.ltab", "de", "app", ("save", "Sichern"));
		File.WriteAllText(_Files.File("broken.locale"), "no zip");
		File.WriteAllText(_Files.File("broken.cltab"), "no gzip");
		_Files.WriteTable("nolang.ltab", "", "app", ("x", "y"));
		_Files.WriteLocale("de", "Doppelt", fileName: "zz-de.locale");
		Locale.Reload();
		Locale.Current = Locale.Locales["de"];

		Assert.That(Locale.Locales, Has.Count.EqualTo(3));
		Assert.That(Locale.Locales["de"]!.Name, Is.EqualTo("Deutsch"));
		Assert.That(Locale.LookUp("app:save", "x"), Is.EqualTo("Sichern"));
		Assert.That(Locale.Issues.Select(m => Path.GetFileName(m.Resource)),
					Is.EquivalentTo(new[] { "broken.cltab", "broken.locale", "nolang.ltab", "zz-de.locale", "zz.de.ltab" }));
		Assert.That(Locale.Issues.Single(m => m.Resource.EndsWith("broken.locale")).Exception, Is.Not.Null);
	}


	/// <summary>Several prefixes provide the symbols under each prefix.</summary>
	[Test]
	public void LookUp_SeveralPrefixes()
	{
		_Files.WriteLocale("en", "English");
		Robbiblubber.Localization.Definition.LanguageTable t = new() { LocaleKey = "en" };
		t.Prefixes.AddRange(["a", "b"]);
		t.Entries.Add(new("x", "X"));
		t.Save(_Files.File("t.ltab"));
		Locale.Sources.Set(new DirectoryLocalizationSource(_Files.Path));

		Assert.That(Locale.LookUp("a:x", "-") + Locale.LookUp("b:x", "-") + Locale.LookUp("x", "-"), Is.EqualTo("XX-"));
	}


	/// <summary>Embedded locales and tables of the original format are used; a cycle of base locales does not hang.</summary>
	[Test]
	public void EmbeddedSource_OriginalFiles()
	{
		_Files.WriteLocale("a", "A", "b");
		_Files.WriteLocale("b", "B", "a");
		_Files.WriteTable("a.ltab", "a", null, ("x", "a"));
		_Files.WriteTable("b.ltab", "b", null, ("x", "b"), ("y", "b"));
		Locale.Sources.Set(new EmbeddedLocalizationSource(typeof(LocaleLookupTests).Assembly),
						   new DirectoryLocalizationSource(_Files.Path));

		Assert.That(Locale.Current!.Key, Is.EqualTo("en"));
		Assert.That(Locale.Locales["de"]!.HasSmallIcon, Is.True);

		Locale.Current = new Robbiblubber.Localization.Definition.LocaleDefinition() { Key = "fr" }.ToLocale();
		Assert.That(Locale.LookUp("led:but.cancel", "x"), Is.EqualTo("&Annuler"));

		Locale.Current = Locale.Locales["a"];
		Assert.That(Locale.LookUp("x") + Locale.LookUp("y"), Is.EqualTo("ab"));
	}


	/// <summary>Without sources there are no locales; lookups return the default text.</summary>
	[Test]
	public void NoSources()
	{
		Locale.Sources.Clear();

		Assert.That(Locale.Locales, Is.Empty);
		Assert.That(Locale.Current, Is.Null);
		Assert.That(Locale.LookUp("a:b", "x"), Is.EqualTo("x"));
	}


	/// <summary>The culture is resolved from the key; unknown keys have no culture.</summary>
	[Test]
	public void Culture()
	{
		_Standard();
		_Files.WriteLocale("zz-xx-yy", "Unknown");
		Locale.Reload();

		Assert.That(Locale.Locales["de-at"]!.Culture!.Name, Is.EqualTo("de-AT"));
		Assert.That(Locale.Locales["zz-xx-yy"]!.Culture, Is.Null);
	}


	/// <summary>Lookups from several threads while the locale changes do not fail.</summary>
	[Test]
	public void LookUp_Concurrent()
	{
		_Standard();
		ConcurrentBag<string> results = new();
		Locale[] locales = Locale.Locales.ToArray();

		Parallel.For(0, 2000, i =>
		{
			if(i % 50 == 0) { Locale.Current = locales[(i / 50) % locales.Length]; }
			results.Add(Locale.LookUp("app:save", "x"));
		});

		Assert.That(results.Distinct(), Is.SubsetOf(new[] { "Save", "Speichern" }));
	}


	/// <summary>Locales are equal by key regardless of case.</summary>
	[Test]
	public void Locale_Equality()
	{
		_Standard();
		Locale a = Locale.Locales["de"]!;
		Locale b = new Robbiblubber.Localization.Definition.LocaleDefinition() { Key = "DE" }.ToLocale();

		Assert.That(a == b, Is.True);
		Assert.That(a.Equals((object) b), Is.True);
		Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
		Assert.That(a != Locale.Locales["en"], Is.True);
		Assert.That(a.ToString(), Is.EqualTo("de"));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Handles the LocaleChanged event of the Locale class.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Locale_Changed(object? sender, EventArgs e)
	{
		_Changed++;
		Assert.That(sender, Is.Null);
	}
}
