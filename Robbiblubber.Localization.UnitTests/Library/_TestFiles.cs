namespace Robbiblubber.Localization.UnitTests.Library;

using Robbiblubber.Localization.Definition;



/// <summary>This class creates locale definitions and language tables in a temporary directory.</summary>
internal sealed class _TestFiles: IDisposable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public _TestFiles()
	{
		Directory.CreateDirectory(Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "loc-tests-" + Guid.NewGuid().ToString("N")));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the directory path.</summary>
	public string Path
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates PNG-like test data.</summary>
	/// <param name="marker">Marker byte.</param>
	/// <returns>Returns the data.</returns>
	public static byte[] Icon(byte marker)
	{
		return [0x89, 0x50, 0x4E, 0x47, marker];
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets a path in the directory.</summary>
	/// <param name="name">Relative name.</param>
	/// <returns>Returns the full path.</returns>
	public string File(string name)
	{
		return System.IO.Path.Combine(Path, name);
	}


	/// <summary>Writes a locale definition.</summary>
	/// <param name="key">Key.</param>
	/// <param name="name">Name.</param>
	/// <param name="baseKey">Base key.</param>
	/// <param name="isDefault">Default flag.</param>
	/// <param name="fileName">File name relative to the directory; NULL for <c>key.locale</c>.</param>
	/// <returns>Returns the file name.</returns>
	public string WriteLocale(string key, string name, string? baseKey = null, bool isDefault = false, string? fileName = null)
	{
		string rval = File(fileName ?? (key + ".locale"));
		new LocaleDefinition() { Key = key, Name = name, BaseKey = baseKey, IsDefault = isDefault,
								 SmallIcon = Icon(1), LargeIcon = Icon(2) }.Save(rval);

		return rval;
	}


	/// <summary>Writes a language table.</summary>
	/// <param name="fileName">File name relative to the directory.</param>
	/// <param name="localeKey">Locale key.</param>
	/// <param name="prefix">Prefix, NULL for none.</param>
	/// <param name="entries">Entries as pairs of symbol and text.</param>
	/// <returns>Returns the file name.</returns>
	public string WriteTable(string fileName, string localeKey, string? prefix, params (string Symbol, string Text)[] entries)
	{
		string rval = File(fileName);
		LanguageTable t = new() { Key = fileName, LocaleKey = localeKey, Name = fileName };
		if(prefix is not null) { t.Prefixes.Add(prefix); }
		foreach((string Symbol, string Text) i in entries) { t.Entries.Add(new(i.Symbol, i.Text)); }
		t.Save(rval);

		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IDisposable                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Deletes the directory.</summary>
	public void Dispose()
	{
		try
		{
			if(Directory.Exists(Path)) { Directory.Delete(Path, true); }
		}
		catch(IOException) {}
	}
}
