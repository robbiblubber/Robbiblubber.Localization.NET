namespace Robbiblubber.Localization.Controls;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;



/// <summary>This class localizes the dialogs of the Spellbook Controls Library.</summary>
/// <remarks><para>The Spellbook Controls Library does not depend on the Localization Library. Its dialogs,
///          <see cref="ThemedMessageBox"/>, <see cref="ThemedColorDialog"/> and
///          <see cref="Spellbook.Controls.Themes.ThemeEditorDialog"/>, and the <see cref="Spellbook.Controls.Themes.ThemeSelector"/>
///          ask <see cref="ControlTexts.LookUp"/> for their texts. <see cref="Enable"/> connects this function to
///          <see cref="Locale.LookUp(string?, string?)"/>, so the dialogs show the texts of the current locale each time they are
///          opened.</para>
///          <para>The language tables have the prefix <c>spellbook</c>, one table per dialog and locale
///          (<c>spellbook.msgbox.de.ltab</c>, <c>spellbook.themeeditor.de.ltab</c>, <c>spellbook.colordialog.de.ltab</c>,
///          <c>spellbook.themeselector.de.ltab</c>, …) for <c>de</c>, <c>en</c>, <c>fr</c> and
///          <c>id</c>. They are copied to the directory <c>locale</c> in the application directory, where the default source
///          of <see cref="Locale.Sources"/> finds them; an application that sets its own sources includes this directory.
///          Other locales are added with tables with the same prefix; a single text can be changed in a table of a later source.
///          </para>
///          <para>Texts that are set explicitly in <see cref="ThemedMessageBoxTexts"/>, <see cref="ThemedColorDialogTexts"/>,
///          <see cref="Spellbook.Controls.Themes.ThemeEditorTexts"/> or <see cref="Spellbook.Controls.Themes.ThemeSelectorTexts"/>
///          are used instead of the localization.</para></remarks>
[SupportedOSPlatform("windows")]
public static class SpellbookLocalization
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Looks up a text.</summary>
	/// <param name="symbol">Symbol.</param>
	/// <param name="defaultText">Default text.</param>
	/// <returns>Returns the text of the current locale.</returns>
	private static string _LookUp(string symbol, string defaultText)
	{
		return Locale.LookUp(symbol, defaultText);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static properties                                                                                         //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets if the dialogs of the Spellbook Controls Library are localized.</summary>
	public static bool Enabled
	{
		get { return (ControlTexts.LookUp == _LookUp); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the dialogs of the Spellbook Controls Library.</summary>
	public static void Enable()
	{
		ControlTexts.LookUp = _LookUp;
	}


	/// <summary>Ends the localization of the dialogs of the Spellbook Controls Library; they show their English default texts or
	///          the texts set explicitly.</summary>
	public static void Disable()
	{
		if(Enabled) { ControlTexts.LookUp = null; }
	}
}
