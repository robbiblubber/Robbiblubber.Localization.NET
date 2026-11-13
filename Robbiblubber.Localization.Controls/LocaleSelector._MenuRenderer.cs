namespace Robbiblubber.Localization.Controls;

using System.Runtime.Versioning;

using Slot = Robbiblubber.Localization.Controls.LocaleSelectorColors._Slot;



public partial class LocaleSelector
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [class] _MenuRenderer                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This class renders the menu of the button styles with the colors of the selector.</summary>
	[SupportedOSPlatform("windows")]
	private sealed class _MenuRenderer: ToolStripProfessionalRenderer
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private members                                                                                              //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Selector.</summary>
		private readonly LocaleSelector _Owner;



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// constructors                                                                                                 //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Creates a new instance of this class.</summary>
		/// <param name="owner">Selector.</param>
		public _MenuRenderer(LocaleSelector owner): base()
		{
			_Owner = owner;
			RoundedEdges = false;
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
		// [override] ToolStripRenderer                                                                                 //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <inheritdoc/>
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			using SolidBrush brush = new(_Color(Slot.DROP_DOWN_BACK_COLOR));
			e.Graphics.FillRectangle(brush, e.AffectedBounds);
		}


		/// <inheritdoc/>
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			using Pen pen = new(_Color(Slot.DROP_DOWN_BORDER_COLOR));
			e.Graphics.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
		}


		/// <inheritdoc/>
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			using SolidBrush brush = new(_Color(Slot.DROP_DOWN_BACK_COLOR));
			e.Graphics.FillRectangle(brush, e.AffectedBounds);
		}


		/// <inheritdoc/>
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			if(!(e.Item.Selected && e.Item.Enabled)) { return; }

			Rectangle bounds = new(2, 0, e.Item.Width - 4, e.Item.Height);
			using SolidBrush brush = new(_Color(Slot.SELECTION_BACK_COLOR));
			e.Graphics.FillRectangle(brush, bounds);
		}


		/// <inheritdoc/>
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			e.TextColor = ((!e.Item.Enabled) ? _Color(Slot.DISABLED_FORE_COLOR) :
						   (e.Item.Selected ? _Color(Slot.SELECTION_FORE_COLOR) : _Color(Slot.DROP_DOWN_FORE_COLOR)));
			base.OnRenderItemText(e);
		}


		/// <inheritdoc/>
		/// <remarks>The current locale is marked with a frame in the focus color around its image.</remarks>
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			Rectangle bounds = Rectangle.Inflate(e.ImageRectangle, 2, 2);

			using(Pen pen = new(_Color(Slot.FOCUS_COLOR))) { e.Graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1); }
			if(e.Image is not null) { e.Graphics.DrawImage(e.Image, e.ImageRectangle); }
		}
	}
}
