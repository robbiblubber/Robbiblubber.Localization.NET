namespace Robbiblubber.Localization.UnitTests.Editor;

using System.Text.RegularExpressions;

using NUnit.Framework;

using Robbiblubber.Localization.Definition;



/// <summary>This class tests the locale definitions and language tables of the editor.</summary>
[TestFixture]
public class EditorTablesTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the directory of the locale files in the output directory.</summary>
	/// <returns>Returns the directory.</returns>
	private static string _GetLocaleDirectory()
	{
		return Path.Combine(AppContext.BaseDirectory, "locale");
	}


	/// <summary>Loads the tables of the editor from the directory locale.</summary>
	/// <returns>Returns the tables by locale key.</returns>
	private static Dictionary<string, LanguageTable> _LoadTables()
	{
		Dictionary<string, LanguageTable> rval = new();

		foreach(string i in Directory.GetFiles(_GetLocaleDirectory(), "led.*.ltab"))
		{
			LanguageTable t = LanguageTable.Load(i);
			rval.Add(t.LocaleKey, t);
		}
		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// tests                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>The locale definitions and tables of the editor are files in the directory locale, not embedded resources.</summary>
	[Test]
	public void Files_InLocaleDirectory()
	{
		foreach(string i in new[] { "de", "en", "fr", "id" })
		{
			Assert.That(File.Exists(Path.Combine(_GetLocaleDirectory(), i + ".locale")), Is.True, i + ".locale");
			Assert.That(File.Exists(Path.Combine(_GetLocaleDirectory(), "led." + i + ".ltab")), Is.True, "led." + i + ".ltab");
		}

		Assert.That(typeof(Robbiblubber.Localization.Editor._TableDocument).Assembly.GetManifestResourceNames()
					.Where(m => m.EndsWith(".locale") || m.EndsWith(".ltab")), Is.Empty);
	}


	/// <summary>The editor brings tables for English, German, French and Indonesian with the prefix led.</summary>
	[Test]
	public void Tables_Languages()
	{
		Dictionary<string, LanguageTable> tables = _LoadTables();

		Assert.That(tables.Keys, Is.EquivalentTo(new[] { "en", "de", "fr", "id" }));
		foreach(LanguageTable i in tables.Values)
		{
			Assert.That(i.Prefixes, Is.EqualTo(new[] { "led" }), i.LocaleKey);
			Assert.That(i.Key, Is.EqualTo("led"), i.LocaleKey);
		}
	}


	/// <summary>All tables define the same symbols, no text is empty, and every placeholder of the English text is used.</summary>
	[Test]
	public void Tables_Complete()
	{
		Dictionary<string, LanguageTable> tables = _LoadTables();
		Dictionary<string, string> en = tables["en"].Entries.ToDictionary(m => m.Symbol, m => m.Text);

		foreach(LanguageTable i in tables.Values)
		{
			Dictionary<string, string> texts = i.Entries.ToDictionary(m => m.Symbol, m => m.Text);
			Assert.That(texts.Keys, Is.EquivalentTo(en.Keys), i.LocaleKey);

			foreach(KeyValuePair<string, string> k in en)
			{
				Assert.That(texts[k.Key], Is.Not.Empty, i.LocaleKey + ": " + k.Key);
				foreach(Match m in Regex.Matches(k.Value, @"\$[A-Z]+"))
				{
					Assert.That(texts[k.Key], Does.Contain(m.Value), i.LocaleKey + ": " + k.Key);
				}
			}
		}
	}
}
