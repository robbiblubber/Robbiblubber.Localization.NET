namespace Robbiblubber.Localization.Controls;

using System.Runtime.Versioning;



/// <summary>This class implements a localizable list view item.</summary>
/// <remarks>The item is localized when it is created and by <see cref="Localize"/>. Only the text of the item is localized, not
///          the texts of its sub-items. If no default text is set, the text the item has when it is localized for the first time
///          becomes the default text.</remarks>
[SupportedOSPlatform("windows")]
public class LocalizableListViewItem: ListViewItem, ILocaleSymbolBearing, ILocalizable
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
	public LocalizableListViewItem(): this(string.Empty)
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="localeSymbol">Locale symbol.</param>
	/// <param name="defaultText">Default text, displayed if the symbol is not defined.</param>
	public LocalizableListViewItem(string localeSymbol, string? defaultText = null): base()
	{
		LocaleSymbol = (localeSymbol ?? string.Empty);
		DefaultText = defaultText;
		Text = defaultText;
		Localize();
	}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="localeSymbol">Locale symbol.</param>
	/// <param name="defaultText">Default text, displayed if the symbol is not defined.</param>
	/// <param name="imageKey">Image key.</param>
	public LocalizableListViewItem(string localeSymbol, string? defaultText, string imageKey): this(localeSymbol, defaultText)
	{
		ImageKey = imageKey;
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



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] ILocalizable                                                                                         //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the item with the texts of the current locale.</summary>
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



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] ListViewItem                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override object Clone()
	{
		object rval = base.Clone();

		if(rval is LocalizableListViewItem n)
		{
			n.LocaleSymbol = LocaleSymbol;
			n.DefaultText = DefaultText;
			n._Captured = _Captured;
		}

		return rval;
	}
}
