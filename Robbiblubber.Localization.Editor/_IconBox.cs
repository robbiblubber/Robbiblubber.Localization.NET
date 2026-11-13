namespace Robbiblubber.Localization.Editor;

using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Media;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class implements a box that shows and edits the icon of a locale.</summary>
/// <remarks><para>The icon is kept as the bytes of the image file and is never converted, so it is saved as it was loaded. Images
///          from the clipboard are stored as PNG.</para>
///          <para>The box takes part in the edit commands: Cut, Copy, Paste and Delete work on the icon. A double click raises
///          <see cref="LoadRequested"/>; an image file can also be dropped onto the box.</para>
///          <para>The image is shown in its original size; larger images are scaled down to fit.</para>
///          <para>The box draws its border and its disabled background in the colors of the theme of its window.</para></remarks>
[SupportedOSPlatform("windows")]
internal sealed class _IconBox: Control, IEditable, IThemedControl
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Clipboard format for PNG data.</summary>
	private const string _PNG_FORMAT = "PNG";



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Start of a PNG file.</summary>
	private static readonly byte[] _PNG_SIGNATURE = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Image data.</summary>
	private byte[]? _Data = null;

	/// <summary>Image shown.</summary>
	private Image? _Image = null;

	/// <summary>Color table that provides the theme.</summary>
	private readonly ThemedTextBoxColors _Colors = new();



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	public _IconBox()
	{
		SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
				 ControlStyles.ResizeRedraw | ControlStyles.StandardDoubleClick, true);
		TabStop = true;
		AllowDrop = true;
		BackColor = SystemColors.Window;

		_Colors.Changed += _Colors_Changed;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public events                                                                                                    //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Occurs when the image data has been changed.</summary>
	public event EventHandler? DataChanged;


	/// <summary>Occurs when the user wants to load an image file (double click).</summary>
	public event EventHandler? LoadRequested;


	/// <summary>Occurs when the commands that can be executed have changed.</summary>
	public event EventHandler? EditStateChanged;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal static methods                                                                                          //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates an image from image data.</summary>
	/// <param name="data">Image data.</param>
	/// <returns>Returns a new image that does not depend on the data, NULL if the data is not a valid image.</returns>
	internal static Image? _CreateImage(byte[]? data)
	{
		if((data is null) || (data.Length == 0)) { return null; }

		try
		{
			using MemoryStream s = new(data, false);
			using Image i = Image.FromStream(s);
			return new Bitmap(i);
		}
		catch(Exception) { return null; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns PNG data for image data.</summary>
	/// <param name="data">Image data.</param>
	/// <param name="image">Image created from the data.</param>
	/// <returns>Returns the data itself if it is a PNG file, otherwise the image encoded as PNG.</returns>
	private static byte[] _ToPng(byte[] data, Image image)
	{
		if((data.Length >= _PNG_SIGNATURE.Length) && data.AsSpan(0, _PNG_SIGNATURE.Length).SequenceEqual(_PNG_SIGNATURE)) { return data; }

		using MemoryStream rval = new();
		image.Save(rval, ImageFormat.Png);
		return rval.ToArray();
	}


	/// <summary>Returns the image file of a data object, if it contains exactly one file.</summary>
	/// <param name="data">Data object.</param>
	/// <returns>Returns the file name, NULL if the data object does not contain exactly one file.</returns>
	private static string? _GetFile(IDataObject? data)
	{
		return (((data?.GetData(DataFormats.FileDrop) is string[] files) && (files.Length == 1) && File.Exists(files[0])) ? files[0] : null);
	}


	/// <summary>Reads image data from the clipboard.</summary>
	/// <returns>Returns the image data, NULL if the clipboard does not contain an image.</returns>
	/// <remarks>PNG data is taken as it is, so that transparency is kept; other images are encoded as PNG. A single image file on
	///          the clipboard is read as well.</remarks>
	private static byte[]? _ReadClipboard()
	{
		try
		{
			IDataObject? data = Clipboard.GetDataObject();
			if(data is null) { return null; }

			if(data.GetData(_PNG_FORMAT) is MemoryStream png) { return png.ToArray(); }

			if(_GetFile(data) is string file)
			{
				byte[] rval = File.ReadAllBytes(file);
				using Image? img = _CreateImage(rval);
				return ((img is null) ? null : rval);
			}

			if(Clipboard.GetImage() is Image image)
			{
				using(image)
				using(MemoryStream rval = new())
				{
					image.Save(rval, ImageFormat.Png);
					return rval.ToArray();
				}
			}
		}
		catch(Exception) {}

		return null;
	}


	/// <summary>Returns if the clipboard contains an image.</summary>
	/// <returns>Returns TRUE if the clipboard contains an image or a single file, otherwise returns FALSE.</returns>
	private static bool _ClipboardHasImage()
	{
		try
		{
			return (Clipboard.ContainsImage() || Clipboard.ContainsData(_PNG_FORMAT) || (Clipboard.ContainsFileDropList() && (Clipboard.GetFileDropList().Count == 1)));
		}
		catch(Exception) { return false; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets or sets the image data, NULL if there is no image.</summary>
	/// <remarks>Data that is not a valid image is kept, but not shown.</remarks>
	[Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public byte[]? Data
	{
		get { return _Data; }
		set
		{
			if(ReferenceEquals(value, _Data)) return;

			_Data = (((value is null) || (value.Length == 0)) ? null : value);
			_Image?.Dispose();
			_Image = _CreateImage(_Data);
			Invalidate();

			DataChanged?.Invoke(this, EventArgs.Empty);
			EditStateChanged?.Invoke(this, EventArgs.Empty);
		}
	}


	/// <summary>Gets the size of the image, NULL if there is no valid image.</summary>
	public Size? ImageSize
	{
		get { return _Image?.Size; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Sets the image data if it is a valid image.</summary>
	/// <param name="data">Image data.</param>
	/// <returns>Returns TRUE if the data has been set, FALSE if it is not a valid image.</returns>
	public bool SetImageData(byte[] data)
	{
		using Image? img = _CreateImage(data);
		if(img is null) { return false; }

		Data = data;
		return true;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IEditable                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns if a command can be executed.</summary>
	/// <param name="command">Edit command.</param>
	/// <returns>Returns TRUE if the command can be executed, otherwise returns FALSE.</returns>
	public bool CanExecute(EditCommand command)
	{
		return command switch
		{
			EditCommand.CUT or EditCommand.COPY or EditCommand.DELETE => (_Data is not null),
			EditCommand.PASTE => _ClipboardHasImage(),
			_ => false
		};
	}


	/// <summary>Executes a command.</summary>
	/// <param name="command">Edit command.</param>
	public void Execute(EditCommand command)
	{
		switch(command)
		{
			case EditCommand.CUT:
				if(_Copy()) { Data = null; }
				break;
			case EditCommand.COPY:
				_Copy(); break;
			case EditCommand.PASTE:
				if(_ReadClipboard() is byte[] data) { SetImageData(data); }
				break;
			case EditCommand.DELETE:
				Data = null; break;
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Copies the image to the clipboard, as image and as PNG.</summary>
	/// <returns>Returns TRUE if the image has been copied, otherwise returns FALSE.</returns>
	private bool _Copy()
	{
		if((_Data is null) || (_Image is null)) { return false; }

		try
		{
			DataObject data = new();
			data.SetImage(_Image);
			data.SetData(_PNG_FORMAT, false, new MemoryStream(_ToPng(_Data, _Image)));
			Clipboard.SetDataObject(data, true);

			EditStateChanged?.Invoke(this, EventArgs.Empty);
			return true;
		}
		catch(Exception) { return false; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IThemedControl                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the color table that provides the theme.</summary>
	ControlColorTable IThemedControl.ThemeColors
	{
		get { return _Colors; }
	}


	/// <summary>Gets if the theme manager applies the theme to the child controls.</summary>
	bool IThemedControl.ThemesChildren
	{
		get { return false; }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>The theme has changed: the box is drawn again.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _Colors_Changed(object? sender, EventArgs e)
	{
		Invalidate();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Control                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the default size of the control.</summary>
	protected override Size DefaultSize
	{
		get { return new Size(52, 52); }
	}


	/// <summary>Paints the control.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle area = ClientRectangle;
		Theme theme = _Colors.EffectiveTheme;
		Color border = ((!Enabled) ? theme[ThemeColor.DISABLED_BORDER] : (Focused ? theme[ThemeColor.FIELD_BORDER_FOCUSED] : theme[ThemeColor.FIELD_BORDER]));

		using(SolidBrush brush = new(Enabled ? BackColor : theme[ThemeColor.DISABLED_BACK])) { e.Graphics.FillRectangle(brush, area); }
		ControlPaint.DrawBorder(e.Graphics, area, border, ButtonBorderStyle.Solid);

		if(_Image is not null)
		{
			Rectangle inner = Rectangle.Inflate(area, -4, -4);
			float scale = Math.Min(1f, Math.Min((float) inner.Width / _Image.Width, (float) inner.Height / _Image.Height));
			int width = Math.Max(1, (int) (_Image.Width * scale)), height = Math.Max(1, (int) (_Image.Height * scale));
			Rectangle bounds = new(inner.Left + ((inner.Width - width) / 2), inner.Top + ((inner.Height - height) / 2), width, height);

			e.Graphics.InterpolationMode = ((scale < 1f) ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor);
			e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
			e.Graphics.DrawImage(_Image, bounds);
		}

		if(Focused && ShowFocusCues) { ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(area, -2, -2)); }
	}


	/// <summary>Focus received.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnGotFocus(EventArgs e)
	{
		base.OnGotFocus(e);
		Invalidate();
	}


	/// <summary>Focus lost.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnLostFocus(EventArgs e)
	{
		base.OnLostFocus(e);
		Invalidate();
	}


	/// <summary>Mouse button pressed: the box takes the focus.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnMouseDown(MouseEventArgs e)
	{
		if(CanFocus) { Focus(); }
		base.OnMouseDown(e);
	}


	/// <summary>Double click: raises <see cref="LoadRequested"/>.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnDoubleClick(EventArgs e)
	{
		base.OnDoubleClick(e);
		LoadRequested?.Invoke(this, EventArgs.Empty);
	}


	/// <summary>Something is dragged into the box.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnDragEnter(DragEventArgs e)
	{
		base.OnDragEnter(e);
		e.Effect = ((_GetFile(e.Data) is null) ? DragDropEffects.None : DragDropEffects.Copy);
	}


	/// <summary>Something is dropped onto the box: an image file is taken over.</summary>
	/// <param name="e">Event arguments.</param>
	protected override void OnDragDrop(DragEventArgs e)
	{
		base.OnDragDrop(e);
		if(_GetFile(e.Data) is not string file) return;

		try
		{
			if(SetImageData(File.ReadAllBytes(file))) { Focus(); return; }
		}
		catch(Exception) {}
		SystemSounds.Beep.Play();
	}


	/// <summary>Releases the resources used by the control.</summary>
	/// <param name="disposing">Determines if managed resources are released.</param>
	protected override void Dispose(bool disposing)
	{
		if(disposing)
		{
			_Colors.Changed -= _Colors_Changed;
			_Image?.Dispose();
			_Image = null;
		}
		base.Dispose(disposing);
	}
}
