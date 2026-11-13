namespace Robbiblubber.Localization;

using System.Reflection;



/// <summary>This class provides locale definitions and language tables embedded as resources in an assembly.</summary>
/// <remarks>All manifest resources with a supported extension are used. Embed the files with
///          <c>&lt;EmbeddedResource Include="locale\*.ltab" /&gt;</c>.</remarks>
public sealed class EmbeddedLocalizationSource: ILocalizationSource
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="assembly">Assembly.</param>
	/// <exception cref="ArgumentNullException">Thrown when the assembly is NULL.</exception>
	public EmbeddedLocalizationSource(Assembly assembly)
	{
		ArgumentNullException.ThrowIfNull(assembly);

		Assembly = assembly;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the assembly.</summary>
	public Assembly Assembly
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] ILocalizationSource                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	/// <remarks>The resources are ordered by their manifest resource name. Resource names are prefixed with the assembly
	///          name.</remarks>
	public IEnumerable<LocalizationResource> GetResources()
	{
		string prefix = (Assembly.GetName().Name ?? string.Empty) + ":";

		return Assembly.GetManifestResourceNames()
					   .Where(LocalizationResource._IsSupported)
					   .Order(StringComparer.Ordinal)
					   .Select(m => new LocalizationResource(prefix + m, () => Assembly.GetManifestResourceStream(m)!))
					   .ToArray();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override string ToString()
	{
		return (Assembly.GetName().Name ?? string.Empty);
	}
}
