namespace Tyne.Results;

/// <summary>
///		Default implementation of <see cref="IFormValidationErrorMetadata"/>.
/// </summary>
public class FormValidationErrorMetadata : IFormValidationErrorMetadata
{
	public string Message { get; }
	public string PropertyName { get; }

	public FormValidationErrorMetadata(string message, string propertyName)
	{
		Message = message ?? throw new ArgumentNullException(nameof(message));
		PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
	}
}
