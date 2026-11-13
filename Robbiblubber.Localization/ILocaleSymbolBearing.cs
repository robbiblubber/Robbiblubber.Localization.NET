namespace Robbiblubber.Localization;



/// <summary>Classes that carry a locale symbol implement this interface.</summary>
public interface ILocaleSymbolBearing
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the locale symbol for this instance.</summary>
	public string LocaleSymbol { get; }
}
