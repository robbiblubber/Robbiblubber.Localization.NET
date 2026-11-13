namespace Robbiblubber.Localization.Editor;



/// <summary>This class is the base class for documents that are edited in a window.</summary>
/// <remarks>Changes are detected by comparing the document with the state it was loaded or saved in, so undoing a change by hand
///          clears the change again.</remarks>
internal abstract class _EditorDocument
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// private static members                                                                                           //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Start of a zip file.</summary>
	private static readonly byte[] _ZIP_SIGNATURE = [0x50, 0x4B, 0x03, 0x04];



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public static methods                                                                                            //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Loads a document from a file.</summary>
	/// <param name="fileName">File name.</param>
	/// <returns>Returns a <see cref="_LocaleDefinitionDocument"/> for a locale definition, otherwise a <see cref="_TableDocument"/>.</returns>
	/// <remarks>The type is recognized by the content: locale definitions are zip files, language tables are ddn files.</remarks>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	/// <exception cref="FormatException">Thrown when the file is neither a language table nor a locale definition.</exception>
	public static _EditorDocument Open(string fileName)
	{
		string path = Path.GetFullPath(fileName);
		return (IsZipFile(path) ? _LocaleDefinitionDocument.Load(path) : _TableDocument.Load(path));
	}


	/// <summary>Returns if a file is a zip file.</summary>
	/// <param name="fileName">File name.</param>
	/// <returns>Returns TRUE if the file starts with the signature of a zip file, otherwise returns FALSE.</returns>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	public static bool IsZipFile(string fileName)
	{
		Span<byte> head = stackalloc byte[4];
		using FileStream s = new(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		return ((s.ReadAtLeast(head, 4, false) == 4) && head.SequenceEqual(_ZIP_SIGNATURE));
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Gets the full file name, NULL if the document has not been saved yet.</summary>
	public string? FileName
	{
		get; protected set;
	}


	/// <summary>Gets if the document has unsaved changes.</summary>
	public abstract bool IsDirty
	{
		get;
	}


	/// <summary>Gets if the document is new and unchanged, so that it can be replaced by an opened file.</summary>
	public bool IsUntouched
	{
		get { return ((FileName == null) && (!IsDirty)); }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public methods                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>Saves the document.</summary>
	/// <param name="fileName">File name, NULL to save to the current file.</param>
	/// <exception cref="InvalidOperationException">Thrown when no file name is given for a new document.</exception>
	public abstract void Save(string? fileName = null);
}
