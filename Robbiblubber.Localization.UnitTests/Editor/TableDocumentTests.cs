namespace Robbiblubber.Localization.UnitTests.Editor;

using NUnit.Framework;

using Robbiblubber.Localization.Definition;
using Robbiblubber.Localization.Editor;



/// <summary>This class tests the document model of the language table window.</summary>
[TestFixture]
public class TableDocumentTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Test directory.</summary>
	private string _Dir = string.Empty;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// setup                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates the test directory.</summary>
	[SetUp]
	public void SetUp()
	{
		_Dir = Path.Combine(Path.GetTempPath(), "led-tests", Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(_Dir);
	}


	/// <summary>Deletes the test directory.</summary>
	[TearDown]
	public void TearDown()
	{
		if(Directory.Exists(_Dir)) { Directory.Delete(_Dir, true); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Writes a table file.</summary>
	/// <param name="name">File name.</param>
	/// <returns>Returns the full file name.</returns>
	private string _WriteTable(string name)
	{
		LanguageTable t = new() { Key = "sample", LocaleKey = "de", Name = "Sample", Version = "2.1", Author = "Alex" };
		t.Prefixes.Add("sample");
		t.Prefixes.Add("other");
		t.Entries.Add(new("b.text", "Zwei" + Environment.NewLine + "Zeilen"));
		t.Entries.Add(new("a.path", @"C:\Daten\neu"));

		string rval = Path.Combine(_Dir, name);
		t.Save(rval);
		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// tests                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>A new document is untouched and has no entries.</summary>
	[Test]
	public void New_Untouched()
	{
		_TableDocument d = new();

		Assert.That(d.FileName, Is.Null);
		Assert.That(d.IsDirty, Is.False);
		Assert.That(d.IsUntouched, Is.True);
		Assert.That(d.EntryCount, Is.Zero);
		Assert.That(d.Version, Is.EqualTo("1.0"));
	}


	/// <summary>Loading takes over the header, the prefixes and the entries in single-line form.</summary>
	[Test]
	public void Load_Values()
	{
		string file = _WriteTable("sample-de.ltab");
		_TableDocument d = _TableDocument.Load(file);

		Assert.That(d.FileName, Is.EqualTo(file));
		Assert.That((d.Key, d.LocaleKey, d.Name, d.Version, d.Author), Is.EqualTo(("sample", "de", "Sample", "2.1", "Alex")));
		Assert.That(d.Prefixes, Is.EqualTo("sample; other"));
		Assert.That(d.Rows.Select(m => m.Symbol), Is.EquivalentTo(new[] { "a.path", "b.text" }));
		Assert.That(d.Rows.First(m => m.Symbol == "b.text").Text, Is.EqualTo(@"Zwei\nZeilen"));
		Assert.That(d.Rows.First(m => m.Symbol == "a.path").Text, Is.EqualTo(@"C:\Daten\\neu"));
		Assert.That(d.IsDirty, Is.False);
		Assert.That(d.IsUntouched, Is.False);
	}


	/// <summary>Changes are detected and undoing them by hand clears the state.</summary>
	[Test]
	public void IsDirty_Changes()
	{
		_TableDocument d = _TableDocument.Load(_WriteTable("t.ltab"));

		d.Name = "Changed";
		Assert.That(d.IsDirty, Is.True);
		d.Name = "Sample";
		Assert.That(d.IsDirty, Is.False);

		d.Rows[0].Text = "x";
		Assert.That(d.IsDirty, Is.True);
	}


	/// <summary>Adding and removing rows is a change; an empty new row is not.</summary>
	[Test]
	public void IsDirty_Rows()
	{
		_TableDocument d = new();

		d.Rows.Add(new _TableRow());
		Assert.That(d.IsDirty, Is.False);

		d.Rows[0].Symbol = "s";
		Assert.That(d.IsDirty, Is.True);

		d.Rows.RemoveAt(0);
		Assert.That(d.IsDirty, Is.False);
	}


	/// <summary>Saving writes the table, keeps further prefixes and clears the changes.</summary>
	[Test]
	public void Save_Roundtrip()
	{
		_TableDocument d = _TableDocument.Load(_WriteTable("t.ltab"));
		d.Rows.Add(new _TableRow("c.tab", @"A\tB"));
		d.Prefixes = "renamed, other ;; extra";

		string file = Path.Combine(_Dir, "out.cltab");
		d.Save(file);

		Assert.That(d.IsDirty, Is.False);
		Assert.That(d.FileName, Is.EqualTo(file));

		LanguageTable t = LanguageTable.Load(file);
		Assert.That(t.Prefixes, Is.EqualTo(new[] { "renamed", "other", "extra" }));
		Assert.That(t.Entries.First(m => m.Symbol == "c.tab").Text, Is.EqualTo("A\tB"));
		Assert.That(t.Entries.First(m => m.Symbol == "b.text").Text, Is.EqualTo("Zwei" + Environment.NewLine + "Zeilen"));
		Assert.That(t.Entries.First(m => m.Symbol == "a.path").Text, Is.EqualTo(@"C:\Daten\neu"));
	}


	/// <summary>A new table cannot be saved without file name.</summary>
	[Test]
	public void Save_NoFileName()
	{
		Assert.Throws<InvalidOperationException>(() => new _TableDocument().Save());
	}


	/// <summary>A file that is not a language table is reported as format error.</summary>
	[Test]
	public void Load_InvalidFile()
	{
		string file = Path.Combine(_Dir, "broken.ltab");
		File.WriteAllText(file, "data { a = 1; ");

		Assert.Throws<FormatException>(() => _TableDocument.Load(file));
		Assert.Throws<FileNotFoundException>(() => _TableDocument.Load(Path.Combine(_Dir, "missing.ltab")));
	}


	/// <summary>Blank rows are skipped, symbols are trimmed.</summary>
	[Test]
	public void ToTable_SkipsBlankRows()
	{
		_TableDocument d = new();
		d.Rows.Add(new _TableRow(" a ", "A"));
		d.Rows.Add(new _TableRow("", ""));
		d.Rows.Add(new _TableRow("b", ""));

		LanguageTable t = d.ToTable();
		Assert.That(t.Entries.Select(m => m.Symbol), Is.EqualTo(new[] { "a", "b" }));
		Assert.That(d.EntryCount, Is.EqualTo(2));
	}


	/// <summary>Rows without symbol and duplicate symbols are reported as problems.</summary>
	[Test]
	public void GetProblem_Rows()
	{
		_TableDocument d = new();
		d.Rows.Add(new _TableRow("a", "1"));
		d.Rows.Add(new _TableRow("a ", "2"));
		d.Rows.Add(new _TableRow("", "text"));
		d.Rows.Add(new _TableRow("b", "3"));
		d.Rows.Add(new _TableRow("", ""));

		HashSet<string> duplicates = d.GetDuplicateSymbols();
		Assert.That(duplicates, Is.EquivalentTo(new[] { "a" }));
		Assert.That(d.Rows.Select(m => _TableDocument.GetProblem(m, duplicates)),
					Is.EqualTo(new _RowProblem?[] { _RowProblem.DUPLICATE, _RowProblem.DUPLICATE, _RowProblem.NO_SYMBOL, null, null }));
	}


	/// <summary>NULL values from the table are stored as empty strings.</summary>
	[Test]
	public void TableRow_Null()
	{
		_TableRow r = new() { Symbol = null!, Text = null! };
		Assert.That((r.Symbol, r.Text), Is.EqualTo((string.Empty, string.Empty)));
	}


	/// <summary>Prefixes are separated by semicolons or commas; blanks, empty and repeated entries are removed.</summary>
	[Test]
	public void ParsePrefixes()
	{
		Assert.That(_TableDocument.ParsePrefixes(" a; b ,c;;a, "), Is.EqualTo(new[] { "a", "b", "c" }));
		Assert.That(_TableDocument.ParsePrefixes("  "), Is.Empty);
		Assert.That(_TableDocument.ParsePrefixes(null), Is.Empty);
	}


	/// <summary>Changing only the separators of the prefixes is not a change of the table.</summary>
	[Test]
	public void Prefixes_SeparatorsAreNoChange()
	{
		_TableDocument d = _TableDocument.Load(_WriteTable("p.ltab"));

		d.Prefixes = "sample,other";
		Assert.That(d.IsDirty, Is.False);

		d.Prefixes = "other; sample";
		Assert.That(d.IsDirty, Is.True);
	}
}
