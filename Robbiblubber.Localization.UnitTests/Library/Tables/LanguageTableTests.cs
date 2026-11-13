namespace Robbiblubber.Localization.UnitTests.Library.Tables;

using NUnit.Framework;

using Robbiblubber.Localization.Definition;
using Robbiblubber.Spellbook;



/// <summary>This class tests language tables.</summary>
[TestFixture]
public class LanguageTableTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a sample table.</summary>
	/// <returns>Returns the table.</returns>
	private static LanguageTable _Sample()
	{
		LanguageTable rval = new() { Key = "lt.app", LocaleKey = "de", Version = "1.0", Name = "app.de", Author = "robbiblubber.org" };
		rval.Prefixes.Add("app");
		rval.Entries.Add(new("mw.save", "Speichern"));
		rval.Entries.Add(new("mw.path", @"C:\neu\tabelle"));
		rval.Entries.Add(new("mw.special", "  a = b; {c}, d/e // f\r\nZeile\t2 "));
		rval.Entries.Add(new("path/with/slash", "x"));

		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test directory.</summary>
	[SetUp]
	public void SetUp()
	{
		_Files = new();
	}


	/// <summary>Deletes the test directory.</summary>
	[TearDown]
	public void TearDown()
	{
		_Files.Dispose();
	}


	/// <summary>Saving and loading keeps header and entries, including special characters.</summary>
	[TestCase("app.ltab")]
	[TestCase("app.cltab")]
	public void SaveAndLoad_RoundTrip(string fileName)
	{
		string file = _Files.File(fileName);
		_Sample().Save(file);

		LanguageTable t = LanguageTable.Load(file);

		Assert.That(t.Key, Is.EqualTo("lt.app"));
		Assert.That(t.LocaleKey, Is.EqualTo("de"));
		Assert.That(t.Version, Is.EqualTo("1.0"));
		Assert.That(t.Name, Is.EqualTo("app.de"));
		Assert.That(t.Author, Is.EqualTo("robbiblubber.org"));
		Assert.That(t.Prefixes, Is.EqualTo(new[] { "app" }));
		Assert.That(t.Entries.Select(m => m.Symbol + "=" + m.Text),
					Is.EquivalentTo(_Sample().Entries.Select(m => m.Symbol + "=" + m.Text.ReplaceLineEndings())));
	}


	/// <summary>A compressed table is not readable as plain ddn.</summary>
	[Test]
	public void Save_CompressedByExtension()
	{
		string file = _Files.File("app.cltab");
		_Sample().Save(file);

		Assert.That(File.ReadAllText(file), Does.Not.Contain("Speichern"));
		Assert.That(LanguageTable.Load(file).Entries, Has.Count.EqualTo(4));
	}


	/// <summary>Streams can be written and read compressed and uncompressed.</summary>
	[TestCase(false)]
	[TestCase(true)]
	public void SaveAndLoad_Stream(bool compressed)
	{
		using(MemoryStream s = new())
		{
			_Sample().Save(s, compressed);
			s.Position = 0;

			Assert.That(LanguageTable.Load(s, compressed).Entries.Single(m => m.Symbol == "mw.save").Text, Is.EqualTo("Speichern"));
		}
	}


	/// <summary>The original table of the editor (format 3.0) can be read.</summary>
	[Test]
	public void Load_OriginalFormat()
	{
		using(Stream s = typeof(LanguageTableTests).Assembly.GetManifestResourceStream("Robbiblubber.Localization.UnitTests.TestData.led-fr.ltab")!)
		{
			LanguageTable t = LanguageTable.Load(s);

			Assert.That(t.LocaleKey, Is.EqualTo("fr"));
			Assert.That(t.Prefixes, Is.EqualTo(new[] { "led" }));
			Assert.That(t.Entries.Single(m => m.Symbol == "ap.version").Text, Is.EqualTo("Version"));
			Assert.That(t.Entries.Single(m => m.Symbol == "but.cancel").Text, Is.EqualTo("&Annuler"));
		}
	}


	/// <summary>Several prefixes are read; empty data and missing header values do not fail.</summary>
	[Test]
	public void FromDdn_SeveralPrefixesAndEmptyData()
	{
		LanguageTable t = LanguageTable.FromDdn(Ddn.Create("data = \\0; header { lang = de; prefixes = a, b; }"));

		Assert.That(t.Prefixes, Is.EqualTo(new[] { "a", "b" }));
		Assert.That(t.Entries, Is.Empty);
		Assert.That(t.Name, Is.Empty);
	}


	/// <summary>Lookup entries carry each prefix; without prefix the symbol is used as it is.</summary>
	[Test]
	public void GetLookupEntries_Prefixes()
	{
		LanguageTable t = new();
		t.Entries.Add(new("x", "1"));

		Assert.That(t.GetLookupEntries().Select(m => m.Key), Is.EqualTo(new[] { "x" }));

		t.Prefixes.AddRange(["a", "b"]);
		Assert.That(t.GetLookupEntries().Select(m => m.Key), Is.EqualTo(new[] { "a:x", "b:x" }));
	}


	/// <summary>Entries with empty symbols are not written; the last of duplicate symbols wins.</summary>
	[Test]
	public void ToDdn_SkipsEmptyAndDuplicates()
	{
		LanguageTable t = new() { LocaleKey = "de" };
		t.Entries.Add(new(" ", "leer"));
		t.Entries.Add(new("a", "1"));
		t.Entries.Add(new("a", "2"));

		LanguageTable r = LanguageTable.FromDdn(Ddn.Create(t.ToDdn().Text));

		Assert.That(r.Entries.Select(m => m.Symbol + "=" + m.Text), Is.EqualTo(new[] { "a=2" }));
	}


	/// <summary>Entries sort by symbol.</summary>
	[Test]
	public void Entries_Sort()
	{
		List<LanguageTableEntry> l = [new("b", ""), new("a", ""), new("B", "")];
		l.Sort();

		Assert.That(l.Select(m => m.Symbol), Is.EqualTo(new[] { "B", "a", "b" }));
	}
}
