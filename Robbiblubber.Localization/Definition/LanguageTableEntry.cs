namespace Robbiblubber.Localization.Definition;



/// <summary>This class represents an entry of a language table.</summary>
public sealed class LanguageTableEntry: IComparable<LanguageTableEntry>, IComparable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public LanguageTableEntry()
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="symbol">Symbol.</param>
	/// <param name="text">Text.</param>
	public LanguageTableEntry(string symbol, string text)
	{
		Symbol = symbol;
		Text = text;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the symbol.</summary>
	/// <remarks>The symbol is used without the table prefix.</remarks>
	public string Symbol
	{
		get; set;
	} = string.Empty;


	/// <summary>Gets or sets the text.</summary>
	public string Text
	{
		get; set;
	} = string.Empty;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IComparable<LanguageTableEntry>                                                                      //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Compares this instance to another entry by symbol.</summary>
	/// <param name="other">Entry.</param>
	/// <returns>Returns a value less than zero if this instance precedes the other entry, zero if both have the same symbol,
	///          and a value greater than zero if this instance follows the other entry or the other entry is NULL.</returns>
	public int CompareTo(LanguageTableEntry? other)
	{
		if(other is null) { return 1; }
		return string.CompareOrdinal(Symbol, other.Symbol);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IComparable                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	int IComparable.CompareTo(object? obj)
	{
		return CompareTo(obj as LanguageTableEntry);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override string ToString()
	{
		return (Symbol + " = " + Text);
	}
}
