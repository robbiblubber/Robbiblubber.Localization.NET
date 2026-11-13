namespace Robbiblubber.Localization.Builder;



/// <summary>This class contains the main entry point.</summary>
internal static class _Program
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// main entry point                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Main entry point.</summary>
	/// <param name="args">Arguments.</param>
	/// <returns>Returns the exit code: 0 for success, 1 for invalid arguments, 2 for a missing or invalid input file, 3 if the
	///          output file could not be written and 4 if the output file exists and may not be overwritten.</returns>
	private static int Main(string[] args)
	{
		return new _Builder(Console.In, Console.Out, Console.Error).Run(args);
	}
}
