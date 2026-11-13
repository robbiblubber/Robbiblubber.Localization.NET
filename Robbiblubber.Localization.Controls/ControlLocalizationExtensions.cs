namespace Robbiblubber.Localization.Controls;

using System.Collections;
using System.ComponentModel;
using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;



/// <summary>This class provides extension methods that localize the items of list and tree controls.</summary>
/// <remarks>Items that implement <see cref="ILocalizable"/> are localized, for example <see cref="LocalizableItem"/>,
///          <see cref="LocalizableListViewItem"/>, <see cref="LocalizableColumnHeader"/> and <see cref="LocalizableTreeNode"/>.
///          The controls keep their items and their selection; items provided by a data source are refreshed through the
///          currency manager of the data source.</remarks>
[SupportedOSPlatform("windows")]
public static class ControlLocalizationExtensions
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes an item.</summary>
	/// <param name="item">Item.</param>
	/// <returns>Returns TRUE if the item is localizable, otherwise returns FALSE.</returns>
	private static bool _Localize(object? item)
	{
		if(item is not ILocalizable l) { return false; }

		l.Localize();
		return true;
	}


	/// <summary>Localizes an item and determines if its display text has changed.</summary>
	/// <param name="item">Item.</param>
	/// <param name="getText">Function that returns the display text of an item.</param>
	/// <returns>Returns TRUE if the item is localizable and its display text has changed, otherwise returns FALSE.</returns>
	private static bool _Localize(object? item, Func<object?, string?> getText)
	{
		if(item is not ILocalizable) { return false; }

		string? before = getText(item);
		_Localize(item);
		return (getText(item) != before);
	}


	/// <summary>Localizes tree nodes and their child nodes.</summary>
	/// <param name="nodes">Tree nodes.</param>
	private static void _Localize(TreeNodeCollection nodes)
	{
		foreach(TreeNode i in nodes)
		{
			_Localize(i);
			_Localize(i.Nodes);
		}
	}


	/// <summary>Localizes the items of a data source and refreshes the bound control.</summary>
	/// <param name="control">Control.</param>
	/// <param name="dataSource">Data source.</param>
	private static void _LocalizeDataSource(Control control, object dataSource)
	{
		bool changed = false;
		IList? list = ((dataSource is IListSource s) ? s.GetList() : (dataSource as IList));

		if(list is null) { return; }
		foreach(object? i in list) { changed |= _Localize(i); }

		if(changed && (control.BindingContext?[dataSource] is CurrencyManager m)) { m.Refresh(); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Localizes the nodes of a tree view.</summary>
	/// <param name="treeView">Tree view.</param>
	/// <exception cref="ArgumentNullException">Thrown when the tree view is NULL.</exception>
	public static void Localize(this TreeView treeView)
	{
		ArgumentNullException.ThrowIfNull(treeView);

		treeView.BeginUpdate();
		try
		{
			_Localize(treeView.Nodes);
		}
		finally { treeView.EndUpdate(); }
	}


	/// <summary>Localizes the columns and items of a list view.</summary>
	/// <param name="listView">List view.</param>
	/// <exception cref="ArgumentNullException">Thrown when the list view is NULL.</exception>
	public static void Localize(this ListView listView)
	{
		ArgumentNullException.ThrowIfNull(listView);

		listView.BeginUpdate();
		try
		{
			foreach(ColumnHeader i in listView.Columns) { _Localize(i); }
			foreach(ListViewItem i in listView.Items) { _Localize(i); }
		}
		finally { listView.EndUpdate(); }
	}


	/// <summary>Localizes the items of a list box.</summary>
	/// <param name="listBox">List box.</param>
	/// <exception cref="ArgumentNullException">Thrown when the list box is NULL.</exception>
	/// <remarks>The items and the selection are kept.</remarks>
	public static void Localize(this ListBox listBox)
	{
		ArgumentNullException.ThrowIfNull(listBox);

		if(listBox.DataSource is not null)
		{
			_LocalizeDataSource(listBox, listBox.DataSource);
			return;
		}

		int[] selected = listBox.SelectedIndices.Cast<int>().ToArray();
		int top = listBox.TopIndex;
		bool changed = false;

		listBox.BeginUpdate();
		try
		{
			for(int i = 0; i < listBox.Items.Count; i++)
			{
				object item = listBox.Items[i];
				if(!_Localize(item, listBox.GetItemText)) { continue; }

				listBox.Items[i] = item;
				changed = true;
			}

			if(changed && (listBox.SelectionMode is SelectionMode.MultiSimple or SelectionMode.MultiExtended))
			{
				foreach(int i in selected) { if(!listBox.GetSelected(i)) { listBox.SetSelected(i, true); } }
			}
			if(changed && (listBox.Items.Count > 0)) { listBox.TopIndex = Math.Min(top, listBox.Items.Count - 1); }
		}
		finally { listBox.EndUpdate(); }
	}


	/// <summary>Localizes the items of a combo box.</summary>
	/// <param name="comboBox">Combo box.</param>
	/// <exception cref="ArgumentNullException">Thrown when the combo box is NULL.</exception>
	/// <remarks>The items and the selection are kept. If the combo box is editable and its text differs from the selected item,
	///          the text is kept as well.</remarks>
	public static void Localize(this ComboBox comboBox)
	{
		ArgumentNullException.ThrowIfNull(comboBox);

		if(comboBox.DataSource is not null)
		{
			_LocalizeDataSource(comboBox, comboBox.DataSource);
			return;
		}

		string? text = null;
		if((comboBox.DropDownStyle != ComboBoxStyle.DropDownList) && (comboBox.Text != comboBox.GetItemText(comboBox.SelectedItem)))
		{
			text = comboBox.Text;
		}

		comboBox.BeginUpdate();
		try
		{
			for(int i = 0; i < comboBox.Items.Count; i++)
			{
				object item = comboBox.Items[i]!;
				if(_Localize(item, comboBox.GetItemText)) { comboBox.Items[i] = item; }
			}

			if((text is not null) && (comboBox.Text != text)) { comboBox.Text = text; }
		}
		finally { comboBox.EndUpdate(); }
	}


	/// <summary>Localizes the items of a themed combo box.</summary>
	/// <param name="comboBox">Themed combo box.</param>
	/// <exception cref="ArgumentNullException">Thrown when the combo box is NULL.</exception>
	/// <remarks>The items and the selection are kept.</remarks>
	public static void Localize(this ThemedComboBox comboBox)
	{
		ArgumentNullException.ThrowIfNull(comboBox);

		if(comboBox.DataSource is not null)
		{
			_LocalizeDataSource(comboBox, comboBox.DataSource);
			return;
		}

		for(int i = 0; i < comboBox.Items.Count; i++)
		{
			object item = comboBox.Items[i];
			if(_Localize(item, comboBox.GetItemText)) { comboBox.Items[i] = item; }
		}
	}
}
