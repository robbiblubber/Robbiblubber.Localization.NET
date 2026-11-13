namespace Robbiblubber.Localization;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

using Robbiblubber.Spellbook;



public sealed partial class Locale
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Lock object.</summary>
	private static readonly object _Lock = new();

	/// <summary>Loaded state, NULL if the state has to be loaded.</summary>
	private static volatile _LocalizationState? _State = null;

	/// <summary>Locale selected as current locale, NULL for the default locale.</summary>
	private static Locale? _SelectedCurrent = null;

	/// <summary>Locale selected as fallback locale, NULL for the default locale.</summary>
	private static Locale? _SelectedFallback = null;

	/// <summary>Determines if the fallback text is preferred over the default text.</summary>
	private static volatile bool _PreferFallback = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Invalidates the loaded state after the sources have changed.</summary>
	internal static void _Invalidate()
	{
		bool loaded;
		lock(_Lock)
		{
			loaded = (_State is not null);
			_State = null;
		}

		if(loaded) { LocaleChanged?.Invoke(null, EventArgs.Empty); }
	}


	/// <summary>Resets all static settings to their defaults without raising events.</summary>
	internal static void _Reset()
	{
		lock(_Lock)
		{
			Sources._Reset();
			_SelectedCurrent = _SelectedFallback = null;
			_PreferFallback = false;
			_State = null;
		}
	}


	/// <summary>Gets the loaded state and loads it if necessary.</summary>
	/// <returns>Returns the state.</returns>
	internal static _LocalizationState _GetState()
	{
		_LocalizationState? rval = _State;
		if(rval is not null) { return rval; }

		lock(_Lock)
		{
			return (_State ??= _LocalizationState._Load(Sources.ToArray())._Select(_SelectedCurrent, _SelectedFallback));
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Replaces placeholders in a text.</summary>
	/// <param name="text">Text.</param>
	/// <param name="values">Values.</param>
	/// <returns>Returns the text with replaced placeholders.</returns>
	/// <remarks>At each <c>$</c> the longest matching name is replaced; replaced values are not scanned again.</remarks>
	private static string _Replace(string text, (string Name, object? Value)[]? values)
	{
		if((values is null) || (values.Length == 0) || (!text.Contains('$'))) { return text; }

		(string Name, object? Value)[] vals = values.Where(m => !string.IsNullOrEmpty(m.Name))
													.OrderByDescending(m => m.Name.Length)
													.ToArray();
		IFormatProvider fmt = (Current?.Culture ?? CultureInfo.CurrentCulture);
		StringBuilder rval = new();

		for(int i = 0; i < text.Length; i++)
		{
			if(text[i] == '$')
			{
				int n = Array.FindIndex(vals, m => (string.CompareOrdinal(text, i + 1, m.Name, 0, m.Name.Length) == 0));
				if(n >= 0)
				{
					rval.Append(Convert.ToString(vals[n].Value, fmt));
					i += vals[n].Name.Length;
					continue;
				}
			}
			rval.Append(text[i]);
		}

		return rval.ToString();
	}


	/// <summary>Changes the selected current or fallback locale.</summary>
	/// <param name="current">Determines if the current locale is selected; FALSE selects the fallback locale.</param>
	/// <param name="value">Selected locale.</param>
	private static void _Select(bool current, Locale? value)
	{
		bool changed;
		lock(_Lock)
		{
			_LocalizationState old = _GetState();

			if(current) { _SelectedCurrent = value; } else { _SelectedFallback = value; }
			_LocalizationState rval = old._Select(_SelectedCurrent, _SelectedFallback);
			_State = rval;

			changed = ((old._Current != rval._Current) || (old._Fallback != rval._Fallback));
		}

		if(changed) { LocaleChanged?.Invoke(null, EventArgs.Empty); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static events                                                                                             //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Occurs when the current or fallback locale has changed or the texts have been reloaded.</summary>
	/// <remarks>The event is raised on the thread that made the change; the sender is NULL.</remarks>
	public static event EventHandler? LocaleChanged;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static properties                                                                                         //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the sources of locale definitions and language tables.</summary>
	/// <remarks>By default, the application directory and its subdirectories are searched. Changing the sources reloads all
	///          locales and tables on the next access. Configure the sources at application start; the collection is not
	///          thread-safe.</remarks>
	public static LocalizationSourceCollection Sources
	{
		get;
	} = new();


	/// <summary>Gets the available locales.</summary>
	public static LocaleCollection Locales
	{
		get { return _GetState()._Locales; }
	}


	/// <summary>Gets the problems that occurred while loading locales and tables.</summary>
	public static IReadOnlyList<LocalizationIssue> Issues
	{
		get { return _GetState()._Issues; }
	}


	/// <summary>Gets or sets the current locale.</summary>
	/// <remarks>If no locale has been set, the default locale of <see cref="Locales"/> is used. Setting NULL selects the
	///          default locale again. A locale with a key that is not available can be set; its texts are taken from the tables
	///          with its key. <see cref="LocaleChanged"/> is raised only if the current locale changes.</remarks>
	public static Locale? Current
	{
		get { return _GetState()._Current; }
		set
		{
			_Select(true, value);
		}
	}


	/// <summary>Gets or sets the fallback locale.</summary>
	/// <remarks>The fallback locale provides texts that are not defined for the current locale. If no locale has been set,
	///          the default locale of <see cref="Locales"/> is used.</remarks>
	public static Locale? Fallback
	{
		get { return _GetState()._Fallback; }
		set
		{
			_Select(false, value);
		}
	}


	/// <summary>Gets or sets if the fallback text is preferred over the default text passed to a lookup.</summary>
	/// <remarks>If FALSE, the fallback locale is only used for lookups without default text.</remarks>
	public static bool PreferFallback
	{
		get { return _PreferFallback; }
		set { _PreferFallback = value; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Reloads all locales and tables from the sources on the next access.</summary>
	/// <remarks>Raises <see cref="LocaleChanged"/>.</remarks>
	public static void Reload()
	{
		lock(_Lock) { _State = null; }
		LocaleChanged?.Invoke(null, EventArgs.Empty);
	}


	/// <summary>Looks up a symbol in the texts of the current locale.</summary>
	/// <param name="symbol">Symbol, for example <c>prefix:symbol</c>.</param>
	/// <param name="defaultText">Default text.</param>
	/// <returns>Returns the text of the current locale. If the symbol is not defined there, returns the default text; if
	///          there is no default text or <see cref="PreferFallback"/> is set, the text of the fallback locale is used
	///          first. Returns an empty string if nothing is found.</returns>
	public static string LookUp(string? symbol, string? defaultText = null)
	{
		if(symbol is null) { return (defaultText ?? string.Empty); }

		_LocalizationState s = _GetState();
		if(s._CurrentTexts.TryGetValue(symbol, out string? rval)) { return rval; }

		if((_PreferFallback || (defaultText is null)) && s._FallbackTexts.TryGetValue(symbol, out rval)) { return rval; }

		return (defaultText ?? string.Empty);
	}


	/// <summary>Looks up a symbol in the texts of the current locale and replaces placeholders.</summary>
	/// <param name="symbol">Symbol, for example <c>prefix:symbol</c>.</param>
	/// <param name="defaultText">Default text.</param>
	/// <param name="values">Placeholder values; a value named <c>NAME</c> replaces <c>$NAME</c> in the text.</param>
	/// <returns>Returns the text as <see cref="LookUp(string?, string?)"/> does, with placeholders replaced. Values are
	///          formatted with the culture of the current locale.</returns>
	public static string LookUp(string? symbol, string? defaultText, params (string Name, object? Value)[] values)
	{
		return _Replace(LookUp(symbol, defaultText), values);
	}


	/// <summary>Looks up a symbol in the texts of the current and the fallback locale.</summary>
	/// <param name="symbol">Symbol, for example <c>prefix:symbol</c>.</param>
	/// <param name="text">Returns the text, NULL if the symbol is not defined.</param>
	/// <returns>Returns TRUE if the symbol is defined, otherwise returns FALSE.</returns>
	public static bool TryLookUp(string? symbol, [NotNullWhen(true)] out string? text)
	{
		text = null;
		if(symbol is null) { return false; }

		_LocalizationState s = _GetState();
		return (s._CurrentTexts.TryGetValue(symbol, out text) || s._FallbackTexts.TryGetValue(symbol, out text));
	}
}
