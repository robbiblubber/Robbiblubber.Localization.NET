namespace Robbiblubber.Localization;



/// <summary>This class provides locale definitions and language tables from a directory.</summary>
public sealed class DirectoryLocalizationSource: ILocalizationSource
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="path">Directory path.</param>
	/// <param name="recursive">Determines if subdirectories are searched.</param>
	/// <exception cref="ArgumentException">Thrown when the path is NULL or empty.</exception>
	public DirectoryLocalizationSource(string path, bool recursive = false)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);

		Path = System.IO.Path.GetFullPath(path);
		Recursive = recursive;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the directory path.</summary>
	public string Path
	{
		get;
	}


	/// <summary>Gets if subdirectories are searched.</summary>
	public bool Recursive
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] ILocalizationSource                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	/// <remarks>A directory that does not exist has no resources. Inaccessible subdirectories are skipped. The resources are
	///          ordered by their path.</remarks>
	public IEnumerable<LocalizationResource> GetResources()
	{
		if(!Directory.Exists(Path)) { return []; }

		EnumerationOptions opts = new() { RecurseSubdirectories = Recursive, IgnoreInaccessible = true };

		return Directory.EnumerateFiles(Path, "*", opts)
						.Where(LocalizationResource._IsSupported)
						.Order(StringComparer.Ordinal)
						.Select(m => new LocalizationResource(m, () => File.OpenRead(m)))
						.ToArray();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override string ToString()
	{
		return Path;
	}
}
