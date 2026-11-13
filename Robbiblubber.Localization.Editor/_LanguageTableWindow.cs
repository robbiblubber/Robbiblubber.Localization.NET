namespace Robbiblubber.Localization.Editor;

using System.Runtime.Versioning;

using Robbiblubber.Spellbook.Controls;



/// <summary>This class implements the window of a language table.</summary>
[SupportedOSPlatform("windows")]
internal sealed partial class _LanguageTableWindow: _DocumentWindow
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private constants                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Maximum length of a cell value that is suggested as search text.</summary>
	private const int _MAX_SUGGESTION_LENGTH = 100;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private members                                                                                                  //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Symbols that occur more than once.</summary>
	private HashSet<string> _Duplicates = new();

	/// <summary>Column of the symbols.</summary>
	private readonly DataGridViewTextBoxColumn _ColumnSymbol = new()
	{
		Name = "_ColumnSymbol",
		DataPropertyName = nameof(_TableRow.Symbol),
		HeaderText = "Symbol",
		MinimumWidth = 60,
		Width = 260,
		SortMode = DataGridViewColumnSortMode.NotSortable
	};

	/// <summary>Column of the texts.</summary>
	private readonly DataGridViewTextBoxColumn _ColumnText = new()
	{
		Name = "_ColumnText",
		DataPropertyName = nameof(_TableRow.Text),
		HeaderText = "Text",
		MinimumWidth = 60,
		AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
		SortMode = DataGridViewColumnSortMode.NotSortable
	};

	/// <summary>Number of rows with problems.</summary>
	private int _ProblemCount = 0;

	/// <summary>Cell found by the last search, NULL if none.</summary>
	private (int Row, int Column)? _LastMatch = null;

	/// <summary>Time of the last click that began in the editing control of a cell (<see cref="Environment.TickCount64"/>); a
	///          double-click that follows it selects a word.</summary>
	private long _EditingClick = 0;

	/// <summary>Time and cell of the last left click on a cell that was not being edited, NULL if none.</summary>
	private (long Time, int Row, int Column)? _GridClick = null;



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="document">Document.</param>
	/// <param name="opener">Window the new window is opened from, NULL if none.</param>
	/// <remarks>The columns of the table are created here and not in the designer, which tends to lose them.</remarks>
	public _LanguageTableWindow(_TableDocument document, Form? opener): base(opener)
	{
		InitializeComponent();

		_GridData.AutoGenerateColumns = false;
		_GridData.Columns.AddRange(_ColumnSymbol, _ColumnText);
		_SetDocument(document);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private properties                                                                                               //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the document.</summary>
	private _TableDocument _Table
	{
		get { return (_TableDocument) _Document; }
	}


	/// <summary>Gets the current cell as row and search column; the row is -1 if there is no current cell or it is in the new
	///          row.</summary>
	private (int Row, int Column) _CurrentCell
	{
		get
		{
			DataGridViewCell? cell = _GridData.CurrentCell;
			if((cell is null) || (cell.RowIndex >= _Table.Rows.Count)) { return (-1, _TableSearch.SYMBOL_COLUMN); }
			return (cell.RowIndex, ((cell.ColumnIndex == _ColumnSymbol.Index) ? _TableSearch.SYMBOL_COLUMN : _TableSearch.TEXT_COLUMN));
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// internal methods                                                                                                 //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns a search text suggested by the table.</summary>
	/// <returns>Returns the selected text of the cell being edited or, if the table has the focus, the value of the current cell;
	///          NULL if there is nothing to suggest.</returns>
	internal string? _GetSearchSuggestion()
	{
		if(!_GridData.ContainsFocus) { return null; }
		if(_GridData.EditingControl is TextBoxBase edit) { return ((edit.SelectionLength > 0) ? edit.SelectedText : null); }

		return (((_GridData.CurrentCell?.Value is string value) && (value.Length > 0) && (value.Length <= _MAX_SUGGESTION_LENGTH)) ? value : null);
	}


	/// <summary>Finds the next cell that contains the search text and selects it.</summary>
	/// <param name="search">Search.</param>
	/// <param name="backward">Determines if the table is searched backwards.</param>
	/// <returns>Returns the cell found, NULL if no cell contains the search text.</returns>
	/// <remarks>The search starts after the current cell if it is the cell found last; otherwise the current cell is searched
	///          first.</remarks>
	/// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">Thrown when the regular expression takes too
	///                                                                              long.</exception>
	internal _TableMatch? _Find(_TableSearch search, bool backward)
	{
		if(!_Commit()) { return null; }

		(int row, int column) = _CurrentCell;
		_TableMatch? rval = search.Find(_Table.Rows, row, column, ((row >= 0) && (_LastMatch == (row, column))), backward);

		if(rval is _TableMatch m)
		{
			DataGridViewCell cell = _GridData[((m.Column == _TableSearch.SYMBOL_COLUMN) ? _ColumnSymbol.Index : _ColumnText.Index), m.Row];
			_GridData.ClearSelection();
			_GridData.CurrentCell = cell;
			cell.Selected = true;
			_LastMatch = (m.Row, m.Column);
		}
		return rval;
	}


	/// <summary>Replaces the search text in the current cell if it is the cell found last.</summary>
	/// <param name="search">Search.</param>
	/// <param name="replacement">Replacement.</param>
	/// <returns>Returns the number of replaced occurrences.</returns>
	/// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">Thrown when the regular expression takes too
	///                                                                              long.</exception>
	internal int _ReplaceCurrent(_TableSearch search, string replacement)
	{
		if(!_Commit()) { return 0; }

		(int row, int column) = _CurrentCell;
		if((row < 0) || (_LastMatch != (row, column))) { return 0; }

		int rval = search.Replace(_Table.Rows[row], column, replacement);
		if(rval > 0)
		{
			_Table.Rows.ResetItem(row);
			_Update();
		}
		return rval;
	}


	/// <summary>Counts the occurrences of the search text.</summary>
	/// <param name="search">Search.</param>
	/// <returns>Returns the number of occurrences and the number of rows that contain them.</returns>
	/// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">Thrown when the regular expression takes too
	///                                                                              long.</exception>
	internal (int Count, int Rows) _CountMatches(_TableSearch search)
	{
		if(!_Commit()) { return (0, 0); }
		return search.Count(_Table.Rows);
	}


	/// <summary>Replaces all occurrences of the search text.</summary>
	/// <param name="search">Search.</param>
	/// <param name="replacement">Replacement.</param>
	/// <returns>Returns the number of replaced occurrences.</returns>
	/// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">Thrown when the regular expression takes too
	///                                                                              long.</exception>
	internal int _ReplaceAll(_TableSearch search, string replacement)
	{
		if(!_Commit()) { return 0; }

		(List<int> rows, int rval) = search.ReplaceAll(_Table.Rows, replacement);
		foreach(int i in rows) { _Table.Rows.ResetItem(i); }

		if(rval > 0) { _Update(); }
		return rval;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static methods                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Returns if a moment lies within the double-click time.</summary>
	/// <param name="time">Moment (<see cref="Environment.TickCount64"/>).</param>
	/// <returns>Returns TRUE if no more than the double-click time has passed since the moment, otherwise returns FALSE.</returns>
	private static bool _IsWithinDoubleClickTime(long time)
	{
		return ((Environment.TickCount64 - time) <= SystemInformation.DoubleClickTime);
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] _DocumentWindow                                                                                       //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <inheritdoc/>
	internal override void _SetDocument(_EditorDocument document)
	{
		_TableDocument table = (_TableDocument) document;

		_Binding = true;
		_TextKey.Text = table.Key;
		_TextLocale.Text = table.LocaleKey;
		_TextName.Text = table.Name;
		_TextVersion.Text = table.Version;
		_TextPrefix.Text = table.Prefixes;
		_TextAuthor.Text = table.Author;
		_GridData.DataSource = table.Rows;
		_LastMatch = null;

		base._SetDocument(document);
	}


	/// <inheritdoc/>
	internal override bool _Commit()
	{
		if(_GridData.IsCurrentCellInEditMode && (!_GridData.EndEdit())) { return false; }
		_GridData.CommitEdit(DataGridViewDataErrorContexts.Commit);
		BindingContext?[_GridData.DataSource!]?.EndCurrentEdit();
		return true;
	}


	/// <inheritdoc/>
	internal override bool _CanSearch
	{
		get { return true; }
	}


	/// <inheritdoc/>
	internal override bool _HasTableTools
	{
		get { return true; }
	}


	/// <inheritdoc/>
	internal override string _HelpPage
	{
		get { return "tables"; }
	}


	/// <inheritdoc/>
	internal override bool _CanShow(_EditorDocument document)
	{
		return (document is _TableDocument);
	}


	/// <inheritdoc/>
	protected override string _SaveFilter
	{
		get
		{
			return "led:filter.tables".LookUp("Language Tables") + " (*.ltab)|*.ltab|" +
				   "led:filter.cltab".LookUp("Compressed Language Tables") + " (*.cltab)|*.cltab|" +
				   "led:filter.all".LookUp("All Files") + " (*.*)|*.*";
		}
	}


	/// <inheritdoc/>
	protected override int _GetFilterIndex(string? fileName)
	{
		return (((fileName != null) && fileName.EndsWith(".cltab", StringComparison.OrdinalIgnoreCase)) ? 2 : 1);
	}


	/// <inheritdoc/>
	/// <remarks>The name is made of the first prefix (or the key) and the locale, for example <c>sample.de.ltab</c>.</remarks>
	protected override string _SuggestFileName()
	{
		string rval = (_TableDocument.ParsePrefixes(_Table.Prefixes).FirstOrDefault() ?? _Table.Key).Trim();
		if(!string.IsNullOrWhiteSpace(_Table.LocaleKey)) { rval += (string.IsNullOrEmpty(rval) ? string.Empty : ".") + _Table.LocaleKey.Trim(); }
		foreach(char i in Path.GetInvalidFileNameChars()) { rval = rval.Replace(i, '_'); }
		return (string.IsNullOrEmpty(rval) ? "table" : rval) + ".ltab";
	}


	/// <inheritdoc/>
	/// <remarks>The user is warned about a missing locale, rows without symbol, symbols with a prefix and duplicate symbols.</remarks>
	protected override bool _CheckBeforeSave()
	{
		List<string> warnings = new();
		if(string.IsNullOrWhiteSpace(_Table.LocaleKey)) { warnings.Add("led:save.nolocale".LookUp("No locale is set; the table will not be used for any locale.")); }
		if(_Table.Rows.Any(m => _TableDocument.GetProblem(m, _Duplicates, _Table.HasPrefix) == _RowProblem.NO_SYMBOL))
		{
			warnings.Add("led:save.nosymbol".LookUp("Rows without symbol are not saved."));
		}
		if(_Table.Rows.Any(m => _TableDocument.GetProblem(m, _Duplicates, _Table.HasPrefix) == _RowProblem.PREFIX))
		{
			warnings.Add("led:save.prefix".LookUp("Some symbols contain a prefix (:); the table puts its own prefix in front of them."));
		}
		if(_Duplicates.Count > 0)
		{
			warnings.Add("led:save.duplicates".LookUp("For symbols that occur more than once, only the last row is saved: $SYMBOLS",
													   ("SYMBOLS", string.Join(", ", _Duplicates.Order().Take(10)) + ((_Duplicates.Count > 10) ? ", …" : string.Empty))));
		}
		return _ConfirmSave(warnings);
	}


	/// <inheritdoc/>
	/// <remarks>The text of the current row is edited; in the new row a row is added if the text is not empty.</remarks>
	protected override void _EditText()
	{
		if(!_Commit()) return;

		(int row, _) = _CurrentCell;
		_TableRow? data = ((row < 0) ? null : _Table.Rows[row]);

		using _TextEditorWindow d = new(data?.Symbol ?? string.Empty, data?.Text ?? string.Empty);
		if(d.ShowDialog(this) != DialogResult.OK) return;

		if(data is null)
		{
			if(d.Value.Length == 0) return;

			row = _Table.Rows.Count;
			_Table.Rows.Add(new _TableRow(string.Empty, d.Value));
		}
		else if(data.Text != d.Value)
		{
			data.Text = d.Value;
			_Table.Rows.ResetItem(row);
		}
		else return;

		_GridData.ClearSelection();
		_GridData.CurrentCell = _GridData[_ColumnText.Index, row];
		_GridData.CurrentCell.Selected = true;
		_Update();
	}


	/// <inheritdoc/>
	/// <remarks>The current cell stays on its row.</remarks>
	protected override void _Sort()
	{
		if(!_Commit()) return;

		DataGridViewCell? cell = _GridData.CurrentCell;
		_TableRow? current = (((cell is null) || (cell.RowIndex >= _Table.Rows.Count)) ? null : _Table.Rows[cell.RowIndex]);
		int column = (cell?.ColumnIndex ?? _ColumnSymbol.Index);

		if(!_Table.Sort()) return;
		_LastMatch = null;

		if(current != null)
		{
			_GridData.ClearSelection();
			_GridData.CurrentCell = _GridData[column, _Table.Rows.IndexOf(current)];
			_GridData.CurrentCell.Selected = true;
		}
		_Update();
	}


	/// <inheritdoc/>
	/// <remarks>Entries whose symbols are missing are appended and selected; existing entries are never changed.</remarks>
	protected override void _Import()
	{
		if(!_Commit()) return;

		using OpenFileDialog d = new()
		{
			Title = "led:import.title".LookUp("Import"),
			Filter = "led:filter.tables".LookUp("Language Tables") + " (*.ltab, *.cltab)|*.ltab;*.cltab|" +
					 "led:filter.all".LookUp("All Files") + " (*.*)|*.*"
		};
		if(_Table.FileName != null) { d.InitialDirectory = (Path.GetDirectoryName(_Table.FileName) ?? string.Empty); }
		if(d.ShowDialog(this) != DialogResult.OK) return;

		_TableDocument source;
		try
		{
			source = _TableDocument.Load(d.FileName);
		}
		catch(Exception ex) when ((ex is FormatException) || (ex is IOException) || (ex is UnauthorizedAccessException))
		{
			ThemedMessageBox.Show(this, Path.GetFileName(d.FileName) + "\n\n" + ((ex is FormatException) ? "led:import.format".LookUp("The file is not a language table.") : ex.Message),
								  "led:import.title".LookUp("Import"), MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		List<int> rows = _Table.Import(source);
		if(rows.Count == 0)
		{
			ThemedMessageBox.Show(this, "led:import.none".LookUp("The table already contains all entries of $NAME.", ("NAME", Path.GetFileName(d.FileName))),
								  "led:import.title".LookUp("Import"), MessageBoxButtons.OK, MessageBoxIcon.Information);
			return;
		}

		_GridData.ClearSelection();
		_GridData.CurrentCell = _GridData[_ColumnSymbol.Index, rows[0]];
		foreach(int i in rows) { _GridData.Rows[i].Selected = true; }
		_Update();
		_ShowMessage("led:import.done".LookUp("Entries imported: $COUNT", ("COUNT", rows.Count)));
	}


	/// <inheritdoc/>
	protected override void _UpdateContents()
	{
		_Duplicates = _Table.GetDuplicateSymbols();
		_ProblemCount = _Table.Rows.Count(m => _TableDocument.GetProblem(m, _Duplicates, _Table.HasPrefix) != null);
		_GridData.Invalidate();

		_LabelInfo.Text = "led:status.entries".LookUp("Entries: $COUNT", ("COUNT", _Table.EntryCount));
		_LabelProblems.Text = ((_ProblemCount == 0) ? string.Empty : "led:status.problems".LookUp("Problems: $COUNT", ("COUNT", _ProblemCount)));
	}


	/// <inheritdoc/>
	protected override void _Localize()
	{
		_ColumnSymbol.HeaderText = "led:table.symbol".LookUp("Symbol");
		_ColumnText.HeaderText = "led:table.text".LookUp("Text");

		base._Localize();
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// event handlers                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Header field changed.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _TextHeader_TextChanged(object? sender, EventArgs e)
	{
		if(_Binding) return;

		_Table.Key = _TextKey.Text;
		_Table.LocaleKey = _TextLocale.Text;
		_Table.Name = _TextName.Text;
		_Table.Version = _TextVersion.Text;
		_Table.Prefixes = _TextPrefix.Text;
		_Table.Author = _TextAuthor.Text;
		_Update();
	}


	/// <summary>Table changed.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _GridData_Changed(object? sender, EventArgs e)
	{
		if(_Binding) return;
		_Update();
	}


	/// <summary>Row error text needed.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _GridData_RowErrorTextNeeded(object? sender, DataGridViewRowErrorTextNeededEventArgs e)
	{
		if(_Binding || (e.RowIndex < 0) || (e.RowIndex >= _Table.Rows.Count)) return;

		e.ErrorText = _TableDocument.GetProblem(_Table.Rows[e.RowIndex], _Duplicates, _Table.HasPrefix) switch
		{
			_RowProblem.NO_SYMBOL => "led:problem.nosymbol".LookUp("The row has no symbol and is not saved."),
			_RowProblem.DUPLICATE => "led:problem.duplicate".LookUp("The symbol occurs more than once; only the last row is saved."),
			_RowProblem.PREFIX => "led:problem.prefix".LookUp("The symbol contains a prefix (:); the table puts its own prefix in front of it."),
			_ => string.Empty
		};
	}


	/// <summary>Mouse button pressed on a cell or header of the table.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _GridData_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
	{
		if(e.Button == MouseButtons.Right) { _GridData.SelectForContextMenu(e.RowIndex, e.ColumnIndex); }

		if((e.Button == MouseButtons.Left) && (e.Clicks == 1) && (!_GridData.IsCurrentCellInEditMode))
		{
			_GridClick = (Environment.TickCount64, e.RowIndex, e.ColumnIndex);
		}
	}


	/// <summary>Context menu > Text Editor.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _CmenuTextEditor_Click(object? sender, EventArgs e)
	{
		_EditText();
	}


	/// <summary>Cell double-clicked.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>A double-click on a text cell opens the text editor; a symbol cell is edited in place. A double-click into a cell
	///          that is being edited only selects a word: the table raises the event for it as well, so a double-click that began in
	///          the editing control is ignored.</remarks>
	private void _GridData_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
	{
		if(_IsWithinDoubleClickTime(_EditingClick)) return;
		if((e.RowIndex >= 0) && (e.ColumnIndex == _ColumnText.Index)) { _EditText(); }
	}


	/// <summary>Editing control shown.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _GridData_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
	{
		e.Control.ContextMenuStrip = _CmenuText;
		e.Control.MouseDown -= _EditingControl_MouseDown;
		e.Control.MouseDown += _EditingControl_MouseDown;
	}


	/// <summary>Mouse button pressed in the editing control of a cell.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>A click on the current cell starts editing it, so the second click of a double-click on that cell reaches the
	///          editing control instead of the table. It opens the text editor here; a double-click the table may raise for it is
	///          ignored. Any other click began in the editing control.</remarks>
	private void _EditingControl_MouseDown(object? sender, MouseEventArgs e)
	{
		if(e.Button != MouseButtons.Left) return;

		DataGridViewCell? cell = _GridData.CurrentCell;
		if((_GridClick is (long time, int row, int column)) && _IsWithinDoubleClickTime(time) && (cell != null) &&
		   (cell.RowIndex == row) && (cell.ColumnIndex == column))
		{
			_GridClick = null;
			_EditingClick = Environment.TickCount64;
			if(column == _ColumnText.Index) { BeginInvoke(_EditText); }
			return;
		}

		_GridClick = null;
		_EditingClick = Environment.TickCount64;
	}


	/// <summary>Data error in the table.</summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">Event arguments.</param>
	private void _GridData_DataError(object? sender, DataGridViewDataErrorEventArgs e)
	{
		e.ThrowException = false;
	}
}
