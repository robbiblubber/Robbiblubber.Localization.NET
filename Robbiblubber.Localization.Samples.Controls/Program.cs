namespace Robbiblubber.Localization.Samples.Controls;

using System.Runtime.Versioning;



/// <summary>This class provides the application entry point.</summary>
[SupportedOSPlatform("windows")]
public static class Program
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Runs the application.</summary>
	/// <remarks>The locales and language tables are loaded from the <c>Locales</c> directory next to the application.</remarks>
	[STAThread]
	public static void Main()
	{
		Locale.Sources.Set(new DirectoryLocalizationSource(Path.Combine(AppContext.BaseDirectory, "locale")));

		ApplicationConfiguration.Initialize();
		Application.Run(new MainWindow());
	}
}
