namespace Tyne.Results;

public class ExceptionMetadata : IErrorMetadata
{
	public string Message { get; }
	public Exception Exception { get; }

	public ExceptionMetadata(string message, Exception exception)
	{
		Message = message ?? throw new ArgumentNullException(nameof(message));
		Exception = exception ?? throw new ArgumentNullException(nameof(exception));
	}
}
