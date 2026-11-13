namespace Robbiblubber.Localization.Samples.Controls;

using System.Runtime.Versioning;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Spellbook.Controls;
using Robbiblubber.Spellbook.Controls.Themes;



/// <summary>This class implements the main window of the sample application.</summary>
/// <remarks>The left group shows controls localized by a <see cref="LocaleProvider"/>, a <see cref="LocalizableToolTip"/> and
///          localizable items. The locale is selected with four <see cref="LocaleSelector"/> controls, one in each style. The
///          options on the right are not localized; all localization events are written to the log.</remarks>
[SupportedOSPlatform("windows")]
public partial class MainWindow: Form
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Maximum number of log entries.</summary>
	private const int _MAX_LOG_ENTRIES = 500;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Item images.</summary>
	private readonly List<Image> _Images = new();



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	public MainWindow()
	{
		InitializeComponent();

		_FillItems();
		_UpdateStatus();

		Locale.LocaleChanged += _Locale_LocaleChanged;
		_LogLocales();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a color sample image.</summary>
	/// <param name="color">Color.</param>
	/// <returns>Returns the image.</returns>
	private static Image _CreateImage(Color color)
	{
		Bitmap rval = new(16, 16);
		using(Graphics g = Graphics.FromImage(rval))
		{
			using SolidBrush brush = new(color);
			g.FillRectangle(brush, 2, 2, 12, 12);
			g.DrawRectangle(Pens.Gray, 2, 2, 11, 11);
		}

		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private methods                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Writes a log entry.</summary>
	/// <param name="message">Message.</param>
	private void _Log(string message)
	{
		_ListLog.BeginUpdate();
		_ListLog.Items.Add($"{DateTime.Now:HH:mm:ss.fff}  {message}");
		while(_ListLog.Items.Count > _MAX_LOG_ENTRIES) { _ListLog.Items.RemoveAt(0); }
		_ListLog.TopIndex = _ListLog.Items.Count - 1;
		_ListLog.EndUpdate();
	}


	/// <summary>Writes the available locales and the loading issues to the log.</summary>
	private void _LogLocales()
	{
		foreach(Locale i in Locale.Locales)
		{
			_Log($"Locale {i.Key,-6} \"{i.Name}\"  default = {i.IsDefault}, right to left = {i.RightToLeft}, icons = {i.HasSmallIcon}/{i.HasLargeIcon}");
		}
		_Log($"Current = {Locale.Current?.Key ?? "(none)"}, fallback = {Locale.Fallback?.Key ?? "(none)"}, {Locale.Issues.Count} issue(s)");

		foreach(LocalizationIssue i in Locale.Issues) { _Log($"Issue: {i.Resource}: {i.Message}"); }
	}


	/// <summary>Fills the lists with localizable items.</summary>
	private void _FillItems()
	{
		(string Symbol, string Text, Color Color)[] colors = new[]
		{
			("sample:color.red", "Red", Color.Red), ("sample:color.green", "Green", Color.Green), ("sample:color.blue", "Blue", Color.Blue)
		};

		foreach((string Symbol, string Text, Color Color) i in colors)
		{
			Image image = _CreateImage(i.Color);
			_Images.Add(image);

			_ListColors.Items.Add(new LocalizableItem(i.Symbol, i.Text, i.Color));
			_ComboColors.Items.Add(new LocalizableItem(i.Symbol, i.Text, i.Color));
			_ThemedComboColors.Items.Add(new LocalizableItem(i.Symbol, i.Text, i.Color) { Image = image, Detail = i.Color.Name });
		}
		_ComboColors.SelectedIndex = 0;
		_ThemedComboColors.SelectedIndex = 0;

		LocalizableListViewItem apple = new("sample:product.apple", "Apple");
		apple.SubItems.Add("1.20");
		LocalizableListViewItem pear = new("sample:product.pear", "Pear");
		pear.SubItems.Add("0.90");
		_ListProducts.Items.AddRange(new ListViewItem[] { apple, pear });

		TreeNode[] fruit = new TreeNode[] { new LocalizableTreeNode("sample:product.apple", "Apple"), new LocalizableTreeNode("sample:product.pear", "Pear") };
		TreeNode[] vegetables = new TreeNode[] { new LocalizableTreeNode("sample:node.carrot", "Carrot") };

		_TreeFood.Nodes.Add(new LocalizableTreeNode("sample:node.fruit", "Fruit", fruit));
		_TreeFood.Nodes.Add(new LocalizableTreeNode("sample:node.vegetables", "Vegetables", vegetables));
		_TreeFood.ExpandAll();
	}


	/// <summary>Updates the status text.</summary>
	/// <remarks>The text is composed in code with a placeholder.</remarks>
	private void _UpdateStatus()
	{
		_LabelStatus.Text = "sample:status.count".LookUp("$COUNT colors selected", ("COUNT", _ListColors.SelectedIndices.Count));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Handles the LocaleChanged event of <see cref="Locale"/>.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Locale_LocaleChanged(object? sender, EventArgs e)
	{
		string text = $"Locale.LocaleChanged on thread {Environment.CurrentManagedThreadId}: current = {Locale.Current?.Key ?? "(none)"}";

		if(InvokeRequired)
		{
			BeginInvoke(() => _Log(text));
		}
		else { _Log(text); }
	}


	/// <summary>Handles the Localized event of the provider.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Provider_Localized(object? sender, EventArgs e)
	{
		_UpdateStatus();
		_Log($"LocaleProvider.Localized on thread {Environment.CurrentManagedThreadId}: title = \"{Text}\"");
	}


	/// <summary>Handles the SelectedLocaleChanged event of the selectors.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _Selector_SelectedLocaleChanged(object? sender, EventArgs e)
	{
		if(sender is LocaleSelector s) { _Log($"{s.Name}.SelectedLocaleChanged: {s.SelectedLocale?.Key ?? "(none)"}"); }
	}


	/// <summary>Handles the SelectedIndexChanged event of the color list.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ListColors_SelectedIndexChanged(object? sender, EventArgs e)
	{
		_UpdateStatus();
	}


	/// <summary>Handles the Click event of the exit menu item.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuFileExit_Click(object? sender, EventArgs e)
	{
		Close();
	}


	/// <summary>Handles the Click event of the about menu item.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _MenuHelpAbout_Click(object? sender, EventArgs e)
	{
		MessageBox.Show(this, "sample:about.text".LookUp("Robbiblubber Localization Controls sample."), Text);
	}


	/// <summary>Handles the Click event of the save button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ToolSave_Click(object? sender, EventArgs e)
	{
		_Log($"Tool bar: \"{_ToolSave.Text}\" clicked.");
	}


	/// <summary>Handles the Click event of the open button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ToolOpen_Click(object? sender, EventArgs e)
	{
		_Log($"Tool bar: \"{_ToolOpen.Text}\" clicked.");
	}


	/// <summary>Handles the Click event of the send button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonSend_Click(object? sender, EventArgs e)
	{
		_Log($"Send: name = \"{_TextName.Text}\", newsletter = {_CheckNewsletter.Checked}, accessible name = \"{_TextName.AccessibleName}\", "
		   + $"accessible description = \"{_TextName.AccessibleDescription}\", tool tip = \"{_ToolTip.GetToolTip(_ButtonSend)}\"");
	}


	/// <summary>Handles the CheckedChanged event of the AutoUpdate check box.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _CheckAutoUpdate_CheckedChanged(object? sender, EventArgs e)
	{
		_Provider.AutoUpdate = _CheckAutoUpdate.Checked;
	}


	/// <summary>Handles the CheckedChanged event of the LocalizeItems check box.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _CheckLocalizeItems_CheckedChanged(object? sender, EventArgs e)
	{
		_Provider.LocalizeItems = _CheckLocalizeItems.Checked;
	}


	/// <summary>Handles the CheckedChanged event of the ApplyRightToLeft check box.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _CheckRightToLeft_CheckedChanged(object? sender, EventArgs e)
	{
		_Provider.ApplyRightToLeft = _CheckRightToLeft.Checked;
		if(!_CheckRightToLeft.Checked)
		{
			RightToLeft = RightToLeft.No;
			RightToLeftLayout = false;
		}
		_Provider.Localize();
	}


	/// <summary>Handles the CheckedChanged event of the tool tip AutoUpdate check box.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _CheckToolTipAutoUpdate_CheckedChanged(object? sender, EventArgs e)
	{
		_ToolTip.AutoUpdate = _CheckToolTipAutoUpdate.Checked;
	}


	/// <summary>Handles the CheckedChanged event of the AutoApply check box.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _CheckAutoApply_CheckedChanged(object? sender, EventArgs e)
	{
		_SelectorList.AutoApply = _SelectorButton.AutoApply = _SelectorDropDown.AutoApply = _SelectorArrow.AutoApply = _CheckAutoApply.Checked;
	}


	/// <summary>Handles the CheckedChanged event of the dark selectors check box.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _CheckDarkSelectors_CheckedChanged(object? sender, EventArgs e)
	{
		Theme? theme = (_CheckDarkSelectors.Checked ? Theme.DARK : null);

		foreach(LocaleSelector i in new[] { _SelectorList, _SelectorButton, _SelectorDropDown, _SelectorArrow }) { i.Colors.Theme = theme; }
	}


	/// <summary>Handles the CheckedChanged event of the PreferFallback check box.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _CheckPreferFallback_CheckedChanged(object? sender, EventArgs e)
	{
		Locale.PreferFallback = _CheckPreferFallback.Checked;
		_Provider.Localize();
		_ToolTip.Localize();
	}


	/// <summary>Handles the Click event of the localize button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonLocalize_Click(object? sender, EventArgs e)
	{
		_Provider.Localize();
		_ToolTip.Localize();
	}


	/// <summary>Handles the Click event of the reload button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonReload_Click(object? sender, EventArgs e)
	{
		Locale.Reload();
		_LogLocales();
	}


	/// <summary>Handles the Click event of the default locale button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonDefault_Click(object? sender, EventArgs e)
	{
		Locale.Current = null;
	}


	/// <summary>Handles the Click event of the background thread button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonThread_Click(object? sender, EventArgs e)
	{
		Task.Run(() =>
		{
			LocaleCollection locales = Locale.Locales;
			if(locales.Count == 0) { return; }

			int index = ((Locale.Current is null) ? -1 : locales.IndexOf(Locale.Current));
			Locale.Current = locales[(index + 1) % locales.Count];
		});
	}


	/// <summary>Handles the Click event of the issues button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonIssues_Click(object? sender, EventArgs e)
	{
		_LogLocales();
	}


	/// <summary>Handles the Click event of the clear log button.</summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">Event arguments.</param>
	private void _ButtonClearLog_Click(object? sender, EventArgs e)
	{
		_ListLog.Items.Clear();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] Form                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		Locale.LocaleChanged -= _Locale_LocaleChanged;
		base.OnFormClosed(e);

		foreach(Image i in _Images) { i.Dispose(); }
		_Images.Clear();
	}
}
