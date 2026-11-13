namespace Robbiblubber.Localization.Editor;

using System.Diagnostics;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook;
using Robbiblubber.Spellbook.Controls;



/// <summary>This class shows the user help.</summary>
/// <remarks>The help consists of HTML pages in the folder <c>help</c> next to the application, with one subfolder per language,
///          for example <c>help\de\find.html</c>. The page is shown in the language of the editor, otherwise in its base language,
///          otherwise in English, and opened in the default browser.</remarks>
[SupportedOSPlatform("windows")]
internal static class _Help
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Language used when the help is not available in the language of the editor.</summary>
	private const string _FALLBACK_LANGUAGE = "en";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal constants                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Start page of the help.</summary>
	internal const string _START_PAGE = "index";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Shows a page of the help.</summary>
	/// <param name="owner">Window that requested the help; error messages are shown over it.</param>
	/// <param name="page">Page name without extension, for example <c>find</c>.</param>
	internal static void _Show(IWin32Window owner, string page)
	{
		string? file = _FindPage(page) ?? _FindPage(_START_PAGE);
		if(file is null)
		{
			ThemedMessageBox.Show(owner, "led:help.missing".LookUp("The help could not be found."), "led:help.caption".LookUp("Help"),
								  MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		try
		{
			Process.Start(new ProcessStartInfo(file) { UseShellExecute = true })?.Dispose();
		}
		catch(Exception ex)
		{
			ThemedMessageBox.Show(owner, "led:help.failed".LookUp("The help could not be opened.\n\n$ERROR", ("ERROR", ex.Message)), "led:help.caption".LookUp("Help"),
								  MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns the file of a help page in the best available language.</summary>
	/// <param name="page">Page name without extension.</param>
	/// <returns>Returns the full file name, NULL if the page does not exist in any of the languages.</returns>
	private static string? _FindPage(string page)
	{
		string folder = Path.Combine(PathOp.ApplicationDirectory, "help");

		foreach(string? i in new string?[] { Locale.Current?.Key, Locale.Current?.BaseKey, _FALLBACK_LANGUAGE })
		{
			if(string.IsNullOrWhiteSpace(i)) continue;

			string file = Path.Combine(folder, i, page + ".html");
			if(File.Exists(file)) { return file; }
		}
		return null;
	}
}
