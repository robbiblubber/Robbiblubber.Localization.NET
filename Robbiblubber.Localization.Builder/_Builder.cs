namespace Robbiblubber.Localization.Builder;

using System.Buffers.Binary;

using Robbiblubber.Localization.Definition;
using Robbiblubber.Spellbook;



/// <summary>This class implements the locale definition build tool.</summary>
/// <remarks>The tool reads and writes through the given readers and writers, so it can be tested without a console.</remarks>
internal sealed class _Builder
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Input reader.</summary>
	private readonly TextReader _In;

	/// <summary>Output writer.</summary>
	private readonly TextWriter _Out;

	/// <summary>Error writer.</summary>
	private readonly TextWriter _Error;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="input">Input reader.</param>
	/// <param name="output">Output writer.</param>
	/// <param name="error">Error writer.</param>
	public _Builder(TextReader input, TextWriter output, TextWriter error)
	{
		_In = input;
		_Out = output;
		_Error = error;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Determines if data is an image in a format that the controls can display.</summary>
	/// <param name="data">Data.</param>
	/// <returns>Returns TRUE if the data starts with the signature of a PNG, JPEG, GIF, BMP, ICO or TIFF image, otherwise
	///          returns FALSE.</returns>
	private static bool _IsImage(byte[] data)
	{
		return (_GetFormat(data) is not null);
	}


	/// <summary>Gets the format of image data.</summary>
	/// <param name="data">Data.</param>
	/// <returns>Returns the format name, NULL if the format is not supported.</returns>
	private static string? _GetFormat(ReadOnlySpan<byte> data)
	{
		if(data.StartsWith((ReadOnlySpan<byte>) [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A])) { return "PNG"; }
		if(data.StartsWith((ReadOnlySpan<byte>) [0xFF, 0xD8, 0xFF])) { return "JPEG"; }
		if(data.StartsWith("GIF87a"u8) || data.StartsWith("GIF89a"u8)) { return "GIF"; }
		if(data.StartsWith("BM"u8)) { return "BMP"; }
		if(data.StartsWith((ReadOnlySpan<byte>) [0x00, 0x00, 0x01, 0x00])) { return "ICO"; }
		if(data.StartsWith("II*\0"u8) || data.StartsWith("MM\0*"u8)) { return "TIFF"; }

		return null;
	}


	/// <summary>Describes image data.</summary>
	/// <param name="data">Data, may be NULL.</param>
	/// <returns>Returns the format, the size in pixels where it can be read from the header, and the length.</returns>
	private static string _Describe(byte[]? data)
	{
		if(data is null) { return "(none)"; }

		ReadOnlySpan<byte> d = data;
		string format = (_GetFormat(d) ?? "unknown format");
		(int Width, int Height)? size = null;

		if((format == "PNG") && (d.Length >= 24))
		{
			size = (BinaryPrimitives.ReadInt32BigEndian(d[16..]), BinaryPrimitives.ReadInt32BigEndian(d[20..]));
		}
		else if((format == "GIF") && (d.Length >= 10))
		{
			size = (BinaryPrimitives.ReadUInt16LittleEndian(d[6..]), BinaryPrimitives.ReadUInt16LittleEndian(d[8..]));
		}
		else if((format == "BMP") && (d.Length >= 26))
		{
			size = (BinaryPrimitives.ReadInt32LittleEndian(d[18..]), Math.Abs(BinaryPrimitives.ReadInt32LittleEndian(d[22..])));
		}
		else if((format == "ICO") && (d.Length >= 8))
		{
			size = (((d[6] == 0) ? 256 : d[6]), ((d[7] == 0) ? 256 : d[7]));
		}

		return ((size is null) ? $"{format}, {data.Length} bytes" : $"{format}, {size.Value.Width} x {size.Value.Height}, {data.Length} bytes");
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Writes an error message.</summary>
	/// <param name="message">Message.</param>
	/// <param name="code">Exit code.</param>
	/// <returns>Returns the exit code.</returns>
	private int _Fail(string message, _ExitCode code)
	{
		_Error.WriteLine("Error: " + message);
		return (int) code;
	}


	/// <summary>Writes the usage information.</summary>
	private void _PrintUsage()
	{
		_Out.WriteLine("Locale Definition Build Tool, version " + VersionOp.ApplicationVersion);
		_Out.WriteLine();
		_Out.WriteLine("Usage:");
		_Out.WriteLine("  bloc [-f] File [-k] Key [-n] Name [-b BaseKey] [-s] SmallImage [-l] LargeImage [-d] [-r] [-o]");
		_Out.WriteLine("  bloc [-f] File -t Template [options]");
		_Out.WriteLine("  bloc -p File");
		_Out.WriteLine("  bloc -i");
		_Out.WriteLine();
		_Out.WriteLine("Options:");
		_Out.WriteLine("  -f, --file File           Output file.");
		_Out.WriteLine("  -t, --template File       Existing locale file whose values are used unless specified.");
		_Out.WriteLine("  -k, --key Key             Locale key, for example de or de-AT.");
		_Out.WriteLine("  -n, --name Name           Locale name, for example Deutsch.");
		_Out.WriteLine("  -b, --base Key            Key of the base locale; \"\" removes the base locale.");
		_Out.WriteLine("  -s, --small File          Small icon image (16 x 16); \"\" removes the icon.");
		_Out.WriteLine("  -l, --large File          Large icon image (32 x 32); \"\" removes the icon.");
		_Out.WriteLine("  -d, --default             Marks the locale as default locale.");
		_Out.WriteLine("  -r, --rtl                 Marks the locale as written right to left.");
		_Out.WriteLine("  -o, --overwrite           Overwrites an existing output file.");
		_Out.WriteLine("  -p, --info File           Shows the content of a locale file.");
		_Out.WriteLine("  -i, --interactive         Starts the interactive mode.");
		_Out.WriteLine("  -?, -h, --help            Shows this information.");
		_Out.WriteLine();
		_Out.WriteLine("  Long options also accept --option=value. Arguments without a switch are used in the order");
		_Out.WriteLine("  file, key, name, small image, large image. Icons are optional.");
		_Out.WriteLine();
		_Out.WriteLine("Exit codes:");
		_Out.WriteLine("  0  Success.");
		_Out.WriteLine("  1  Invalid or incomplete arguments.");
		_Out.WriteLine("  2  An input file is missing or invalid.");
		_Out.WriteLine("  3  The output file could not be written.");
		_Out.WriteLine("  4  The output file exists and --overwrite has not been specified.");
	}


	/// <summary>Loads a template.</summary>
	/// <param name="fileName">File name.</param>
	/// <param name="definition">Returns the definition, NULL if it could not be loaded.</param>
	/// <returns>Returns the exit code.</returns>
	private int _LoadTemplate(string fileName, out LocaleDefinition? definition)
	{
		definition = null;
		if(!File.Exists(fileName)) { return _Fail($"The template file \"{fileName}\" does not exist.", _ExitCode.INPUT_ERROR); }

		try
		{
			definition = LocaleDefinition.Load(fileName);
			return (int) _ExitCode.OK;
		}
		catch(Exception ex) when ((ex is IOException) || (ex is InvalidDataException) || (ex is UnauthorizedAccessException))
		{
			return _Fail($"The template file \"{fileName}\" is not a valid locale file: {ex.Message}", _ExitCode.INPUT_ERROR);
		}
	}


	/// <summary>Loads an image.</summary>
	/// <param name="fileName">File name; an empty string removes the image.</param>
	/// <param name="data">Returns the image data, NULL if the image is removed or could not be loaded.</param>
	/// <returns>Returns the exit code.</returns>
	private int _LoadImage(string fileName, out byte[]? data)
	{
		data = null;
		if(fileName.Length == 0) { return (int) _ExitCode.OK; }
		if(!File.Exists(fileName)) { return _Fail($"The image file \"{fileName}\" does not exist.", _ExitCode.INPUT_ERROR); }

		try
		{
			data = File.ReadAllBytes(fileName);
		}
		catch(Exception ex) when ((ex is IOException) || (ex is UnauthorizedAccessException))
		{
			return _Fail($"The image file \"{fileName}\" cannot be read: {ex.Message}", _ExitCode.INPUT_ERROR);
		}

		if(!_IsImage(data))
		{
			data = null;
			return _Fail($"The file \"{fileName}\" is not a supported image (PNG, JPEG, GIF, BMP, ICO or TIFF).", _ExitCode.INPUT_ERROR);
		}

		return (int) _ExitCode.OK;
	}


	/// <summary>Saves a locale definition.</summary>
	/// <param name="definition">Definition.</param>
	/// <param name="fileName">File name.</param>
	/// <returns>Returns the exit code.</returns>
	private int _Save(LocaleDefinition definition, string fileName)
	{
		try
		{
			definition.Save(fileName);
		}
		catch(Exception ex) when ((ex is IOException) || (ex is UnauthorizedAccessException) || (ex is NotSupportedException) ||
								  (ex is ArgumentException))
		{
			return _Fail($"The file \"{fileName}\" could not be written: {ex.Message}", _ExitCode.WRITE_ERROR);
		}

		_Out.WriteLine($"Created {Path.GetFullPath(fileName)}.");
		return (int) _ExitCode.OK;
	}


	/// <summary>Builds a locale file from the arguments.</summary>
	/// <param name="args">Arguments.</param>
	/// <returns>Returns the exit code.</returns>
	private int _Build(_Arguments args)
	{
		if(string.IsNullOrWhiteSpace(args.FileName)) { return _Fail("No output file specified.", _ExitCode.INVALID_ARGUMENTS); }

		LocaleDefinition? def = null;
		int rval;

		if(args.Template is not null)
		{
			if((rval = _LoadTemplate(args.Template, out def)) != 0) { return rval; }
		}
		else
		{
			if(string.IsNullOrWhiteSpace(args.Key)) { return _Fail("No locale key specified.", _ExitCode.INVALID_ARGUMENTS); }
			if(string.IsNullOrWhiteSpace(args.Name)) { return _Fail("No locale name specified.", _ExitCode.INVALID_ARGUMENTS); }
		}
		def ??= new();

		if(args.Key is not null)
		{
			if(string.IsNullOrWhiteSpace(args.Key)) { return _Fail("The locale key must not be empty.", _ExitCode.INVALID_ARGUMENTS); }
			def.Key = args.Key.Trim();
		}
		if(args.Name is not null) { def.Name = args.Name; }
		if(args.BaseKey is not null) { def.BaseKey = (string.IsNullOrWhiteSpace(args.BaseKey) ? null : args.BaseKey.Trim()); }
		if(args.IsDefault) { def.IsDefault = true; }
		if(args.RightToLeft) { def.RightToLeft = true; }

		if(args.SmallImage is not null)
		{
			if((rval = _LoadImage(args.SmallImage, out byte[]? data)) != 0) { return rval; }
			def.SmallIcon = data;
		}
		if(args.LargeImage is not null)
		{
			if((rval = _LoadImage(args.LargeImage, out byte[]? data)) != 0) { return rval; }
			def.LargeIcon = data;
		}

		if(File.Exists(args.FileName) && (!args.Overwrite))
		{
			return _Fail($"The file \"{args.FileName}\" already exists. Use --overwrite to replace it.", _ExitCode.OUTPUT_EXISTS);
		}

		return _Save(def, args.FileName);
	}


	/// <summary>Shows the content of a locale file.</summary>
	/// <param name="fileName">File name.</param>
	/// <returns>Returns the exit code.</returns>
	private int _ShowInfo(string fileName)
	{
		if(!File.Exists(fileName)) { return _Fail($"The file \"{fileName}\" does not exist.", _ExitCode.INPUT_ERROR); }

		LocaleDefinition def;
		try
		{
			def = LocaleDefinition.Load(fileName);
		}
		catch(Exception ex) when ((ex is IOException) || (ex is InvalidDataException) || (ex is UnauthorizedAccessException))
		{
			return _Fail($"The file \"{fileName}\" is not a valid locale file: {ex.Message}", _ExitCode.INPUT_ERROR);
		}

		_Out.WriteLine($"File:          {Path.GetFullPath(fileName)}");
		_Out.WriteLine($"Key:           {def.Key}");
		_Out.WriteLine($"Name:          {def.Name}");
		_Out.WriteLine($"Base:          {def.BaseKey ?? "(none)"}");
		_Out.WriteLine($"Default:       {(def.IsDefault ? "yes" : "no")}");
		_Out.WriteLine($"Right to left: {(def.RightToLeft ? "yes" : "no")}");
		_Out.WriteLine($"Small icon:    {_Describe(def.SmallIcon)}");
		_Out.WriteLine($"Large icon:    {_Describe(def.LargeIcon)}");

		return (int) _ExitCode.OK;
	}


	/// <summary>Asks for a value.</summary>
	/// <param name="prompt">Prompt.</param>
	/// <param name="current">Current value, NULL if there is none.</param>
	/// <returns>Returns the entered text, or the current value if nothing has been entered.</returns>
	private string? _Ask(string prompt, string? current)
	{
		_Out.Write(string.IsNullOrEmpty(current) ? $"{prompt}: " : $"{prompt} [{current}]: ");

		string text = (_In.ReadLine() ?? string.Empty).Trim();
		return ((text.Length == 0) ? current : text);
	}


	/// <summary>Asks a yes or no question.</summary>
	/// <param name="prompt">Prompt.</param>
	/// <param name="current">Current value, used if nothing has been entered.</param>
	/// <returns>Returns the answer.</returns>
	private bool _AskYesNo(string prompt, bool current)
	{
		_Out.Write($"{prompt} [{(current ? "Y/n" : "y/N")}]: ");

		string text = (_In.ReadLine() ?? string.Empty).Trim();
		if(text.Length == 0) { return current; }

		return text.StartsWith("y", StringComparison.OrdinalIgnoreCase);
	}


	/// <summary>Asks for an image file.</summary>
	/// <param name="prompt">Prompt.</param>
	/// <param name="current">Current image data, NULL if there is none.</param>
	/// <param name="data">Returns the image data.</param>
	/// <returns>Returns the exit code.</returns>
	private int _AskImage(string prompt, byte[]? current, out byte[]? data)
	{
		data = current;
		_Out.Write((current is null) ? $"{prompt}: " : $"{prompt} [keep, - to remove]: ");

		string text = (_In.ReadLine() ?? string.Empty).Trim();
		if(text.Length == 0) { return (int) _ExitCode.OK; }

		return _LoadImage(((text == "-") ? string.Empty : text), out data);
	}


	/// <summary>Runs the interactive mode.</summary>
	/// <returns>Returns the exit code.</returns>
	private int _RunInteractive()
	{
		_Out.WriteLine("Locale Definition Build Tool, version " + VersionOp.ApplicationVersion);
		_Out.WriteLine();

		LocaleDefinition? def = null;
		int rval;

		string? template = _Ask("Template file (empty for none)", null);
		if(template is not null)
		{
			if((rval = _LoadTemplate(template, out def)) != 0) { return rval; }
		}
		def ??= new();

		string? key = _Ask("Locale key", def.Key);
		if(string.IsNullOrWhiteSpace(key)) { return _Fail("No locale key specified.", _ExitCode.INVALID_ARGUMENTS); }
		def.Key = key;

		def.Name = (_Ask("Locale name", def.Name) ?? key);

		string? baseKey = _Ask(((def.BaseKey is null) ? "Base key (empty for none)" : "Base key (- to remove)"), def.BaseKey);
		def.BaseKey = ((string.IsNullOrWhiteSpace(baseKey) || (baseKey == "-")) ? null : baseKey);

		def.IsDefault = _AskYesNo("Default locale", def.IsDefault);
		def.RightToLeft = _AskYesNo("Right to left", def.RightToLeft);

		if((rval = _AskImage("Small image file", def.SmallIcon, out byte[]? small)) != 0) { return rval; }
		def.SmallIcon = small;
		if((rval = _AskImage("Large image file", def.LargeIcon, out byte[]? large)) != 0) { return rval; }
		def.LargeIcon = large;

		_Out.WriteLine();
		string? fileName = _Ask("Output file", template);
		if(string.IsNullOrWhiteSpace(fileName)) { return _Fail("No output file specified.", _ExitCode.INVALID_ARGUMENTS); }

		if(File.Exists(fileName))
		{
			if(!_AskYesNo($"The file \"{fileName}\" already exists. Overwrite", false))
			{
				return _Fail($"The file \"{fileName}\" already exists.", _ExitCode.OUTPUT_EXISTS);
			}
		}
		else if(!_AskYesNo("Write output file", true))
		{
			_Out.WriteLine("Cancelled.");
			return (int) _ExitCode.OK;
		}

		return _Save(def, fileName);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Runs the tool.</summary>
	/// <param name="args">Command line arguments.</param>
	/// <returns>Returns the exit code.</returns>
	public int Run(string[] args)
	{
		if(args.Length == 0)
		{
			_PrintUsage();
			return (int) _ExitCode.INVALID_ARGUMENTS;
		}

		_Arguments? a = _Arguments.Parse(args, out string? error);
		if(a is null) { return _Fail(error + " Use --help for the usage.", _ExitCode.INVALID_ARGUMENTS); }

		if(a.Help)
		{
			_PrintUsage();
			return (int) _ExitCode.OK;
		}

		if(a.Interactive)
		{
			if(a.HasBuildValues || (a.Info is not null)) { return _Fail("The interactive mode cannot be combined with other arguments.", _ExitCode.INVALID_ARGUMENTS); }
			return _RunInteractive();
		}

		if(a.Info is not null)
		{
			if(a.HasBuildValues) { return _Fail("--info cannot be combined with other arguments.", _ExitCode.INVALID_ARGUMENTS); }
			return _ShowInfo(a.Info);
		}

		return _Build(a);
	}
}
