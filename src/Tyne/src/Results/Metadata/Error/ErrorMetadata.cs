namespace Tyne.Results;

/// <summary>
///		Default implementation of <see cref="IErrorMetadata"/>.
/// </summary>
public class ErrorMetadata : IErrorMetadata
{
	public string Message { get; }

	public ErrorMetadata(string message)
	{
		Message = message ?? throw new ArgumentNullException(nameof(message));
	}
}
