namespace Robbiblubber.Localization.Controls;

using System.Runtime.Versioning;



/// <summary>This enumeration defines the styles of a <see cref="LocaleSelector"/>.</summary>
[SupportedOSPlatform("windows")]
public enum LocaleSelectorStyle: int
{
	/// <summary>A drop-down list shows the locales with their small icons and names.</summary>
	DROPDOWN = 0,
	/// <summary>A button shows the large icon of the selected locale and opens a menu with the locales.</summary>
	BUTTON = 1,
	/// <summary>A button shows the large icon of the selected locale next to a box with a drop-down arrow and opens a menu with
	///          the locales.</summary>
	DROPDOWN_BUTTON = 2,
	/// <summary>A button shows the large icon of the selected locale followed by a drop-down arrow without a box and opens a menu
	///          with the locales.</summary>
	ARROW_BUTTON = 3
}
