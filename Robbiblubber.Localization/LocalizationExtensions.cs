namespace Robbiblubber.Localization;



/// <summary>This class provides extension methods for localization.</summary>
public static class LocalizationExtensions
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Looks up a symbol in the texts of the current locale.</summary>
	/// <param name="symbol">Symbol.</param>
	/// <param name="defaultText">Default text.</param>
	/// <returns>Returns the text; see <see cref="Locale.LookUp(string?, string?)"/>.</returns>
	public static string LookUp(this string? symbol, string? defaultText = null)
	{
		return Locale.LookUp(symbol, defaultText);
	}


	/// <summary>Looks up a symbol in the texts of the current locale and replaces placeholders.</summary>
	/// <param name="symbol">Symbol.</param>
	/// <param name="defaultText">Default text.</param>
	/// <param name="values">Placeholder values; a value named <c>NAME</c> replaces <c>$NAME</c>.</param>
	/// <returns>Returns the text; see <see cref="Locale.LookUp(string?, string?, ValueTuple{string, object?}[])"/>.</returns>
	public static string LookUp(this string? symbol, string? defaultText, params (string Name, object? Value)[] values)
	{
		return Locale.LookUp(symbol, defaultText, values);
	}
}
