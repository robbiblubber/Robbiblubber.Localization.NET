namespace Robbiblubber.Localization.Controls;

using System.ComponentModel;
using System.Runtime.Versioning;



/// <summary>This class implements a tool tip component with localizable tool tip texts.</summary>
/// <remarks><para>The <c>LocaleSymbol</c> extender property localizes the tool tip text of a control. The tool tip text a control
///          has when it is localized for the first time is its default text: it is displayed if the symbol is not defined.
///          Setting an empty symbol restores the default text and releases the control.</para>
///          <para>The tool tip text of a control is localized when the handle of the control is created, and when the current
///          locale changes, if <see cref="AutoUpdate"/> is set.</para></remarks>
[ProvideProperty("LocaleSymbol", typeof(Control))]
[ToolboxBitmap(typeof(LocalizableToolTip), "Resources.localizabletooltip.bmp")]
[SupportedOSPlatform("windows")]
public sealed class LocalizableToolTip: ToolTip, ILocalizable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Entries by control.</summary>
	private readonly Dictionary<Control, _Entry> _Entries = new();

	/// <summary>Disposed flag.</summary>
	private bool _Disposed = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public LocalizableToolTip(): base()
	{
		Locale.LocaleChanged += _Locale_LocaleChanged;
	}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="container">Container.</param>
	public LocalizableToolTip(IContainer container): base(container)
	{
		Locale.LocaleChanged += _Locale_LocaleChanged;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the tool tip text of a control.</summary>
	/// <param name="control">Control.</param>
	/// <param name="entry">Entry.</param>
	private void _Localize(Control control, _Entry entry)
	{
		if(!entry.Captured)
		{
			string? text = GetToolTip(control);
			entry.DefaultText = (string.IsNullOrEmpty(text) ? null : text);
			entry.Captured = true;
		}

		string value = Locale.LookUp(entry.Symbol, entry.DefaultText);
		if(GetToolTip(control) != value) { SetToolTip(control, value); }
	}


	/// <summary>Removes a control from the component.</summary>
	/// <param name="control">Control.</param>
	private void _Release(Control control)
	{
		control.HandleCreated -= _Control_HandleCreated;
		control.Disposed -= _Control_Disposed;
		_Entries.Remove(control);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets if the tool tip texts are localized automatically when the current locale changes.</summary>
	[Category("Localization")][Description("Specifies if the tool tip texts are localized automatically when the current locale changes.")][DefaultValue(true)]
	public bool AutoUpdate
	{
		get; set;
	} = true;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the locale symbol for the tool tip text of a control.</summary>
	/// <param name="control">Control.</param>
	/// <returns>Returns the locale symbol, an empty string if no symbol is set.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the control is NULL.</exception>
	[Category("Localization")][Description("Locale symbol for the tool tip text of the control.")][DefaultValue("")]
	public string GetLocaleSymbol(Control control)
	{
		ArgumentNullException.ThrowIfNull(control);
		return (_Entries.TryGetValue(control, out _Entry? rval) ? rval.Symbol : string.Empty);
	}


	/// <summary>Sets the locale symbol for the tool tip text of a control.</summary>
	/// <param name="control">Control.</param>
	/// <param name="value">Locale symbol, NULL or empty to remove the symbol.</param>
	/// <exception cref="ArgumentNullException">Thrown when the control is NULL.</exception>
	public void SetLocaleSymbol(Control control, string? value)
	{
		ArgumentNullException.ThrowIfNull(control);
		value ??= string.Empty;

		if(!_Entries.TryGetValue(control, out _Entry? e))
		{
			if(value.Length == 0) { return; }

			_Entries.Add(control, e = new _Entry());
			control.HandleCreated += _Control_HandleCreated;
			control.Disposed += _Control_Disposed;
		}

		if(value.Length == 0)
		{
			if(e.Captured && (!DesignMode)) { SetToolTip(control, e.DefaultText); }
			_Release(control);
			return;
		}

		e.Symbol = value;
		if((!DesignMode) && (e.Captured || control.IsHandleCreated)) { _Localize(control, e); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Handles the LocaleChanged event of <see cref="Locale"/>.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>The event may be raised on any thread; localization is marshaled to the thread of the controls.</remarks>
	private void _Locale_LocaleChanged(object? sender, EventArgs e)
	{
		if(_Disposed || (!AutoUpdate) || DesignMode) { return; }

		Control? sync = _Entries.Keys.FirstOrDefault(m => m.IsHandleCreated);
		if((sync is not null) && sync.InvokeRequired)
		{
			sync.BeginInvoke(() => { if(!_Disposed) { Localize(); } });
		}
		else { Localize(); }
	}


	/// <summary>Handles the HandleCreated event of controls.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Control_HandleCreated(object? sender, EventArgs e)
	{
		if((!DesignMode) && (sender is Control c) && _Entries.TryGetValue(c, out _Entry? entry)) { _Localize(c, entry); }
	}


	/// <summary>Handles the Disposed event of controls.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Control_Disposed(object? sender, EventArgs e)
	{
		if(sender is Control c) { _Release(c); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] ToolTip                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if(disposing && (!_Disposed))
		{
			_Disposed = true;
			Locale.LocaleChanged -= _Locale_LocaleChanged;

			foreach(Control i in _Entries.Keys.ToArray()) { _Release(i); }
		}

		base.Dispose(disposing);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] ILocalizable                                                                                         //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the tool tip texts with the texts of the current locale.</summary>
	/// <remarks>Controls whose handle has not been created yet are skipped, so their default text is captured when they are
	///          shown; they are localized when their handle is created.</remarks>
	public void Localize()
	{
		if(_Disposed) { return; }

		foreach(KeyValuePair<Control, _Entry> i in _Entries.ToArray())
		{
			if(i.Key.IsDisposed)
			{
				_Release(i.Key);
			}
			else if(i.Value.Captured || i.Key.IsHandleCreated) { _Localize(i.Key, i.Value); }
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [class] _Entry                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This class stores the symbol and the default text of a control.</summary>
	private sealed class _Entry
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public properties                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets or sets the symbol.</summary>
		public string Symbol
		{
			get; set;
		} = string.Empty;


		/// <summary>Gets or sets the default text.</summary>
		public string? DefaultText
		{
			get; set;
		} = null;


		/// <summary>Gets or sets if the default text has been captured.</summary>
		public bool Captured
		{
			get; set;
		} = false;
	}
}
