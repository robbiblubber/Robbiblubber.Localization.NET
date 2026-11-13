namespace Robbiblubber.Localization.UnitTests.Controls.Images;

using System.Runtime.Versioning;

using NUnit.Framework;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Localization.Definition;
using Robbiblubber.Localization.UnitTests.Library;



/// <summary>This class tests the <see cref="LocaleImageExtensions"/> class.</summary>
[TestFixture][NonParallelizable][SupportedOSPlatform("windows")]
public class LocaleImageExtensionsTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test directory and resets the localization services.</summary>
	[SetUp]
	public void SetUp()
	{
		Locale._Reset();
		_Files = new();
	}


	/// <summary>Deletes the test directory and resets the localization services.</summary>
	[TearDown]
	public void TearDown()
	{
		_Files.Dispose();
		Locale._Reset();
	}


	/// <summary>Images are created from valid icons and do not depend on a stream.</summary>
	[Test]
	public void CreateImages_ValidIcons()
	{
		new LocaleDefinition() { Key = "en", Name = "English", SmallIcon = _ControlTestData.CreatePng(16), LargeIcon = _ControlTestData.CreatePng(32) }
			.Save(_Files.File("en.locale"));
		Locale.Sources.Set(new DirectoryLocalizationSource(_Files.Path));
		Locale locale = Locale.Locales["en"]!;

		using Image? small = locale.CreateSmallImage();
		using Image? large = locale.CreateLargeImage();

		Assert.That(small, Is.Not.Null);
		Assert.That(small!.Size, Is.EqualTo(new Size(16, 16)));
		Assert.That(large!.Size, Is.EqualTo(new Size(32, 32)));
		Assert.That(((Bitmap) small).GetPixel(8, 8).ToArgb(), Is.EqualTo(Color.SteelBlue.ToArgb()));

		using Image? again = locale.CreateSmallImage();
		Assert.That(again, Is.Not.SameAs(small));
	}


	/// <summary>Missing and invalid icons create no image.</summary>
	[Test]
	public void CreateImages_MissingOrInvalidIcons()
	{
		new LocaleDefinition() { Key = "en", Name = "English" }.Save(_Files.File("en.locale"));
		_Files.WriteLocale("de", "Deutsch");
		Locale.Sources.Set(new DirectoryLocalizationSource(_Files.Path));

		Assert.That(Locale.Locales["en"]!.CreateSmallImage(), Is.Null);
		Assert.That(Locale.Locales["en"]!.CreateLargeImage(), Is.Null);
		Assert.That(Locale.Locales["de"]!.CreateSmallImage(), Is.Null);
		Assert.That(() => LocaleImageExtensions.CreateSmallImage(null!), Throws.ArgumentNullException);
	}


	/// <summary>The icon that fits the size best is chosen: the smallest one that is large enough, otherwise the largest one.</summary>
	[Test]
	public void SelectImage_BestFit()
	{
		using Bitmap small = new(16, 16);
		using Bitmap large = new(32, 32);

		Assert.That(LocaleImageExtensions._SelectImage(16, small, large), Is.SameAs(small));
		Assert.That(LocaleImageExtensions._SelectImage(12, large, small), Is.SameAs(small));
		Assert.That(LocaleImageExtensions._SelectImage(20, small, large), Is.SameAs(large));
		Assert.That(LocaleImageExtensions._SelectImage(48, small, large), Is.SameAs(large));
		Assert.That(LocaleImageExtensions._SelectImage(24, small, null), Is.SameAs(small));
		Assert.That(LocaleImageExtensions._SelectImage(24, null, null), Is.Null);
	}


	/// <summary>CreateImage returns an image of the requested size from the icon that fits best.</summary>
	[Test]
	public void CreateImage_Sizes()
	{
		new LocaleDefinition() { Key = "en", Name = "English", SmallIcon = _ControlTestData.CreatePng(16), LargeIcon = _ControlTestData.CreatePng(32) }
			.Save(_Files.File("en.locale"));
		new LocaleDefinition() { Key = "xx", Name = "None" }.Save(_Files.File("xx.locale"));
		Locale.Sources.Set(new DirectoryLocalizationSource(_Files.Path));

		foreach(int i in new[] { 16, 20, 24, 32, 40 })
		{
			using Image? image = Locale.Locales["en"]!.CreateImage(i);
			Assert.That(image!.Size, Is.EqualTo(new Size(i, i)), i.ToString());
			Assert.That(((Bitmap) image).GetPixel(i / 2, i / 2).ToArgb(), Is.EqualTo(Color.SteelBlue.ToArgb()), i.ToString());
		}

		Assert.That(Locale.Locales["xx"]!.CreateImage(16), Is.Null);
		Assert.That(() => LocaleImageExtensions.CreateImage(null!, 16), Throws.ArgumentNullException);
	}


	/// <summary>An image that already has the size is copied unchanged.</summary>
	[Test]
	public void CreateScaledImage_SameSize()
	{
		using Bitmap source = new(16, 16);
		source.SetPixel(0, 0, Color.Red);
		source.SetPixel(1, 0, Color.Blue);

		using Bitmap copy = (Bitmap) LocaleImageExtensions._CreateScaledImage(source, 16);

		Assert.That(copy, Is.Not.SameAs(source));
		Assert.That(copy.GetPixel(0, 0).ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
		Assert.That(copy.GetPixel(1, 0).ToArgb(), Is.EqualTo(Color.Blue.ToArgb()));
	}


	/// <summary>Scaled images are square, centered and keep the aspect ratio.</summary>
	[Test]
	public void CreateScaledImage()
	{
		using Bitmap source = new(40, 20);
		using(Graphics g = Graphics.FromImage(source)) { g.Clear(Color.Red); }

		using Bitmap scaled = (Bitmap) LocaleImageExtensions._CreateScaledImage(source, 20);

		Assert.That(scaled.Size, Is.EqualTo(new Size(20, 20)));
		Assert.That(scaled.GetPixel(10, 10).ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
		Assert.That(scaled.GetPixel(10, 1).A, Is.Zero);
	}
}
