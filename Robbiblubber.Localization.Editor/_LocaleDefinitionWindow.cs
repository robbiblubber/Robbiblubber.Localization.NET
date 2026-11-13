namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;



/// <summary>This class implements the window of a locale definition.</summary>
/// <remarks>The icons are kept as the bytes of their image files and saved unchanged; sizes other than 16 × 16 and 32 × 32 pixels are
///          only pointed out, because the controls choose the icon that fits best and scale it when they show it.</remarks>
[SupportedOSPlatform("windows")]
internal sealed partial class _LocaleDefinitionWindow: _DocumentWindow
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Usual size of the small icon in pixels.</summary>
	private const int _SMALL_ICON_SIZE = 16;

	/// <summary>Usual size of the large icon in pixels.</summary>
	private const int _LARGE_ICON_SIZE = 32;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="document">Document.</param>
	/// <param name="opener">Window the new window is opened from, NULL if none.</param>
	public _LocaleDefinitionWindow(_LocaleDefinitionDocument document, Form? opener): base(opener)
	{
		InitializeComponent();

		_ComboBase.Items.AddRange(Locale.Locales.Select(m => m.Key).Order(StringComparer.OrdinalIgnoreCase).ToArray<object>());
		_SetDocument(document);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Shows the size of an icon.</summary>
	/// <param name="label">Label.</param>
	/// <param name="box">Icon box.</param>
	/// <param name="usual">Usual size in pixels.</param>
	private static void _ShowIconSize(Label label, _IconBox box, int usual)
	{
		if(box.ImageSize is not Size size)
		{
			label.Text = "led:definition.noicon".LookUp("No icon. Double-click to load one.");
		}
		else if((size.Width == usual) && (size.Height == usual))
		{
			label.Text = "led:definition.iconsize".LookUp("$WIDTH × $HEIGHT pixels", ("WIDTH", size.Width), ("HEIGHT", size.Height));
		}
		else
		{
			label.Text = "led:definition.iconsize.unusual".LookUp("$WIDTH × $HEIGHT pixels; usually $SIZE × $SIZE",
																	("WIDTH", size.Width), ("HEIGHT", size.Height), ("SIZE", usual));
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private properties                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the document.</summary>
	private _LocaleDefinitionDocument _Definition
	{
		get { return (_LocaleDefinitionDocument) _Document; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns the errors that prevent saving.</summary>
	/// <param name="all">Determines if an empty key is reported; it is not reported while the user edits the definition.</param>
	/// <returns>Returns the error messages.</returns>
	private List<string> _GetErrors(bool all)
	{
		List<string> rval = new();
		string key = _Definition.Key.Trim();

		if(key.Length == 0)
		{
			if(all) { rval.Add("led:definition.nokey".LookUp("Enter a key for the locale.")); }
		}
		else if(!_LocaleDefinitionDocument.IsValidKey(key)) { rval.Add("led:definition.badkey".LookUp("The key must not contain blanks.")); }

		if((key.Length > 0) && string.Equals(key, _Definition.BaseKey.Trim(), StringComparison.OrdinalIgnoreCase))
		{
			rval.Add("led:definition.basekey".LookUp("The base locale must not be the locale itself."));
		}
		return rval;
	}


	/// <summary>Lets the user load an image file into an icon box.</summary>
	/// <param name="box">Icon box.</param>
	private void _LoadIcon(_IconBox box)
	{
		using OpenFileDialog d = new()
		{
			Filter = "led:filter.images".LookUp("Image Files") + " (*.png, *.gif, *.bmp, *.jpg, *.ico)|*.png;*.gif;*.bmp;*.jpg;*.jpeg;*.ico|" +
					 "led:filter.all".LookUp("All Files") + " (*.*)|*.*"
		};
		if(_Document.FileName != null) { d.InitialDirectory = (Path.GetDirectoryName(_Document.FileName) ?? string.Empty); }
		if(d.ShowDialog(this) != DialogResult.OK) return;

		try
		{
			if(box.SetImageData(File.ReadAllBytes(d.FileName)))
			{
				box.Focus();
				return;
			}
			ThemedMessageBox.Show(this, "led:definition.icon.invalid".LookUp("The file is not an image."), "led:open.caption".LookUp("Open"), MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		catch(Exception ex)
		{
			ThemedMessageBox.Show(this, ex.Message, "led:open.caption".LookUp("Open"), MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] _DocumentWindow                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <inheritdoc/>
	internal override string _HelpPage
	{
		get { return "definitions"; }
	}


	/// <inheritdoc/>
	internal override void _SetDocument(_EditorDocument document)
	{
		_LocaleDefinitionDocument definition = (_LocaleDefinitionDocument) document;

		_Binding = true;
		_TextKey.Text = definition.Key;
		_ComboBase.Text = definition.BaseKey;
		_TextName.Text = definition.Name;
		_CheckDefault.Checked = definition.IsDefault;
		_CheckRightToLeft.Checked = definition.RightToLeft;
		_IconSmall.Data = definition.SmallIcon;
		_IconLarge.Data = definition.LargeIcon;

		base._SetDocument(document);
	}


	/// <inheritdoc/>
	internal override bool _CanShow(_EditorDocument document)
	{
		return (document is _LocaleDefinitionDocument);
	}


	/// <inheritdoc/>
	protected override string _SaveFilter
	{
		get
		{
			return "led:filter.locale".LookUp("Locale Definitions") + " (*.locale)|*.locale|" +
				   "led:filter.all".LookUp("All Files") + " (*.*)|*.*";
		}
	}


	/// <inheritdoc/>
	/// <remarks>The name is the key, for example <c>de.locale</c>.</remarks>
	protected override string _SuggestFileName()
	{
		string rval = _Definition.Key.Trim();
		foreach(char i in Path.GetInvalidFileNameChars()) { rval = rval.Replace(i, '_'); }
		return (string.IsNullOrEmpty(rval) ? "locale" : rval) + ".locale";
	}


	/// <inheritdoc/>
	/// <remarks>An empty or invalid key and a base locale that is the locale itself prevent saving; the user is warned about missing
	///          icons.</remarks>
	protected override bool _CheckBeforeSave()
	{
		if(!_CheckErrors(_GetErrors(true))) { return false; }

		List<string> warnings = new();
		if(_Definition.SmallIcon is null) { warnings.Add("led:definition.nosmall".LookUp("The locale has no small icon; the locale selector shows a placeholder for it.")); }
		if(_Definition.LargeIcon is null) { warnings.Add("led:definition.nolarge".LookUp("The locale has no large icon; the small icon is used instead.")); }
		return _ConfirmSave(warnings);
	}


	/// <inheritdoc/>
	protected override void _UpdateContents()
	{
		_ShowIconSize(_LabelSmallSize, _IconSmall, _SMALL_ICON_SIZE);
		_ShowIconSize(_LabelLargeSize, _IconLarge, _LARGE_ICON_SIZE);

		_LabelProblems.Text = string.Join(" ", _GetErrors(false));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Field changed.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Field_Changed(object? sender, EventArgs e)
	{
		if(_Binding) return;

		_Definition.Key = _TextKey.Text;
		_Definition.BaseKey = _ComboBase.Text;
		_Definition.Name = _TextName.Text;
		_Definition.IsDefault = _CheckDefault.Checked;
		_Definition.RightToLeft = _CheckRightToLeft.Checked;
		_Definition.SmallIcon = _IconSmall.Data;
		_Definition.LargeIcon = _IconLarge.Data;
		_Update();
	}


	/// <summary>Load requested in an icon box (double click).</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Icon_LoadRequested(object? sender, EventArgs e)
	{
		if(sender is _IconBox box) { _LoadIcon(box); }
	}


	/// <summary>Icon context menu > Load.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _CmenuIconLoad_Click(object? sender, EventArgs e)
	{
		if(_CmenuIcon.SourceControl is _IconBox box) { _LoadIcon(box); }
	}
}
