namespace Robbiblubber.Localization.Editor;

using System.ComponentModel;

using Robbiblubber.Localization.Definition;
using Robbiblubber.Spellbook;



/// <summary>This class represents a language table that is edited in a window.</summary>
/// <remarks>The document keeps the header values and the rows of the table in the form they are edited in.</remarks>
internal sealed class _TableDocument: _EditorDocument
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public constants                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Separator used to show the prefixes of a table.</summary>
	public const string PREFIX_SEPARATOR = "; ";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Content identifier of language tables.</summary>
	private const string _CID = "lang.def";

	/// <summary>Separator between prefix and symbol.</summary>
	private const char _PREFIX_SEPARATOR = ':';



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Table as it was loaded or saved.</summary>
	private byte[] _Saved;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new, empty document.</summary>
	public _TableDocument(): this(new LanguageTable() { Version = "1.0" }, null)
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="table">Table.</param>
	/// <param name="fileName">File name, NULL for a new table.</param>
	private _TableDocument(LanguageTable table, string? fileName)
	{
		FileName = fileName;
		Key = table.Key;
		LocaleKey = table.LocaleKey;
		Name = table.Name;
		Version = table.Version;
		Author = table.Author;
		Prefixes = string.Join(PREFIX_SEPARATOR, table.Prefixes.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()));

		Rows = new BindingList<_TableRow>(table.Entries.Select(m => new _TableRow(m.Symbol, _TextEscaping._Escape(m.Text))).ToList());
		_Saved = _Serialize(ToTable());
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Loads a document from a file.</summary>
	/// <param name="fileName">File name; files with the extension <c>.cltab</c> are read as compressed tables.</param>
	/// <returns>Returns the document.</returns>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	/// <exception cref="FormatException">Thrown when the file is not a language table (content identifier <c>lang.def</c>).</exception>
	public static _TableDocument Load(string fileName)
	{
		string path = Path.GetFullPath(fileName);
		bool compressed = path.EndsWith(".cltab", StringComparison.OrdinalIgnoreCase);
		DdnElement elm;
		try
		{
			elm = Ddn.Open(path, (compressed ? new Ddn.Options(GZip.BASE62) : Ddn.Options.DEFAULT));
		}
		catch(Exception ex) when ((ex is not IOException) && (ex is not UnauthorizedAccessException))
		{
			throw new FormatException("The file is not a language table.", ex);
		}

		if(elm["cid"].Value != _CID) { throw new FormatException("The file is not a language table."); }

		_TableDocument rval = new(LanguageTable.FromDdn(elm), path);
		rval.FileDuplicates = elm.ParsedDuplicates.Where(m => (!m.IsSection) && (m.Parent?.Name == "data") && (m.Parent.Parent?.Parent is null))
												  .Select(m => m.Name)
												  .Distinct(StringComparer.Ordinal)
												  .Order(StringComparer.Ordinal)
												  .ToList();
		return rval;
	}


	/// <summary>Returns if a row has a problem: a text without symbol, a symbol that occurs more than once, or a symbol with a
	///          prefix in a table that has a prefix.</summary>
	/// <param name="row">Row.</param>
	/// <param name="duplicates">Duplicate symbols as returned by <see cref="GetDuplicateSymbols"/>.</param>
	/// <param name="hasPrefix">Determines if the table has a prefix (see <see cref="HasPrefix"/>).</param>
	/// <returns>Returns the problem, or NULL if the row is fine. If a row has several problems, the first in this order is
	///          returned.</returns>
	public static _RowProblem? GetProblem(_TableRow row, HashSet<string> duplicates, bool hasPrefix = false)
	{
		if(_IsBlank(row)) { return null; }
		if(string.IsNullOrWhiteSpace(row.Symbol)) { return _RowProblem.NO_SYMBOL; }
		if(duplicates.Contains(row.Symbol.Trim())) { return _RowProblem.DUPLICATE; }
		if(hasPrefix && row.Symbol.Contains(_PREFIX_SEPARATOR)) { return _RowProblem.PREFIX; }
		return null;
	}



	/// <summary>Compares two symbols in the order in which ddn files are written.</summary>
	/// <param name="a">Symbol.</param>
	/// <param name="b">Symbol.</param>
	/// <returns>Returns a negative value if a precedes b, 0 if they are equal and a positive value if b precedes a.</returns>
	/// <remarks>Characters are compared by their code, so upper case letters precede lower case letters. Sequences of digits are
	///          compared by their numeric value (<c>item2</c> before <c>item10</c>).</remarks>
	public static int CompareSymbols(string a, string b)
	{
		int i = 0, k = 0;

		while((i < a.Length) && (k < b.Length))
		{
			if(char.IsAsciiDigit(a[i]) && char.IsAsciiDigit(b[k]))
			{
				int si = i, sk = k;
				while((i < a.Length) && char.IsAsciiDigit(a[i])) { i++; }
				while((k < b.Length) && char.IsAsciiDigit(b[k])) { k++; }

				string na = a[si..i].TrimStart('0'), nb = b[sk..k].TrimStart('0');
				int rval = ((na.Length != nb.Length) ? na.Length.CompareTo(nb.Length) : string.CompareOrdinal(na, nb));
				if(rval != 0) { return rval; }
			}
			else
			{
				int rval = a[i].CompareTo(b[k]);
				if(rval != 0) { return rval; }
				i++; k++;
			}
		}

		int rest = (a.Length - i).CompareTo(b.Length - k);
		return ((rest != 0) ? rest : string.CompareOrdinal(a, b));
	}


	/// <summary>Splits the prefixes of a table as they are entered.</summary>
	/// <param name="value">Prefixes, separated by semicolons or commas.</param>
	/// <returns>Returns the trimmed prefixes without empty and repeated entries, in the order given.</returns>
	public static List<string> ParsePrefixes(string? value)
	{
		if(string.IsNullOrWhiteSpace(value)) { return new List<string>(); }
		return value.Split([';', ','], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Distinct(StringComparer.Ordinal).ToList();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Serializes a table for comparison.</summary>
	/// <param name="table">Table.</param>
	/// <returns>Returns the serialized table.</returns>
	private static byte[] _Serialize(LanguageTable table)
	{
		using MemoryStream rval = new();
		table.Save(rval);
		return rval.ToArray();
	}


	/// <summary>Returns if a row is blank.</summary>
	/// <param name="row">Row.</param>
	/// <returns>Returns TRUE if symbol and text are empty, otherwise returns FALSE.</returns>
	private static bool _IsBlank(_TableRow row)
	{
		return (string.IsNullOrWhiteSpace(row.Symbol) && string.IsNullOrEmpty(row.Text));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets or sets the table key.</summary>
	public string Key
	{
		get; set;
	}


	/// <summary>Gets or sets the key of the locale the table belongs to.</summary>
	public string LocaleKey
	{
		get; set;
	}


	/// <summary>Gets or sets the table name.</summary>
	public string Name
	{
		get; set;
	}


	/// <summary>Gets or sets the table version.</summary>
	public string Version
	{
		get; set;
	}


	/// <summary>Gets or sets the author.</summary>
	public string Author
	{
		get; set;
	}


	/// <summary>Gets or sets the prefixes of the table as they are entered, separated by semicolons or commas; an empty string for a
	///          table without prefix.</summary>
	/// <remarks>Loaded tables show their prefixes separated by <see cref="PREFIX_SEPARATOR"/>. See <see cref="ParsePrefixes"/>.</remarks>
	public string Prefixes
	{
		get; set;
	}


	/// <summary>Gets the rows.</summary>
	public BindingList<_TableRow> Rows
	{
		get;
	}


	/// <summary>Gets if the table has a prefix that is put in front of its symbols.</summary>
	public bool HasPrefix
	{
		get { return (ParsePrefixes(Prefixes).Count > 0); }
	}


	/// <summary>Gets the symbols that occur more than once in the file the table has been loaded from.</summary>
	/// <remarks>When a file is loaded, the last entry for a symbol replaces the earlier ones, so these duplicates are not visible in
	///          the table. The list is empty for new tables and is not changed when the table is saved.</remarks>
	public IReadOnlyList<string> FileDuplicates
	{
		get; private set;
	} = [];


	/// <summary>Gets the number of entries, without empty rows.</summary>
	public int EntryCount
	{
		get { return Rows.Count(m => !_IsBlank(m)); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates the language table from the document.</summary>
	/// <returns>Returns the table.</returns>
	/// <remarks>Rows without symbol and text are skipped. Symbols are trimmed.</remarks>
	public LanguageTable ToTable()
	{
		LanguageTable rval = new()
		{
			Key = Key.Trim(),
			LocaleKey = LocaleKey.Trim(),
			Name = Name.Trim(),
			Version = Version.Trim(),
			Author = Author.Trim()
		};

		rval.Prefixes.AddRange(ParsePrefixes(Prefixes));

		foreach(_TableRow i in Rows)
		{
			if(_IsBlank(i)) { continue; }
			rval.Entries.Add(new LanguageTableEntry(i.Symbol.Trim(), _TextEscaping._Unescape(i.Text)));
		}
		return rval;
	}


	/// <summary>Sorts the rows by symbol.</summary>
	/// <returns>Returns TRUE if the order of the rows has changed, otherwise returns FALSE.</returns>
	/// <remarks><para>The rows are sorted in the order in which the entries are written to the file (see <see cref="CompareSymbols"/>),
	///          so the table looks the same as after saving and opening it again. Since the file is always written in this order,
	///          sorting is no change of the document.</para>
	///          <para>Rows without symbol are moved to the end. Rows that compare equal keep their order. The list raises a single
	///          reset event.</para></remarks>
	public bool Sort()
	{
		List<_TableRow> sorted = Rows.OrderBy(m => string.IsNullOrWhiteSpace(m.Symbol))
									 .ThenBy(m => m.Symbol.Trim(), Comparer<string>.Create(CompareSymbols))
									 .ToList();
		if(sorted.SequenceEqual(Rows)) { return false; }

		Rows.RaiseListChangedEvents = false;
		Rows.Clear();
		foreach(_TableRow i in sorted) { Rows.Add(i); }
		Rows.RaiseListChangedEvents = true;
		Rows.ResetBindings();

		return true;
	}


	/// <summary>Appends the entries of another table whose symbols do not occur in this table.</summary>
	/// <param name="source">Table to import from.</param>
	/// <returns>Returns the indexes of the appended rows.</returns>
	/// <remarks>Existing rows are never changed. The entries are appended in the order of the source; rows without symbol and
	///          header values of the source are ignored.</remarks>
	public List<int> Import(_TableDocument source)
	{
		HashSet<string> symbols = Rows.Where(m => !string.IsNullOrWhiteSpace(m.Symbol)).Select(m => m.Symbol.Trim()).ToHashSet(StringComparer.Ordinal);
		List<int> rval = new();

		foreach(_TableRow i in source.Rows)
		{
			string symbol = i.Symbol.Trim();
			if((symbol.Length == 0) || (!symbols.Add(symbol))) { continue; }

			rval.Add(Rows.Count);
			Rows.Add(new _TableRow(symbol, i.Text));
		}
		return rval;
	}


	/// <summary>Gets the symbols that occur more than once.</summary>
	/// <returns>Returns the trimmed symbols.</returns>
	public HashSet<string> GetDuplicateSymbols()
	{
		return Rows.Where(m => !string.IsNullOrWhiteSpace(m.Symbol))
				   .GroupBy(m => m.Symbol.Trim(), StringComparer.Ordinal)
				   .Where(m => m.Count() > 1)
				   .Select(m => m.Key)
				   .ToHashSet(StringComparer.Ordinal);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] _EditorDocument                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <inheritdoc/>
	public override bool IsDirty
	{
		get { return !_Serialize(ToTable()).AsSpan().SequenceEqual(_Saved); }
	}


	/// <inheritdoc/>
	/// <remarks>Files with the extension <c>.cltab</c> are written compressed.</remarks>
	public override void Save(string? fileName = null)
	{
		string path = Path.GetFullPath(fileName ?? FileName ?? throw new InvalidOperationException("The table has no file name."));
		LanguageTable table = ToTable();

		table.Save(path);
		FileName = path;
		_Saved = _Serialize(table);
	}
}
