namespace Robbiblubber.Localization.Editor;



/// <summary>This enumeration defines the columns of a language table that are searched.</summary>
[Flags]
internal enum _SearchScope: int
{
	/// <summary>The symbols are searched.</summary>
	SYMBOL = 1,

	/// <summary>The texts are searched.</summary>
	TEXT = 2,

	/// <summary>Symbols and texts are searched.</summary>
	BOTH = (SYMBOL | TEXT)
}
