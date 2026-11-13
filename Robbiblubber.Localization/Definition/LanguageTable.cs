namespace Robbiblubber.Localization.Definition;

using Robbiblubber.Spellbook;



/// <summary>This class represents a language table.</summary>
/// <remarks><para>A language table contains the texts of one locale for one or more prefixes. It is stored as ddn in a
///          <c>.ltab</c> file, or compressed in a <c>.cltab</c> file.</para>
///          <para>Symbols of a table with prefixes are looked up as <c>prefix:symbol</c>, symbols of a table without prefix
///          as <c>symbol</c>.</para></remarks>
public sealed class LanguageTable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Content identifier.</summary>
	private const string _CID = "lang.def";

	/// <summary>Format version.</summary>
	private const string _VERSION = "3.0";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the ddn options for a table.</summary>
	/// <param name="compressed">Determines if the table is compressed.</param>
	/// <returns>Returns the options.</returns>
	internal static Ddn.Options _GetOptions(bool compressed)
	{
		return (compressed ? new Ddn.Options(GZip.BASE62) : Ddn.Options.DEFAULT);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the table key.</summary>
	public string Key
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets or sets the key of the locale the table belongs to.</summary>
	public string LocaleKey
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets or sets the table version.</summary>
	public string Version
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets or sets the table name.</summary>
	public string Name
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets or sets the author.</summary>
	public string Author
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets the prefixes of the table.</summary>
	/// <remarks>A table usually has one prefix or none. Tables with several prefixes provide their symbols under each prefix.</remarks>
	public List<string> Prefixes
	{
		get;
	} = new();


	/// <summary>Gets the entries.</summary>
	public List<LanguageTableEntry> Entries
	{
		get;
	} = new();



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Loads a language table from a file.</summary>
	/// <param name="fileName">File name; files with the extension <c>.cltab</c> are read as compressed tables.</param>
	/// <returns>Returns the table.</returns>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	public static LanguageTable Load(string fileName)
	{
		return FromDdn(Ddn.Open(fileName, _GetOptions(LocalizationResource._IsCompressedTable(fileName))));
	}


	/// <summary>Loads a language table from a stream.</summary>
	/// <param name="stream">Stream; the stream is read to its end and not closed.</param>
	/// <param name="compressed">Determines if the table is compressed.</param>
	/// <returns>Returns the table.</returns>
	public static LanguageTable Load(Stream stream, bool compressed = false)
	{
		return FromDdn(Ddn.Open(stream, _GetOptions(compressed)));
	}


	/// <summary>Creates a language table from ddn.</summary>
	/// <param name="elm">ddn element.</param>
	/// <returns>Returns the table.</returns>
	/// <remarks>Missing header values are empty. Entries are read from the <c>data</c> section in their order; sections inside
	///          <c>data</c> are ignored. Line breaks in texts are converted to <see cref="Environment.NewLine"/>.</remarks>
	public static LanguageTable FromDdn(DdnElement elm)
	{
		ArgumentNullException.ThrowIfNull(elm);

		LanguageTable rval = new()
		{
			Key = (elm["header/key"].Value ?? string.Empty),
			LocaleKey = (elm["header/lang"].Value ?? string.Empty),
			Version = (elm["header/version"].Value ?? string.Empty),
			Name = (elm["header/name"].Value ?? string.Empty),
			Author = (elm["header/author"].Value ?? string.Empty)
		};

		foreach(string? i in (elm.Get<string?[]?>("header/prefixes") ?? []))
		{
			if(!string.IsNullOrWhiteSpace(i)) { rval.Prefixes.Add(i.Trim()); }
		}

		foreach(DdnElement i in elm["data"].Values)
		{
			rval.Entries.Add(new(i.Name, (i.Value ?? string.Empty).ReplaceLineEndings()));
		}

		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the ddn representation of this table.</summary>
	/// <returns>Returns the root element.</returns>
	/// <remarks>Entries with an empty symbol are skipped; if a symbol occurs more than once, the last entry wins. Files are
	///          written in alphabetical order of the symbols.</remarks>
	public DdnElement ToDdn()
	{
		DdnElement rval = Ddn.Create();

		rval.Set("cid", _CID);
		rval.Set("version", _VERSION);

		DdnElement header = rval["header"];
		header.Element("key").Set(Key);
		header.Element("lang").Set(LocaleKey);
		header.Element("version").Set(Version);
		header.Element("name").Set(Name);
		header.Element("prefixes").Set(Prefixes.Where(m => !string.IsNullOrWhiteSpace(m)).ToArray());
		header.Element("author").Set(Author);

		DdnElement data = rval["data"];
		foreach(LanguageTableEntry i in Entries)
		{
			if(string.IsNullOrWhiteSpace(i.Symbol)) { continue; }
			data.Element(i.Symbol).Set(i.Text);
		}

		return rval;
	}


	/// <summary>Saves the table to a file.</summary>
	/// <param name="fileName">File name; files with the extension <c>.cltab</c> are written compressed.</param>
	/// <remarks>The file is replaced only after it has been written completely.</remarks>
	public void Save(string fileName)
	{
		ToDdn().Save(fileName, _GetOptions(LocalizationResource._IsCompressedTable(fileName)));
	}


	/// <summary>Saves the table to a stream.</summary>
	/// <param name="stream">Stream; the stream is not closed.</param>
	/// <param name="compressed">Determines if the table is written compressed.</param>
	public void Save(Stream stream, bool compressed = false)
	{
		ToDdn().Save(stream, _GetOptions(compressed));
	}


	/// <summary>Gets the texts of this table as lookup symbols.</summary>
	/// <returns>Returns pairs of lookup symbol (with prefix) and text.</returns>
	public IEnumerable<KeyValuePair<string, string>> GetLookupEntries()
	{
		string[] prefixes = Prefixes.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()).Distinct().ToArray();

		foreach(LanguageTableEntry i in Entries)
		{
			if(string.IsNullOrWhiteSpace(i.Symbol)) { continue; }

			if(prefixes.Length == 0)
			{
				yield return new(i.Symbol, i.Text);
			}
			else
			{
				foreach(string k in prefixes) { yield return new(k + ":" + i.Symbol, i.Text); }
			}
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override string ToString()
	{
		return (string.IsNullOrEmpty(Name) ? Key : Name);
	}
}
