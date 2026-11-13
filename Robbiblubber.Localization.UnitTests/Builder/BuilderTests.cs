namespace Robbiblubber.Localization.UnitTests.Builder;

using NUnit.Framework;

using Robbiblubber.Localization.Builder;
using Robbiblubber.Localization.Definition;
using Robbiblubber.Localization.UnitTests.Library;



/// <summary>This class tests the locale definition build tool.</summary>
[TestFixture][NonParallelizable]
public class BuilderTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;

	/// <summary>Output of the last run.</summary>
	private StringWriter _Out = null!;

	/// <summary>Error output of the last run.</summary>
	private StringWriter _Error = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the header of a PNG image.</summary>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	/// <returns>Returns the data.</returns>
	private static byte[] _Png(int width, int height)
	{
		byte[] rval = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0, 0, 0, 13, 0x49, 0x48, 0x44, 0x52, 0, 0, 0, 0, 0, 0, 0, 0, 8, 6, 0, 0, 0];
		rval[16] = (byte) (width >> 24); rval[17] = (byte) (width >> 16); rval[18] = (byte) (width >> 8); rval[19] = (byte) width;
		rval[20] = (byte) (height >> 24); rval[21] = (byte) (height >> 16); rval[22] = (byte) (height >> 8); rval[23] = (byte) height;

		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Runs the tool.</summary>
	/// <param name="args">Arguments.</param>
	/// <returns>Returns the exit code.</returns>
	private int _Run(params string[] args)
	{
		return _RunWithInput(string.Empty, args);
	}


	/// <summary>Runs the tool with console input.</summary>
	/// <param name="input">Input lines.</param>
	/// <param name="args">Arguments.</param>
	/// <returns>Returns the exit code.</returns>
	private int _RunWithInput(string input, params string[] args)
	{
		_Out = new();
		_Error = new();

		return new _Builder(new StringReader(input), _Out, _Error).Run(args);
	}


	/// <summary>Gets the full path of a test file.</summary>
	/// <param name="name">File name.</param>
	/// <returns>Returns the path.</returns>
	private string _F(string name)
	{
		return _Files.File(name);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test directory with two images and a text file.</summary>
	[SetUp]
	public void SetUp()
	{
		_Files = new();
		File.WriteAllBytes(_F("small.png"), _Png(16, 16));
		File.WriteAllBytes(_F("large.png"), _Png(24, 24));
		File.WriteAllText(_F("text.txt"), "no image");
	}


	/// <summary>Deletes the test directory.</summary>
	[TearDown]
	public void TearDown()
	{
		_Out?.Dispose();
		_Error?.Dispose();
		_Files.Dispose();
	}


	/// <summary>Without arguments, the usage is shown with exit code 1.</summary>
	[Test]
	public void NoArguments_ShowsUsage()
	{
		Assert.That(_Run(), Is.EqualTo(1));
		Assert.That(_Out.ToString(), Does.Contain("Usage:"));
	}


	/// <summary>All help switches show the usage with exit code 0.</summary>
	[TestCase("-?")][TestCase("-h")][TestCase("--help")]
	public void Help_ShowsUsage(string arg)
	{
		Assert.That(_Run(arg), Is.EqualTo(0));
		Assert.That(_Out.ToString(), Does.Contain("--overwrite"));
	}


	/// <summary>Positional arguments create a locale file.</summary>
	[Test]
	public void Build_Positional()
	{
		Assert.That(_Run(_F("de.locale"), "de", "Deutsch", _F("small.png"), _F("large.png")), Is.EqualTo(0), _Error.ToString());

		LocaleDefinition d = LocaleDefinition.Load(_F("de.locale"));
		Assert.That(d.Key, Is.EqualTo("de"));
		Assert.That(d.Name, Is.EqualTo("Deutsch"));
		Assert.That(d.BaseKey, Is.Null);
		Assert.That(d.IsDefault, Is.False);
		Assert.That(d.RightToLeft, Is.False);
		Assert.That(d.SmallIcon, Is.EqualTo(_Png(16, 16)));
		Assert.That(d.LargeIcon, Is.EqualTo(_Png(24, 24)));
	}


	/// <summary>Short switches, long switches and long switches with a value after an equals sign give the same result.</summary>
	[Test]
	public void Build_ShortAndLongSwitches()
	{
		Assert.That(_Run("-f", _F("a.locale"), "-k", "de-AT", "-n", "Österreich", "-b", "de", "-s", _F("small.png"), "-l", _F("large.png"), "-d", "-r"),
					Is.EqualTo(0), _Error.ToString());
		Assert.That(_Run("--file", _F("b.locale"), "--key", "de-AT", "--name", "Österreich", "--base", "de", "--small", _F("small.png"),
						 "--large", _F("large.png"), "--default", "--rtl"), Is.EqualTo(0), _Error.ToString());
		Assert.That(_Run("--file=" + _F("c.locale"), "--key=de-AT", "--name=Österreich", "--base=de", "--small=" + _F("small.png"),
						 "--large=" + _F("large.png"), "--default", "--rtl"), Is.EqualTo(0), _Error.ToString());

		foreach(string i in new[] { "a.locale", "b.locale", "c.locale" })
		{
			LocaleDefinition d = LocaleDefinition.Load(_F(i));
			Assert.That(d.Key, Is.EqualTo("de-AT"), i);
			Assert.That(d.Name, Is.EqualTo("Österreich"), i);
			Assert.That(d.BaseKey, Is.EqualTo("de"), i);
			Assert.That(d.IsDefault, Is.True, i);
			Assert.That(d.RightToLeft, Is.True, i);
			Assert.That(d.SmallIcon, Is.Not.Null, i);
			Assert.That(d.LargeIcon, Is.Not.Null, i);
		}
	}


	/// <summary>Arguments after a switch are still parsed, and positional arguments fill the values that are not set.</summary>
	[Test]
	public void Build_SwitchBeforePositional()
	{
		Assert.That(_Run("-k", "fr", _F("fr.locale"), "Français"), Is.EqualTo(0), _Error.ToString());

		LocaleDefinition d = LocaleDefinition.Load(_F("fr.locale"));
		Assert.That(d.Key, Is.EqualTo("fr"));
		Assert.That(d.Name, Is.EqualTo("Français"));
	}


	/// <summary>Icons are optional.</summary>
	[Test]
	public void Build_WithoutIcons()
	{
		Assert.That(_Run(_F("en.locale"), "en", "English"), Is.EqualTo(0), _Error.ToString());

		LocaleDefinition d = LocaleDefinition.Load(_F("en.locale"));
		Assert.That(d.SmallIcon, Is.Null);
		Assert.That(d.LargeIcon, Is.Null);
	}


	/// <summary>Invalid or incomplete arguments return exit code 1 and do not create a file.</summary>
	[TestCase("-k", "de", "-n", "Deutsch")]
	[TestCase("out.locale", "-n", "Deutsch")]
	[TestCase("out.locale", "de")]
	[TestCase("out.locale", "de", "Deutsch", "-x")]
	[TestCase("out.locale", "de", "Deutsch", "--key")]
	[TestCase("out.locale", "de", "Deutsch", "s.png", "l.png", "extra")]
	[TestCase("out.locale", "-k", " ", "-n", "Deutsch")]
	[TestCase("-i", "out.locale")]
	[TestCase("-p", "x.locale", "-k", "de")]
	[TestCase("--rtl=yes", "out.locale", "de", "Deutsch")]
	public void Build_InvalidArguments(params string[] args)
	{
		string[] a = args.Select(m => (m.EndsWith(".locale") ? _F(m) : m)).ToArray();

		Assert.That(_Run(a), Is.EqualTo(1));
		Assert.That(_Error.ToString(), Does.StartWith("Error:"));
		Assert.That(File.Exists(_F("out.locale")), Is.False);
	}


	/// <summary>A missing or invalid image returns exit code 2.</summary>
	[Test]
	public void Build_InvalidImage()
	{
		Assert.That(_Run(_F("out.locale"), "de", "Deutsch", _F("missing.png")), Is.EqualTo(2));
		Assert.That(_Run(_F("out.locale"), "de", "Deutsch", _F("small.png"), _F("text.txt")), Is.EqualTo(2));
		Assert.That(_Error.ToString(), Does.Contain("not a supported image"));
		Assert.That(File.Exists(_F("out.locale")), Is.False);
	}


	/// <summary>An existing output file is only replaced with --overwrite.</summary>
	[Test]
	public void Build_Overwrite()
	{
		Assert.That(_Run(_F("de.locale"), "de", "Deutsch"), Is.EqualTo(0));

		Assert.That(_Run(_F("de.locale"), "de", "German"), Is.EqualTo(4));
		Assert.That(LocaleDefinition.Load(_F("de.locale")).Name, Is.EqualTo("Deutsch"));

		Assert.That(_Run(_F("de.locale"), "de", "German", "-o"), Is.EqualTo(0));
		Assert.That(LocaleDefinition.Load(_F("de.locale")).Name, Is.EqualTo("German"));

		Assert.That(_Run(_F("de.locale"), "de", "Deutsch", "--overwrite"), Is.EqualTo(0));
		Assert.That(LocaleDefinition.Load(_F("de.locale")).Name, Is.EqualTo("Deutsch"));
	}


	/// <summary>An output file that cannot be written returns exit code 3.</summary>
	[Test]
	public void Build_WriteError()
	{
		Directory.CreateDirectory(_F("folder.locale"));

		Assert.That(_Run(_F("folder.locale"), "de", "Deutsch", "-o"), Is.EqualTo(3));
		Assert.That(_Error.ToString(), Does.Contain("could not be written"));
	}


	/// <summary>A template provides all values that are not specified; empty values remove the base key and icons.</summary>
	[Test]
	public void Build_Template()
	{
		Assert.That(_Run(_F("t.locale"), "de-AT", "Österreich", _F("small.png"), _F("large.png"), "-b", "de", "-d"), Is.EqualTo(0));

		Assert.That(_Run(_F("copy.locale"), "-t", _F("t.locale")), Is.EqualTo(0), _Error.ToString());
		LocaleDefinition d = LocaleDefinition.Load(_F("copy.locale"));
		Assert.That(d.Key, Is.EqualTo("de-AT"));
		Assert.That(d.Name, Is.EqualTo("Österreich"));
		Assert.That(d.BaseKey, Is.EqualTo("de"));
		Assert.That(d.IsDefault, Is.True);
		Assert.That(d.SmallIcon, Is.Not.Null);

		Assert.That(_Run(_F("t.locale"), "--template", _F("t.locale"), "--rtl", "--base=", "--large", "", "--name", "Austria", "-o"),
					Is.EqualTo(0), _Error.ToString());
		d = LocaleDefinition.Load(_F("t.locale"));
		Assert.That(d.Key, Is.EqualTo("de-AT"));
		Assert.That(d.Name, Is.EqualTo("Austria"));
		Assert.That(d.BaseKey, Is.Null);
		Assert.That(d.IsDefault, Is.True);
		Assert.That(d.RightToLeft, Is.True);
		Assert.That(d.SmallIcon, Is.Not.Null);
		Assert.That(d.LargeIcon, Is.Null);
	}


	/// <summary>A missing or invalid template returns exit code 2.</summary>
	[Test]
	public void Build_InvalidTemplate()
	{
		Assert.That(_Run(_F("out.locale"), "-t", _F("missing.locale")), Is.EqualTo(2));
		Assert.That(_Run(_F("out.locale"), "-t", _F("text.txt")), Is.EqualTo(2));
		Assert.That(File.Exists(_F("out.locale")), Is.False);
	}


	/// <summary>The info switch shows the content of a locale file.</summary>
	[Test]
	public void Info_ShowsContent()
	{
		Assert.That(_Run(_F("ar.locale"), "ar", "العربية", _F("small.png"), "-r"), Is.EqualTo(0));

		Assert.That(_Run("--info", _F("ar.locale")), Is.EqualTo(0), _Error.ToString());
		string text = _Out.ToString();
		Assert.That(text, Does.Contain("Key:           ar"));
		Assert.That(text, Does.Contain("Name:          العربية"));
		Assert.That(text, Does.Contain("Base:          (none)"));
		Assert.That(text, Does.Contain("Right to left: yes"));
		Assert.That(text, Does.Contain("Small icon:    PNG, 16 x 16, 29 bytes"));
		Assert.That(text, Does.Contain("Large icon:    (none)"));

		Assert.That(_Run("-p", _F("ar.locale")), Is.EqualTo(0));
	}


	/// <summary>The info switch returns exit code 2 for a missing or invalid file.</summary>
	[Test]
	public void Info_InvalidFile()
	{
		Assert.That(_Run("-p", _F("missing.locale")), Is.EqualTo(2));
		Assert.That(_Run("-p", _F("text.txt")), Is.EqualTo(2));
	}


	/// <summary>The interactive mode asks for all values and creates the file.</summary>
	[Test]
	public void Interactive_CreatesFile()
	{
		string input = string.Join(Environment.NewLine, "", "fr", "Français", "", "y", "", _F("small.png"), "", _F("fr.locale"), "") + Environment.NewLine;

		Assert.That(_RunWithInput(input, "-i"), Is.EqualTo(0), _Error.ToString());

		LocaleDefinition d = LocaleDefinition.Load(_F("fr.locale"));
		Assert.That(d.Key, Is.EqualTo("fr"));
		Assert.That(d.Name, Is.EqualTo("Français"));
		Assert.That(d.IsDefault, Is.True);
		Assert.That(d.RightToLeft, Is.False);
		Assert.That(d.SmallIcon, Is.Not.Null);
		Assert.That(d.LargeIcon, Is.Null);
	}


	/// <summary>The interactive mode edits a template; empty answers keep the values, and declining to overwrite returns exit
	///          code 4.</summary>
	[Test]
	public void Interactive_Template()
	{
		Assert.That(_Run(_F("de.locale"), "de", "Deutsch", _F("small.png"), _F("large.png"), "-b", "en"), Is.EqualTo(0));

		string input = string.Join(Environment.NewLine, _F("de.locale"), "", "German", "-", "", "y", "", "-", "", "y") + Environment.NewLine;
		Assert.That(_RunWithInput(input, "--interactive"), Is.EqualTo(0), _Error.ToString());

		LocaleDefinition d = LocaleDefinition.Load(_F("de.locale"));
		Assert.That(d.Key, Is.EqualTo("de"));
		Assert.That(d.Name, Is.EqualTo("German"));
		Assert.That(d.BaseKey, Is.Null);
		Assert.That(d.RightToLeft, Is.True);
		Assert.That(d.SmallIcon, Is.Not.Null);
		Assert.That(d.LargeIcon, Is.Null);

		input = string.Join(Environment.NewLine, _F("de.locale"), "", "Deutsch", "", "", "", "", "", "", "n") + Environment.NewLine;
		Assert.That(_RunWithInput(input, "-i"), Is.EqualTo(4));
		Assert.That(LocaleDefinition.Load(_F("de.locale")).Name, Is.EqualTo("German"));
	}
}
