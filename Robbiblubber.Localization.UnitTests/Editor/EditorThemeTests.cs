namespace Robbiblubber.Localization.UnitTests.Editor;

using System.Runtime.Versioning;

using NUnit.Framework;

using Robbiblubber.Localization.Editor;
using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class tests the themes in the editor: the themed controls of the windows, the theme selection in the settings
///          window and the icon box.</summary>
[TestFixture][Apartment(ApartmentState.STA)][NonParallelizable][SupportedOSPlatform("windows")]
public class EditorThemeTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Theme of the application before the test.</summary>
	private Theme _Theme = Theme.AUTO;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns a control and all its child controls.</summary>
	/// <param name="control">Control.</param>
	/// <returns>Returns the controls.</returns>
	private static IEnumerable<Control> _GetAll(Control control)
	{
		yield return control;
		foreach(Control i in control.Controls)
		{
			foreach(Control k in _GetAll(i)) { yield return k; }
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// setup                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Remembers the theme of the application.</summary>
	[SetUp]
	public void SetUp()
	{
		_Theme = ThemeManager.Theme;
	}


	/// <summary>Restores the theme of the application.</summary>
	[TearDown]
	public void TearDown()
	{
		ThemeManager.Theme = _Theme;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// tests                                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>The settings window uses the themed controls and offers the theme selector with the link to the theme editor.</summary>
	[Test]
	public void SettingsWindow_UsesThemedControls()
	{
		using _SettingsWindow window = new();
		Control[] controls = _GetAll(window).ToArray();

		Assert.That(controls.OfType<Button>().All(m => m is ThemedButton), Is.True);
		Assert.That(controls.OfType<GroupBox>().All(m => m is ThemedGroupBox), Is.True);
		Assert.That(controls.OfType<TabControl>().All(m => m is ThemedTabControl), Is.True);

		ThemeSelector selector = controls.OfType<ThemeSelector>().Single();
		Assert.That(selector.ShowEditorLink, Is.True);
		Assert.That(selector.AutoApply, Is.True);
		Assert.That(selector.SaveOnEditorOk, Is.True);
	}


	/// <summary>The text editor window uses the themed controls.</summary>
	[Test]
	public void TextEditorWindow_UsesThemedControls()
	{
		using _TextEditorWindow window = new("app:symbol", "Text");
		Control[] controls = _GetAll(window).ToArray();

		Assert.That(controls.OfType<Button>().All(m => m is ThemedButton), Is.True);
		Assert.That(controls.OfType<TextBox>().All(m => m is ThemedTextBox), Is.True);
	}


	/// <summary>The icon box takes the theme of its window and is drawn in its colors without an exception.</summary>
	[Test]
	public void IconBox_FollowsTheme()
	{
		using Form form = new();
		using _IconBox box = new();
		form.Controls.Add(box);

		ThemeManager.Apply(form, Theme.DARK);
		Assert.That(((IThemedControl) box).ThemeColors.EffectiveTheme, Is.SameAs(Theme.DARK));
		Assert.That(((IThemedControl) box).ThemesChildren, Is.False);

		using Bitmap bitmap = new(box.Width, box.Height);
		Assert.That(() => box.DrawToBitmap(bitmap, box.ClientRectangle), Throws.Nothing);
		Assert.That(bitmap.GetPixel(0, 0).ToArgb(), Is.EqualTo(Theme.DARK[ThemeColor.FIELD_BORDER].ToArgb()));

		box.Enabled = false;
		box.DrawToBitmap(bitmap, box.ClientRectangle);
		Assert.That(bitmap.GetPixel(box.Width / 2, box.Height / 2).ToArgb(), Is.EqualTo(Theme.DARK[ThemeColor.DISABLED_BACK].ToArgb()));

		ThemeManager.Apply(form, Theme.LIGHT);
		Assert.That(((IThemedControl) box).ThemeColors.EffectiveTheme, Is.SameAs(Theme.LIGHT));
	}


	/// <summary>A menu item without a dark variant in the resources gets none; the result is cached.</summary>
	[Test]
	public void DarkImages_MissingVariant()
	{
		Assert.That(_DarkImages._Get("_MenuDoesNotExist"), Is.Null);
		Assert.That(_DarkImages._Get("_MenuDoesNotExist"), Is.Null);
		Assert.That(() => _DarkImages._Enable(), Throws.Nothing);
	}


	/// <summary>The menu items with images have dark variants of 16 x 16 pixels in the resources; the results are cached.</summary>
	[Test]
	public void DarkImages_MenuVariants()
	{
		string[] names = { "_MenuFileNew", "_MenuFileNewTable", "_MenuFileNewDefinition", "_MenuFileOpen", "_MenuFileSave",
		                   "_MenuFileSaveAs", "_MenuFileClose", "_MenuEditUndo", "_MenuEditRedo", "_MenuEditCut", "_MenuEditCopy",
		                   "_MenuEditPaste", "_MenuEditDelete", "_MenuEditSelectAll", "_MenuEditFind", "_MenuEditFindNext",
		                   "_MenuEditReplace", "_MenuToolsImport", "_MenuToolsSort", "_MenuToolsTextEditor", "_MenuToolsSettings",
		                   "_MenuHelpShowHelp", "_MenuHelpAbout" };

		foreach(string i in names)
		{
			Image? dark = _DarkImages._Get(i);

			Assert.That(dark, Is.Not.Null, i);
			Assert.That(dark!.Size, Is.EqualTo(new Size(16, 16)), i);
			Assert.That(_DarkImages._Get(i), Is.SameAs(dark), i);
		}
	}


	/// <summary>The recent files list shows the images of the editor for its file types, with dark variants; other files keep the
	///          shell icon.</summary>
	[Test]
	public void RecentFiles_FileImages()
	{
		foreach(string i in new[] { @"C:\tables\app.de.ltab", @"C:\tables\APP.DE.CLTAB", @"C:\locales\de.locale" })
		{
			Image? image = _RecentFileList._GetFileImage(i);

			Assert.That(image, Is.Not.Null, i);
			Assert.That(image!.Size, Is.EqualTo(new Size(16, 16)), i);
			Assert.That(_RecentFileList._GetFileImage(i), Is.SameAs(image), i);
		}

		Image? table = _RecentFileList._GetFileImage(@"C:\tables\app.de.ltab");
		Assert.That(_RecentFileList._GetFileImage(@"C:\tables\app.de.cltab"), Is.Not.SameAs(table));
		Assert.That(_RecentFileList._GetFileImage(@"C:\readme.txt"), Is.Null);
		Assert.That(_RecentFileList._GetFileImage(@"C:\tables"), Is.Null);

		foreach(string i in new[] { "_FileLtab", "_FileCltab", "_FileLocale" })
		{
			Assert.That(_DarkImages._Get(i), Is.Not.Null, i);
		}
	}
}
