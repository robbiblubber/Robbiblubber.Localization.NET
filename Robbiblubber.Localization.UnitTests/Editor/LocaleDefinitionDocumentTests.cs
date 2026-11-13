namespace Robbiblubber.Localization.UnitTests.Editor;

using NUnit.Framework;

using Robbiblubber.Localization.Definition;
using Robbiblubber.Localization.Editor;



/// <summary>This class tests the document model of the locale definition window and the recognition of the file type.</summary>
[TestFixture]
public class LocaleDefinitionDocumentTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Small icon data; the bytes are not a valid image, so it can be checked that they are not converted.</summary>
	private static readonly byte[] _SMALL = [1, 2, 3, 4, 5];

	/// <summary>Large icon data.</summary>
	private static readonly byte[] _LARGE = [6, 7, 8];



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Test directory.</summary>
	private string _Dir = string.Empty;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// setup                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates the test directory.</summary>
	[SetUp]
	public void SetUp()
	{
		_Dir = Path.Combine(Path.GetTempPath(), "led-tests", Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(_Dir);
	}


	/// <summary>Deletes the test directory.</summary>
	[TearDown]
	public void TearDown()
	{
		if(Directory.Exists(_Dir)) { Directory.Delete(_Dir, true); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Writes a locale definition file.</summary>
	/// <param name="name">File name.</param>
	/// <returns>Returns the full file name.</returns>
	private string _WriteDefinition(string name)
	{
		LocaleDefinition d = new() { Key = "de-AT", BaseKey = "de", Name = "Österreichisch", IsDefault = true, RightToLeft = false, SmallIcon = _SMALL, LargeIcon = _LARGE };

		string rval = Path.Combine(_Dir, name);
		d.Save(rval);
		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// tests                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>A new document is untouched and empty.</summary>
	[Test]
	public void New_Untouched()
	{
		_LocaleDefinitionDocument d = new();

		Assert.That(d.FileName, Is.Null);
		Assert.That(d.IsDirty, Is.False);
		Assert.That(d.IsUntouched, Is.True);
		Assert.That(d.Key, Is.Empty);
		Assert.That(d.BaseKey, Is.Empty);
		Assert.That(d.SmallIcon, Is.Null);
		Assert.That(d.LargeIcon, Is.Null);
	}


	/// <summary>Loading takes over all values and the icons as they are.</summary>
	[Test]
	public void Load_Values()
	{
		string file = _WriteDefinition("de-AT.locale");
		_LocaleDefinitionDocument d = _LocaleDefinitionDocument.Load(file);

		Assert.That(d.FileName, Is.EqualTo(Path.GetFullPath(file)));
		Assert.That(d.Key, Is.EqualTo("de-AT"));
		Assert.That(d.BaseKey, Is.EqualTo("de"));
		Assert.That(d.Name, Is.EqualTo("Österreichisch"));
		Assert.That(d.IsDefault, Is.True);
		Assert.That(d.RightToLeft, Is.False);
		Assert.That(d.SmallIcon, Is.EqualTo(_SMALL));
		Assert.That(d.LargeIcon, Is.EqualTo(_LARGE));
		Assert.That(d.IsDirty, Is.False);
		Assert.That(d.IsUntouched, Is.False);
	}


	/// <summary>Every field is detected as change; restoring the value clears the change.</summary>
	[Test]
	public void IsDirty_Changes()
	{
		_LocaleDefinitionDocument d = _LocaleDefinitionDocument.Load(_WriteDefinition("d.locale"));

		d.Key = "de-CH";
		Assert.That(d.IsDirty, Is.True);
		d.Key = "de-AT";
		Assert.That(d.IsDirty, Is.False);

		d.BaseKey = string.Empty;
		Assert.That(d.IsDirty, Is.True);
		d.BaseKey = "de";

		d.Name = "Austrian";
		Assert.That(d.IsDirty, Is.True);
		d.Name = "Österreichisch";

		d.IsDefault = false;
		Assert.That(d.IsDirty, Is.True);
		d.IsDefault = true;

		d.RightToLeft = true;
		Assert.That(d.IsDirty, Is.True);
		d.RightToLeft = false;
		Assert.That(d.IsDirty, Is.False);
	}


	/// <summary>Icons are compared by their bytes: other bytes are a change, the same bytes in a new array are not.</summary>
	[Test]
	public void IsDirty_Icons()
	{
		_LocaleDefinitionDocument d = _LocaleDefinitionDocument.Load(_WriteDefinition("d.locale"));

		d.SmallIcon = [1, 2, 3];
		Assert.That(d.IsDirty, Is.True);
		d.SmallIcon = _SMALL.ToArray();
		Assert.That(d.IsDirty, Is.False);

		d.LargeIcon = null;
		Assert.That(d.IsDirty, Is.True);
		d.LargeIcon = _LARGE.ToArray();
		Assert.That(d.IsDirty, Is.False);
	}


	/// <summary>Blanks around key and base key and an empty name that would be saved as the key are no change.</summary>
	[Test]
	public void IsDirty_Normalized()
	{
		_LocaleDefinitionDocument d = _LocaleDefinitionDocument.Load(_WriteDefinition("d.locale"));

		d.Key = " de-AT ";
		d.BaseKey = "de ";
		Assert.That(d.IsDirty, Is.False);

		d.Name = "de-AT";
		Assert.That(d.IsDirty, Is.True);
		d.Name = "Österreichisch";

		_LocaleDefinitionDocument n = new();
		n.Name = "  ";
		Assert.That(n.IsDirty, Is.False);
	}


	/// <summary>Saving writes all values and the icons unchanged; the document is not dirty afterwards.</summary>
	[Test]
	public void Save_Roundtrip()
	{
		_LocaleDefinitionDocument d = new() { Key = " xx ", BaseKey = " en ", Name = "", RightToLeft = true, SmallIcon = _SMALL, LargeIcon = _LARGE };
		Assert.That(d.IsDirty, Is.True);

		string file = Path.Combine(_Dir, "xx.locale");
		d.Save(file);

		Assert.That(d.FileName, Is.EqualTo(file));
		Assert.That(d.IsDirty, Is.False);

		LocaleDefinition l = LocaleDefinition.Load(file);
		Assert.That(l.Key, Is.EqualTo("xx"));
		Assert.That(l.BaseKey, Is.EqualTo("en"));
		Assert.That(l.Name, Is.EqualTo("xx"));
		Assert.That(l.IsDefault, Is.False);
		Assert.That(l.RightToLeft, Is.True);
		Assert.That(l.SmallIcon, Is.EqualTo(_SMALL));
		Assert.That(l.LargeIcon, Is.EqualTo(_LARGE));
	}


	/// <summary>Missing icons are saved as missing.</summary>
	[Test]
	public void Save_WithoutIcons()
	{
		_LocaleDefinitionDocument d = new() { Key = "xx" };
		string file = Path.Combine(_Dir, "xx.locale");
		d.Save(file);

		LocaleDefinition l = LocaleDefinition.Load(file);
		Assert.That(l.SmallIcon, Is.Null);
		Assert.That(l.LargeIcon, Is.Null);
		Assert.That(_LocaleDefinitionDocument.Load(file).IsDirty, Is.False);
	}


	/// <summary>A new document cannot be saved without file name, and not without key.</summary>
	[Test]
	public void Save_Invalid()
	{
		Assert.Throws<InvalidOperationException>(() => new _LocaleDefinitionDocument() { Key = "xx" }.Save());
		Assert.Throws<InvalidOperationException>(() => new _LocaleDefinitionDocument().Save(Path.Combine(_Dir, "empty.locale")));
		Assert.That(File.Exists(Path.Combine(_Dir, "empty.locale")), Is.False);
	}


	/// <summary>Keys must not be empty or contain blanks.</summary>
	[Test]
	public void IsValidKey()
	{
		Assert.That(_LocaleDefinitionDocument.IsValidKey("de"), Is.True);
		Assert.That(_LocaleDefinitionDocument.IsValidKey(" de-AT "), Is.True);
		Assert.That(_LocaleDefinitionDocument.IsValidKey(null), Is.False);
		Assert.That(_LocaleDefinitionDocument.IsValidKey("  "), Is.False);
		Assert.That(_LocaleDefinitionDocument.IsValidKey("de AT"), Is.False);
		Assert.That(_LocaleDefinitionDocument.IsValidKey("de\tAT"), Is.False);
	}


	/// <summary>Open recognizes the type by the content, whatever the extension.</summary>
	[Test]
	public void Open_RecognizesType()
	{
		string definition = _WriteDefinition("d.ltab");

		LanguageTable t = new() { Key = "k", LocaleKey = "de" };
		string table = Path.Combine(_Dir, "t.locale");
		t.Save(table);

		Assert.That(_EditorDocument.Open(definition), Is.InstanceOf<_LocaleDefinitionDocument>());
		Assert.That(_EditorDocument.Open(table), Is.InstanceOf<_TableDocument>());
	}


	/// <summary>Open reports files of other types and broken definitions as format errors.</summary>
	[Test]
	public void Open_InvalidFiles()
	{
		string text = Path.Combine(_Dir, "x.txt");
		File.WriteAllText(text, "hello");
		Assert.Throws<FormatException>(() => _EditorDocument.Open(text));

		string zip = Path.Combine(_Dir, "x.locale");
		using(System.IO.Compression.ZipArchive z = System.IO.Compression.ZipFile.Open(zip, System.IO.Compression.ZipArchiveMode.Create)) { z.CreateEntry("other.txt"); }
		Assert.Throws<FormatException>(() => _EditorDocument.Open(zip));

		string empty = Path.Combine(_Dir, "empty.ltab");
		File.WriteAllBytes(empty, []);
		Assert.Throws<FormatException>(() => _EditorDocument.Open(empty));

		Assert.Throws<FileNotFoundException>(() => _EditorDocument.Open(Path.Combine(_Dir, "missing.locale")));
	}
}
