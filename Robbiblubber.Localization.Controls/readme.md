# robbiblubber.org Localization Controls Library

The robbiblubber.org Localization Controls Library localizes Windows Forms user
interfaces with the locales and language tables of the robbiblubber.org
Localization Library. The components are added to a form in the designer and
attach locale symbols to the existing controls.

The library targets .NET 8 and .NET 10 on Windows.

The repository can be found at https://github.com/robbiblubber/Robbiblubber.Localization.NET.
For general information see http://robbiblubber.org/.

The robbiblubber.org Localization Controls Library is provided under the MIT
license and may be used without restrictions under the conditions of this
license agreement.


## Components

- **`LocaleProvider`** adds the extender properties `LocaleSymbol`,
  `AccessibleNameLocaleSymbol` and `AccessibleDescriptionLocaleSymbol` to the
  controls, tool strip items and column headers of a form. The text set in the
  designer is the default text. The provider localizes when the form is created
  and, with `AutoUpdate`, whenever the current locale changes; it raises
  `Localized` afterwards, so texts composed in code can be updated there.
  Optionally it applies the reading direction of the locale
  (`ApplyRightToLeft`).
- **`LocalizableToolTip`** localizes tool tip texts (`LocaleSymbol`).
- **`LocaleSelector`** lets the user select a locale and applies it as current
  locale (`AutoApply`). `Style` chooses a drop-down list (`DROPDOWN`) or a
  button with the large icon that opens a menu: without arrow (`BUTTON`), with
  a drop-down arrow (`ARROW_BUTTON`) or with a box with a drop-down arrow
  (`DROPDOWN_BUTTON`). Its colors follow the theme of the application or of its
  form, with optional overrides of single colors (`Colors`), like the
  self-drawn controls of Spellbook.Controls.


## Items and extensions

- **`LocalizableItem`**, **`LocalizableTreeNode`**, **`LocalizableListViewItem`**
  and **`LocalizableColumnHeader`** are localizable items for lists and trees.
  The provider localizes them (`LocalizeItems`), or the `Localize()` extension
  methods for list boxes, combo boxes, themed combo boxes, list views and tree
  views. The items and the selection of a control are kept.
- **`CreateSmallImage()`** and **`CreateLargeImage()`** create images from the
  icons of a locale. **`CreateImage(size)`** creates an image of a given size
  from the icon that fits best; the `LocaleSelector` uses it, so icons of
  16 × 16 and 32 × 32 pixels look sharp at any display scaling.


## Spellbook dialogs

The dialogs of the Spellbook Controls Library (`ThemedMessageBox`,
`ThemedColorDialog` and `ThemeEditorDialog`) and its `ThemeSelector` do not
depend on this library; they ask `ControlTexts.LookUp` for their texts.
**`SpellbookLocalization.Enable()`** connects them to the current locale,
usually once at the start of the application. `Disable()` ends the
localization.

The language tables have the prefix `spellbook`, one table per dialog and
locale (`spellbook.msgbox.de.ltab`, `spellbook.colordialog.de.ltab`,
`spellbook.themeeditor.de.ltab`, `spellbook.themeselector.de.ltab`) for `de`,
`en`, `fr` and `id`. The build copies them to `locale` in the application
directory, where the default source finds them; an application that sets its
own `Locale.Sources` includes this directory. Other locales are added with
tables with the prefix `spellbook`. Texts set explicitly in
`ThemedMessageBoxTexts`, `ThemedColorDialogTexts`, `ThemeEditorTexts` or
`ThemeSelectorTexts` win over the localization.
