namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class supplies the dark variants of the menu images.</summary>
/// <remarks><para>In dark themes, a menu item named for example <c>_MenuEditCopy</c> shows the image resource
///          <c>_MenuEditCopy_dark</c> from <c>Properties\Resources.resx</c> if there is one.</para>
///          <para>Images loaded with <see cref="_GetLight"/> (e.g. the file type images of the recent files) are replaced by their
///          <c>_dark</c> resource wherever they are shown.</para>
///          <para>Without a dark variant, the theme manager lightens the dark outlines of the image.</para></remarks>
[SupportedOSPlatform("windows")]
internal static class _DarkImages
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Suffix of the resource names of the dark variants.</summary>
	private const string _SUFFIX = "_dark";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Dark variants by item name, NULL if an item has none.</summary>
	private static readonly Dictionary<string, Image?> _Images = new();

	/// <summary>Light images loaded from the resources, by name.</summary>
	private static readonly Dictionary<string, Image?> _LightImages = new();

	/// <summary>Resource names by light image.</summary>
	private static readonly Dictionary<Image, string> _Names = new(ReferenceEqualityComparer.Instance);



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Lets the menus show the dark variants of their images in dark themes.</summary>
	internal static void _Enable()
	{
		ThemeManager.ResolveImage -= _ThemeManager_ResolveImage;
		ThemeManager.ResolveImage += _ThemeManager_ResolveImage;
	}


	/// <summary>Gets the dark variant of the image of an item.</summary>
	/// <param name="name">Name of the item.</param>
	/// <returns>Returns the image, NULL if there is no dark variant. The image is cached and owned by this class.</returns>
	internal static Image? _Get(string name)
	{
		if(_Images.TryGetValue(name, out Image? rval)) { return rval; }

		try
		{
			rval = Properties.Resources.ResourceManager.GetObject(name + _SUFFIX) as Image;
		}
		catch(Exception) { rval = null; }

		_Images.Add(name, rval);
		return rval;
	}


	/// <summary>Gets an image from the resources that is replaced by its dark variant in dark themes.</summary>
	/// <param name="name">Resource name.</param>
	/// <returns>Returns the image, NULL if there is no such resource. The image is cached and owned by this class.</returns>
	internal static Image? _GetLight(string name)
	{
		if(_LightImages.TryGetValue(name, out Image? rval)) { return rval; }

		try
		{
			rval = Properties.Resources.ResourceManager.GetObject(name) as Image;
		}
		catch(Exception) { rval = null; }

		_LightImages.Add(name, rval);
		if(rval is not null) { _Names[rval] = name; }
		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>An image of a menu is drawn: in dark themes, the dark variant of the item or of the image is shown if there is
	///          one.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private static void _ThemeManager_ResolveImage(object? sender, ThemeImageEventArgs e)
	{
		if(!e.Theme.IsDark) return;

		if((e.Source is ToolStripItem item) && (!string.IsNullOrEmpty(item.Name)) && (_Get(item.Name) is Image dark))
		{
			e.Image = dark;
		}
		else if(_Names.TryGetValue(e.Original, out string? name) && (_Get(name) is Image variant))
		{
			e.Image = variant;
		}
	}
}
