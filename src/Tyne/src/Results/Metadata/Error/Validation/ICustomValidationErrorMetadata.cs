namespace Tyne.Results;

/// <summary>
///		Indicates a custom form validation error for property  <see cref="IValidationErrorMetadata.PropertyName"/>.
/// </summary>
/// <remarks>
///		This indicates a custom validation errors (e.g. a duplicate name).
///		<see cref="IFormValidationErrorMetadata"/> should be used for normal validation error (e.g. an input is empty).
/// </remarks>
public interface ICustomValidationErrorMetadata : IValidationErrorMetadata
{
}
