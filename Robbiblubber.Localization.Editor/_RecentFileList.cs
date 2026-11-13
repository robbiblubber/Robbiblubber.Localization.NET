namespace Robbiblubber.Localization.Editor;

using System.ComponentModel;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;



/// <summary>This class implements the recent files list of the editor.</summary>
/// <remarks>Language tables, compressed language tables and locale definitions are shown with the images of the editor instead of
///          the shell icons; in dark themes, their dark variants are shown (see <see cref="_DarkImages"/>).</remarks>
[SupportedOSPlatform("windows")]
internal class _RecentFileList: RecentFileList
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Resource names of the images by file extension.</summary>
	private static readonly Dictionary<string, string> _ImageNames = new(StringComparer.OrdinalIgnoreCase)
	{
		[".ltab"] = "_FileLtab",
		[".cltab"] = "_FileCltab",
		[".locale"] = "_FileLocale"
	};



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	public _RecentFileList(): base()
	{}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="container">Container.</param>
	public _RecentFileList(IContainer container): base(container)
	{}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the image of a file type.</summary>
	/// <param name="path">File name.</param>
	/// <returns>Returns the image of the editor for language tables, compressed language tables and locale definitions, otherwise
	///          NULL.</returns>
	internal static Image? _GetFileImage(string path)
	{
		if(_ImageNames.TryGetValue(Path.GetExtension(path), out string? name)) { return _DarkImages._GetLight(name); }
		return null;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] RecentFileList                                                                                        //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <inheritdoc/>
	/// <remarks>Returns the image of the editor for its file types, otherwise the shell icon.</remarks>
	protected override Image? _GetImage(RecentItem item)
	{
		return (_GetFileImage(item.Key) ?? base._GetImage(item));
	}
}
