namespace Robbiblubber.Localization.Controls;

using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;

using Slot = Robbiblubber.Localization.Controls.LocaleSelectorColors._Slot;



public partial class LocaleSelector
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [class] _SelectorButton                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This class implements the self-drawn button of the button styles.</summary>
	/// <remarks>The button shows an image, in the <see cref="LocaleSelectorStyle.ARROW_BUTTON"/> style followed by a drop-down
	///          arrow and in the <see cref="LocaleSelectorStyle.DROPDOWN_BUTTON"/> style followed by a box with a drop-down arrow.
	///          It is operated with the mouse or the keyboard (Space, Enter, Down, F4).</remarks>
	[SupportedOSPlatform("windows")]
	private sealed class _SelectorButton: Control
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private constants                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Width of the arrow box in logical pixels.</summary>
		private const int _ARROW_BOX_WIDTH = 16;

		/// <summary>Width of the arrow area without a box in logical pixels.</summary>
		private const int _ARROW_WIDTH = 12;

		/// <summary>Half width of the arrow in logical pixels.</summary>
		private const int _ARROW_SIZE = 4;



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private members                                                                                              //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Selector.</summary>
		private readonly LocaleSelector _Owner;

		/// <summary>Image.</summary>
		private Image? _Image = null;

		/// <summary>Determines if the mouse is over the button.</summary>
		private bool _Hot = false;

		/// <summary>Determines if the menu is open.</summary>
		private bool _Pressed = false;



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// constructors                                                                                                 //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Creates a new instance of this class.</summary>
		/// <param name="owner">Selector.</param>
		/// <param name="style">Button style.</param>
		public _SelectorButton(LocaleSelector owner, LocaleSelectorStyle style): base()
		{
			_Owner = owner;
			Style = style;

			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
					 ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
			TabStop = true;
			AccessibleRole = AccessibleRole.ButtonDropDown;
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private static methods                                                                                       //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Draws the drop-down arrow.</summary>
		/// <param name="g">Graphics.</param>
		/// <param name="bounds">Arrow box bounds.</param>
		/// <param name="color">Arrow color.</param>
		/// <param name="size">Half width of the arrow in device pixels.</param>
		/// <param name="width">Pen width in device pixels.</param>
		private static void _DrawArrow(Graphics g, Rectangle bounds, Color color, float size, float width)
		{
			float cx = bounds.Left + (bounds.Width / 2f);
			float cy = bounds.Top + (bounds.Height / 2f);
			SmoothingMode mode = g.SmoothingMode;

			g.SmoothingMode = SmoothingMode.AntiAlias;
			using(Pen pen = new(color, width) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round })
			{
				g.DrawLines(pen, new PointF[] { new(cx - size, cy - (size / 2f)), new(cx, cy + (size / 2f)), new(cx + size, cy - (size / 2f)) });
			}
			g.SmoothingMode = mode;
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// internal static methods                                                                                      //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets the width of the arrow area of a style.</summary>
		/// <param name="style">Style.</param>
		/// <returns>Returns the width in logical pixels, 0 if the style shows no arrow.</returns>
		internal static int _GetArrowWidth(LocaleSelectorStyle style)
		{
			return style switch
			{
				LocaleSelectorStyle.DROPDOWN_BUTTON => _ARROW_BOX_WIDTH,
				LocaleSelectorStyle.ARROW_BUTTON => _ARROW_WIDTH,
				_ => 0
			};
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private methods                                                                                              //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets the effective color of a slot.</summary>
		/// <param name="slot">Color slot.</param>
		/// <returns>Returns the color.</returns>
		private Color _Color(Slot slot)
		{
			return _Owner._Colors._Get(slot);
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public properties                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets the button style.</summary>
		public LocaleSelectorStyle Style
		{
			get;
		}


		/// <summary>Gets or sets the image.</summary>
		/// <remarks>The image is owned by the selector.</remarks>
		[Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Image? Image
		{
			get { return _Image; }
			set
			{
				_Image = value;
				Invalidate();
			}
		}


		/// <summary>Gets or sets if the menu is open.</summary>
		[Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Pressed
		{
			get { return _Pressed; }
			set
			{
				if(value == _Pressed) { return; }

				_Pressed = value;
				Invalidate();
			}
		}


		/// <summary>Gets the bounds of the arrow area, empty if the button shows no arrow.</summary>
		public Rectangle ArrowBounds
		{
			get
			{
				int width = _GetArrowWidth(Style);
				if(width == 0) { return Rectangle.Empty; }

				width = Math.Min(LogicalToDeviceUnits(width), Math.Max(0, Width - 2));
				return new Rectangle(Width - 1 - width, 1, width, Math.Max(0, Height - 2));
			}
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public methods                                                                                               //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Opens the menu as if the button was clicked.</summary>
		public void PerformClick()
		{
			if(Enabled) { OnClick(EventArgs.Empty); }
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// [override] Control                                                                                           //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <inheritdoc/>
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new _ButtonAccessibleObject(this);
		}


		/// <inheritdoc/>
		protected override bool IsInputKey(Keys keyData)
		{
			return keyData switch
			{
				Keys.Enter or Keys.Space or Keys.Down or Keys.F4 => true,
				_ => base.IsInputKey(keyData)
			};
		}


		/// <inheritdoc/>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if(e.Handled || (e.Modifiers != Keys.None)) { return; }

			if((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Space) || (e.KeyCode == Keys.Down) || (e.KeyCode == Keys.F4))
			{
				PerformClick();
				e.Handled = true;
			}
		}


		/// <inheritdoc/>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if((e.Button == MouseButtons.Left) && CanFocus) { Focus(); }
			base.OnMouseDown(e);
		}


		/// <inheritdoc/>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			_Hot = true;
			Invalidate();
		}


		/// <inheritdoc/>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			_Hot = false;
			Invalidate();
		}


		/// <inheritdoc/>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Invalidate();
		}


		/// <inheritdoc/>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();
		}


		/// <inheritdoc/>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}


		/// <inheritdoc/>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			bool enabled = Enabled;
			bool focused = (enabled && Focused);
			Rectangle arrow = ArrowBounds;

			bool boxed = (Style == LocaleSelectorStyle.DROPDOWN_BUTTON);
			Color back;
			Color? border = null;
			if(boxed)
			{
				back = ((!enabled) ? _Color(Slot.DISABLED_BACK_COLOR) : (_Pressed ? _Color(Slot.PRESSED_BACK_COLOR) :
																	   (_Hot ? _Color(Slot.HOT_BACK_COLOR) : _Color(Slot.BACK_COLOR))));
				border = ((!enabled) ? _Color(Slot.DISABLED_BORDER_COLOR) : (focused ? _Color(Slot.FOCUS_COLOR) :
																		  ((_Hot || _Pressed) ? _Color(Slot.HOT_BORDER_COLOR) : _Color(Slot.BORDER_COLOR))));
			}
			else
			{
				back = (((!enabled) || ((!_Hot) && (!_Pressed))) ? _Owner.BackColor : (_Pressed ? _Color(Slot.PRESSED_BACK_COLOR) : _Color(Slot.HOT_BACK_COLOR)));
				if(enabled && (_Hot || _Pressed)) { border = _Color(Slot.HOT_BORDER_COLOR); }
			}

			if(back.A < 255) { base.OnPaintBackground(e); }
			using(SolidBrush brush = new(back)) { g.FillRectangle(brush, ClientRectangle); }

			if(!arrow.IsEmpty)
			{
				if(boxed)
				{
					Color box = ((enabled && (_Hot || _Pressed)) ? _Color(Slot.BUTTON_HOT_BACK_COLOR) : _Color(Slot.BUTTON_BACK_COLOR));
					using SolidBrush brush = new(box);
					g.FillRectangle(brush, arrow);
				}

				_DrawArrow(g, arrow, (enabled ? _Color(Slot.BUTTON_FORE_COLOR) : _Color(Slot.DISABLED_FORE_COLOR)),
						   LogicalToDeviceUnits(_ARROW_SIZE), Math.Max(1f, LogicalToDeviceUnits(100) / 75f));
			}

			if(_Image is not null)
			{
				int right = (arrow.IsEmpty ? Width : arrow.Left);
				int size = Math.Min(_Image.Width, Math.Min(right, Height));
				Rectangle bounds = new((right - size) / 2, (Height - size) / 2, size, size);

				if(enabled)
				{
					g.DrawImage(_Image, bounds);
				}
				else { ControlPaint.DrawImageDisabled(g, _Image, bounds.X, bounds.Y, back); }
			}

			if(border is not null)
			{
				using Pen pen = new(border.Value);
				g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
			}

			if(focused && (!boxed) && ShowFocusCues)
			{
				int inset = LogicalToDeviceUnits(2);
				ControlPaint.DrawFocusRectangle(g, Rectangle.Inflate(ClientRectangle, -inset, -inset), _Color(Slot.FORE_COLOR), back);
			}
		}



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// [class] _ButtonAccessibleObject                                                                              //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>This class provides the accessibility information of the button.</summary>
		[SupportedOSPlatform("windows")]
		private sealed class _ButtonAccessibleObject: ControlAccessibleObject
		{
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////
			// constructors                                                                                             //
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////

			/// <summary>Creates a new instance of this class.</summary>
			/// <param name="owner">Button.</param>
			public _ButtonAccessibleObject(_SelectorButton owner): base(owner)
			{}



			//////////////////////////////////////////////////////////////////////////////////////////////////////////////
			// [override] AccessibleObject                                                                              //
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////

			/// <inheritdoc/>
			public override string DefaultAction
			{
				get { return "Open"; }
			}


			/// <inheritdoc/>
			public override AccessibleStates State
			{
				get
				{
					return (base.State | (((_SelectorButton) Owner!).Pressed ? AccessibleStates.Expanded : AccessibleStates.Collapsed));
				}
			}


			/// <inheritdoc/>
			public override void DoDefaultAction()
			{
				((_SelectorButton) Owner!).PerformClick();
			}
		}
	}
}
