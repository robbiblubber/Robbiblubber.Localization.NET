namespace Robbiblubber.Localization.Definition;

using System.IO.Compression;

using Robbiblubber.Spellbook;



/// <summary>This class represents an editable locale definition.</summary>
/// <remarks>A locale definition is stored as a zip file with the extension <c>.locale</c> that contains the entries
///          <c>locale.def</c> (ddn), <c>small.icon</c> and <c>large.icon</c> (images, usually 16 × 16 and 32 × 32 PNG). The controls choose the icon that fits
///          the size they need best and scale it, so other sizes work as well.</remarks>
public sealed class LocaleDefinition
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Definition entry name.</summary>
	private const string _DEFINITION_ENTRY = "locale.def";

	/// <summary>Small icon entry name.</summary>
	private const string _SMALL_ICON_ENTRY = "small.icon";

	/// <summary>Large icon entry name.</summary>
	private const string _LARGE_ICON_ENTRY = "large.icon";

	/// <summary>Content identifier.</summary>
	private const string _CID = "locale.def";

	/// <summary>Format version.</summary>
	private const string _VERSION = "3.0";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Reads a zip entry.</summary>
	/// <param name="zip">Zip archive.</param>
	/// <param name="name">Entry name.</param>
	/// <returns>Returns the entry data, NULL if the entry does not exist or is empty.</returns>
	private static byte[]? _Read(ZipArchive zip, string name)
	{
		ZipArchiveEntry? e = zip.GetEntry(name);
		if(e is null) { return null; }

		using(Stream s = e.Open())
		{
			using(MemoryStream rval = new())
			{
				s.CopyTo(rval);
				return ((rval.Length == 0) ? null : rval.ToArray());
			}
		}
	}


	/// <summary>Writes a zip entry.</summary>
	/// <param name="zip">Zip archive.</param>
	/// <param name="name">Entry name.</param>
	/// <param name="data">Data.</param>
	private static void _Write(ZipArchive zip, string name, byte[] data)
	{
		using(Stream s = zip.CreateEntry(name).Open())
		{
			s.Write(data);
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Validates the definition.</summary>
	/// <exception cref="InvalidOperationException">Thrown when the key is empty.</exception>
	private void _Validate()
	{
		if(string.IsNullOrWhiteSpace(Key)) { throw new InvalidOperationException("The locale key must not be empty."); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the locale key, for example <c>de</c> or <c>de-AT</c>.</summary>
	public string Key
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets or sets the key of the base locale, NULL if the locale has no base locale.</summary>
	/// <remarks>Texts that are not defined for the locale are taken from the base locale.</remarks>
	public string? BaseKey
	{
		get; set;
	}


	/// <summary>Gets or sets the locale name, for example <c>Deutsch</c>.</summary>
	public string Name
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets or sets if the locale is the default locale.</summary>
	public bool IsDefault
	{
		get; set;
	}


	/// <summary>Gets or sets if the locale is written right to left.</summary>
	public bool RightToLeft
	{
		get; set;
	}


	/// <summary>Gets or sets the image data of the small icon, NULL if there is none.</summary>
	public byte[]? SmallIcon
	{
		get; set;
	}


	/// <summary>Gets or sets the image data of the large icon, NULL if there is none.</summary>
	public byte[]? LargeIcon
	{
		get; set;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Loads a locale definition from a file.</summary>
	/// <param name="fileName">File name.</param>
	/// <returns>Returns the locale definition.</returns>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	/// <exception cref="InvalidDataException">Thrown when the file is not a valid locale definition.</exception>
	public static LocaleDefinition Load(string fileName)
	{
		using(FileStream s = File.OpenRead(fileName))
		{
			return Load(s);
		}
	}


	/// <summary>Loads a locale definition from a stream.</summary>
	/// <param name="stream">Stream; the stream is not closed.</param>
	/// <returns>Returns the locale definition.</returns>
	/// <exception cref="InvalidDataException">Thrown when the data is not a valid locale definition.</exception>
	public static LocaleDefinition Load(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		using(ZipArchive zip = new(stream, ZipArchiveMode.Read, true))
		{
			byte[] def = (_Read(zip, _DEFINITION_ENTRY) ?? throw new InvalidDataException("The locale definition entry is missing."));
			DdnElement elm;

			using(MemoryStream s = new(def)) { elm = Ddn.Open(s); }

			LocaleDefinition rval = new()
			{
				Key = (elm["key"].Value ?? string.Empty).Trim(),
				BaseKey = (string.IsNullOrWhiteSpace(elm["base"].Value) ? null : elm["base"].Value!.Trim()),
				Name = (elm["name"].Value ?? string.Empty),
				IsDefault = elm.Get("default", false),
				RightToLeft = elm.Get("rtl", false),
				SmallIcon = _Read(zip, _SMALL_ICON_ENTRY),
				LargeIcon = _Read(zip, _LARGE_ICON_ENTRY)
			};

			if(rval.Key.Length == 0) { throw new InvalidDataException("The locale definition has no key."); }
			if(string.IsNullOrWhiteSpace(rval.Name)) { rval.Name = rval.Key; }

			return rval;
		}
	}


	/// <summary>Creates a locale definition from a locale.</summary>
	/// <param name="locale">Locale.</param>
	/// <returns>Returns the locale definition.</returns>
	public static LocaleDefinition FromLocale(Locale locale)
	{
		ArgumentNullException.ThrowIfNull(locale);

		return new()
		{
			Key = locale.Key,
			BaseKey = locale.BaseKey,
			Name = locale.Name,
			IsDefault = locale.IsDefault,
			RightToLeft = locale.RightToLeft,
			SmallIcon = locale._SmallIcon,
			LargeIcon = locale._LargeIcon
		};
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Saves the locale definition to a file.</summary>
	/// <param name="fileName">File name; missing directories are created.</param>
	/// <exception cref="InvalidOperationException">Thrown when the key is empty.</exception>
	/// <remarks>The file is replaced only after it has been written completely.</remarks>
	public void Save(string fileName)
	{
		string path = Path.GetFullPath(fileName);
		string dir = Path.GetDirectoryName(path)!;
		string temp = Path.Combine(dir, $"~{Guid.NewGuid():N}.tmp");

		_Validate();
		Directory.CreateDirectory(dir);

		try
		{
			using(FileStream s = new(temp, FileMode.CreateNew, FileAccess.Write))
			{
				Save(s);
			}
			File.Move(temp, path, true);
		}
		finally
		{
			if(File.Exists(temp)) { File.Delete(temp); }
		}
	}


	/// <summary>Saves the locale definition to a stream.</summary>
	/// <param name="stream">Stream; the stream is not closed.</param>
	/// <exception cref="InvalidOperationException">Thrown when the key is empty.</exception>
	public void Save(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);
		_Validate();

		DdnElement def = Ddn.Create().Set("cid", _CID).Set("version", _VERSION)
									 .Set("key", Key.Trim())
									 .Set("base", (string.IsNullOrWhiteSpace(BaseKey) ? null : BaseKey.Trim()))
									 .Set("name", (string.IsNullOrWhiteSpace(Name) ? Key.Trim() : Name))
									 .Set("default", IsDefault)
									 .Set("rtl", RightToLeft);

		using(ZipArchive zip = new(stream, ZipArchiveMode.Create, true))
		{
			using(MemoryStream s = new())
			{
				def.Save(s);
				_Write(zip, _DEFINITION_ENTRY, s.ToArray());
			}

			if(SmallIcon?.Length > 0) { _Write(zip, _SMALL_ICON_ENTRY, SmallIcon); }
			if(LargeIcon?.Length > 0) { _Write(zip, _LARGE_ICON_ENTRY, LargeIcon); }
		}
	}


	/// <summary>Creates a locale from this definition.</summary>
	/// <returns>Returns the locale.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the key is empty.</exception>
	public Locale ToLocale()
	{
		_Validate();
		return new Locale(this);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override string ToString()
	{
		return Key;
	}
}
