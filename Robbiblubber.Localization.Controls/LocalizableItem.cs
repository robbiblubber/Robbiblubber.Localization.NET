namespace Robbiblubber.Localization.Controls;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;



/// <summary>This class implements a localizable item for list boxes, and themed combo boxes.</summary>
/// <remarks>The item is localized when it is created and by <see cref="Localize"/>; the extension methods in
///          <see cref="ControlLocalizationExtensions"/> and <see cref="LocaleProvider"/> localize the items of a control and
///          update the control. The item displays <see cref="Text"/>.</remarks>
[SupportedOSPlatform("windows")]
public class LocalizableItem: ILocaleSymbolBearing, ILocalizable, IListItem
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="localeSymbol">Locale symbol.</param>
	/// <param name="defaultText">Default text, displayed if the symbol is not defined.</param>
	public LocalizableItem(string localeSymbol, string? defaultText = null): base()
	{
		LocaleSymbol = (localeSymbol ?? string.Empty);
		DefaultText = defaultText;
		Localize();
	}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="localeSymbol">Locale symbol.</param>
	/// <param name="defaultText">Default text, displayed if the symbol is not defined.</param>
	/// <param name="tag">Tag.</param>
	public LocalizableItem(string localeSymbol, string? defaultText, object? tag): this(localeSymbol, defaultText)
	{
		Tag = tag;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the default text, displayed if the symbol is not defined.</summary>
	public string? DefaultText
	{
		get; set;
	}


	/// <summary>Gets or sets the locale symbol.</summary>
	public string LocaleSymbol
	{
		get; set;
	}


	/// <summary>Gets the localized text.</summary>
	public string Text
	{
		get; private set;
	} = string.Empty;


	/// <summary>Gets or sets the detail text.</summary>
	public string? Detail
	{
		get; set;
	}


	/// <summary>Gets or sets the description.</summary>
	public string? Description
	{
		get; set;
	}


	/// <summary>Gets or sets the image.</summary>
	public Image? Image
	{
		get; set;
	}


	/// <summary>Gets or sets the image key.</summary>
	public string? ImageKey
	{
		get; set;
	}


	/// <summary>Gets or sets the font style.</summary>
	public FontStyle FontStyle
	{
		get; set;
	} = FontStyle.Regular;


	/// <summary>Gets or sets the text color, <see cref="Color.Empty"/> for the default color.</summary>
	public Color ForeColor
	{
		get; set;
	} = Color.Empty;


	/// <summary>Gets or sets the tag.</summary>
	public object? Tag
	{
		get; set;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IListItem                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	string? IListItem.FilterText
	{
		get { return null; }
	}


	/// <inheritdoc/>
	string? IListItem.SortText
	{
		get { return null; }
	}


	/// <inheritdoc/>
	string? IListItem.Category
	{
		get { return null; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] ILocalizable                                                                                         //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the item with the texts of the current locale.</summary>
	public virtual void Localize()
	{
		Text = LocaleSymbol.LookUp(DefaultText);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Returns a string representation of this instance.</summary>
	/// <returns>Returns the localized text.</returns>
	public override string ToString()
	{
		return Text;
	}
}
