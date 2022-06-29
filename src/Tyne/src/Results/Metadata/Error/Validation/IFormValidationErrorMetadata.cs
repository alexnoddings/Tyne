namespace Tyne.Results;

/// <summary>
///		Indicates a standard form validation error for property <see cref="IValidationErrorMetadata.PropertyName"/>.
/// </summary>
/// <remarks>
///		This indicates a normal validation error (e.g. an input is empty).
///		<see cref="ICustomValidationErrorMetadata"/> should be used for custom validation errors (e.g. a duplicate name).
/// </remarks>
public interface IFormValidationErrorMetadata : IValidationErrorMetadata
{
}
