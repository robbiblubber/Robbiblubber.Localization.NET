namespace Robbiblubber.Localization.Controls;

using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;



/// <summary>This class provides extension methods that create images from the icons of a locale.</summary>
/// <remarks>Each call creates a new image that does not depend on the locale or a stream. The caller owns the image and disposes
///          it when it is no longer needed.</remarks>
[SupportedOSPlatform("windows")]
public static class LocaleImageExtensions
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates an image from image data.</summary>
	/// <param name="stream">Stream with image data, may be NULL.</param>
	/// <returns>Returns the image, NULL if there is no data or the data is not a valid image.</returns>
	private static Image? _CreateImage(Stream? stream)
	{
		if(stream is null) { return null; }

		try
		{
			using(stream)
			using(Image image = Image.FromStream(stream))
			{
				return new Bitmap(image);
			}
		}
		catch(ArgumentException) { return null; }
		catch(ExternalException) { return null; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a scaled copy of an image.</summary>
	/// <param name="image">Image.</param>
	/// <param name="size">Width and height of the new image in pixels.</param>
	/// <returns>Returns a new square image; the image is centered and keeps its aspect ratio. An image that already has the size is
	///          copied unchanged.</returns>
	internal static Image _CreateScaledImage(Image image, int size)
	{
		size = Math.Max(1, size);
		if((image.Width == size) && (image.Height == size)) { return new Bitmap(image); }

		Bitmap rval = new(size, size);
		float scale = Math.Min((float) size / image.Width, (float) size / image.Height);
		float width = (image.Width * scale), height = (image.Height * scale);

		using(Graphics g = Graphics.FromImage(rval))
		{
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.DrawImage(image, (size - width) / 2f, (size - height) / 2f, width, height);
		}

		return rval;
	}


	/// <summary>Selects the image that fits a size best.</summary>
	/// <param name="size">Width and height in pixels.</param>
	/// <param name="images">Images, may contain NULL.</param>
	/// <returns>Returns the smallest image whose larger side is at least the size, otherwise the largest image; NULL if there is no
	///          image.</returns>
	internal static Image? _SelectImage(int size, params Image?[] images)
	{
		Image[] valid = images.OfType<Image>().OrderBy(m => Math.Max(m.Width, m.Height)).ToArray();
		return (valid.FirstOrDefault(m => (Math.Max(m.Width, m.Height) >= size)) ?? valid.LastOrDefault());
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates an image of a given size from the icon of a locale that fits the size best.</summary>
	/// <param name="locale">Locale.</param>
	/// <param name="size">Width and height of the image in pixels (device pixels, e.g. from
	///                    <see cref="Control.LogicalToDeviceUnits(int)"/>).</param>
	/// <returns>Returns a new square image, NULL if the locale has no valid icon.</returns>
	/// <remarks>Of the valid icons, the smallest one that is at least as large as the size is used, otherwise the largest one. It is
	///          scaled to the size, centered and with its aspect ratio kept; an icon that already has the size is used unchanged. With
	///          icons of 16 × 16 and 32 × 32 pixels, a 16 pixel image is sharp at 100 % and is scaled down from the large icon at
	///          higher scaling.</remarks>
	/// <exception cref="ArgumentNullException">Thrown when the locale is NULL.</exception>
	public static Image? CreateImage(this Locale locale, int size)
	{
		ArgumentNullException.ThrowIfNull(locale);

		using Image? small = locale.CreateSmallImage();
		using Image? large = locale.CreateLargeImage();

		Image? source = _SelectImage(size, small, large);
		return ((source is null) ? null : _CreateScaledImage(source, size));
	}


	/// <summary>Creates an image from the small icon of a locale.</summary>
	/// <param name="locale">Locale.</param>
	/// <returns>Returns a new image, NULL if the locale has no small icon or the icon is not a valid image.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the locale is NULL.</exception>
	public static Image? CreateSmallImage(this Locale locale)
	{
		ArgumentNullException.ThrowIfNull(locale);
		return _CreateImage(locale.OpenSmallIcon());
	}


	/// <summary>Creates an image from the large icon of a locale.</summary>
	/// <param name="locale">Locale.</param>
	/// <returns>Returns a new image, NULL if the locale has no large icon or the icon is not a valid image.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the locale is NULL.</exception>
	public static Image? CreateLargeImage(this Locale locale)
	{
		ArgumentNullException.ThrowIfNull(locale);
		return _CreateImage(locale.OpenLargeIcon());
	}
}
