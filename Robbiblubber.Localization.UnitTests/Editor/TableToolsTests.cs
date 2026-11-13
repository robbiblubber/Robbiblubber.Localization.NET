namespace Robbiblubber.Localization.UnitTests.Editor;

using System.ComponentModel;

using NUnit.Framework;

using Robbiblubber.Localization.Definition;
using Robbiblubber.Localization.Editor;



/// <summary>This class tests the table tools of the editor: sorting, importing and the checks for prefixes and for entries that
///          occur more than once in a file.</summary>
[TestFixture]
public class TableToolsTests
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
	/// <summary>Creates a document with rows.</summary>
	/// <param name="symbols">Symbols; each row gets the text <c>t:</c> and the symbol.</param>
	/// <returns>Returns the document.</returns>
	private static _TableDocument _Create(params string[] symbols)
	{
		_TableDocument rval = new();
		foreach(string i in symbols) { rval.Rows.Add(new _TableRow(i, "t:" + i)); }
		return rval;
	}


	/// <summary>Writes a text file.</summary>
	/// <param name="name">File name.</param>
	/// <param name="text">Content.</param>
	/// <returns>Returns the full file name.</returns>
	private string _Write(string name, string text)
	{
		string rval = Path.Combine(_Dir, name);
		File.WriteAllText(rval, text);
		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// sort                                                                                                             //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Rows are sorted by character code, digit sequences by value; rows without symbol go to the end.</summary>
	[Test]
	public void Sort_OrdersLikeFile()
	{
		_TableDocument d = _Create("b", "item10", "", "B", "a", "item2", "c");
		d.Rows[2].Text = "no symbol";

		Assert.That(d.Sort(), Is.True);
		Assert.That(d.Rows.Select(m => m.Symbol), Is.EqualTo(new[] { "B", "a", "b", "c", "item2", "item10", "" }));
		Assert.That(d.Rows.Select(m => m.Text).Last(), Is.EqualTo("no symbol"));
	}


	/// <summary>A sorted table shows the rows in the order they have after saving and opening the table again.</summary>
	[Test]
	public void Sort_SameOrderAsReopened()
	{
		_TableDocument d = _Create("zeta", "Alpha", "x10", "x9", "alpha", "m.b", "m.a");
		string file = Path.Combine(_Dir, "t.ltab");
		d.Save(file);

		d.Sort();
		Assert.That(d.Rows.Select(m => m.Symbol), Is.EqualTo(_TableDocument.Load(file).Rows.Select(m => m.Symbol)));
	}


	/// <summary>Sorting is no change of the document; a sorted table returns FALSE.</summary>
	[Test]
	public void Sort_IsNoChange()
	{
		_TableDocument d = _Create("b", "a");
		d.Save(Path.Combine(_Dir, "t.ltab"));

		Assert.That(d.Sort(), Is.True);
		Assert.That(d.IsDirty, Is.False);
		Assert.That(d.Sort(), Is.False);
	}


	/// <summary>The list raises a single reset event.</summary>
	[Test]
	public void Sort_RaisesSingleReset()
	{
		_TableDocument d = _Create("c", "b", "a");
		List<ListChangedType> events = new();
		d.Rows.ListChanged += (s, e) => events.Add(e.ListChangedType);

		d.Sort();
		Assert.That(events, Is.EqualTo(new[] { ListChangedType.Reset }));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// import                                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Only entries whose symbol is missing are appended, in the order of the source; existing rows stay unchanged.</summary>
	[Test]
	public void Import_AppendsMissingEntries()
	{
		_TableDocument d = _Create("a", "b ");
		_TableDocument source = _Create("d", " a", "c", "b", "", "d");
		source.Rows[0].Text = "first d";

		Assert.That(d.Import(source), Is.EqualTo(new[] { 2, 3 }));
		Assert.That(d.Rows.Select(m => (m.Symbol, m.Text)), Is.EqualTo(new[] { ("a", "t:a"), ("b ", "t:b "), ("d", "first d"), ("c", "t:c") }));
	}


	/// <summary>Nothing is appended if all symbols exist.</summary>
	[Test]
	public void Import_NothingMissing()
	{
		_TableDocument d = _Create("a", "b");
		Assert.That(d.Import(_Create("b", "a")), Is.Empty);
		Assert.That(d.Rows, Has.Count.EqualTo(2));
	}


	/// <summary>Texts of a loaded table, also of a compressed one, are taken over in their single-line form.</summary>
	[Test]
	public void Import_FromCompressedFile()
	{
		LanguageTable t = new() { Key = "src", LocaleKey = "en" };
		t.Prefixes.Add("other");
		t.Entries.Add(new("multi", "One" + Environment.NewLine + "Two"));
		string file = Path.Combine(_Dir, "src.cltab");
		t.Save(file);

		_TableDocument d = new() { Prefixes = "led" };
		d.Import(_TableDocument.Load(file));

		Assert.That(d.Rows.Single().Text, Is.EqualTo(@"One\nTwo"));
		Assert.That(d.Prefixes, Is.EqualTo("led"));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// prefix check                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>A table has a prefix if at least one prefix is entered.</summary>
	[Test]
	public void HasPrefix()
	{
		Assert.That(new _TableDocument() { Prefixes = " ; ," }.HasPrefix, Is.False);
		Assert.That(new _TableDocument() { Prefixes = "led" }.HasPrefix, Is.True);
	}


	/// <summary>A symbol with a prefix is a problem only in a table with a prefix; a duplicate is reported first.</summary>
	[Test]
	public void GetProblem_Prefix()
	{
		_TableRow row = new("led:menu.file", "File");
		HashSet<string> none = new();

		Assert.That(_TableDocument.GetProblem(row, none, true), Is.EqualTo(_RowProblem.PREFIX));
		Assert.That(_TableDocument.GetProblem(row, none, false), Is.Null);
		Assert.That(_TableDocument.GetProblem(row, ["led:menu.file"], true), Is.EqualTo(_RowProblem.DUPLICATE));
		Assert.That(_TableDocument.GetProblem(new _TableRow("menu.file", "File"), none, true), Is.Null);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// duplicates in the file                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Entries that occur more than once in the file are reported; the last value is shown.</summary>
	[Test]
	public void Load_ReportsFileDuplicates()
	{
		string file = _Write("dup.ltab", "cid = lang.def;\nversion = 3.0;\nheader\n{\n    key = k;\n    key = k2;\n}\n" +
										 "data\n{\n    b = 1;\n    a = 2;\n    b = 3;\n    c = 4;\n    a = 5;\n}\n");
		_TableDocument d = _TableDocument.Load(file);

		Assert.That(d.FileDuplicates, Is.EqualTo(new[] { "a", "b" }));
		Assert.That(d.Rows.Single(m => m.Symbol == "b").Text, Is.EqualTo("3"));
		Assert.That(d.IsDirty, Is.False);
	}


	/// <summary>New tables and files without duplicates report none.</summary>
	[Test]
	public void FileDuplicates_None()
	{
		_TableDocument d = _Create("a", "b");
		Assert.That(d.FileDuplicates, Is.Empty);

		string file = Path.Combine(_Dir, "t.ltab");
		d.Save(file);
		Assert.That(_TableDocument.Load(file).FileDuplicates, Is.Empty);
	}
}
