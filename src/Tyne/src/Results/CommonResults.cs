using System.Diagnostics.Contracts;
using Tyne.Results;

namespace Tyne.Results;

/// <summary>
///     Helper methods for creating common <see cref="Result{T}"/>s.
/// </summary>
public static class CommonResults
{
	#region CustomValidationFailure
	[Pure]
	public static Result<Unit> CustomValidationFailure(string message, string propertyName, params IMetadata[] metadata) =>
		CustomValidationFailure<Unit>(message, propertyName, metadata);

	[Pure]
	public static Result<Unit> CustomValidationFailure(string message, string propertyName, IEnumerable<IMetadata> metadata) =>
		CustomValidationFailure<Unit>(message, propertyName, metadata);

	[Pure]
	public static Result<T> CustomValidationFailure<T>(string message, string propertyName, params IMetadata[] metadata) =>
		CustomValidationFailure<T>(message, propertyName, (IEnumerable<IMetadata>)metadata);

	[Pure]
	public static Result<T> CustomValidationFailure<T>(string message, string propertyName, IEnumerable<IMetadata> metadata) =>
		Result<T>.Failure(metadata.Prepend(new CustomValidationErrorMetadata(message, propertyName)));
	#endregion

	#region UnhandledException
	[Pure]
	public static Result<Unit> UnhandledException(Exception exception, string message, params IMetadata[] metadata) =>
		UnhandledException<Unit>(exception, message, metadata);

	[Pure]
	public static Result<Unit> UnhandledException(Exception exception, string message, IEnumerable<IMetadata> metadata) =>
		UnhandledException<Unit>(exception, message, metadata);

	[Pure]
	public static Result<T> UnhandledException<T>(Exception exception, string message, params IMetadata[] metadata) =>
		UnhandledException<T>(exception, message, (IEnumerable<IMetadata>)metadata);

	[Pure]
	public static Result<T> UnhandledException<T>(Exception exception, string message, IEnumerable<IMetadata> metadata) =>
		Result<T>.Failure(metadata.Prepend(new ExceptionMetadata(message, exception)));
	#endregion
}
