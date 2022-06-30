using System.Diagnostics.Contracts;

namespace Tyne.Results;

/// <summary>
///     Helper methods for creating <see cref="Result{T}"/> of type <see cref="Unit"/>.
/// </summary>
public static class Result
{
	/// <summary>
	///     Creates an empty, successful result.
	/// </summary>
	[Pure]
	public static Result<Unit> Empty() => Successful();

	#region Successful
	/// <summary>
	///     Creates a successful <see cref="Result{T}"/> with a <see cref="SuccessMetadata"/> from <paramref name="successMessage"/>.
	/// </summary>
	/// <param name="successMessage">A success message to add as a <see cref="SuccessMetadata"/>.</param>
	/// <param name="metadata">Optionally, extra metadata parameters to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.</returns>
	[Pure]
	public static Result<Unit> Successful(string successMessage, params IMetadata[] metadata) =>
		Result<Unit>.Successful(Unit.Value, successMessage, (IEnumerable<IMetadata>)metadata);

	/// <summary>
	///     Creates a successful <see cref="Result{T}"/> with a <see cref="SuccessMetadata"/> from <paramref name="successMessage"/>.
	/// </summary>
	/// <param name="successMessage">A success message to add as a <see cref="SuccessMetadata"/>.</param>
	/// <param name="metadata">Extra metadata to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.
	[Pure]
	public static Result<Unit> Successful(string successMessage, IEnumerable<IMetadata> metadata) =>
		Result<Unit>.Successful(Unit.Value, successMessage, metadata);

	/// <summary>
	///     Creates a successful <see cref="Result{T}"/> with optional metadata.
	/// </summary>
	/// <param name="metadata">Optionally, metadata parameters to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.
	[Pure]
	public static Result<Unit> Successful(params IMetadata[] metadata) =>
		Result<Unit>.Successful(Unit.Value, metadata);

	/// <summary>
	///     Creates a successful <see cref="Result{T}"/> with metadata.
	/// </summary>
	/// <param name="value">The value of the result.</param>
	/// <param name="metadata">Metadata to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.
	[Pure]
	public static Result<Unit> Successful(IEnumerable<IMetadata> metadata) =>
		Result<Unit>.Successful(Unit.Value, metadata);
	#endregion

	#region Failure
	/// <summary>
	///     Creates a failure <see cref="Result{T}"/> with an <see cref="ErrorMetadata"/> from <paramref name="errorMessage"/>.
	/// </summary>
	/// <param name="errorMessage">A failure message to add as a <see cref="ErrorMetadata"/>.</param>
	/// <param name="metadata">Optionally, extra metadata parameters to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
	[Pure]
	public static Result<Unit> Failure(string errorMessage, params IMetadata[] metadata) =>
		Result<Unit>.Failure(errorMessage, metadata);

	/// <summary>
	///     Creates a failure <see cref="Result{T}"/> with an <see cref="ErrorMetadata"/> from <paramref name="errorMessage"/>.
	/// </summary>
	/// <param name="errorMessage">A failure message to add as a <see cref="ErrorMetadata"/>.</param>
	/// <param name="metadata">Extra metadata to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
	[Pure]
	public static Result<Unit> Failure(string errorMessage, IEnumerable<IMetadata> metadata) =>
		Result<Unit>.Failure(errorMessage, metadata);

	/// <summary>
	///     Creates a failure <see cref="Result{T}"/> with optional metadata.
	/// </summary>
	/// <param name="metadata">Optionally, metadata parameters to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
	[Pure]
	public static Result<Unit> Failure(params IMetadata[] metadata) =>
		Result<Unit>.Failure(metadata);

	/// <summary>
	///     Creates a failure <see cref="Result{T}"/> with an <see cref="ErrorMetadata"/> from <paramref name="errorMessage"/>.
	/// </summary>
	/// <param name="metadata">Metadata to add.</param>
	/// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
	[Pure]
	public static Result<Unit> Failure(IEnumerable<IMetadata> metadata) =>
		Result<Unit>.Failure(metadata);
	#endregion
}
