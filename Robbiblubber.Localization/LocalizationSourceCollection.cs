namespace Robbiblubber.Localization;

using System.Collections.ObjectModel;

using Robbiblubber.Spellbook;



/// <summary>This class provides the collection of localization sources.</summary>
/// <remarks>Changing the collection reloads locales and tables on the next access. Sources that appear later override
///          tables and locales of earlier sources only where symbols are equal; duplicate locales are taken from the first
///          source.</remarks>
public sealed class LocalizationSourceCollection: Collection<ILocalizationSource>
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	internal LocalizationSourceCollection()
	{
		_AddDefault();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Adds the default source.</summary>
	private void _AddDefault()
	{
		Items.Add(new DirectoryLocalizationSource(PathOp.ApplicationDirectory, true));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal methods                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Resets the collection to the default source without invalidation.</summary>
	internal void _Reset()
	{
		Items.Clear();
		_AddDefault();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Replaces all sources.</summary>
	/// <param name="sources">Sources.</param>
	/// <exception cref="ArgumentNullException">Thrown when a source is NULL.</exception>
	public void Set(params ILocalizationSource[] sources)
	{
		ArgumentNullException.ThrowIfNull(sources);
		if(sources.Any(m => m is null)) { throw new ArgumentNullException(nameof(sources)); }

		Items.Clear();
		foreach(ILocalizationSource i in sources) { Items.Add(i); }

		Locale._Invalidate();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Collection<ILocalizationSource>                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	protected override void InsertItem(int index, ILocalizationSource item)
	{
		ArgumentNullException.ThrowIfNull(item);

		base.InsertItem(index, item);
		Locale._Invalidate();
	}


	/// <inheritdoc/>
	protected override void SetItem(int index, ILocalizationSource item)
	{
		ArgumentNullException.ThrowIfNull(item);

		base.SetItem(index, item);
		Locale._Invalidate();
	}


	/// <inheritdoc/>
	protected override void RemoveItem(int index)
	{
		base.RemoveItem(index);
		Locale._Invalidate();
	}


	/// <inheritdoc/>
	protected override void ClearItems()
	{
		base.ClearItems();
		Locale._Invalidate();
	}
}
