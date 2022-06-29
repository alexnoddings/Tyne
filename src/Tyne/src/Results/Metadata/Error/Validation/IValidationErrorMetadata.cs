namespace Tyne.Results;

/// <summary>
///		Indicates an error when validating the property <see cref="PropertyName"/>.
/// </summary>
/// <remarks>
///		<para>
///			If multiple validation errors occur, one <see cref="IValidationErrorMetadata"/> should be present for each error.
///		</para>
///		<para>
///			<see cref="ICustomValidationErrorMetadata"/> and <see cref="IFormValidationErrorMetadata"/> provide more context on the validation error.
///		</para>
/// </remarks>
public interface IValidationErrorMetadata : IErrorMetadata
{
	public string PropertyName { get; }
}
