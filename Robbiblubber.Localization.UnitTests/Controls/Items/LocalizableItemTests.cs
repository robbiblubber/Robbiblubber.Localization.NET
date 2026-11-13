namespace Robbiblubber.Localization.UnitTests.Controls.Items;

using System.ComponentModel;
using System.Runtime.Versioning;

using NUnit.Framework;

using Robbiblubber.Localization.Controls;
using Robbiblubber.Localization.UnitTests.Library;
using Robbiblubber.Spellbook.Controls;



/// <summary>This class tests the localizable items and the extension methods that localize list and tree controls.</summary>
[TestFixture][Apartment(ApartmentState.STA)][NonParallelizable][SupportedOSPlatform("windows")]
public class LocalizableItemTests
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Test files.</summary>
	private _TestFiles _Files = null!;

	/// <summary>Form for the controls.</summary>
	private Form _Form = null!;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates the test data and a form.</summary>
	[SetUp]
	public void SetUp()
	{
		Locale._Reset();
		_Files = new();
		_ControlTestData.WriteStandard(_Files);
		_Form = _ControlTestData.CreateForm();
	}


	/// <summary>Disposes the form and deletes the test data.</summary>
	[TearDown]
	public void TearDown()
	{
		_Form.Dispose();
		_Files.Dispose();
		Locale._Reset();
	}


	/// <summary>An item is localized when it is created and when it is localized.</summary>
	[Test]
	public void Item_Localize()
	{
		LocalizableItem item = new("app:item", "Item default", 42);

		Assert.That(item.Text, Is.EqualTo("Item"));
		Assert.That(item.ToString(), Is.EqualTo("Item"));
		Assert.That(item.Tag, Is.EqualTo(42));
		Assert.That(item.LocaleSymbol, Is.EqualTo("app:item"));

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(item.Text, Is.EqualTo("Item"));

		item.Localize();
		Assert.That(item.Text, Is.EqualTo("Eintrag"));
	}


	/// <summary>An undefined symbol displays the default text; without default text, the fallback locale is used.</summary>
	[Test]
	public void Item_DefaultAndFallback()
	{
		Locale.Current = _ControlTestData.Get("ar");

		Assert.That(new LocalizableItem("app:undefined", "Default").Text, Is.EqualTo("Default"));
		Assert.That(new LocalizableItem("app:open").Text, Is.EqualTo("Open"));
		Assert.That(new LocalizableItem("app:undefined").Text, Is.Empty);
	}


	/// <summary>An item provides its properties as list item for themed combo boxes.</summary>
	[Test]
	public void Item_IsListItem()
	{
		IListItem item = new LocalizableItem("app:item") { Detail = "detail", ImageKey = "key", FontStyle = FontStyle.Bold, Tag = "tag" };

		Assert.That(item.Text, Is.EqualTo("Item"));
		Assert.That(item.Detail, Is.EqualTo("detail"));
		Assert.That(item.ImageKey, Is.EqualTo("key"));
		Assert.That(item.FontStyle, Is.EqualTo(FontStyle.Bold));
		Assert.That(item.Tag, Is.EqualTo("tag"));
		Assert.That(item.FilterText, Is.Null);
	}


	/// <summary>Tree nodes, list view items and column headers are localized when they are created.</summary>
	[Test]
	public void Components_LocalizedOnCreation()
	{
		Locale.Current = _ControlTestData.Get("de");

		using LocalizableColumnHeader column = new("app:column", "Column default");

		Assert.That(new LocalizableTreeNode("app:node", "Node default").Text, Is.EqualTo("Knoten"));
		Assert.That(new LocalizableListViewItem("app:item", "Item default", "key").Text, Is.EqualTo("Eintrag"));
		Assert.That(column.Text, Is.EqualTo("Spalte"));
		Assert.That(new LocalizableTreeNode("app:undefined", "Node default").Text, Is.EqualTo("Node default"));
	}


	/// <summary>Without default text, the text set before the first localization becomes the default text.</summary>
	[Test]
	public void Components_CaptureText()
	{
		using LocalizableColumnHeader column = new() { Text = "Header" };
		column.LocaleSymbol = "app:undefined";

		column.Localize();
		Assert.That(column.DefaultText, Is.EqualTo("Header"));
		Assert.That(column.Text, Is.EqualTo("Header"));

		column.LocaleSymbol = "app:column";
		column.Localize();
		Assert.That(column.Text, Is.EqualTo("Column"));
	}


	/// <summary>Cloned tree nodes and list view items keep their symbols.</summary>
	[Test]
	public void Components_Clone()
	{
		LocalizableTreeNode node = new("app:node", "Node default");
		LocalizableListViewItem item = new("app:item", "Item default");

		LocalizableTreeNode nodeClone = (LocalizableTreeNode) node.Clone();
		LocalizableListViewItem itemClone = (LocalizableListViewItem) item.Clone();

		Assert.That(nodeClone.LocaleSymbol, Is.EqualTo("app:node"));
		Assert.That(nodeClone.DefaultText, Is.EqualTo("Node default"));
		Assert.That(itemClone.LocaleSymbol, Is.EqualTo("app:item"));
		Assert.That(itemClone.DefaultText, Is.EqualTo("Item default"));

		Locale.Current = _ControlTestData.Get("de");
		nodeClone.Localize();
		Assert.That(nodeClone.Text, Is.EqualTo("Knoten"));
	}


	/// <summary>A tree view localizes all nodes including child nodes.</summary>
	[Test]
	public void TreeView_Localize()
	{
		TreeView tree = new();
		LocalizableTreeNode child = new("app:child", "Child default");
		tree.Nodes.Add(new LocalizableTreeNode("app:node", "Node default", new TreeNode[] { child }));
		tree.Nodes.Add(new TreeNode("Plain"));
		_Form.Controls.Add(tree);
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");
		tree.Localize();

		Assert.That(tree.Nodes[0].Text, Is.EqualTo("Knoten"));
		Assert.That(child.Text, Is.EqualTo("Kind"));
		Assert.That(tree.Nodes[1].Text, Is.EqualTo("Plain"));
	}


	/// <summary>A list view localizes its columns and items.</summary>
	[Test]
	public void ListView_Localize()
	{
		ListView list = new() { View = View.Details };
		list.Columns.Add(new LocalizableColumnHeader("app:column"));
		list.Items.Add(new LocalizableListViewItem("app:item"));
		_Form.Controls.Add(list);
		_ControlTestData.Show(_Form);

		Locale.Current = _ControlTestData.Get("de");
		list.Localize();

		Assert.That(list.Columns[0].Text, Is.EqualTo("Spalte"));
		Assert.That(list.Items[0].Text, Is.EqualTo("Eintrag"));
	}


	/// <summary>A list box localizes its items and keeps the selection.</summary>
	[Test]
	public void ListBox_Localize()
	{
		ListBox list = new() { SelectionMode = SelectionMode.MultiExtended };
		list.Items.AddRange(new object[] { new LocalizableItem("app:save"), "Plain", new LocalizableItem("app:open") });
		_Form.Controls.Add(list);
		_ControlTestData.Show(_Form);
		list.SetSelected(0, true);
		list.SetSelected(2, true);

		Locale.Current = _ControlTestData.Get("de");
		list.Localize();

		Assert.That(list.GetItemText(list.Items[0]), Is.EqualTo("Speichern"));
		Assert.That(list.GetItemText(list.Items[2]), Is.EqualTo("Öffnen"));
		Assert.That(list.Items.Count, Is.EqualTo(3));
		Assert.That(list.SelectedIndices.Cast<int>(), Is.EqualTo(new[] { 0, 2 }));
	}


	/// <summary>A combo box localizes its items and keeps the selection.</summary>
	[Test]
	public void ComboBox_Localize()
	{
		ComboBox combo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
		combo.Items.AddRange(new object[] { new LocalizableItem("app:save"), new LocalizableItem("app:open") });
		_Form.Controls.Add(combo);
		_ControlTestData.Show(_Form);
		combo.SelectedIndex = 1;

		Locale.Current = _ControlTestData.Get("de");
		combo.Localize();

		Assert.That(combo.SelectedIndex, Is.EqualTo(1));
		Assert.That(combo.Text, Is.EqualTo("Öffnen"));
		Assert.That(combo.GetItemText(combo.Items[0]), Is.EqualTo("Speichern"));
	}


	/// <summary>An editable combo box keeps a text typed by the user.</summary>
	[Test]
	public void ComboBox_KeepsFreeText()
	{
		ComboBox combo = new() { DropDownStyle = ComboBoxStyle.DropDown };
		combo.Items.AddRange(new object[] { new LocalizableItem("app:save"), new LocalizableItem("app:open") });
		_Form.Controls.Add(combo);
		_ControlTestData.Show(_Form);
		combo.Text = "Typed";

		Locale.Current = _ControlTestData.Get("de");
		combo.Localize();

		Assert.That(combo.Text, Is.EqualTo("Typed"));
		Assert.That(combo.GetItemText(combo.Items[1]), Is.EqualTo("Öffnen"));
	}


	/// <summary>A combo box bound to a data source is refreshed and keeps its position.</summary>
	[Test]
	public void ComboBox_DataSource()
	{
		BindingList<LocalizableItem> items = new() { new LocalizableItem("app:save"), new LocalizableItem("app:open") };
		ComboBox combo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
		_Form.Controls.Add(combo);
		combo.DataSource = items;
		_ControlTestData.Show(_Form);
		combo.SelectedIndex = 1;

		Locale.Current = _ControlTestData.Get("de");
		Assert.That(() => combo.Localize(), Throws.Nothing);

		Assert.That(combo.SelectedIndex, Is.EqualTo(1));
		Assert.That(combo.Text, Is.EqualTo("Öffnen"));
	}


	/// <summary>A themed combo box localizes its items and keeps the selection.</summary>
	[Test]
	public void ThemedComboBox_Localize()
	{
		ThemedComboBox combo = new();
		combo.Items.AddRange(new LocalizableItem("app:save"), new LocalizableItem("app:open"));
		_Form.Controls.Add(combo);
		_ControlTestData.Show(_Form);
		combo.SelectedIndex = 0;

		Locale.Current = _ControlTestData.Get("de");
		combo.Localize();

		Assert.That(combo.SelectedIndex, Is.EqualTo(0));
		Assert.That(combo.Text, Is.EqualTo("Speichern"));
		Assert.That(combo.GetItemText(combo.Items[1]), Is.EqualTo("Öffnen"));
	}
}
