namespace Robbiblubber.Localization.Editor;

using System.Text.RegularExpressions;



/// <summary>This class searches and replaces text in the rows of a language table.</summary>
/// <remarks><para>The cells are searched row by row, the symbol before the text. Texts are searched in their single-line form, as they
///          are shown in the table; the replacement is inserted in this form as well.</para>
///          <para>Every search is carried out with a regular expression: plain search texts are escaped, and a whole word is a match
///          that is neither preceded nor followed by a letter, digit or underscore.</para></remarks>
internal sealed class _TableSearch
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public constants                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Column of the symbol.</summary>
	public const int SYMBOL_COLUMN = 0;

	/// <summary>Column of the text.</summary>
	public const int TEXT_COLUMN = 1;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Number of columns.</summary>
	private const int _COLUMNS = 2;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Maximum time a regular expression may take for a cell.</summary>
	private static readonly TimeSpan _TIMEOUT = TimeSpan.FromSeconds(1);



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Regular expression.</summary>
	private readonly Regex _Regex;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="text">Search text.</param>
	/// <param name="scope">Columns that are searched.</param>
	/// <param name="matchCase">Determines if upper and lower case are distinguished.</param>
	/// <param name="wholeWord">Determines if only whole words are found.</param>
	/// <param name="regularExpression">Determines if the search text is a regular expression.</param>
	/// <exception cref="ArgumentException">Thrown when the search text is empty or not a valid regular expression.</exception>
	public _TableSearch(string text, _SearchScope scope, bool matchCase, bool wholeWord, bool regularExpression)
	{
		if(string.IsNullOrEmpty(text)) { throw new ArgumentException("The search text must not be empty.", nameof(text)); }

		string pattern = (regularExpression ? text : Regex.Escape(text));
		if(wholeWord) { pattern = @"(?<!\w)(?:" + pattern + @")(?!\w)"; }

		_Regex = new Regex(pattern, (RegexOptions.CultureInvariant | (matchCase ? RegexOptions.None : RegexOptions.IgnoreCase)), _TIMEOUT);
		Text = text;
		Scope = scope;
		IsRegularExpression = regularExpression;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the search text.</summary>
	public string Text
	{
		get;
	}


	/// <summary>Gets the columns that are searched.</summary>
	public _SearchScope Scope
	{
		get;
	}


	/// <summary>Gets if the search text is a regular expression.</summary>
	/// <remarks>If it is, the replacement may refer to groups (<c>$1</c>, <c>${name}</c>); otherwise it is inserted as it is.</remarks>
	public bool IsRegularExpression
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns if a cell contains the search text.</summary>
	/// <param name="row">Row.</param>
	/// <param name="column">Column.</param>
	/// <returns>Returns TRUE if the column is searched and contains the search text, otherwise returns FALSE.</returns>
	/// <exception cref="RegexMatchTimeoutException">Thrown when the regular expression takes too long.</exception>
	public bool IsMatch(_TableRow row, int column)
	{
		return (_IsSearched(column) && _Regex.IsMatch(_GetValue(row, column)));
	}


	/// <summary>Finds the next cell that contains the search text.</summary>
	/// <param name="rows">Rows.</param>
	/// <param name="row">Row of the current cell, -1 to start at the beginning (or, searching backwards, at the end).</param>
	/// <param name="column">Column of the current cell.</param>
	/// <param name="skipCurrent">Determines if the search starts after the current cell; otherwise the current cell is searched
	///                           first.</param>
	/// <param name="backward">Determines if the table is searched backwards.</param>
	/// <returns>Returns the cell found, NULL if no cell contains the search text. When the search passes the end of the table, it
	///          continues at the other end, so the current cell is found again if it is the only match.</returns>
	/// <exception cref="RegexMatchTimeoutException">Thrown when the regular expression takes too long.</exception>
	public _TableMatch? Find(IReadOnlyList<_TableRow> rows, int row, int column, bool skipCurrent, bool backward)
	{
		int total = (rows.Count * _COLUMNS);
		if(total == 0) { return null; }

		int start;
		if((row < 0) || (row >= rows.Count))
		{
			start = (backward ? (total - 1) : 0);
		}
		else { start = ((row * _COLUMNS) + Math.Clamp(column, 0, (_COLUMNS - 1)) + (skipCurrent ? (backward ? -1 : 1) : 0)); }

		for(int i = 0; i < total; i++)
		{
			int index = (backward ? (start - i) : (start + i));
			bool wrapped = ((index < 0) || (index >= total));
			index = (((index % total) + total) % total);

			if(IsMatch(rows[index / _COLUMNS], (index % _COLUMNS))) { return new _TableMatch((index / _COLUMNS), (index % _COLUMNS), wrapped); }
		}
		return null;
	}


	/// <summary>Replaces the search text in a cell.</summary>
	/// <param name="row">Row.</param>
	/// <param name="column">Column.</param>
	/// <param name="replacement">Replacement.</param>
	/// <returns>Returns the number of replaced occurrences; 0 if the column is not searched or does not contain the search
	///          text.</returns>
	/// <remarks>All occurrences in the cell are replaced.</remarks>
	/// <exception cref="RegexMatchTimeoutException">Thrown when the regular expression takes too long.</exception>
	public int Replace(_TableRow row, int column, string replacement)
	{
		if(!_IsSearched(column)) { return 0; }

		string value = _GetValue(row, column);
		int rval = _Regex.Count(value);
		if(rval == 0) { return 0; }

		value = _Regex.Replace(value, (IsRegularExpression ? replacement : replacement.Replace("$", "$$")));
		if(column == SYMBOL_COLUMN)
		{
			row.Symbol = value;
		}
		else { row.Text = value; }

		return rval;
	}


	/// <summary>Counts the occurrences of the search text.</summary>
	/// <param name="rows">Rows.</param>
	/// <returns>Returns the number of occurrences and the number of rows that contain them.</returns>
	/// <exception cref="RegexMatchTimeoutException">Thrown when the regular expression takes too long.</exception>
	public (int Count, int Rows) Count(IEnumerable<_TableRow> rows)
	{
		int count = 0, found = 0;

		foreach(_TableRow i in rows)
		{
			int n = 0;
			for(int k = 0; k < _COLUMNS; k++)
			{
				if(_IsSearched(k)) { n += _Regex.Count(_GetValue(i, k)); }
			}
			count += n;
			if(n > 0) { found++; }
		}
		return (count, found);
	}


	/// <summary>Replaces all occurrences of the search text.</summary>
	/// <param name="rows">Rows.</param>
	/// <param name="replacement">Replacement.</param>
	/// <returns>Returns the indexes of the changed rows and the number of replaced occurrences.</returns>
	/// <exception cref="RegexMatchTimeoutException">Thrown when the regular expression takes too long.</exception>
	public (List<int> Rows, int Count) ReplaceAll(IReadOnlyList<_TableRow> rows, string replacement)
	{
		List<int> changed = new();
		int count = 0;

		for(int i = 0; i < rows.Count; i++)
		{
			int n = 0;
			for(int k = 0; k < _COLUMNS; k++) { n += Replace(rows[i], k, replacement); }

			count += n;
			if(n > 0) { changed.Add(i); }
		}
		return (changed, count);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns the value of a cell.</summary>
	/// <param name="row">Row.</param>
	/// <param name="column">Column.</param>
	/// <returns>Returns the symbol or the text.</returns>
	private static string _GetValue(_TableRow row, int column)
	{
		return ((column == SYMBOL_COLUMN) ? row.Symbol : row.Text);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns if a column is searched.</summary>
	/// <param name="column">Column.</param>
	/// <returns>Returns TRUE if the column is within the scope, otherwise returns FALSE.</returns>
	private bool _IsSearched(int column)
	{
		return column switch
		{
			SYMBOL_COLUMN => Scope.HasFlag(_SearchScope.SYMBOL),
			TEXT_COLUMN => Scope.HasFlag(_SearchScope.TEXT),
			_ => false
		};
	}
}
