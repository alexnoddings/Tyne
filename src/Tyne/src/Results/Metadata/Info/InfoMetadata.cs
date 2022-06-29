namespace Tyne.Results;

/// <summary>
///		Default implementation of <see cref="IInfoMetadata"/>.
/// </summary>
public class InfoMetadata : IInfoMetadata
{
	public string Message { get; }

	public InfoMetadata(string message)
	{
		Message = message ?? throw new ArgumentNullException(nameof(message));
	}
}
