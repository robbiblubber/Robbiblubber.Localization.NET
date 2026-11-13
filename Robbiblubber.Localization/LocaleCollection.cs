namespace Robbiblubber.Localization;

using System.Collections;
using System.Diagnostics.CodeAnalysis;



/// <summary>This class provides a read-only collection of locales.</summary>
/// <remarks>The locales are ordered by name.</remarks>
public sealed class LocaleCollection: IReadOnlyList<Locale>
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Items.</summary>
	private readonly Locale[] _Items;

	/// <summary>Items by key.</summary>
	private readonly Dictionary<string, Locale> _ByKey;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="locales">Locales; locales with duplicate keys are ignored.</param>
	internal LocaleCollection(IEnumerable<Locale> locales)
	{
		_ByKey = new(StringComparer.OrdinalIgnoreCase);
		foreach(Locale i in locales) { _ByKey.TryAdd(i.Key, i); }

		_Items = _ByKey.Values.OrderBy(m => m.Name, StringComparer.CurrentCultureIgnoreCase)
							  .ThenBy(m => m.Key, StringComparer.OrdinalIgnoreCase)
							  .ToArray();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the locale at an index.</summary>
	/// <param name="index">Index.</param>
	/// <returns>Returns the locale.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
	public Locale this[int index]
	{
		get
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, _Items.Length);

			return _Items[index];
		}
	}


	/// <summary>Gets the locale with a key.</summary>
	/// <param name="key">Key; the key is compared regardless of case.</param>
	/// <returns>Returns the locale, NULL if there is no locale with this key.</returns>
	public Locale? this[string? key]
	{
		get { return (TryGet(key, out Locale? rval) ? rval : null); }
	}


	/// <summary>Gets the number of locales.</summary>
	public int Count
	{
		get { return _Items.Length; }
	}


	/// <summary>Gets the keys of the locales in collection order.</summary>
	public IReadOnlyList<string> Keys
	{
		get { return _Items.Select(m => m.Key).ToArray(); }
	}


	/// <summary>Gets the default locale.</summary>
	/// <remarks>Returns the first locale marked as default, the first locale if none is marked, or NULL if the collection is
	///          empty.</remarks>
	public Locale? Default
	{
		get { return (_Items.FirstOrDefault(m => m.IsDefault) ?? _Items.FirstOrDefault()); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the locale with a key.</summary>
	/// <param name="key">Key; the key is compared regardless of case.</param>
	/// <param name="locale">Returns the locale, NULL if there is no locale with this key.</param>
	/// <returns>Returns TRUE if the locale exists, otherwise returns FALSE.</returns>
	public bool TryGet(string? key, [NotNullWhen(true)] out Locale? locale)
	{
		locale = null;
		if(string.IsNullOrWhiteSpace(key)) { return false; }

		return _ByKey.TryGetValue(key.Trim(), out locale);
	}


	/// <summary>Determines if the collection contains a locale with a key.</summary>
	/// <param name="key">Key.</param>
	/// <returns>Returns TRUE if the collection contains the locale, otherwise returns FALSE.</returns>
	public bool Contains(string? key)
	{
		return TryGet(key, out _);
	}


	/// <summary>Determines if the collection contains a locale.</summary>
	/// <param name="locale">Locale.</param>
	/// <returns>Returns TRUE if the collection contains a locale with the same key, otherwise returns FALSE.</returns>
	public bool Contains(Locale? locale)
	{
		return ((locale is not null) && Contains(locale.Key));
	}


	/// <summary>Gets the index of a locale.</summary>
	/// <param name="locale">Locale.</param>
	/// <returns>Returns the index of the locale with the same key, -1 if the collection does not contain it.</returns>
	public int IndexOf(Locale? locale)
	{
		return ((locale is null) ? -1 : Array.FindIndex(_Items, m => m.Equals(locale)));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IEnumerable<Locale>                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public IEnumerator<Locale> GetEnumerator()
	{
		return ((IEnumerable<Locale>) _Items).GetEnumerator();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IEnumerable                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
