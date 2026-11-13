namespace Robbiblubber.Localization.Builder;



/// <summary>This enumeration defines the exit codes of the build tool.</summary>
internal enum _ExitCode: int
{
	/// <summary>The operation has been completed successfully.</summary>
	OK = 0,

	/// <summary>The arguments are invalid or incomplete.</summary>
	INVALID_ARGUMENTS = 1,

	/// <summary>An input file (template, image or locale file) is missing or invalid.</summary>
	INPUT_ERROR = 2,

	/// <summary>The output file could not be written.</summary>
	WRITE_ERROR = 3,

	/// <summary>The output file exists and may not be overwritten.</summary>
	OUTPUT_EXISTS = 4
}
