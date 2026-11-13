namespace Robbiblubber.Localization;



/// <summary>Sources of locale definitions and language tables implement this interface.</summary>
/// <remarks>A source lists resources; the resource type is determined by the extension of the resource name: <c>.locale</c>
///          for locale definitions, <c>.ltab</c> for language tables and <c>.cltab</c> for compressed language tables.
///          Resources with other extensions are ignored.</remarks>
public interface ILocalizationSource
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the resources of this source.</summary>
	/// <returns>Returns the resources.</returns>
	public IEnumerable<LocalizationResource> GetResources();
}
