namespace Robbiblubber.Localization.Editor;



/// <summary>This enumeration defines problems of a table row.</summary>
internal enum _RowProblem: int
{
	/// <summary>The row has a text but no symbol; it is not saved.</summary>
	NO_SYMBOL = 0,

	/// <summary>The symbol occurs more than once; only the last row is saved.</summary>
	DUPLICATE = 1,

	/// <summary>The symbol contains a prefix, although the table puts its own prefix in front of it.</summary>
	PREFIX = 2
}
