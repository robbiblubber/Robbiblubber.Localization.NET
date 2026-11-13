namespace Robbiblubber.Localization.UnitTests.Editor;

using NUnit.Framework;

using Robbiblubber.Localization.Editor;



/// <summary>This class tests the single-line form of texts in the editor table.</summary>
[TestFixture]
public class TextEscapingTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// tests                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Line breaks and tabs are shown as escape sequences.</summary>
	[Test]
	public void Escape_LineBreaksAndTabs()
	{
		Assert.That(_TextEscaping._Escape("a\r\nb\nc\rd\te"), Is.EqualTo(@"a\nb\nc\nd\te"));
	}


	/// <summary>Backslashes are only doubled where they would start an escape sequence.</summary>
	[Test]
	public void Escape_Backslashes()
	{
		Assert.That(_TextEscaping._Escape(@"C:\Data\file.txt"), Is.EqualTo(@"C:\Data\file.txt"));
		Assert.That(_TextEscaping._Escape(@"C:\neu\temp"), Is.EqualTo(@"C:\\neu\\temp"));
		Assert.That(_TextEscaping._Escape(@"a\\b"), Is.EqualTo(@"a\\\b"));
		Assert.That(_TextEscaping._Escape(@"end\"), Is.EqualTo(@"end\"));
	}


	/// <summary>Escape sequences are converted back; other backslashes stay.</summary>
	[Test]
	public void Unescape_Sequences()
	{
		Assert.That(_TextEscaping._Unescape(@"a\nb\tc\\d\xe\"), Is.EqualTo("a" + Environment.NewLine + "b\tc\\d\\xe\\"));
		Assert.That(_TextEscaping._Unescape(null), Is.Empty);
		Assert.That(_TextEscaping._Escape(null), Is.Empty);
	}


	/// <summary>Every text survives the conversion to the single-line form and back.</summary>
	[Test]
	public void Roundtrip_Random()
	{
		Random rnd = new(4711);
		char[] chars = ['a', 'n', 't', '\\', '\t', ' ', 'x'];

		for(int i = 0; i < 2000; i++)
		{
			int length = rnd.Next(0, 12);
			string text = new(Enumerable.Range(0, length).Select(_ => chars[rnd.Next(chars.Length)]).ToArray());
			if(rnd.Next(3) == 0) { text = text.Insert(rnd.Next(text.Length + 1), Environment.NewLine); }

			string line = _TextEscaping._Escape(text);
			Assert.That(line, Does.Not.Contain("\n").And.Not.Contain("\r").And.Not.Contain("\t"));
			Assert.That(_TextEscaping._Unescape(line), Is.EqualTo(text), "Text: " + text.Replace("\t", "<TAB>"));
		}
	}
}
