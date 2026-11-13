namespace Robbiblubber.Localization.Controls;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This control lets the user select a locale.</summary>
/// <remarks><para>In the <see cref="LocaleSelectorStyle.DROPDOWN"/> style, a drop-down list shows the available locales with
///          their small icons and names. In the <see cref="LocaleSelectorStyle.BUTTON"/> style, a button shows the large icon of
///          the selected locale and opens a menu with the locales; the <see cref="LocaleSelectorStyle.ARROW_BUTTON"/> style adds
///          a drop-down arrow to the button, the <see cref="LocaleSelectorStyle.DROPDOWN_BUTTON"/> style a box with a drop-down
///          arrow. The button is operated with the mouse or the keyboard (Space, Enter, Down, F4); clicking it while the menu is
///          open closes the menu.</para>
///          <para>The list and the menu show the icons with 16, the button with 20 logical pixels. For each of them, the icon
///          of the locale that fits the size in device pixels best is chosen and scaled (see
///          <see cref="LocaleImageExtensions.CreateImage(Locale, int)"/>), so icons of 16 × 16 and 32 × 32 pixels look sharp at
///          any display scaling. When the scaling changes, the images are created again.</para>
///          <para>The colors are defined by the color table <see cref="Colors"/>: a theme and optional overrides of single colors.
///          Without a theme of its own, the selector uses the theme applied to its form (<see cref="ThemeManager.Apply(Control, Theme?)"/>,
///          <see cref="ThemeProvider"/>) or the theme of the application.</para>
///          <para>If <see cref="AutoApply"/> is set, the selected locale becomes the current locale, and the selector follows the
///          current locale when it is changed elsewhere. The list is updated when the locales are reloaded. If the selected
///          locale is no longer available after a reload, it stays selected and remains in the list until another locale is
///          selected.</para>
///          <para>In the designer, the selector shows a placeholder and does not load any locales.</para></remarks>
[DefaultEvent(nameof(SelectedLocaleChanged))][DefaultProperty(nameof(Style))]
[ToolboxBitmap(typeof(LocaleSelector), "Resources.localeselector.bmp")]
[SupportedOSPlatform("windows")]
public partial class LocaleSelector: Control, IThemedControl
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Default width in logical pixels.</summary>
	private const int _DEFAULT_WIDTH = 160;

	/// <summary>Button size in logical pixels.</summary>
	private const int _BUTTON_SIZE = 26;

	/// <summary>Button image size in logical pixels.</summary>
	private const int _BUTTON_IMAGE_SIZE = 20;

	/// <summary>Size of the images in the drop-down list and the menu in logical pixels.</summary>
	private const int _ITEM_IMAGE_SIZE = 16;

	/// <summary>Default text displayed when there are no locales.</summary>
	private const string _DEFAULT_EMPTY_TEXT = "No locales";

	/// <summary>Text displayed in the designer.</summary>
	private const string _DESIGN_TEXT = "Locale";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Style.</summary>
	private LocaleSelectorStyle _Style = LocaleSelectorStyle.DROPDOWN;

	/// <summary>Drop-down list, NULL in the button style.</summary>
	private ThemedComboBox? _Combo = null;

	/// <summary>Button, NULL in the drop-down style.</summary>
	private _SelectorButton? _Button = null;

	/// <summary>Button menu, NULL in the drop-down style.</summary>
	private ContextMenuStrip? _Menu = null;

	/// <summary>Color table.</summary>
	private readonly LocaleSelectorColors _Colors = new();

	/// <summary>Tool tip for the button.</summary>
	private readonly ToolTip _ToolTip = new();

	/// <summary>Displayed locales: the available locales and, if it is not available, the selected locale.</summary>
	private Locale[] _Locales = Array.Empty<Locale>();

	/// <summary>Selected locale.</summary>
	private Locale? _Selected = null;

	/// <summary>Determines if the selected locale is applied as current locale.</summary>
	private bool _AutoApply = true;

	/// <summary>Text displayed when there are no locales.</summary>
	private string _EmptyText = _DEFAULT_EMPTY_TEXT;

	/// <summary>Determines if the locales have been loaded.</summary>
	private bool _Filled = false;

	/// <summary>Determines if the elements are being filled.</summary>
	private bool _Filling = false;

	/// <summary>Determines if the next click on the button is ignored, because the mouse button closed the menu.</summary>
	private bool _IgnoreClick = false;

	/// <summary>Images owned by the control.</summary>
	private readonly List<Image> _Images = new();

	/// <summary>Disposed flag.</summary>
	private bool _Disposed = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public LocaleSelector(): base()
	{
		SetStyle(ControlStyles.Selectable, false);
		_Colors.Changed += _Colors_Changed;
		_CreateElements();

		Locale.LocaleChanged += _Locale_LocaleChanged;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Loads an image from the resources of this assembly.</summary>
	/// <param name="name">Resource file name.</param>
	/// <returns>Returns a new image.</returns>
	private static Image _LoadImage(string name)
	{
		using Stream? s = typeof(LocaleSelector).Assembly.GetManifestResourceStream("Robbiblubber.Localization.Controls.Resources." + name);
		if(s is null) { return new Bitmap(16, 16); }

		using Image image = Image.FromStream(s);
		return new Bitmap(image);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Takes ownership of an image.</summary>
	/// <param name="image">Image.</param>
	/// <returns>Returns the image.</returns>
	private Image _Own(Image image)
	{
		_Images.Add(image);
		return image;
	}


	/// <summary>Creates the elements for the current style.</summary>
	private void _CreateElements()
	{
		if(_Combo is not null)
		{
			Controls.Remove(_Combo);
			_Combo.SelectionChangeCommitted -= _Combo_SelectionChangeCommitted;
			_Combo.SizeChanged -= _Combo_SizeChanged;
			_Combo.Dispose();
			_Combo = null;
		}
		if(_Button is not null)
		{
			Controls.Remove(_Button);
			_ToolTip.SetToolTip(_Button, null);
			_Button.Click -= _Button_Click;
			_Button.MouseUp -= _Button_MouseUp;
			_Button.Dispose();
			_Button = null;
		}
		if(_Menu is not null)
		{
			_ClearMenu();
			_Menu.Closing -= _Menu_Closing;
			_Menu.Closed -= _Menu_Closed;
			_Menu.Dispose();
			_Menu = null;
		}

		if(_Style == LocaleSelectorStyle.DROPDOWN)
		{
			_Combo = new ThemedComboBox() { Dock = DockStyle.Fill, DropDownStyle = ThemedComboBoxStyle.LIST };
			_Colors._ApplyTo(_Combo.Colors);
			_Combo.SelectionChangeCommitted += _Combo_SelectionChangeCommitted;
			_Combo.SizeChanged += _Combo_SizeChanged;
			Controls.Add(_Combo);
		}
		else
		{
			_Button = new _SelectorButton(this, _Style) { Dock = DockStyle.Fill };
			_Button.Click += _Button_Click;
			_Button.MouseUp += _Button_MouseUp;
			Controls.Add(_Button);

			_Menu = new ContextMenuStrip() { Renderer = new _MenuRenderer(this) };
			_Menu.Closing += _Menu_Closing;
			_Menu.Closed += _Menu_Closed;
		}

		_UpdateSize();
		if(_Filled || IsHandleCreated) { _Fill(true); }
	}


	/// <summary>Removes and disposes the menu items.</summary>
	private void _ClearMenu()
	{
		if(_Menu is null) { return; }

		ToolStripItem[] items = _Menu.Items.Cast<ToolStripItem>().ToArray();
		_Menu.Items.Clear();
		foreach(ToolStripItem i in items)
		{
			i.Click -= _MenuItem_Click;
			i.Dispose();
		}
	}


	/// <summary>Gets the fixed size of the button styles.</summary>
	/// <returns>Returns the size in device pixels.</returns>
	private Size _GetButtonSize()
	{
		int size = LogicalToDeviceUnits(_BUTTON_SIZE);
		int arrow = LogicalToDeviceUnits(_SelectorButton._GetArrowWidth(_Style));

		return new Size(size + arrow, size);
	}


	/// <summary>Applies the fixed size of the current style.</summary>
	private void _UpdateSize()
	{
		if(_Style != LocaleSelectorStyle.DROPDOWN)
		{
			Size = _GetButtonSize();
		}
		else if(_Combo is not null) { Height = _Combo.Height; }
	}


	/// <summary>Loads the locales and fills the elements.</summary>
	/// <param name="force">Determines if the elements are rebuilt even if the locales have not changed.</param>
	/// <remarks>Raises <see cref="SelectedLocaleChanged"/> if the selected locale changes, but not when the locales are loaded
	///          for the first time.</remarks>
	private void _Fill(bool force)
	{
		if(_Disposed) { return; }

		bool first = (!_Filled);
		Locale? previous = _Selected;
		Locale[] locales = Array.Empty<Locale>();

		if(!DesignMode)
		{
			locales = Locale.Locales.ToArray();

			Locale? wanted = ((_AutoApply || (_Selected is null)) ? Locale.Current : _Selected);
			_Selected = ((wanted is null) ? null : (locales.FirstOrDefault(m => m == wanted) ?? wanted));
			_Filled = true;
		}
		else { _Selected = null; }

		if((_Selected is not null) && (!locales.Contains(_Selected))) { locales = [.. locales, _Selected]; }

		if(force || (!locales.SequenceEqual(_Locales, ReferenceEqualityComparer.Instance)))
		{
			Image[] old = _Images.ToArray();
			_Images.Clear();
			_Locales = locales;

			_Filling = true;
			try
			{
				if(_Combo is not null) { _FillCombo(); }
				if(_Menu is not null) { _FillMenu(); }
			}
			finally { _Filling = false; }

			foreach(Image i in old) { i.Dispose(); }
		}

		_ShowSelection();
		if((!first) && (previous != _Selected)) { _OnSelectedLocaleChanged(EventArgs.Empty); }
	}


	/// <summary>Fills the drop-down list.</summary>
	private void _FillCombo()
	{
		List<object> items = new();

		int size = LogicalToDeviceUnits(_ITEM_IMAGE_SIZE);
		foreach(Locale i in _Locales) { items.Add(new _LocaleItem(i.Name, _Own(i.CreateImage(size) ?? _LoadImage("sunk.png")), i)); }
		if(items.Count == 0) { items.Add(new _LocaleItem((DesignMode ? _DESIGN_TEXT : _EmptyText), _Own(_LoadImage("sunk.png")), null)); }

		_Combo!.Items.Clear();
		_Combo.Items.AddRange(items);
	}


	/// <summary>Fills the button menu.</summary>
	private void _FillMenu()
	{
		_ClearMenu();

		int size = LogicalToDeviceUnits(_ITEM_IMAGE_SIZE);
		_Menu!.ImageScalingSize = new Size(size, size);
		foreach(Locale i in _Locales)
		{
			Image image = _Own(i.CreateImage(size) ?? _LoadImage("sunk.png"));
			_Menu!.Items.Add(new ToolStripMenuItem(i.Name, image, _MenuItem_Click) { Tag = i });
		}
	}


	/// <summary>Shows the selected locale.</summary>
	private void _ShowSelection()
	{
		_Filling = true;
		try
		{
			if(_Combo is not null)
			{
				int index = ((_Selected is null) ? -1 : Array.IndexOf(_Locales, _Selected));
				if((_Locales.Length == 0) && (_Combo.Items.Count > 0)) { index = 0; }
				if(_Combo.SelectedIndex != index) { _Combo.SelectedIndex = index; }
			}

			if(_Button is not null)
			{
				int size = LogicalToDeviceUnits(_BUTTON_IMAGE_SIZE);
				Image? old = _Button.Image;

				if(_Selected?.CreateImage(size) is Image image)
				{
					_Button.Image = _Own(image);
				}
				else
				{
					using Image source = _LoadImage("lunk.png");
					_Button.Image = _Own(LocaleImageExtensions._CreateScaledImage(source, size));
				}
				if(old is not null)
				{
					_Images.Remove(old);
					old.Dispose();
				}

				string text = (DesignMode ? _DESIGN_TEXT : (_Selected?.Name ?? ((_Locales.Length == 0) ? _EmptyText : string.Empty)));
				_Button.AccessibleName = text;
				_ToolTip.SetToolTip(_Button, text);
			}

			if(_Menu is not null)
			{
				foreach(ToolStripMenuItem i in _Menu.Items.OfType<ToolStripMenuItem>()) { i.Checked = ((i.Tag is Locale l) && (l == _Selected)); }
			}
		}
		finally { _Filling = false; }
	}


	/// <summary>Selects a locale.</summary>
	/// <param name="value">Locale.</param>
	private void _Select(Locale? value)
	{
		if(value == _Selected) { return; }

		_Selected = value;
		_ShowSelection();
		_OnSelectedLocaleChanged(EventArgs.Empty);

		if(_AutoApply && (value is not null) && (!DesignMode)) { Locale.Current = value; }

		if((!DesignMode) && _Locales.Any(m => ((m != value) && (!Locale.Locales.Contains(m)))))
		{
			if(IsHandleCreated) { BeginInvoke(() => { if(!_Disposed) { _Fill(false); } }); }
			else { _Fill(false); }
		}
	}


	/// <summary>Loads the locales if they have not been loaded yet.</summary>
	private void _EnsureFilled()
	{
		if((!_Filled) && (!DesignMode)) { _Fill(true); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// protected methods                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Raises the <see cref="SelectedLocaleChanged"/> event.</summary>
	/// <param name="e">Event arguments.</param>
	protected virtual void _OnSelectedLocaleChanged(EventArgs e)
	{
		SelectedLocaleChanged?.Invoke(this, e);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public events                                                                                                    //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Occurs when the selected locale has changed.</summary>
	[Category("Behavior")][Description("Occurs when the selected locale has changed.")]
	public event EventHandler? SelectedLocaleChanged;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the style.</summary>
	[Category("Appearance")][Description("The style of the selector: a drop-down list, or a button with or without a drop-down arrow that opens a menu.")]
	[DefaultValue(LocaleSelectorStyle.DROPDOWN)]
	public LocaleSelectorStyle Style
	{
		get { return _Style; }
		set
		{
			if(value == _Style) { return; }

			_Style = value;
			_CreateElements();
		}
	}


	/// <summary>Gets the color table.</summary>
	/// <remarks>The table defines a theme and optional overrides of single colors. The
	///          <see cref="LocaleSelectorStyle.BUTTON"/> and <see cref="LocaleSelectorStyle.ARROW_BUTTON"/> styles draw their
	///          background with <see cref="Control.BackColor"/> and use the table for the hot, pressed and focus states, the arrow
	///          and the menu.</remarks>
	[Category("Appearance")][Description("The colors of the control: a theme and optional overrides of single colors.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public LocaleSelectorColors Colors
	{
		get { return _Colors; }
	}


	/// <summary>Gets or sets if the selected locale is applied as current locale.</summary>
	/// <remarks>If set, selecting a locale sets <see cref="Locale.Current"/>, and the selector follows the current locale when it
	///          is changed elsewhere.</remarks>
	[Category("Behavior")][Description("Specifies if the selected locale becomes the current locale.")][DefaultValue(true)]
	public bool AutoApply
	{
		get { return _AutoApply; }
		set { _AutoApply = value; }
	}


	/// <summary>Gets or sets the text displayed when there are no locales.</summary>
	[Category("Appearance")][Description("The text displayed when there are no locales.")][DefaultValue(_DEFAULT_EMPTY_TEXT)]
	[AllowNull]
	public string EmptyText
	{
		get { return _EmptyText; }
		set
		{
			if((value ?? string.Empty) == _EmptyText) { return; }

			_EmptyText = (value ?? string.Empty);
			if(_Filled) { _Fill(true); }
		}
	}


	/// <summary>Gets or sets the selected locale.</summary>
	/// <remarks>If <see cref="AutoApply"/> is set, setting a locale also sets <see cref="Locale.Current"/>.</remarks>
	[Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Locale? SelectedLocale
	{
		get
		{
			_EnsureFilled();
			return _Selected;
		}
		set
		{
			_EnsureFilled();
			_Select((value is null) ? null : (_Locales.FirstOrDefault(m => m == value) ?? value));
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Resets the color table.</summary>
	public void ResetColors()
	{
		_Colors.Reset();
	}


	/// <summary>Determines if the color table has to be serialized by the designer.</summary>
	/// <returns>Returns <c>true</c> if the color table differs from the default, otherwise returns <c>false</c>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColors()
	{
		return (!_Colors.IsDefault);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Handles the Changed event of the color table.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Colors_Changed(object? sender, EventArgs e)
	{
		if(_Combo is not null) { _Colors._ApplyTo(_Combo.Colors); }
		_Button?.Invalidate();
		_Menu?.Invalidate();
	}


	/// <summary>Handles the LocaleChanged event of <see cref="Locale"/>.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>The event may be raised on any thread; the update is marshaled to the thread of the control.</remarks>
	private void _Locale_LocaleChanged(object? sender, EventArgs e)
	{
		if(_Disposed || DesignMode) { return; }

		if(IsHandleCreated && InvokeRequired)
		{
			BeginInvoke(() => { if(!_Disposed) { _Fill(false); } });
		}
		else if(_Filled || IsHandleCreated) { _Fill(false); }
	}


	/// <summary>Handles the SelectionChangeCommitted event of the drop-down list.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Combo_SelectionChangeCommitted(object? sender, EventArgs e)
	{
		if((!_Filling) && (_Combo?.SelectedItem is _LocaleItem { Locale: not null } item)) { _Select(item.Locale); }
	}


	/// <summary>Handles the SizeChanged event of the drop-down list.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Combo_SizeChanged(object? sender, EventArgs e)
	{
		if((_Combo is not null) && (Height != _Combo.Height)) { Height = _Combo.Height; }
	}


	/// <summary>Handles the Click event of the button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Button_Click(object? sender, EventArgs e)
	{
		if(_IgnoreClick)
		{
			_IgnoreClick = false;
			return;
		}
		if((_Button is null) || (_Menu is null) || (_Menu.Items.Count == 0)) { return; }

		_Button.Pressed = true;
		_Menu.Show(_Button, new Point(0, _Button.Height));
	}


	/// <summary>Handles the MouseUp event of the button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>The click is raised before this event, so a click that has not been ignored does not affect the next one.</remarks>
	private void _Button_MouseUp(object? sender, MouseEventArgs e)
	{
		_IgnoreClick = false;
	}


	/// <summary>Handles the Closing event of the menu.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>If the menu is closed by a mouse click on the button, the following click on the button must not open it
	///          again.</remarks>
	private void _Menu_Closing(object? sender, ToolStripDropDownClosingEventArgs e)
	{
		_IgnoreClick = ((e.CloseReason == ToolStripDropDownCloseReason.AppClicked) && (_Button is not null) &&
						_Button.ClientRectangle.Contains(_Button.PointToClient(Cursor.Position)));
	}


	/// <summary>Handles the Closed event of the menu.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Menu_Closed(object? sender, ToolStripDropDownClosedEventArgs e)
	{
		if(_Button is not null) { _Button.Pressed = false; }
	}


	/// <summary>Handles the Click event of the menu items.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuItem_Click(object? sender, EventArgs e)
	{
		if(sender is ToolStripItem { Tag: Locale l }) { _Select(l); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IThemedControl                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the color table of the control.</summary>
	ControlColorTable IThemedControl.ThemeColors
	{
		get { return _Colors; }
	}


	/// <summary>Gets if the theme applies to the child controls.</summary>
	bool IThemedControl.ThemesChildren
	{
		get { return false; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Control                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	protected override Size DefaultSize
	{
		get { return new Size(_DEFAULT_WIDTH, 24); }
	}


	/// <inheritdoc/>
	/// <remarks>The height of the drop-down style follows the drop-down list; the button styles have a fixed size.</remarks>
	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		if(_Style != LocaleSelectorStyle.DROPDOWN)
		{
			Size size = _GetButtonSize();
			width = size.Width;
			height = size.Height;
		}
		else if(_Combo is not null) { height = _Combo.Height; }

		base.SetBoundsCore(x, y, width, height, specified);
	}


	/// <inheritdoc/>
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if(!_Filled) { _Fill(true); }
	}


	/// <inheritdoc/>
	protected override void OnDpiChangedAfterParent(EventArgs e)
	{
		base.OnDpiChangedAfterParent(e);

		_UpdateSize();
		if(_Filled) { _Fill(true); }
	}


	/// <inheritdoc/>
	/// <remarks>The <see cref="LocaleSelectorStyle.BUTTON"/> and <see cref="LocaleSelectorStyle.ARROW_BUTTON"/> styles draw their
	///          background with the back color.</remarks>
	protected override void OnBackColorChanged(EventArgs e)
	{
		base.OnBackColorChanged(e);
		_Button?.Invalidate();
	}


	/// <inheritdoc/>
	protected override void OnFontChanged(EventArgs e)
	{
		base.OnFontChanged(e);
		_UpdateSize();
	}


	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if(disposing && (!_Disposed))
		{
			_Disposed = true;
			Locale.LocaleChanged -= _Locale_LocaleChanged;
			_Colors.Changed -= _Colors_Changed;

			_ClearMenu();
			_Menu?.Dispose();
			_ToolTip.Dispose();
			if(_Button is not null) { _Button.Image = null; }

			foreach(Image i in _Images) { i.Dispose(); }
			_Images.Clear();
		}

		base.Dispose(disposing);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [class] _LocaleItem                                                                                              //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This class implements the drop-down list item of a locale.</summary>
	[SupportedOSPlatform("windows")]
	private sealed class _LocaleItem: IListItem
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// constructors                                                                                                 //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Creates a new instance of this class.</summary>
		/// <param name="text">Text.</param>
		/// <param name="image">Image.</param>
		/// <param name="locale">Locale, NULL for a placeholder.</param>
		public _LocaleItem(string text, Image image, Locale? locale)
		{
			Text = text;
			Image = image;
			Locale = locale;
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public properties                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets the locale, NULL for a placeholder.</summary>
		public Locale? Locale
		{
			get;
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// [interface] IListItem                                                                                        //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <inheritdoc/>
		public string Text
		{
			get;
		}


		/// <inheritdoc/>
		public string? FilterText
		{
			get { return null; }
		}


		/// <inheritdoc/>
		public string? SortText
		{
			get { return null; }
		}


		/// <inheritdoc/>
		public string? Detail
		{
			get { return null; }
		}


		/// <inheritdoc/>
		public string? Description
		{
			get { return null; }
		}


		/// <inheritdoc/>
		public string? Category
		{
			get { return null; }
		}


		/// <inheritdoc/>
		public Image? Image
		{
			get;
		}


		/// <inheritdoc/>
		public string? ImageKey
		{
			get { return null; }
		}


		/// <inheritdoc/>
		public FontStyle FontStyle
		{
			get { return FontStyle.Regular; }
		}


		/// <inheritdoc/>
		public Color ForeColor
		{
			get { return Color.Empty; }
		}


		/// <inheritdoc/>
		public object? Tag
		{
			get { return Locale; }
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// [override] object                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Returns a string representation of this instance.</summary>
		/// <returns>Returns the text.</returns>
		public override string ToString()
		{
			return Text;
		}
	}
}
