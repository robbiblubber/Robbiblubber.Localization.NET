namespace Robbiblubber.Localization;



/// <summary>This class represents a resource provided by a <see cref="ILocalizationSource"/>.</summary>
public sealed class LocalizationResource
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Locale definition file extension.</summary>
	internal const string _LOCALE_EXTENSION = ".locale";

	/// <summary>Language table file extension.</summary>
	internal const string _TABLE_EXTENSION = ".ltab";

	/// <summary>Compressed language table file extension.</summary>
	internal const string _COMPRESSED_TABLE_EXTENSION = ".cltab";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Function that opens the resource.</summary>
	private readonly Func<Stream> _Open;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="name">Resource name; the extension determines the resource type.</param>
	/// <param name="open">Function that opens a readable stream for the resource.</param>
	/// <exception cref="ArgumentNullException">Thrown when an argument is NULL.</exception>
	public LocalizationResource(string name, Func<Stream> open)
	{
		ArgumentNullException.ThrowIfNull(name);
		ArgumentNullException.ThrowIfNull(open);

		Name = name;
		_Open = open;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets if a name has a locale definition extension.</summary>
	/// <param name="name">Name.</param>
	/// <returns>Returns TRUE if the name ends with the locale definition extension, otherwise returns FALSE.</returns>
	internal static bool _IsLocale(string name)
	{
		return name.EndsWith(_LOCALE_EXTENSION, StringComparison.OrdinalIgnoreCase);
	}


	/// <summary>Gets if a name has a language table extension.</summary>
	/// <param name="name">Name.</param>
	/// <returns>Returns TRUE if the name ends with a language table extension, otherwise returns FALSE.</returns>
	internal static bool _IsTable(string name)
	{
		return (name.EndsWith(_TABLE_EXTENSION, StringComparison.OrdinalIgnoreCase) || _IsCompressedTable(name));
	}


	/// <summary>Gets if a name has the compressed language table extension.</summary>
	/// <param name="name">Name.</param>
	/// <returns>Returns TRUE if the name ends with the compressed language table extension, otherwise returns FALSE.</returns>
	internal static bool _IsCompressedTable(string name)
	{
		return name.EndsWith(_COMPRESSED_TABLE_EXTENSION, StringComparison.OrdinalIgnoreCase);
	}


	/// <summary>Gets if a name has one of the supported extensions.</summary>
	/// <param name="name">Name.</param>
	/// <returns>Returns TRUE if the name ends with a supported extension, otherwise returns FALSE.</returns>
	internal static bool _IsSupported(string name)
	{
		return (_IsLocale(name) || _IsTable(name));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the resource name.</summary>
	public string Name
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Opens the resource.</summary>
	/// <returns>Returns a readable stream; the caller disposes it.</returns>
	public Stream Open()
	{
		return _Open();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override string ToString()
	{
		return Name;
	}
}
