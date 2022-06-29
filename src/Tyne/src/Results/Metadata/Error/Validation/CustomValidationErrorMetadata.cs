namespace Tyne.Results;

/// <summary>
///		Default implementation of <see cref="ICustomValidationErrorMetadata"/>.
/// </summary>
public class CustomValidationErrorMetadata : ICustomValidationErrorMetadata
{
	public string Message { get; }
	public string PropertyName { get; }

	public CustomValidationErrorMetadata(string message, string propertyName)
	{
		Message = message ?? throw new ArgumentNullException(nameof(message));
		PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
	}
}
