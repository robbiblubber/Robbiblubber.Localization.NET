namespace Robbiblubber.Localization;

using System.Collections.Concurrent;
using System.Collections.Frozen;

using Robbiblubber.Localization.Definition;



/// <summary>This class holds an immutable snapshot of the loaded locales and texts.</summary>
internal sealed class _LocalizationState
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Merged texts of each locale level by locale key, without base locales.</summary>
	private readonly IReadOnlyDictionary<string, Dictionary<string, string>> _Levels;

	/// <summary>Cache of texts including base locales by locale key.</summary>
	private readonly ConcurrentDictionary<string, FrozenDictionary<string, string>> _Texts;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="locales">Locales.</param>
	/// <param name="issues">Issues.</param>
	/// <param name="levels">Merged texts of each locale level.</param>
	/// <param name="texts">Text cache.</param>
	/// <param name="current">Current locale.</param>
	/// <param name="fallback">Fallback locale.</param>
	private _LocalizationState(LocaleCollection locales, IReadOnlyList<LocalizationIssue> issues,
							   IReadOnlyDictionary<string, Dictionary<string, string>> levels,
							   ConcurrentDictionary<string, FrozenDictionary<string, string>> texts, Locale? current, Locale? fallback)
	{
		_Locales = locales;
		_Issues = issues;
		_Levels = levels;
		_Texts = texts;
		_Current = current;
		_Fallback = fallback;

		_CurrentTexts = ((current is null) ? FrozenDictionary<string, string>.Empty : _GetTexts(current));
		_FallbackTexts = ((fallback is null) ? FrozenDictionary<string, string>.Empty : _GetTexts(fallback));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Loads locales and tables from sources.</summary>
	/// <param name="sources">Sources.</param>
	/// <returns>Returns the state without selected locales.</returns>
	internal static _LocalizationState _Load(IEnumerable<ILocalizationSource> sources)
	{
		List<LocalizationIssue> issues = new();
		List<Locale> locales = new();
		Dictionary<string, Dictionary<string, string>> levels = new(StringComparer.OrdinalIgnoreCase);
		Dictionary<string, Dictionary<string, string>> origins = new(StringComparer.OrdinalIgnoreCase);

		foreach(ILocalizationSource i in sources)
		{
			LocalizationResource[] resources;
			try
			{
				resources = i.GetResources().ToArray();
			}
			catch(Exception ex)
			{
				issues.Add(new(i.ToString() ?? string.Empty, "The source could not be read.", ex));
				continue;
			}

			foreach(LocalizationResource k in resources)
			{
				if(LocalizationResource._IsLocale(k.Name))
				{
					_LoadLocale(k, locales, issues);
				}
				else if(LocalizationResource._IsTable(k.Name))
				{
					_LoadTable(k, levels, origins, issues);
				}
			}
		}

		return new(new LocaleCollection(locales), issues.AsReadOnly(), levels, new(StringComparer.OrdinalIgnoreCase), null, null);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Loads a locale definition.</summary>
	/// <param name="resource">Resource.</param>
	/// <param name="locales">Locales.</param>
	/// <param name="issues">Issues.</param>
	private static void _LoadLocale(LocalizationResource resource, List<Locale> locales, List<LocalizationIssue> issues)
	{
		Locale l;
		try
		{
			using(Stream s = resource.Open())
			{
				l = LocaleDefinition.Load(s).ToLocale();
			}
		}
		catch(Exception ex)
		{
			issues.Add(new(resource.Name, "The locale definition could not be read: " + ex.Message, ex));
			return;
		}

		if(locales.Contains(l))
		{
			issues.Add(new(resource.Name, "The locale \"" + l.Key + "\" has already been defined; the definition is ignored."));
			return;
		}

		locales.Add(l);
	}


	/// <summary>Loads a language table.</summary>
	/// <param name="resource">Resource.</param>
	/// <param name="levels">Merged texts by locale key.</param>
	/// <param name="origins">Resource names of the texts by locale key.</param>
	/// <param name="issues">Issues.</param>
	private static void _LoadTable(LocalizationResource resource, Dictionary<string, Dictionary<string, string>> levels,
								   Dictionary<string, Dictionary<string, string>> origins, List<LocalizationIssue> issues)
	{
		LanguageTable t;
		try
		{
			using(Stream s = resource.Open())
			{
				t = LanguageTable.Load(s, LocalizationResource._IsCompressedTable(resource.Name));
			}
		}
		catch(Exception ex)
		{
			issues.Add(new(resource.Name, "The language table could not be read: " + ex.Message, ex));
			return;
		}

		string key = t.LocaleKey.Trim();
		if(key.Length == 0)
		{
			issues.Add(new(resource.Name, "The language table has no locale key; it is ignored."));
			return;
		}

		if(!levels.TryGetValue(key, out Dictionary<string, string>? texts))
		{
			levels.Add(key, texts = new(StringComparer.Ordinal));
			origins.Add(key, new(StringComparer.Ordinal));
		}
		Dictionary<string, string> orig = origins[key];

		foreach(KeyValuePair<string, string> i in t.GetLookupEntries())
		{
			if(orig.TryGetValue(i.Key, out string? prev))
			{
				issues.Add(new(resource.Name, "The symbol \"" + i.Key + "\" for locale \"" + key + "\" has already been defined in " +
											  prev + "; the later definition is used."));
			}

			texts[i.Key] = i.Value;
			orig[i.Key] = resource.Name;
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Resolves a selected locale.</summary>
	/// <param name="selected">Selected locale, NULL for the default locale.</param>
	/// <returns>Returns the locale from the collection with the same key, the selected locale if it is not in the
	///          collection, or the default locale if nothing is selected.</returns>
	private Locale? _Resolve(Locale? selected)
	{
		if(selected is null) { return _Locales.Default; }
		return (_Locales[selected.Key] ?? selected);
	}


	/// <summary>Builds the texts of a locale including its base locales.</summary>
	/// <param name="locale">Locale.</param>
	/// <returns>Returns the texts.</returns>
	private FrozenDictionary<string, string> _BuildTexts(Locale locale)
	{
		List<string> chain = new();
		HashSet<string> visited = new(StringComparer.OrdinalIgnoreCase);

		string? key = locale.Key;
		Locale? l = locale;
		while((key is not null) && visited.Add(key))
		{
			chain.Insert(0, key);
			key = (l?.BaseKey);
			l = ((key is null) ? null : _Locales[key]);
		}

		Dictionary<string, string> rval = new(StringComparer.Ordinal);
		foreach(string i in chain)
		{
			if(_Levels.TryGetValue(i, out Dictionary<string, string>? texts))
			{
				foreach(KeyValuePair<string, string> k in texts) { rval[k.Key] = k.Value; }
			}
		}

		return rval.ToFrozenDictionary(StringComparer.Ordinal);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal properties                                                                                              //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the available locales.</summary>
	internal LocaleCollection _Locales
	{
		get;
	}


	/// <summary>Gets the problems that occurred while loading.</summary>
	internal IReadOnlyList<LocalizationIssue> _Issues
	{
		get;
	}


	/// <summary>Gets the effective current locale.</summary>
	internal Locale? _Current
	{
		get;
	}


	/// <summary>Gets the effective fallback locale.</summary>
	internal Locale? _Fallback
	{
		get;
	}


	/// <summary>Gets the texts of the current locale.</summary>
	internal FrozenDictionary<string, string> _CurrentTexts
	{
		get;
	}


	/// <summary>Gets the texts of the fallback locale.</summary>
	internal FrozenDictionary<string, string> _FallbackTexts
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal methods                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a state with selected locales that shares the loaded data with this state.</summary>
	/// <param name="current">Selected current locale, NULL for the default locale.</param>
	/// <param name="fallback">Selected fallback locale, NULL for the default locale.</param>
	/// <returns>Returns the state.</returns>
	internal _LocalizationState _Select(Locale? current, Locale? fallback)
	{
		return new(_Locales, _Issues, _Levels, _Texts, _Resolve(current), _Resolve(fallback));
	}


	/// <summary>Gets the texts of a locale including its base locales.</summary>
	/// <param name="locale">Locale.</param>
	/// <returns>Returns the texts.</returns>
	internal FrozenDictionary<string, string> _GetTexts(Locale locale)
	{
		return _Texts.GetOrAdd(locale.Key, m => _BuildTexts(locale));
	}
}
