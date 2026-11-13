namespace Robbiblubber.Localization.UnitTests.Library.Definitions;

using System.IO.Compression;

using NUnit.Framework;

using Robbiblubber.Localization.Definition;



/// <summary>This class tests locale definitions.</summary>
[TestFixture]
public class LocaleDefinitionTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test directory.</summary>
	[SetUp]
	public void SetUp()
	{
		_Files = new();
	}


	/// <summary>Deletes the test directory.</summary>
	[TearDown]
	public void TearDown()
	{
		_Files.Dispose();
	}


	/// <summary>Saving and loading keeps all values and both icons in their places.</summary>
	[Test]
	public void SaveAndLoad_RoundTrip()
	{
		string file = _Files.File("sub/de-at.locale");
		new LocaleDefinition() { Key = "de-AT", BaseKey = "de", Name = "Österreichisch", IsDefault = true, RightToLeft = true,
								 SmallIcon = _TestFiles.Icon(1), LargeIcon = _TestFiles.Icon(2) }.Save(file);

		LocaleDefinition d = LocaleDefinition.Load(file);

		Assert.That(d.Key, Is.EqualTo("de-AT"));
		Assert.That(d.BaseKey, Is.EqualTo("de"));
		Assert.That(d.Name, Is.EqualTo("Österreichisch"));
		Assert.That(d.IsDefault, Is.True);
		Assert.That(d.RightToLeft, Is.True);
		Assert.That(d.SmallIcon, Is.EqualTo(_TestFiles.Icon(1)));
		Assert.That(d.LargeIcon, Is.EqualTo(_TestFiles.Icon(2)));
	}


	/// <summary>A definition without icons and base key is written and read; the name defaults to the key.</summary>
	[Test]
	public void SaveAndLoad_Minimal()
	{
		using(MemoryStream s = new())
		{
			new LocaleDefinition() { Key = "xx" }.Save(s);
			s.Position = 0;

			LocaleDefinition d = LocaleDefinition.Load(s);

			Assert.That(d.Name, Is.EqualTo("xx"));
			Assert.That(d.BaseKey, Is.Null);
			Assert.That(d.SmallIcon, Is.Null);
			Assert.That(d.LargeIcon, Is.Null);
		}
	}


	/// <summary>The original locale files of the editor (format 3.0) can be read.</summary>
	[Test]
	public void Load_OriginalFormat()
	{
		using(Stream s = typeof(LocaleDefinitionTests).Assembly.GetManifestResourceStream("Robbiblubber.Localization.UnitTests.TestData.de.locale")!)
		{
			LocaleDefinition d = LocaleDefinition.Load(s);

			Assert.That(d.Key, Is.EqualTo("de"));
			Assert.That(d.Name, Is.EqualTo("Deutsch"));
			Assert.That(d.BaseKey, Is.Null);
			Assert.That(d.IsDefault, Is.False);
			Assert.That(d.RightToLeft, Is.False);
			Assert.That(d.SmallIcon!.Take(4), Is.EqualTo(new byte[] { 0x89, 0x50, 0x4E, 0x47 }));
			Assert.That(d.LargeIcon!.Length, Is.GreaterThan(d.SmallIcon!.Length));
		}
	}


	/// <summary>Saving without key throws.</summary>
	[Test]
	public void Save_WithoutKey_Throws()
	{
		Assert.That(() => new LocaleDefinition() { Name = "x" }.Save(_Files.File("x.locale")), Throws.InvalidOperationException);
		Assert.That(File.Exists(_Files.File("x.locale")), Is.False);
	}


	/// <summary>Saving replaces an existing file and leaves no temporary file.</summary>
	[Test]
	public void Save_ReplacesFile()
	{
		string file = _Files.File("a.locale");
		new LocaleDefinition() { Key = "a" }.Save(file);
		new LocaleDefinition() { Key = "b" }.Save(file);

		Assert.That(LocaleDefinition.Load(file).Key, Is.EqualTo("b"));
		Assert.That(Directory.GetFiles(_Files.Path), Is.EqualTo(new[] { file }));
	}


	/// <summary>Invalid data throws an InvalidDataException.</summary>
	[Test]
	public void Load_InvalidData_Throws()
	{
		using(MemoryStream s = new())
		{
			using(ZipArchive zip = new(s, ZipArchiveMode.Create, true)) { zip.CreateEntry("other.txt"); }
			s.Position = 0;

			Assert.That(() => LocaleDefinition.Load(s), Throws.TypeOf<InvalidDataException>());
		}

		using(MemoryStream s = new())
		{
			new LocaleDefinition() { Key = "x" }.Save(s);
			s.Position = 0;
			using(ZipArchive zip = new(s, ZipArchiveMode.Update, true))
			{
				zip.GetEntry("locale.def")!.Delete();
				using(StreamWriter w = new(zip.CreateEntry("locale.def").Open())) { w.Write("name = x;"); }
			}
			s.Position = 0;

			Assert.That(() => LocaleDefinition.Load(s), Throws.TypeOf<InvalidDataException>());
		}

		using(MemoryStream s = new([1, 2, 3]))
		{
			Assert.That(() => LocaleDefinition.Load(s), Throws.TypeOf<InvalidDataException>());
		}
	}


	/// <summary>A locale created from a definition copies the values; the definition can be recreated.</summary>
	[Test]
	public void ToLocale_AndBack()
	{
		LocaleDefinition d = new() { Key = " fr ", Name = "Français", SmallIcon = _TestFiles.Icon(7) };
		Locale l = d.ToLocale();
		d.SmallIcon![4] = 9;

		Assert.That(l.Key, Is.EqualTo("fr"));
		Assert.That(l.HasSmallIcon, Is.True);
		Assert.That(l.HasLargeIcon, Is.False);
		Assert.That(l.OpenLargeIcon(), Is.Null);

		using(MemoryStream s = new())
		{
			l.OpenSmallIcon()!.CopyTo(s);
			Assert.That(s.ToArray(), Is.EqualTo(_TestFiles.Icon(7)));
		}

		Assert.That(LocaleDefinition.FromLocale(l).Name, Is.EqualTo("Français"));
		Assert.That(() => new LocaleDefinition().ToLocale(), Throws.InvalidOperationException);
	}
}
