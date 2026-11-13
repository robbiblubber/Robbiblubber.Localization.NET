namespace Robbiblubber.Localization.Controls;

using System.ComponentModel;
using System.Runtime.Versioning;



/// <summary>This class implements a localizable list view column header.</summary>
/// <remarks>The column header is localized when it is created and by <see cref="Localize"/>. If no default text is set, the text
///          the column header has when it is localized for the first time becomes the default text.</remarks>
[SupportedOSPlatform("windows")]
public class LocalizableColumnHeader: ColumnHeader, ILocaleSymbolBearing, ILocalizable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Determines if the default text has been determined.</summary>
	private bool _Captured = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public LocalizableColumnHeader(): this(string.Empty)
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="localeSymbol">Locale symbol.</param>
	/// <param name="defaultText">Default text, displayed if the symbol is not defined.</param>
	public LocalizableColumnHeader(string localeSymbol, string? defaultText = null): base()
	{
		LocaleSymbol = (localeSymbol ?? string.Empty);
		DefaultText = defaultText;
		Text = defaultText;
		Localize();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the default text, displayed if the symbol is not defined.</summary>
	[Category("Localization")][Description("Default text, displayed if the symbol is not defined.")][DefaultValue(null)]
	public string? DefaultText
	{
		get; set;
	}


	/// <summary>Gets or sets the locale symbol.</summary>
	[Category("Localization")][Description("Locale symbol for the text of the column header.")][DefaultValue("")]
	public string LocaleSymbol
	{
		get; set;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] ILocalizable                                                                                         //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the column header with the texts of the current locale.</summary>
	public virtual void Localize()
	{
		if(LocaleSymbol.Length == 0) { return; }

		if(!_Captured)
		{
			if((DefaultText is null) && (!string.IsNullOrEmpty(Text))) { DefaultText = Text; }
			_Captured = true;
		}

		string text = LocaleSymbol.LookUp(DefaultText);
		if(Text != text) { Text = text; }
	}
}
