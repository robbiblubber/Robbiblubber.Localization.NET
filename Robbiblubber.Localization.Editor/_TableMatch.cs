namespace Robbiblubber.Localization.Editor;



/// <summary>This structure describes a cell of a language table that contains the search text.</summary>
/// <param name="Row">Row index.</param>
/// <param name="Column">Column: <see cref="_TableSearch.SYMBOL_COLUMN"/> or <see cref="_TableSearch.TEXT_COLUMN"/>.</param>
/// <param name="Wrapped">Determines if the search has passed the end (or, searching backwards, the beginning) of the table and
///                       continued at the other end.</param>
internal readonly record struct _TableMatch(int Row, int Column, bool Wrapped);
