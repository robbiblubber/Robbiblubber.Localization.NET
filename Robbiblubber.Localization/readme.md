# robbiblubber.org Localization Library

The robbiblubber.org Localization Library provides locale definitions and
language tables for applications. The library targets .NET 8 and .NET 10 and
contains no platform-dependent code; Windows Forms support is provided by the
Localization Controls Library.

The repository can be found at https://github.com/robbiblubber/Robbiblubber.Localization.NET.
For general information see http://robbiblubber.org/.

The robbiblubber.org Localization Library and tools are provided under the MIT
license and may be used without restrictions under the conditions of this
license agreement.


## Usage

```csharp
using Robbiblubber.Localization;

// optional: the default source is the application directory including subdirectories
Locale.Sources.Set(new DirectoryLocalizationSource(Path.Combine(AppContext.BaseDirectory, "locale")));

Locale.Current = Locale.Locales["de"];

string save = "app:mw.save".LookUp("Save");
string info = "app:mw.saved".LookUp("$NAME has been saved.", ("NAME", fileName));
```

- `Locale.Locales` lists the available locales, ordered by name; a locale is
  found by its key regardless of case (`Locale.Locales["de"]`).
- `Locale.Current` is the current locale, `Locale.Fallback` the fallback
  locale; if none is set, the locale marked as default (or the first one) is
  used. `Locale.LocaleChanged` is raised when the current or fallback locale
  changes, the sources change or the texts are reloaded.
- `Locale.LookUp(symbol, defaultText)` and the extension method
  `symbol.LookUp(defaultText)` return the text of the current locale. If the
  symbol is not defined, the default text is returned; without default text
  (or with `Locale.PreferFallback`), the text of the fallback locale is used
  first. `Locale.TryLookUp` tells if a symbol is defined, `locale.GetText(symbol)`
  returns the text of a given locale.
- Placeholders: a value named `NAME` replaces `$NAME` in the text. Values are
  formatted with the culture of the current locale (`Locale.Culture`).
- `Locale.Issues` lists sources and files that could not be read, duplicate
  locales and duplicate symbols. Loading never fails because of a single broken
  file.
- `Locale.Reload()` reads all sources again. Lookups are thread-safe.


## Locales and language tables

A **locale definition** (`.locale`) is a zip file with the entries
`locale.def` (ddn: `key`, `base`, `name`, `default`, `rtl`), `small.icon` and
`large.icon` (images, usually 16 × 16 and 32 × 32 PNG). A locale with a base
locale (`de-AT` based on `de`) takes all texts it does not define from the base
locale.

A **language table** (`.ltab`, compressed `.cltab`) is a ddn file with a header
and the texts of one locale:

```
cid = lang.def;
version = 3.0;

header
{
    key = lt.app;
    lang = de;
    name = app.de;
    prefixes = app;
    version = 1.0;
    author = robbiblubber.org;
}

data
{
    mw.save = Speichern;
    mw.saved = $NAME wurde gespeichert.;
}
```

The **prefix** is a namespace: the symbols of this table are looked up as
`app:mw.save`, so tables of different components do not collide. A table
without prefix provides its symbols as they are (`mw.save`).

`LocaleDefinition` and `LanguageTable` in `Robbiblubber.Localization.Definition`
read and write these files.


## Sources

`Locale.Sources` lists the sources the locales and tables are loaded from.
By default it contains the application directory including its
subdirectories. Available sources are:

- `DirectoryLocalizationSource(path, recursive)` reads the files of a
  directory, optionally with its subdirectories.
- `EmbeddedLocalizationSource(assembly)` reads the embedded resources of an
  assembly.
- Other sources implement `ILocalizationSource` and return
  `LocalizationResource` objects; the extension of the resource name
  (`.locale`, `.ltab`, `.cltab`) determines the type.

The sources are read in order, the files of a source in the order of their
names. If a locale is defined twice, the first definition is used; if a symbol
is defined twice for the same locale, the later table wins. Both are reported
as issues. A table of a later source can therefore change single texts.


## Embedded tables

The locale definitions and language tables of the applications and libraries
of robbiblubber.org are files in the directory `locale` in the application
directory, so they can be changed and extended without building again.

For single-file deployments, the files can be embedded instead:

```csharp
Locale.Sources.Add(new EmbeddedLocalizationSource(typeof(Program).Assembly));
```

MSBuild treats a file name like `app.fr.ltab` as a culture-specific resource
and moves it into a satellite assembly, so embedded files must not have a
culture name before the extension (for example `app-fr.ltab`).


## Tools

- **Localization Editor** (`led`) creates and edits language tables and locale
  definitions, with find and replace, a text editor for long texts, sorting,
  import of missing entries from another table and a help in English, German,
  French and Indonesian.
- **Locale Definition Build Tool** (`bloc`) creates locale definitions on the
  command line, for example
  `bloc de.locale -k de -n Deutsch -s de16.png -l de32.png`; `bloc --help`
  shows all options, `bloc -i` starts an interactive mode.
