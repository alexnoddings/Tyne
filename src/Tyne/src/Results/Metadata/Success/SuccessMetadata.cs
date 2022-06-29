namespace Tyne.Results;

/// <summary>
///		Default implementation of <see cref="ISuccessMetadata"/>.
/// </summary>
public class SuccessMetadata : ISuccessMetadata
{
	public string Message { get; }

	public SuccessMetadata(string message)
	{
		Message = message ?? throw new ArgumentNullException(nameof(message));
	}
}
