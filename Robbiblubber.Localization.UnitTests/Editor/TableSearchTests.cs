namespace Robbiblubber.Localization.UnitTests.Editor;

using System.Text.RegularExpressions;

using NUnit.Framework;

using Robbiblubber.Localization.Editor;



/// <summary>This class tests searching and replacing in the rows of a language table.</summary>
[TestFixture]
public class TableSearchTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates the rows of a table.</summary>
	/// <returns>Returns the rows.</returns>
	private static List<_TableRow> _CreateRows()
	{
		return
		[
			new _TableRow("app.open", "Open file"),
			new _TableRow("app.close", "Close the file"),
			new _TableRow("app.name", "Hello $NAME!"),
			new _TableRow("app.multi", @"First line\nSecond line"),
			new _TableRow("app.profile", "Profiles")
		];
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructor                                                                                                      //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>An empty search text is rejected.</summary>
	[Test]
	public void Constructor_EmptyText_Throws()
	{
		Assert.Throws<ArgumentException>(() => new _TableSearch(string.Empty, _SearchScope.BOTH, false, false, false));
	}


	/// <summary>An invalid regular expression is rejected.</summary>
	[Test]
	public void Constructor_InvalidRegularExpression_Throws()
	{
		Assert.Throws<RegexParseException>(() => new _TableSearch("(open", _SearchScope.BOTH, false, false, true));
	}


	/// <summary>Characters with a special meaning in regular expressions are found literally in a plain search.</summary>
	[Test]
	public void Constructor_PlainText_IsEscaped()
	{
		_TableSearch s = new("(open", _SearchScope.BOTH, false, false, false);
		Assert.That(s.IsMatch(new _TableRow("x", "to (open) it"), _TableSearch.TEXT_COLUMN), Is.True);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// find                                                                                                             //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>The symbol of a row is searched before its text, and the rows in their order.</summary>
	[Test]
	public void Find_Forward_SymbolBeforeText()
	{
		List<_TableRow> rows = _CreateRows();
		_TableSearch s = new("close", _SearchScope.BOTH, false, false, false);

		Assert.That(s.Find(rows, -1, 0, false, false), Is.EqualTo(new _TableMatch(1, _TableSearch.SYMBOL_COLUMN, false)));
		Assert.That(s.Find(rows, 1, _TableSearch.SYMBOL_COLUMN, true, false), Is.EqualTo(new _TableMatch(1, _TableSearch.TEXT_COLUMN, false)));
	}


	/// <summary>The current cell is searched first unless it is skipped.</summary>
	[Test]
	public void Find_CurrentCell_SkippedOnlyWhenRequested()
	{
		List<_TableRow> rows = _CreateRows();
		_TableSearch s = new("file", _SearchScope.TEXT, false, false, false);

		Assert.That(s.Find(rows, 0, _TableSearch.TEXT_COLUMN, false, false), Is.EqualTo(new _TableMatch(0, _TableSearch.TEXT_COLUMN, false)));
		Assert.That(s.Find(rows, 0, _TableSearch.TEXT_COLUMN, true, false), Is.EqualTo(new _TableMatch(1, _TableSearch.TEXT_COLUMN, false)));
	}


	/// <summary>At the end of the table, the search continues at the beginning and reports it.</summary>
	[Test]
	public void Find_PastEnd_Wraps()
	{
		List<_TableRow> rows = _CreateRows();
		_TableSearch s = new("open", _SearchScope.SYMBOL, false, false, false);

		Assert.That(s.Find(rows, 3, _TableSearch.SYMBOL_COLUMN, true, false), Is.EqualTo(new _TableMatch(0, _TableSearch.SYMBOL_COLUMN, true)));
	}


	/// <summary>Searching backwards finds the previous cell and continues at the end of the table.</summary>
	[Test]
	public void Find_Backward_FindsPreviousAndWraps()
	{
		List<_TableRow> rows = _CreateRows();
		_TableSearch s = new("file", _SearchScope.BOTH, false, false, false);

		Assert.That(s.Find(rows, 1, _TableSearch.TEXT_COLUMN, true, true), Is.EqualTo(new _TableMatch(0, _TableSearch.TEXT_COLUMN, false)));
		Assert.That(s.Find(rows, 0, _TableSearch.TEXT_COLUMN, true, true), Is.EqualTo(new _TableMatch(4, _TableSearch.TEXT_COLUMN, true)));
		Assert.That(s.Find(rows, -1, 0, false, true), Is.EqualTo(new _TableMatch(4, _TableSearch.TEXT_COLUMN, false)));
	}


	/// <summary>A cell that is the only match is found again after passing the end.</summary>
	[Test]
	public void Find_OnlyMatch_FoundAgain()
	{
		List<_TableRow> rows = _CreateRows();
		_TableSearch s = new("Hello", _SearchScope.BOTH, false, false, false);

		Assert.That(s.Find(rows, 2, _TableSearch.TEXT_COLUMN, true, false), Is.EqualTo(new _TableMatch(2, _TableSearch.TEXT_COLUMN, true)));
	}


	/// <summary>NULL is returned if nothing is found or the table is empty.</summary>
	[Test]
	public void Find_NothingFound_ReturnsNull()
	{
		_TableSearch s = new("missing", _SearchScope.BOTH, false, false, false);

		Assert.That(s.Find(_CreateRows(), 0, 0, false, false), Is.Null);
		Assert.That(s.Find(new List<_TableRow>(), -1, 0, false, false), Is.Null);
	}


	/// <summary>Only the columns within the scope are searched.</summary>
	[Test]
	public void Find_Scope_IsRespected()
	{
		List<_TableRow> rows = _CreateRows();

		Assert.That(new _TableSearch("profile", _SearchScope.SYMBOL, false, false, false).Find(rows, -1, 0, false, false)?.Column, Is.EqualTo(_TableSearch.SYMBOL_COLUMN));
		Assert.That(new _TableSearch("app", _SearchScope.TEXT, false, false, false).Find(rows, -1, 0, false, false), Is.Null);
	}


	/// <summary>Upper and lower case are distinguished only when requested.</summary>
	[Test]
	public void IsMatch_MatchCase()
	{
		_TableRow row = new("x", "Open file");

		Assert.That(new _TableSearch("open", _SearchScope.TEXT, false, false, false).IsMatch(row, _TableSearch.TEXT_COLUMN), Is.True);
		Assert.That(new _TableSearch("open", _SearchScope.TEXT, true, false, false).IsMatch(row, _TableSearch.TEXT_COLUMN), Is.False);
	}


	/// <summary>A whole word must not be part of a longer word; dots and other characters separate words.</summary>
	[Test]
	public void IsMatch_WholeWord()
	{
		_TableSearch s = new("file", _SearchScope.BOTH, false, true, false);

		Assert.That(s.IsMatch(new _TableRow("x", "Open file"), _TableSearch.TEXT_COLUMN), Is.True);
		Assert.That(s.IsMatch(new _TableRow("x", "Profiles"), _TableSearch.TEXT_COLUMN), Is.False);
		Assert.That(s.IsMatch(new _TableRow("menu.file.open", "x"), _TableSearch.SYMBOL_COLUMN), Is.True);
		Assert.That(new _TableSearch("$NAME", _SearchScope.TEXT, false, true, false).IsMatch(new _TableRow("x", "Hello $NAME!"), _TableSearch.TEXT_COLUMN), Is.True);
	}


	/// <summary>Texts are searched in their single-line form.</summary>
	[Test]
	public void IsMatch_EscapedText()
	{
		_TableSearch s = new(@"line\nSecond", _SearchScope.TEXT, true, false, false);
		Assert.That(s.IsMatch(_CreateRows()[3], _TableSearch.TEXT_COLUMN), Is.True);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// replace                                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>All occurrences in a cell are replaced and counted.</summary>
	[Test]
	public void Replace_AllOccurrencesInCell()
	{
		_TableRow row = new("x", "line one, line two");
		_TableSearch s = new("line", _SearchScope.BOTH, false, false, false);

		Assert.That(s.Replace(row, _TableSearch.TEXT_COLUMN, "row"), Is.EqualTo(2));
		Assert.That(row.Text, Is.EqualTo("row one, row two"));
		Assert.That(row.Symbol, Is.EqualTo("x"));
	}


	/// <summary>A column outside the scope is not changed.</summary>
	[Test]
	public void Replace_OutsideScope_ReturnsZero()
	{
		_TableRow row = new("app.open", "Open");
		_TableSearch s = new("open", _SearchScope.TEXT, true, false, false);

		Assert.That(s.Replace(row, _TableSearch.SYMBOL_COLUMN, "close"), Is.EqualTo(0));
		Assert.That(row.Symbol, Is.EqualTo("app.open"));
	}


	/// <summary>In a plain search, the replacement is inserted as it is, including dollar signs.</summary>
	[Test]
	public void Replace_PlainText_InsertsLiteral()
	{
		_TableRow row = new("x", "Hello NAME");
		new _TableSearch("NAME", _SearchScope.TEXT, true, false, false).Replace(row, _TableSearch.TEXT_COLUMN, "$1 $NAME");

		Assert.That(row.Text, Is.EqualTo("Hello $1 $NAME"));
	}


	/// <summary>With a regular expression, the replacement can refer to groups.</summary>
	[Test]
	public void Replace_RegularExpression_UsesGroups()
	{
		_TableRow row = new("menu.file.open", "x");
		new _TableSearch(@"^menu\.(\w+)\.(\w+)$", _SearchScope.SYMBOL, true, false, true).Replace(row, _TableSearch.SYMBOL_COLUMN, "$1.$2.menu");

		Assert.That(row.Symbol, Is.EqualTo("file.open.menu"));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// count and replace all                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Occurrences and rows are counted within the scope.</summary>
	[Test]
	public void Count_OccurrencesAndRows()
	{
		List<_TableRow> rows = _CreateRows();

		Assert.That(new _TableSearch("file", _SearchScope.BOTH, false, false, false).Count(rows), Is.EqualTo((4, 3)));
		Assert.That(new _TableSearch("file", _SearchScope.TEXT, false, false, false).Count(rows), Is.EqualTo((3, 3)));
		Assert.That(new _TableSearch("missing", _SearchScope.BOTH, false, false, false).Count(rows), Is.EqualTo((0, 0)));
	}


	/// <summary>All occurrences are replaced; the changed rows are returned.</summary>
	[Test]
	public void ReplaceAll_ReplacesAndReturnsChangedRows()
	{
		List<_TableRow> rows = _CreateRows();
		(List<int> changed, int count) = new _TableSearch("app.", _SearchScope.SYMBOL, false, false, false).ReplaceAll(rows, "led:");

		Assert.That(count, Is.EqualTo(5));
		Assert.That(changed, Is.EqualTo(new[] { 0, 1, 2, 3, 4 }));
		Assert.That(rows.Select(m => m.Symbol), Is.EqualTo(new[] { "led:open", "led:close", "led:name", "led:multi", "led:profile" }));
		Assert.That(rows[0].Text, Is.EqualTo("Open file"));
	}
}
