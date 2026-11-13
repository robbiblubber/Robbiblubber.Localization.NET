namespace Robbiblubber.Localization;



/// <summary>This class describes a problem that occurred while loading locale definitions and language tables.</summary>
/// <remarks>Problems do not stop loading: unreadable resources are skipped, duplicate locales and symbols are resolved by
///          order.</remarks>
public sealed class LocalizationIssue
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// constructors                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Creates a new instance of this class.</summary>
	/// <param name="resource">Name of the affected resource.</param>
	/// <param name="message">Message.</param>
	/// <param name="exception">Exception that caused the problem, NULL if there is none.</param>
	internal LocalizationIssue(string resource, string message, Exception? exception = null)
	{
		Resource = resource;
		Message = message;
		Exception = exception;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// public properties                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>Gets the name of the affected resource.</summary>
	public string Resource
	{
		get;
	}


	/// <summary>Gets the message.</summary>
	public string Message
	{
		get;
	}


	/// <summary>Gets the exception that caused the problem, NULL if there is none.</summary>
	public Exception? Exception
	{
		get;
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [override] object                                                                                                //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <inheritdoc/>
	public override string ToString()
	{
		return (Resource + ": " + Message);
	}
}
