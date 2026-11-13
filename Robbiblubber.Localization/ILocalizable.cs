namespace Robbiblubber.Localization;



/// <summary>Localizable objects implement this interface.</summary>
public interface ILocalizable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the object with the texts of the current locale.</summary>
	public void Localize();
}
