namespace Robbiblubber.Localization.UnitTests.Controls;

using System.Drawing.Imaging;
using System.Runtime.Versioning;

using Robbiblubber.Localization.Definition;
using Robbiblubber.Localization.UnitTests.Library;



/// <summary>This class provides test data and helpers for the control tests.</summary>
/// <remarks>The standard data defines the locales <c>en</c> (default), <c>de</c> and <c>ar</c> (right to left) with texts for
///          the prefix <c>app</c>.</remarks>
[SupportedOSPlatform("windows")]
internal static class _ControlTestData
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Writes the standard test data and uses the test directory as only source.</summary>
	/// <param name="files">Test files.</param>
	public static void WriteStandard(_TestFiles files)
	{
		files.WriteLocale("en", "English", isDefault: true);
		files.WriteLocale("de", "Deutsch");
		new LocaleDefinition() { Key = "ar", Name = "Arabic", RightToLeft = true }.Save(files.File("ar.locale"));

		files.WriteTable("app.en.ltab", "en", "app", ("save", "Save"), ("open", "Open"), ("tip", "Tool tip"), ("name", "Name"),
						 ("desc", "Description"), ("item", "Item"), ("node", "Node"), ("child", "Child"), ("column", "Column"));
		files.WriteTable("app.de.ltab", "de", "app", ("save", "Speichern"), ("open", "Öffnen"), ("tip", "Kurzinfo"), ("name", "Name DE"),
						 ("desc", "Beschreibung"), ("item", "Eintrag"), ("node", "Knoten"), ("child", "Kind"), ("column", "Spalte"));
		files.WriteTable("app.ar.ltab", "ar", "app", ("save", "حفظ"));

		Locale.Sources.Set(new DirectoryLocalizationSource(files.Path));
	}


	/// <summary>Gets a locale by its key.</summary>
	/// <param name="key">Key.</param>
	/// <returns>Returns the locale.</returns>
	public static Locale Get(string key)
	{
		return Locale.Locales[key]!;
	}


	/// <summary>Creates a form outside the visible screen area.</summary>
	/// <param name="controls">Controls to add.</param>
	/// <returns>Returns the form; the form has not been shown yet.</returns>
	public static Form CreateForm(params Control[] controls)
	{
		Form rval = new() { StartPosition = FormStartPosition.Manual, Location = new Point(-20000, -20000), Size = new Size(400, 300), ShowInTaskbar = false };
		rval.Controls.AddRange(controls);

		return rval;
	}


	/// <summary>Shows a form and processes pending messages.</summary>
	/// <param name="form">Form.</param>
	public static void Show(Form form)
	{
		form.Show();
		Application.DoEvents();
	}


	/// <summary>Creates PNG image data.</summary>
	/// <param name="size">Width and height in pixels.</param>
	/// <returns>Returns the image data.</returns>
	public static byte[] CreatePng(int size)
	{
		using Bitmap b = new(size, size);
		using(Graphics g = Graphics.FromImage(b)) { g.Clear(Color.SteelBlue); }

		using MemoryStream rval = new();
		b.Save(rval, ImageFormat.Png);

		return rval.ToArray();
	}
}
