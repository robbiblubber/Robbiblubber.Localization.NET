namespace Robbiblubber.Localization.Editor;

using Robbiblubber.Spellbook;
using Robbiblubber.Spellbook.Controls.Themes;

using System.Runtime.Versioning;



/// <summary>This class provides the user settings of the editor.</summary>
/// <remarks>The settings are kept in <c>led.settings</c> in the user configuration path. The selected theme is kept in
///          <c>led.themes</c>; the custom themes are shared with other applications (<see cref="SharedThemeStore"/>).</remarks>
[SupportedOSPlatform("windows")]
internal static class _Settings
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal constants                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Maximum number of entries in the search history.</summary>
	internal const int _HISTORY_SIZE = 10;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static properties                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the settings file.</summary>
	internal static string _FileName
	{
		get { return Path.Combine(PathOp.UserConfigurationPath, "led.settings"); }
	}


	/// <summary>Gets the recent files list file.</summary>
	internal static string _RecentFileName
	{
		get { return Path.Combine(PathOp.UserConfigurationPath, "led.recent"); }
	}


	/// <summary>Gets the file that keeps the selected theme.</summary>
	internal static string _ThemeFileName
	{
		get { return Path.Combine(PathOp.UserConfigurationPath, "led.themes"); }
	}


	/// <summary>Gets the window layout file.</summary>
	internal static string _LayoutFileName
	{
		get { return Path.Combine(PathOp.UserConfigurationPath, "led.layout"); }
	}



	/// <summary>Gets the recent search texts, the latest first.</summary>
	internal static List<string> _FindHistory
	{
		get;
	} = new();


	/// <summary>Gets the recent replacements, the latest first.</summary>
	internal static List<string> _ReplaceHistory
	{
		get;
	} = new();


	/// <summary>Gets or sets the columns that are searched.</summary>
	internal static _SearchScope _FindScope
	{
		get; set;
	} = _SearchScope.BOTH;


	/// <summary>Gets or sets if the search distinguishes upper and lower case.</summary>
	internal static bool _FindMatchCase
	{
		get; set;
	}


	/// <summary>Gets or sets if the search finds whole words only.</summary>
	internal static bool _FindWholeWord
	{
		get; set;
	}


	/// <summary>Gets or sets if the search text is a regular expression.</summary>
	internal static bool _FindRegularExpression
	{
		get; set;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Adds an entry to a history list.</summary>
	/// <param name="history">History list.</param>
	/// <param name="value">Entry.</param>
	/// <remarks>The entry is moved to the top of the list; the list is limited to <see cref="_HISTORY_SIZE"/> entries.</remarks>
	internal static void _AddToHistory(List<string> history, string value)
	{
		history.Remove(value);
		history.Insert(0, value);
		if(history.Count > _HISTORY_SIZE) { history.RemoveRange(_HISTORY_SIZE, (history.Count - _HISTORY_SIZE)); }
	}



	/// <summary>Applies the saved settings: the theme, the language and the search options and history.</summary>
	/// <remarks>Errors are ignored; the defaults are used then.</remarks>
	internal static void _Load()
	{
		try
		{
			ThemeManager.Load(new SharedThemeStore(null, _ThemeFileName));
		}
		catch(Exception) {}

		try
		{
			if(!File.Exists(_FileName)) return;

			DdnElement cfg = Ddn.Open(_FileName);
			string? key = cfg["locale"].Value;
			if((!string.IsNullOrWhiteSpace(key)) && Locale.Locales.TryGet(key, out Locale? l)) { Locale.Current = l; }

			_FindHistory.Clear();
			_FindHistory.AddRange(cfg.Get<string[]>("find/history", []).Where(m => !string.IsNullOrEmpty(m)).Take(_HISTORY_SIZE));
			_ReplaceHistory.Clear();
			_ReplaceHistory.AddRange(cfg.Get<string[]>("find/replace", []).Take(_HISTORY_SIZE));
			_FindScope = cfg.Get("find/scope", _SearchScope.BOTH);
			if((_FindScope & _SearchScope.BOTH) == 0) { _FindScope = _SearchScope.BOTH; }
			_FindMatchCase = cfg.Get("find/case", false);
			_FindWholeWord = cfg.Get("find/word", false);
			_FindRegularExpression = cfg.Get("find/regex", false);
		}
		catch(Exception) {}
	}


	/// <summary>Saves the selected theme and the custom themes.</summary>
	/// <remarks>Errors are ignored.</remarks>
	internal static void _SaveTheme()
	{
		try
		{
			ThemeManager.Save();
		}
		catch(Exception) {}
	}


	/// <summary>Saves the settings.</summary>
	/// <remarks>Errors are ignored.</remarks>
	internal static void _Save()
	{
		_SaveTheme();

		try
		{
			DdnElement cfg = Ddn.Open(_FileName, true);
			cfg.Set("locale", Locale.Current?.Key);
			cfg.Set("find/history", _FindHistory);
			cfg.Set("find/replace", _ReplaceHistory);
			cfg.Set("find/scope", _FindScope);
			cfg.Set("find/case", _FindMatchCase);
			cfg.Set("find/word", _FindWholeWord);
			cfg.Set("find/regex", _FindRegularExpression);
			cfg.Save(_FileName);
		}
		catch(Exception) {}
	}
}
