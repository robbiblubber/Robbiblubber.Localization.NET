namespace Robbiblubber.Localization.Editor;



/// <summary>This class represents a row of the language table as it is shown and edited in the table.</summary>
internal sealed class _TableRow
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Symbol.</summary>
	private string _Symbol = string.Empty;

	/// <summary>Text.</summary>
	private string _Text = string.Empty;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	public _TableRow()
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="symbol">Symbol.</param>
	/// <param name="text">Text in its single-line form.</param>
	public _TableRow(string symbol, string text)
	{
		Symbol = symbol;
		Text = text;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets or sets the symbol.</summary>
	/// <remarks>NULL is stored as an empty string.</remarks>
	public string Symbol
	{
		get { return _Symbol; }
		set { _Symbol = (value ?? string.Empty); }
	}


	/// <summary>Gets or sets the text in its single-line form.</summary>
	/// <remarks>NULL is stored as an empty string.</remarks>
	public string Text
	{
		get { return _Text; }
		set { _Text = (value ?? string.Empty); }
	}
}
