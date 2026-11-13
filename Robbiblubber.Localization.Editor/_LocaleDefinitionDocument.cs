namespace Robbiblubber.Localization.Editor;

using Robbiblubber.Localization.Definition;



/// <summary>This class represents a locale definition that is edited in a window.</summary>
/// <remarks>The document keeps the values in the form they are edited in. The icons are kept as the bytes of the image files and
///          saved unchanged.</remarks>
internal sealed class _LocaleDefinitionDocument: _EditorDocument
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Definition as it was loaded or saved.</summary>
	private LocaleDefinition _Saved;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new, empty document.</summary>
	public _LocaleDefinitionDocument(): this(new LocaleDefinition(), null)
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="definition">Definition.</param>
	/// <param name="fileName">File name, NULL for a new definition.</param>
	private _LocaleDefinitionDocument(LocaleDefinition definition, string? fileName)
	{
		FileName = fileName;
		Key = definition.Key;
		BaseKey = (definition.BaseKey ?? string.Empty);
		Name = definition.Name;
		IsDefault = definition.IsDefault;
		RightToLeft = definition.RightToLeft;
		SmallIcon = definition.SmallIcon;
		LargeIcon = definition.LargeIcon;

		_Saved = ToDefinition();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Loads a document from a file.</summary>
	/// <param name="fileName">File name.</param>
	/// <returns>Returns the document.</returns>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	/// <exception cref="FormatException">Thrown when the file is not a locale definition.</exception>
	public static _LocaleDefinitionDocument Load(string fileName)
	{
		string path = Path.GetFullPath(fileName);
		try
		{
			return new _LocaleDefinitionDocument(LocaleDefinition.Load(path), path);
		}
		catch(InvalidDataException ex)
		{
			throw new FormatException("The file is not a locale definition.", ex);
		}
	}


	/// <summary>Returns if a locale key is valid.</summary>
	/// <param name="key">Key.</param>
	/// <returns>Returns TRUE if the key is not empty and contains no white space after trimming, otherwise returns FALSE.</returns>
	public static bool IsValidKey(string? key)
	{
		key = key?.Trim();
		return ((!string.IsNullOrEmpty(key)) && (!key.Any(char.IsWhiteSpace)));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns if two icons are equal.</summary>
	/// <param name="a">Icon data.</param>
	/// <param name="b">Icon data.</param>
	/// <returns>Returns TRUE if both are missing or have the same bytes, otherwise returns FALSE.</returns>
	private static bool _Equal(byte[]? a, byte[]? b)
	{
		if((a is null) || (b is null)) { return ((a is null) && (b is null)); }
		return a.AsSpan().SequenceEqual(b);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets or sets the locale key.</summary>
	public string Key
	{
		get; set;
	}


	/// <summary>Gets or sets the key of the base locale; an empty string for a locale without base locale.</summary>
	public string BaseKey
	{
		get; set;
	}


	/// <summary>Gets or sets the locale name.</summary>
	/// <remarks>If the name is empty, the key is saved as name.</remarks>
	public string Name
	{
		get; set;
	}


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
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates the locale definition from the document.</summary>
	/// <returns>Returns the definition.</returns>
	/// <remarks>Key and base key are trimmed; an empty name is replaced by the key, as when the definition is saved.</remarks>
	public LocaleDefinition ToDefinition()
	{
		string key = Key.Trim();

		return new LocaleDefinition()
		{
			Key = key,
			BaseKey = (string.IsNullOrWhiteSpace(BaseKey) ? null : BaseKey.Trim()),
			Name = (string.IsNullOrWhiteSpace(Name) ? key : Name),
			IsDefault = IsDefault,
			RightToLeft = RightToLeft,
			SmallIcon = ((SmallIcon?.Length > 0) ? SmallIcon : null),
			LargeIcon = ((LargeIcon?.Length > 0) ? LargeIcon : null)
		};
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] _EditorDocument                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <inheritdoc/>
	public override bool IsDirty
	{
		get
		{
			LocaleDefinition d = ToDefinition();
			return ((d.Key != _Saved.Key) || (d.BaseKey != _Saved.BaseKey) || (d.Name != _Saved.Name) || (d.IsDefault != _Saved.IsDefault) ||
					(d.RightToLeft != _Saved.RightToLeft) || (!_Equal(d.SmallIcon, _Saved.SmallIcon)) || (!_Equal(d.LargeIcon, _Saved.LargeIcon)));
		}
	}


	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when the key is empty.</exception>
	public override void Save(string? fileName = null)
	{
		string path = Path.GetFullPath(fileName ?? FileName ?? throw new InvalidOperationException("The definition has no file name."));
		LocaleDefinition definition = ToDefinition();

		definition.Save(path);
		FileName = path;
		_Saved = definition;
	}
}
