namespace Robbiblubber.Localization.Controls;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;



/// <summary>This component attaches locale symbols to the controls and components of a form or user control and localizes their
///          texts.</summary>
/// <remarks><para>The <c>LocaleSymbol</c> extender property localizes the text of controls, tool strip items, column headers and
///          other components with a string property <c>Text</c>. The <c>AccessibleNameLocaleSymbol</c> and
///          <c>AccessibleDescriptionLocaleSymbol</c> extender properties localize the accessibility properties of
///          controls.</para>
///          <para>The text a component has when it is localized for the first time is its default text: it is displayed if the
///          symbol is not defined. Setting an empty symbol restores the default text and releases the component. Symbols set
///          after the first localization are applied immediately.</para>
///          <para>The provider localizes when the handle of <see cref="ContainerControl"/> is created (before the form is
///          displayed), and when the current locale changes, if <see cref="AutoUpdate"/> is set. With <see cref="LocalizeItems"/>
///          set, localizable items in list views, list boxes, combo boxes and tree views of the container control are localized
///          as well, and controls implementing <see cref="ILocalizable"/> are localized.</para></remarks>
[ProvideProperty("LocaleSymbol", typeof(Component))]
[ProvideProperty("AccessibleNameLocaleSymbol", typeof(Control))]
[ProvideProperty("AccessibleDescriptionLocaleSymbol", typeof(Control))]
[ToolboxBitmap(typeof(LocaleProvider), "Resources.localeprovider.bmp")]
[SupportedOSPlatform("windows")]
public partial class LocaleProvider: Component, IExtenderProvider, ILocalizable
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Entries by component.</summary>
	private readonly Dictionary<Component, _Entry> _Entries = new();

	/// <summary>Container control.</summary>
	private ContainerControl? _ContainerControl = null;

	/// <summary>Determines if the components have been localized at least once.</summary>
	private bool _HasLocalized = false;

	/// <summary>Determines if the provider is localizing.</summary>
	private bool _Localizing = false;

	/// <summary>Disposed flag.</summary>
	private bool _Disposed = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public LocaleProvider(): base()
	{
		Locale.LocaleChanged += _Locale_LocaleChanged;
	}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="container">Container.</param>
	public LocaleProvider(IContainer container): this()
	{
		container.Add(this);
	}


	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="containerControl">Container control (usually the form) whose texts are localized.</param>
	public LocaleProvider(ContainerControl containerControl): this()
	{
		ContainerControl = containerControl;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the writable string property <c>Text</c> of a component.</summary>
	/// <param name="component">Component.</param>
	/// <returns>Returns the property, NULL if the component has no such property.</returns>
	private static PropertyInfo? _GetTextProperty(Component component)
	{
		return component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
						.FirstOrDefault(m => (m.Name == "Text") && (m.PropertyType == typeof(string)) && m.CanRead && m.CanWrite
										  && (m.GetIndexParameters().Length == 0));
	}


	/// <summary>Gets a localized value of a component.</summary>
	/// <param name="component">Component.</param>
	/// <param name="kind">Value kind.</param>
	/// <returns>Returns the value.</returns>
	private static string? _GetValue(Component component, _Kind kind)
	{
		return (kind, component) switch
		{
			(_Kind.TEXT, Control m) => m.Text,
			(_Kind.TEXT, ToolStripItem m) => m.Text,
			(_Kind.TEXT, ColumnHeader m) => m.Text,
			(_Kind.TEXT, _) => (string?) _GetTextProperty(component)?.GetValue(component),
			(_Kind.ACCESSIBLE_NAME, Control m) => m.AccessibleName,
			(_Kind.ACCESSIBLE_DESCRIPTION, Control m) => m.AccessibleDescription,
			_ => null
		};
	}


	/// <summary>Sets a localized value of a component.</summary>
	/// <param name="component">Component.</param>
	/// <param name="kind">Value kind.</param>
	/// <param name="value">Value.</param>
	private static void _SetValue(Component component, _Kind kind, string? value)
	{
		switch(kind, component)
		{
			case (_Kind.TEXT, Control m): m.Text = value; break;
			case (_Kind.TEXT, ToolStripItem m): m.Text = value; break;
			case (_Kind.TEXT, ColumnHeader m): m.Text = value; break;
			case (_Kind.TEXT, _): _GetTextProperty(component)?.SetValue(component, value); break;
			case (_Kind.ACCESSIBLE_NAME, Control m): m.AccessibleName = value; break;
			case (_Kind.ACCESSIBLE_DESCRIPTION, Control m): m.AccessibleDescription = value; break;
		}
	}



	/// <summary>Localizes a value of a component.</summary>
	/// <param name="component">Component.</param>
	/// <param name="kind">Value kind.</param>
	/// <param name="slot">Slot.</param>
	private static void _Localize(Component component, _Kind kind, _Slot slot)
	{
		if(slot.Symbol.Length == 0) { return; }

		if(!slot.Captured)
		{
			slot.DefaultText = _GetValue(component, kind);
			slot.Captured = true;
		}

		string text = Locale.LookUp(slot.Symbol, slot.DefaultText);
		if(_GetValue(component, kind) != text) { _SetValue(component, kind, text); }
	}


	/// <summary>Localizes the items of a control and its child controls.</summary>
	/// <param name="control">Control.</param>
	private static void _LocalizeItems(Control control)
	{
		switch(control)
		{
			case ILocalizable m: m.Localize(); break;
			case ListView m: m.Localize(); break;
			case ListBox m: m.Localize(); break;
			case ComboBox m: m.Localize(); break;
			case TreeView m: m.Localize(); break;
			case ThemedComboBox m: m.Localize(); break;
		}

		foreach(Control i in control.Controls) { _LocalizeItems(i); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the symbol of a component.</summary>
	/// <param name="component">Component.</param>
	/// <param name="kind">Value kind.</param>
	/// <returns>Returns the symbol, an empty string if no symbol is set.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the component is NULL.</exception>
	private string _GetSymbol(Component component, _Kind kind)
	{
		ArgumentNullException.ThrowIfNull(component);
		return (_Entries.TryGetValue(component, out _Entry? e) ? e[kind].Symbol : string.Empty);
	}


	/// <summary>Sets the symbol of a component.</summary>
	/// <param name="component">Component.</param>
	/// <param name="kind">Value kind.</param>
	/// <param name="value">Symbol, NULL or empty to remove the symbol.</param>
	/// <exception cref="ArgumentNullException">Thrown when the component is NULL.</exception>
	private void _SetSymbol(Component component, _Kind kind, string? value)
	{
		ArgumentNullException.ThrowIfNull(component);
		value ??= string.Empty;

		if(!_Entries.TryGetValue(component, out _Entry? e))
		{
			if(value.Length == 0) { return; }

			_Entries.Add(component, e = new _Entry());
			component.Disposed += _Component_Disposed;
		}

		_Slot slot = e[kind];
		if(value.Length == 0)
		{
			if(slot.Captured && (!DesignMode)) { _SetValue(component, kind, slot.DefaultText); }
			slot.Clear();

			if(e.IsEmpty) { _Release(component); }
			return;
		}

		slot.Symbol = value;
		if((!DesignMode) && (slot.Captured || _HasLocalized)) { _Localize(component, kind, slot); }
	}


	/// <summary>Removes a component from the provider.</summary>
	/// <param name="component">Component.</param>
	private void _Release(Component component)
	{
		component.Disposed -= _Component_Disposed;
		_Entries.Remove(component);
	}


	/// <summary>Applies the reading direction of the current locale to the container control.</summary>
	private void _ApplyRightToLeft()
	{
		if(_ContainerControl is null) { return; }

		bool rtl = (Locale.Current?.RightToLeft ?? false);
		RightToLeft value = (rtl ? RightToLeft.Yes : RightToLeft.No);

		if(_ContainerControl.RightToLeft != value) { _ContainerControl.RightToLeft = value; }
		if((_ContainerControl is Form f) && (f.RightToLeftLayout != rtl)) { f.RightToLeftLayout = rtl; }
	}


	/// <summary>Gets the control used to marshal localization to the user interface thread.</summary>
	/// <returns>Returns a control, NULL if there is no control.</returns>
	private Control? _GetSyncControl()
	{
		if(_ContainerControl is not null) { return _ContainerControl; }
		return _Entries.Keys.OfType<Control>().FirstOrDefault(m => m.IsHandleCreated);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// protected methods                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Raises the <see cref="Localized"/> event.</summary>
	/// <param name="e">Event arguments.</param>
	protected virtual void _OnLocalized(EventArgs e)
	{
		Localized?.Invoke(this, e);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public events                                                                                                    //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Occurs after the provider has localized the components.</summary>
	/// <remarks>Use this event to update texts that are composed in code.</remarks>
	[Category("Localization")][Description("Occurs after the provider has localized the components.")]
	public event EventHandler? Localized;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets or sets the container control (usually the form) whose texts are localized.</summary>
	/// <remarks>In the designer, this property is set to the form or user control automatically. The provider localizes when the
	///          handle of the container control is created, and uses it to marshal localization to the user interface
	///          thread.</remarks>
	[Category("Localization")][Description("Form or user control whose texts are localized.")][DefaultValue(null)]
	public ContainerControl? ContainerControl
	{
		get { return _ContainerControl; }
		set
		{
			if(value == _ContainerControl) { return; }

			if(_ContainerControl is not null) { _ContainerControl.HandleCreated -= _ContainerControl_HandleCreated; }
			_ContainerControl = value;
			if(_ContainerControl is not null)
			{
				_ContainerControl.HandleCreated += _ContainerControl_HandleCreated;
				if((!DesignMode) && _ContainerControl.IsHandleCreated) { Localize(); }
			}
		}
	}


	/// <summary>Gets or sets if the components are localized automatically when the current locale changes.</summary>
	[Category("Localization")][Description("Specifies if the components are localized automatically when the current locale changes.")][DefaultValue(true)]
	public bool AutoUpdate
	{
		get; set;
	} = true;


	/// <summary>Gets or sets if localizable items in the controls of the container control are localized.</summary>
	/// <remarks>Items of list views, list boxes, combo boxes, themed combo boxes and tree views that implement
	///          <see cref="ILocalizable"/> are localized, and so are controls that implement <see
	///          cref="ILocalizable"/>.</remarks>
	[Category("Localization")][Description("Specifies if localizable items in the controls of the container control are localized.")][DefaultValue(true)]
	public bool LocalizeItems
	{
		get; set;
	} = true;


	/// <summary>Gets or sets if the reading direction of the current locale is applied to the container control.</summary>
	/// <remarks>If set, <see cref="Control.RightToLeft"/> of the container control and <see cref="Form.RightToLeftLayout"/> of a
	///          form follow <see cref="Locale.RightToLeft"/> of the current locale.</remarks>
	[Category("Localization")][Description("Specifies if the reading direction of the current locale is applied to the container control.")][DefaultValue(false)]
	public bool ApplyRightToLeft
	{
		get; set;
	} = false;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the locale symbol for the text of a component.</summary>
	/// <param name="component">Component.</param>
	/// <returns>Returns the locale symbol, an empty string if no symbol is set.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the component is NULL.</exception>
	[Category("Localization")][Description("Locale symbol for the text of the component.")][DefaultValue("")]
	public virtual string GetLocaleSymbol(Component component)
	{
		return _GetSymbol(component, _Kind.TEXT);
	}


	/// <summary>Sets the locale symbol for the text of a component.</summary>
	/// <param name="component">Component.</param>
	/// <param name="value">Locale symbol, NULL or empty to remove the symbol.</param>
	/// <exception cref="ArgumentNullException">Thrown when the component is NULL.</exception>
	public virtual void SetLocaleSymbol(Component component, string? value)
	{
		_SetSymbol(component, _Kind.TEXT, value);
	}


	/// <summary>Gets the locale symbol for the accessible name of a control.</summary>
	/// <param name="control">Control.</param>
	/// <returns>Returns the locale symbol, an empty string if no symbol is set.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the control is NULL.</exception>
	[Category("Localization")][Description("Locale symbol for the accessible name of the control.")][DefaultValue("")]
	public virtual string GetAccessibleNameLocaleSymbol(Control control)
	{
		return _GetSymbol(control, _Kind.ACCESSIBLE_NAME);
	}


	/// <summary>Sets the locale symbol for the accessible name of a control.</summary>
	/// <param name="control">Control.</param>
	/// <param name="value">Locale symbol, NULL or empty to remove the symbol.</param>
	/// <exception cref="ArgumentNullException">Thrown when the control is NULL.</exception>
	public virtual void SetAccessibleNameLocaleSymbol(Control control, string? value)
	{
		_SetSymbol(control, _Kind.ACCESSIBLE_NAME, value);
	}


	/// <summary>Gets the locale symbol for the accessible description of a control.</summary>
	/// <param name="control">Control.</param>
	/// <returns>Returns the locale symbol, an empty string if no symbol is set.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the control is NULL.</exception>
	[Category("Localization")][Description("Locale symbol for the accessible description of the control.")][DefaultValue("")]
	public virtual string GetAccessibleDescriptionLocaleSymbol(Control control)
	{
		return _GetSymbol(control, _Kind.ACCESSIBLE_DESCRIPTION);
	}


	/// <summary>Sets the locale symbol for the accessible description of a control.</summary>
	/// <param name="control">Control.</param>
	/// <param name="value">Locale symbol, NULL or empty to remove the symbol.</param>
	/// <exception cref="ArgumentNullException">Thrown when the control is NULL.</exception>
	public virtual void SetAccessibleDescriptionLocaleSymbol(Control control, string? value)
	{
		_SetSymbol(control, _Kind.ACCESSIBLE_DESCRIPTION, value);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Handles the LocaleChanged event of <see cref="Locale"/>.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>The event may be raised on any thread; localization is marshaled to the thread of the container
	///          control.</remarks>
	private void _Locale_LocaleChanged(object? sender, EventArgs e)
	{
		if(_Disposed || (!AutoUpdate) || DesignMode) { return; }

		Control? sync = _GetSyncControl();
		if(sync is null)
		{
			Localize();
		}
		else if(sync.IsHandleCreated)
		{
			if(sync.InvokeRequired)
			{
				sync.BeginInvoke(() => { if(!_Disposed) { Localize(); } });
			}
			else { Localize(); }
		}
	}


	/// <summary>Handles the HandleCreated event of the container control.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ContainerControl_HandleCreated(object? sender, EventArgs e)
	{
		if(!DesignMode) { Localize(); }
	}


	/// <summary>Handles the Disposed event of components.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Component_Disposed(object? sender, EventArgs e)
	{
		if(sender is Component component) { _Release(component); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Component                                                                                             //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	/// <remarks>When the component is placed in the designer, <see cref="ContainerControl"/> is set to the form or user
	///          control.</remarks>
	public override ISite? Site
	{
		get { return base.Site; }
		set
		{
			base.Site = value;

			if((_ContainerControl is null) && (value?.GetService(typeof(IDesignerHost)) is IDesignerHost host) && (host.RootComponent is ContainerControl root))
			{
				ContainerControl = root;
			}
		}
	}


	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if(disposing && (!_Disposed))
		{
			_Disposed = true;
			Locale.LocaleChanged -= _Locale_LocaleChanged;

			if(_ContainerControl is not null) { _ContainerControl.HandleCreated -= _ContainerControl_HandleCreated; }
			foreach(Component i in _Entries.Keys) { i.Disposed -= _Component_Disposed; }
			_Entries.Clear();
		}

		base.Dispose(disposing);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] ILocalizable                                                                                         //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the components with the texts of the current locale.</summary>
	/// <remarks>Raises <see cref="Localized"/>. Calls while the provider is localizing are ignored; they occur when applying the
	///          reading direction recreates the handle of the container control.</remarks>
	public virtual void Localize()
	{
		if(_Disposed || _Localizing) { return; }

		_Localizing = true;
		_HasLocalized = true;
		try
		{
			foreach(KeyValuePair<Component, _Entry> i in _Entries.ToArray())
			{
				if((i.Key is Control c) && c.IsDisposed)
				{
					_Release(i.Key);
					continue;
				}

				_Localize(i.Key, _Kind.TEXT, i.Value[_Kind.TEXT]);
				_Localize(i.Key, _Kind.ACCESSIBLE_NAME, i.Value[_Kind.ACCESSIBLE_NAME]);
				_Localize(i.Key, _Kind.ACCESSIBLE_DESCRIPTION, i.Value[_Kind.ACCESSIBLE_DESCRIPTION]);
			}

			if(_ContainerControl is not null)
			{
				if(LocalizeItems) { foreach(Control i in _ContainerControl.Controls) { _LocalizeItems(i); } }
				if(ApplyRightToLeft) { _ApplyRightToLeft(); }
			}
		}
		finally { _Localizing = false; }

		_OnLocalized(EventArgs.Empty);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [interface] IExtenderProvider                                                                                    //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	/// <remarks>Controls, tool strip items, column headers and components with a string property <c>Text</c> can be
	///          extended.</remarks>
	public virtual bool CanExtend(object extendee)
	{
		return extendee switch
		{
			Control or ToolStripItem or ColumnHeader => true,
			LocaleProvider => false,
			Component m => (_GetTextProperty(m) is not null),
			_ => false
		};
	}
}
