namespace Robbiblubber.Localization;

using System.Globalization;

using Robbiblubber.Localization.Definition;



/// <summary>This class represents a locale.</summary>
/// <remarks><para>Instances describe a locale (key, name, icons). The static members provide the localization services of the
///          application: the available locales, the current locale and the lookup of texts.</para>
///          <para>Two locales are equal if their keys are equal, regardless of case.</para></remarks>
public sealed partial class Locale: IEquatable<Locale>
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Culture.</summary>
	private CultureInfo? _Culture;

	/// <summary>Determines if the culture has been resolved.</summary>
	private bool _CultureResolved = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="definition">Locale definition.</param>
	internal Locale(LocaleDefinition definition)
	{
		Key = definition.Key.Trim();
		BaseKey = (string.IsNullOrWhiteSpace(definition.BaseKey) ? null : definition.BaseKey.Trim());
		Name = (string.IsNullOrWhiteSpace(definition.Name) ? Key : definition.Name);
		IsDefault = definition.IsDefault;
		RightToLeft = definition.RightToLeft;
		_SmallIcon = (definition.SmallIcon?.ToArray());
		_LargeIcon = (definition.LargeIcon?.ToArray());
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal properties                                                                                              //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the small icon data.</summary>
	internal byte[]? _SmallIcon
	{
		get;
	}


	/// <summary>Gets the large icon data.</summary>
	internal byte[]? _LargeIcon
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the locale key, for example <c>de</c> or <c>de-AT</c>.</summary>
	public string Key
	{
		get;
	}


	/// <summary>Gets the key of the base locale, NULL if the locale has no base locale.</summary>
	/// <remarks>Texts that are not defined for this locale are taken from the base locale.</remarks>
	public string? BaseKey
	{
		get;
	}


	/// <summary>Gets the locale name.</summary>
	public string Name
	{
		get;
	}


	/// <summary>Gets if the locale is marked as default locale.</summary>
	public bool IsDefault
	{
		get;
	}


	/// <summary>Gets if the locale is written right to left.</summary>
	public bool RightToLeft
	{
		get;
	}


	/// <summary>Gets if the locale has a small icon.</summary>
	public bool HasSmallIcon
	{
		get { return (_SmallIcon is not null); }
	}


	/// <summary>Gets if the locale has a large icon.</summary>
	public bool HasLargeIcon
	{
		get { return (_LargeIcon is not null); }
	}


	/// <summary>Gets the culture for the locale key, NULL if the key is not a known culture name.</summary>
	public CultureInfo? Culture
	{
		get
		{
			if(!_CultureResolved)
			{
				try
				{
					_Culture = CultureInfo.GetCultureInfo(Key, true);
				}
				catch(CultureNotFoundException) { _Culture = null; }

				_CultureResolved = true;
			}

			return _Culture;
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Opens the image data of the small icon.</summary>
	/// <returns>Returns a read-only stream, NULL if the locale has no small icon.</returns>
	public Stream? OpenSmallIcon()
	{
		return ((_SmallIcon is null) ? null : new MemoryStream(_SmallIcon, false));
	}


	/// <summary>Opens the image data of the large icon.</summary>
	/// <returns>Returns a read-only stream, NULL if the locale has no large icon.</returns>
	public Stream? OpenLargeIcon()
	{
		return ((_LargeIcon is null) ? null : new MemoryStream(_LargeIcon, false));
	}


	/// <summary>Gets the text for a symbol in this locale.</summary>
	/// <param name="symbol">Symbol, for example <c>prefix:symbol</c>.</param>
	/// <returns>Returns the text from the tables of this locale or its base locales, NULL if the symbol is not defined.</returns>
	/// <remarks>The fallback locale is not used.</remarks>
	public string? GetText(string? symbol)
	{
		if(symbol is null) { return null; }
		return (_GetState()._GetTexts(this).TryGetValue(symbol, out string? rval) ? rval : null);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IEquatable<Locale>                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Determines if a locale is equal to this instance.</summary>
	/// <param name="other">Locale.</param>
	/// <returns>Returns TRUE if the keys are equal regardless of case, otherwise returns FALSE.</returns>
	public bool Equals(Locale? other)
	{
		return ((other is not null) && string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override bool Equals(object? obj)
	{
		return Equals(obj as Locale);
	}


	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return StringComparer.OrdinalIgnoreCase.GetHashCode(Key);
	}


	/// <summary>Returns a string representation of this instance.</summary>
	/// <returns>Returns the locale key.</returns>
	public override string ToString()
	{
		return Key;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// operators                                                                                                        //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Determines if two locales are equal.</summary>
	/// <param name="a">Locale.</param>
	/// <param name="b">Locale.</param>
	/// <returns>Returns TRUE if both are NULL or their keys are equal, otherwise returns FALSE.</returns>
	public static bool operator ==(Locale? a, Locale? b)
	{
		if(a is null) { return (b is null); }
		return a.Equals(b);
	}


	/// <summary>Determines if two locales are not equal.</summary>
	/// <param name="a">Locale.</param>
	/// <param name="b">Locale.</param>
	/// <returns>Returns TRUE if the locales are not equal, otherwise returns FALSE.</returns>
	public static bool operator !=(Locale? a, Locale? b)
	{
		return !(a == b);
	}
}
