namespace Robbiblubber.Localization.Controls;

using System.ComponentModel;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class implements the color table of a <see cref="LocaleSelector"/>.</summary>
/// <remarks><para>The table consists of a <see cref="ControlColorTable.Theme"/> and optional overrides of single colors. A color
///          that is <see cref="Color.Empty"/> is taken from the theme, as in the color tables of Spellbook.Controls:
///          <c>selector.Colors.Theme = Theme.DARK; selector.Colors.FocusColor = Color.Orange;</c> Without a theme of its own, the
///          table uses the theme applied to the selector by the theme manager, or the theme of the application.</para>
///          <para>The theme colors match those of <see cref="ThemedComboBoxColors"/>. In the
///          <see cref="LocaleSelectorStyle.DROPDOWN"/> style, the table is applied to the drop-down list. The
///          <see cref="LocaleSelectorStyle.DROPDOWN_BUTTON"/> style draws the button and the arrow box with the colors of the
///          table. The <see cref="LocaleSelectorStyle.BUTTON"/> and <see cref="LocaleSelectorStyle.ARROW_BUTTON"/> styles draw
///          their background with the back color of the selector and use the table for the hot, pressed and focus states and
///          the arrow. The menu of the button styles uses the drop-down and selection colors.</para></remarks>
[SupportedOSPlatform("windows")]
public sealed class LocaleSelectorColors: ControlColorTable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Theme colors of the slots.</summary>
	private static readonly ThemeColor[] _ThemeColors = new ThemeColor[]
	{
		ThemeColor.FIELD_BACK,
		ThemeColor.FIELD_TEXT,
		ThemeColor.FIELD_BORDER,
		ThemeColor.FIELD_BORDER_HOT,
		ThemeColor.FIELD_BORDER_FOCUSED,
		ThemeColor.GLYPH,
		ThemeColor.BUTTON_BACK,
		ThemeColor.HOT_BACK,
		ThemeColor.SELECTION_BACK,
		ThemeColor.SELECTION_TEXT,
		ThemeColor.POPUP_BACK,
		ThemeColor.POPUP_TEXT,
		ThemeColor.POPUP_BORDER,
		ThemeColor.DISABLED_BACK,
		ThemeColor.DISABLED_TEXT,
		ThemeColor.DISABLED_BORDER,
		ThemeColor.BUTTON_BACK_HOT,
		ThemeColor.BUTTON_BACK_PRESSED
	};



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public LocaleSelectorColors(): base(18)
	{}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// protected methods                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	protected override Color _GetThemeColor(Theme theme, int slot)
	{
		return theme[_ThemeColors[slot]];
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal methods                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the effective color of a slot.</summary>
	/// <param name="slot">Color slot.</param>
	/// <returns>Returns the color override if it is set, otherwise the color of the theme.</returns>
	internal Color _Get(_Slot slot)
	{
		return _GetEffective((int) slot);
	}



	/// <summary>Applies the effective theme and the color overrides to the color table of a drop-down list.</summary>
	/// <param name="colors">Color table of the drop-down list.</param>
	internal void _ApplyTo(ThemedComboBoxColors colors)
	{
		colors.Reset();
		colors.Theme = ((Theme is null) && (EffectiveTheme == ThemeManager.Theme) ? null : EffectiveTheme);
		colors.BackColor = BackColor;
		colors.ForeColor = ForeColor;
		colors.BorderColor = BorderColor;
		colors.HotBorderColor = HotBorderColor;
		colors.FocusColor = FocusColor;
		colors.ButtonForeColor = ButtonForeColor;
		colors.ButtonHotBackColor = ButtonHotBackColor;
		colors.SelectionBackColor = SelectionBackColor;
		colors.SelectionForeColor = SelectionForeColor;
		colors.DropDownBackColor = DropDownBackColor;
		colors.DropDownForeColor = DropDownForeColor;
		colors.DropDownBorderColor = DropDownBorderColor;
		colors.DisabledBackColor = DisabledBackColor;
		colors.DisabledForeColor = DisabledForeColor;
		colors.DisabledBorderColor = DisabledBorderColor;
		colors.HotBackColor = HotBackColor;
		colors.PressedBackColor = PressedBackColor;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the back color of the control.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the drop-down list and of the drop-down button style. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color BackColor
	{
		get { return _GetValue((int) _Slot.BACK_COLOR); }
		set { _SetValue((int) _Slot.BACK_COLOR, value); }
	}


	/// <summary>Gets or sets the text color of the control.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The text color of the drop-down list. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color ForeColor
	{
		get { return _GetValue((int) _Slot.FORE_COLOR); }
		set { _SetValue((int) _Slot.FORE_COLOR, value); }
	}


	/// <summary>Gets or sets the border color of the control.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The border color of the control. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color BorderColor
	{
		get { return _GetValue((int) _Slot.BORDER_COLOR); }
		set { _SetValue((int) _Slot.BORDER_COLOR, value); }
	}


	/// <summary>Gets or sets the border color of the control while the mouse is over it.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The border color of the control while the mouse is over it. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color HotBorderColor
	{
		get { return _GetValue((int) _Slot.HOT_BORDER_COLOR); }
		set { _SetValue((int) _Slot.HOT_BORDER_COLOR, value); }
	}


	/// <summary>Gets or sets the color that marks the focused control.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The color that marks the focused control. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color FocusColor
	{
		get { return _GetValue((int) _Slot.FOCUS_COLOR); }
		set { _SetValue((int) _Slot.FOCUS_COLOR, value); }
	}


	/// <summary>Gets or sets the color of the drop-down arrow.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The color of the drop-down arrow. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color ButtonForeColor
	{
		get { return _GetValue((int) _Slot.BUTTON_FORE_COLOR); }
		set { _SetValue((int) _Slot.BUTTON_FORE_COLOR, value); }
	}


	/// <summary>Gets or sets the back color of the drop-down arrow box of the drop-down button style.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the drop-down arrow box of the drop-down button style. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color ButtonBackColor
	{
		get { return _GetValue((int) _Slot.BUTTON_BACK_COLOR); }
		set { _SetValue((int) _Slot.BUTTON_BACK_COLOR, value); }
	}


	/// <summary>Gets or sets the back color of the drop-down arrow while the mouse is over it or the list is open.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the drop-down arrow while the mouse is over it or the list or menu is open. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color ButtonHotBackColor
	{
		get { return _GetValue((int) _Slot.BUTTON_HOT_BACK_COLOR); }
		set { _SetValue((int) _Slot.BUTTON_HOT_BACK_COLOR, value); }
	}


	/// <summary>Gets or sets the back color of the selected item in the list or menu.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the selected item in the list or menu. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color SelectionBackColor
	{
		get { return _GetValue((int) _Slot.SELECTION_BACK_COLOR); }
		set { _SetValue((int) _Slot.SELECTION_BACK_COLOR, value); }
	}


	/// <summary>Gets or sets the text color of the selected item in the list or menu.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The text color of the selected item in the list or menu. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color SelectionForeColor
	{
		get { return _GetValue((int) _Slot.SELECTION_FORE_COLOR); }
		set { _SetValue((int) _Slot.SELECTION_FORE_COLOR, value); }
	}


	/// <summary>Gets or sets the back color of the list or menu.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the list or menu. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color DropDownBackColor
	{
		get { return _GetValue((int) _Slot.DROP_DOWN_BACK_COLOR); }
		set { _SetValue((int) _Slot.DROP_DOWN_BACK_COLOR, value); }
	}


	/// <summary>Gets or sets the text color of the list or menu.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The text color of the list or menu. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color DropDownForeColor
	{
		get { return _GetValue((int) _Slot.DROP_DOWN_FORE_COLOR); }
		set { _SetValue((int) _Slot.DROP_DOWN_FORE_COLOR, value); }
	}


	/// <summary>Gets or sets the border color of the list or menu.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The border color of the list or menu. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color DropDownBorderColor
	{
		get { return _GetValue((int) _Slot.DROP_DOWN_BORDER_COLOR); }
		set { _SetValue((int) _Slot.DROP_DOWN_BORDER_COLOR, value); }
	}


	/// <summary>Gets or sets the back color of the control when it is disabled.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the control when it is disabled. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color DisabledBackColor
	{
		get { return _GetValue((int) _Slot.DISABLED_BACK_COLOR); }
		set { _SetValue((int) _Slot.DISABLED_BACK_COLOR, value); }
	}


	/// <summary>Gets or sets the text and arrow color of the control when it is disabled.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The text and arrow color of the control when it is disabled. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color DisabledForeColor
	{
		get { return _GetValue((int) _Slot.DISABLED_FORE_COLOR); }
		set { _SetValue((int) _Slot.DISABLED_FORE_COLOR, value); }
	}


	/// <summary>Gets or sets the border color of the control when it is disabled.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The border color of the control when it is disabled. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color DisabledBorderColor
	{
		get { return _GetValue((int) _Slot.DISABLED_BORDER_COLOR); }
		set { _SetValue((int) _Slot.DISABLED_BORDER_COLOR, value); }
	}


	/// <summary>Gets or sets the back color of the control while the mouse is over it.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the control while the mouse is over it. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color HotBackColor
	{
		get { return _GetValue((int) _Slot.HOT_BACK_COLOR); }
		set { _SetValue((int) _Slot.HOT_BACK_COLOR, value); }
	}


	/// <summary>Gets or sets the back color of the control while the list or menu is open.</summary>
	/// <remarks>If the color is <see cref="Color.Empty"/>, the color of the <see cref="ControlColorTable.Theme"/> is used.</remarks>
	[Category("Appearance")][Description("The back color of the control while the list or menu is open. Empty uses the color of the theme.")]
	[DefaultValue(typeof(Color), "")][NotifyParentProperty(true)]
	public Color PressedBackColor
	{
		get { return _GetValue((int) _Slot.PRESSED_BACK_COLOR); }
		set { _SetValue((int) _Slot.PRESSED_BACK_COLOR, value); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [enum] _Slot                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This enumeration defines the color slots.</summary>
	internal enum _Slot: int
	{
		/// <summary>Back color of the control.</summary>
		BACK_COLOR = 0,

		/// <summary>Text color of the control.</summary>
		FORE_COLOR = 1,

		/// <summary>Border color of the control.</summary>
		BORDER_COLOR = 2,

		/// <summary>Border color of the control while the mouse is over it.</summary>
		HOT_BORDER_COLOR = 3,

		/// <summary>Color that marks the focused control.</summary>
		FOCUS_COLOR = 4,

		/// <summary>Color of the drop-down arrow.</summary>
		BUTTON_FORE_COLOR = 5,

		/// <summary>Back color of the drop-down arrow box of the drop-down button style.</summary>
		BUTTON_BACK_COLOR = 6,

		/// <summary>Back color of the drop-down arrow while the mouse is over it or the list is open.</summary>
		BUTTON_HOT_BACK_COLOR = 7,

		/// <summary>Back color of the selected item in the list or menu.</summary>
		SELECTION_BACK_COLOR = 8,

		/// <summary>Text color of the selected item in the list or menu.</summary>
		SELECTION_FORE_COLOR = 9,

		/// <summary>Back color of the list or menu.</summary>
		DROP_DOWN_BACK_COLOR = 10,

		/// <summary>Text color of the list or menu.</summary>
		DROP_DOWN_FORE_COLOR = 11,

		/// <summary>Border color of the list or menu.</summary>
		DROP_DOWN_BORDER_COLOR = 12,

		/// <summary>Back color of the control when it is disabled.</summary>
		DISABLED_BACK_COLOR = 13,

		/// <summary>Text and arrow color of the control when it is disabled.</summary>
		DISABLED_FORE_COLOR = 14,

		/// <summary>Border color of the control when it is disabled.</summary>
		DISABLED_BORDER_COLOR = 15,

		/// <summary>Back color of the control while the mouse is over it.</summary>
		HOT_BACK_COLOR = 16,

		/// <summary>Back color of the control while the list or menu is open.</summary>
		PRESSED_BACK_COLOR = 17
	}
}
