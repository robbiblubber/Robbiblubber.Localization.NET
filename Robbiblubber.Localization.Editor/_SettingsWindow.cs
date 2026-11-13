namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class implements the settings window.</summary>
/// <remarks>A language or theme selected in the window is applied at once, so that the user sees the editor in it. The settings are
///          saved when the window is closed with OK; otherwise the previous language and theme are restored. Changes made in the theme
///          editor are saved when the editor is closed with OK.</remarks>
[SupportedOSPlatform("windows")]
internal sealed partial class _SettingsWindow: Form
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Language when the window was opened.</summary>
	private readonly Locale? _OriginalLocale;

	/// <summary>Theme when the window was opened.</summary>
	private readonly Theme _OriginalTheme;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	public _SettingsWindow()
	{
		InitializeComponent();

		_OriginalLocale = Locale.Current;
		_SelectorLanguage.SelectedLocale = Locale.Current;
		_OriginalTheme = ThemeManager.Theme;
		Locale.LocaleChanged += _Locale_LocaleChanged;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// protected methods                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Restores the previous language and theme if the window has not been closed with OK.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		if((DialogResult != DialogResult.OK) && (_OriginalLocale is not null) && (Locale.Current?.Key != _OriginalLocale.Key))
		{
			Locale.Current = _OriginalLocale;
		}
		if((DialogResult != DialogResult.OK) && (ThemeManager.Theme != _OriginalTheme) && ThemeManager.Themes.Contains(_OriginalTheme))
		{
			ThemeManager.Theme = _OriginalTheme;
			_Settings._SaveTheme();
		}
		Locale.LocaleChanged -= _Locale_LocaleChanged;
		base.OnFormClosed(e);
	}


	/// <summary>Help requested (F1): shows the help page about the settings.</summary>
	/// <param name="hevent">Event arguments.</param>
	protected override void OnHelpRequested(HelpEventArgs hevent)
	{
		_Help._Show(this, "settings");
		hevent.Handled = true;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Language changed: the theme selector shows the names of the built-in themes in the new language.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Locale_LocaleChanged(object? sender, EventArgs e)
	{
		_SelectorTheme.RefreshTexts();
	}


	/// <summary>OK clicked.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonOK_Click(object? sender, EventArgs e)
	{
		if((_SelectorLanguage.SelectedLocale is Locale l) && (l.Key != Locale.Current?.Key)) { Locale.Current = l; }
		_Settings._Save();
	}
}
