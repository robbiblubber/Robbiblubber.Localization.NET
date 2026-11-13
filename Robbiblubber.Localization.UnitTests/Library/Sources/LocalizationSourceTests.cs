namespace Robbiblubber.Localization.UnitTests.Library.Sources;

using NUnit.Framework;



/// <summary>This class tests the localization sources.</summary>
[TestFixture]
public class LocalizationSourceTests
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


	/// <summary>A directory source lists supported files only, ordered by path; subdirectories only when recursive.</summary>
	[Test]
	public void Directory_ListsSupportedFiles()
	{
		_Files.WriteLocale("de", "Deutsch");
		_Files.WriteTable("b.ltab", "de", null);
		_Files.WriteTable("a.CLTAB", "de", null);
		File.WriteAllText(_Files.File("readme.txt"), "x");
		Directory.CreateDirectory(_Files.File("sub"));
		_Files.WriteTable("sub/c.ltab", "de", null);

		string[] flat = new DirectoryLocalizationSource(_Files.Path).GetResources().Select(m => Path.GetFileName(m.Name)).ToArray();
		string[] deep = new DirectoryLocalizationSource(_Files.Path, true).GetResources().Select(m => Path.GetFileName(m.Name)).ToArray();

		Assert.That(flat, Is.EqualTo(new[] { "a.CLTAB", "b.ltab", "de.locale" }));
		Assert.That(deep, Is.EqualTo(new[] { "a.CLTAB", "b.ltab", "de.locale", "c.ltab" }));
	}


	/// <summary>A missing directory has no resources.</summary>
	[Test]
	public void Directory_Missing_IsEmpty()
	{
		Assert.That(new DirectoryLocalizationSource(_Files.File("missing")).GetResources(), Is.Empty);
		Assert.That(() => new DirectoryLocalizationSource(" "), Throws.ArgumentException);
	}


	/// <summary>Resources of a directory source can be opened.</summary>
	[Test]
	public void Directory_Open()
	{
		_Files.WriteTable("a.ltab", "de", null, ("x", "y"));

		using(Stream s = new DirectoryLocalizationSource(_Files.Path).GetResources().Single().Open())
		{
			Assert.That(s.ReadByte(), Is.GreaterThan(-1));
		}
	}


	/// <summary>An embedded source lists the supported manifest resources of an assembly.</summary>
	[Test]
	public void Embedded_ListsResources()
	{
		string[] names = new EmbeddedLocalizationSource(typeof(LocalizationSourceTests).Assembly).GetResources()
																								 .Select(m => m.Name)
																								 .ToArray();

		Assert.That(names, Is.EqualTo(new[] { "Robbiblubber.Localization.UnitTests:Robbiblubber.Localization.UnitTests.TestData.de.locale",
											  "Robbiblubber.Localization.UnitTests:Robbiblubber.Localization.UnitTests.TestData.en.locale",
											  "Robbiblubber.Localization.UnitTests:Robbiblubber.Localization.UnitTests.TestData.led-fr.ltab" }));
	}
}
