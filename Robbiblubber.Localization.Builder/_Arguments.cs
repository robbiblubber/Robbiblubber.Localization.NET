namespace Robbiblubber.Localization.Builder;



/// <summary>This class contains the parsed command line arguments.</summary>
/// <remarks><para>Every switch has a short form (<c>-k</c>) and a long form (<c>--key</c>). Values follow the switch as next
///          argument; long forms also accept <c>--key=value</c>. Arguments without a switch are assigned in the order output
///          file, key, name, small image, large image to the values that have not been set.</para>
///          <para>A value that is not specified is NULL; an empty value (<c>""</c>) removes the value of a template.</para></remarks>
internal sealed class _Arguments
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Switches with a value by short and long form.</summary>
	private static readonly Dictionary<string, Action<_Arguments, string>> _ValueSwitches = new(StringComparer.Ordinal)
	{
		["-f"] = (m, n) => m.FileName = n, ["--file"] = (m, n) => m.FileName = n,
		["-t"] = (m, n) => m.Template = n, ["--template"] = (m, n) => m.Template = n,
		["-k"] = (m, n) => m.Key = n, ["--key"] = (m, n) => m.Key = n,
		["-n"] = (m, n) => m.Name = n, ["--name"] = (m, n) => m.Name = n,
		["-b"] = (m, n) => m.BaseKey = n, ["--base"] = (m, n) => m.BaseKey = n,
		["-s"] = (m, n) => m.SmallImage = n, ["--small"] = (m, n) => m.SmallImage = n,
		["-l"] = (m, n) => m.LargeImage = n, ["--large"] = (m, n) => m.LargeImage = n,
		["-p"] = (m, n) => m.Info = n, ["--info"] = (m, n) => m.Info = n
	};

	/// <summary>Switches without a value by short and long form.</summary>
	private static readonly Dictionary<string, Action<_Arguments>> _FlagSwitches = new(StringComparer.Ordinal)
	{
		["-d"] = m => m.IsDefault = true, ["--default"] = m => m.IsDefault = true,
		["-r"] = m => m.RightToLeft = true, ["--rtl"] = m => m.RightToLeft = true,
		["-o"] = m => m.Overwrite = true, ["--overwrite"] = m => m.Overwrite = true,
		["-i"] = m => m.Interactive = true, ["--interactive"] = m => m.Interactive = true,
		["-?"] = m => m.Help = true, ["-h"] = m => m.Help = true, ["--help"] = m => m.Help = true
	};



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Parses command line arguments.</summary>
	/// <param name="args">Arguments.</param>
	/// <param name="error">Returns the error message, NULL if the arguments could be parsed.</param>
	/// <returns>Returns the parsed arguments, NULL if the arguments are invalid.</returns>
	public static _Arguments? Parse(string[] args, out string? error)
	{
		_Arguments rval = new();
		error = null;

		for(int i = 0; i < args.Length; i++)
		{
			string arg = args[i];

			if((arg.Length > 1) && arg.StartsWith('-'))
			{
				string name = arg;
				string? value = null;

				if(arg.StartsWith("--") && arg.Contains('='))
				{
					name = arg[..arg.IndexOf('=')];
					value = arg[(arg.IndexOf('=') + 1)..];
				}

				if(_FlagSwitches.TryGetValue(name, out Action<_Arguments>? flag) && (value is null))
				{
					flag(rval);
				}
				else if(_ValueSwitches.TryGetValue(name, out Action<_Arguments, string>? set))
				{
					if(value is null)
					{
						if(i + 1 >= args.Length)
						{
							error = $"The switch {name} requires a value.";
							return null;
						}
						value = args[++i];
					}
					set(rval, value);
				}
				else
				{
					error = $"Unknown switch {arg}.";
					return null;
				}
			}
			else if(rval.FileName is null) { rval.FileName = arg; }
			else if(rval.Key is null) { rval.Key = arg; }
			else if(rval.Name is null) { rval.Name = arg; }
			else if(rval.SmallImage is null) { rval.SmallImage = arg; }
			else if(rval.LargeImage is null) { rval.LargeImage = arg; }
			else
			{
				error = $"Unexpected argument \"{arg}\".";
				return null;
			}
		}

		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the output file name.</summary>
	public string? FileName
	{
		get; private set;
	}


	/// <summary>Gets the template file name.</summary>
	public string? Template
	{
		get; private set;
	}


	/// <summary>Gets the locale key.</summary>
	public string? Key
	{
		get; private set;
	}


	/// <summary>Gets the locale name.</summary>
	public string? Name
	{
		get; private set;
	}


	/// <summary>Gets the base key; an empty string removes the base key of a template.</summary>
	public string? BaseKey
	{
		get; private set;
	}


	/// <summary>Gets the small image file name; an empty string removes the small icon of a template.</summary>
	public string? SmallImage
	{
		get; private set;
	}


	/// <summary>Gets the large image file name; an empty string removes the large icon of a template.</summary>
	public string? LargeImage
	{
		get; private set;
	}


	/// <summary>Gets the name of the locale file to show.</summary>
	public string? Info
	{
		get; private set;
	}


	/// <summary>Gets if the locale is marked as default locale.</summary>
	public bool IsDefault
	{
		get; private set;
	}


	/// <summary>Gets if the locale is marked as right to left.</summary>
	public bool RightToLeft
	{
		get; private set;
	}


	/// <summary>Gets if an existing output file may be overwritten.</summary>
	public bool Overwrite
	{
		get; private set;
	}


	/// <summary>Gets if the interactive mode is requested.</summary>
	public bool Interactive
	{
		get; private set;
	}


	/// <summary>Gets if the usage is requested.</summary>
	public bool Help
	{
		get; private set;
	}


	/// <summary>Gets if any value for building a locale file has been specified.</summary>
	public bool HasBuildValues
	{
		get
		{
			return ((FileName is not null) || (Template is not null) || (Key is not null) || (Name is not null) || (BaseKey is not null) ||
					(SmallImage is not null) || (LargeImage is not null) || IsDefault || RightToLeft || Overwrite);
		}
	}
}
