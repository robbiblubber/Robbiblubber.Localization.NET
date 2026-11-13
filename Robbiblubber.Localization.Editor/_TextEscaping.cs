namespace Robbiblubber.Localization.Editor;

using System.Text;



/// <summary>This class converts texts to the single-line form shown in the table and back.</summary>
/// <remarks>Line breaks are shown as <c>\n</c>, tabs as <c>\t</c>. A backslash is doubled only where it would otherwise be read as
///          the start of such a sequence, so paths like <c>C:\Data</c> keep their form.</remarks>
internal static class _TextEscaping
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns if a backslash followed by a character would be read as the start of an escape sequence.</summary>
	/// <param name="next">Character that follows the backslash in the text.</param>
	/// <returns>Returns TRUE if the backslash has to be doubled, otherwise returns FALSE.</returns>
	/// <remarks>Line breaks and tabs are written as escape sequences, so a backslash before them is doubled as well.</remarks>
	private static bool _StartsSequence(char next)
	{
		return next is 'n' or 't' or '\\' or '\r' or '\n' or '\t';
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Converts a text to its single-line form.</summary>
	/// <param name="text">Text.</param>
	/// <returns>Returns the single-line form.</returns>
	internal static string _Escape(string? text)
	{
		if(string.IsNullOrEmpty(text)) { return string.Empty; }

		StringBuilder rval = new(text.Length + 8);
		for(int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			switch(c)
			{
				case '\r':
					if(((i + 1) < text.Length) && (text[i + 1] == '\n')) { i++; }
					rval.Append("\\n");
					break;
				case '\n':
					rval.Append("\\n"); break;
				case '\t':
					rval.Append("\\t"); break;
				case '\\':
					if(((i + 1) < text.Length) && _StartsSequence(text[i + 1]))
					{
						rval.Append("\\\\");
					}
					else { rval.Append('\\'); }
					break;
				default:
					rval.Append(c); break;
			}
		}
		return rval.ToString();
	}


	/// <summary>Converts a single-line form back to the text.</summary>
	/// <param name="value">Single-line form.</param>
	/// <returns>Returns the text; line breaks are <see cref="Environment.NewLine"/>.</returns>
	internal static string _Unescape(string? value)
	{
		if(string.IsNullOrEmpty(value)) { return string.Empty; }

		StringBuilder rval = new(value.Length);
		for(int i = 0; i < value.Length; i++)
		{
			char c = value[i];
			if((c == '\\') && ((i + 1) < value.Length))
			{
				switch(value[i + 1])
				{
					case 'n':
						rval.Append(Environment.NewLine); i++; continue;
					case 't':
						rval.Append('\t'); i++; continue;
					case '\\':
						rval.Append('\\'); i++; continue;
				}
			}
			rval.Append(c);
		}
		return rval.ToString();
	}
}
