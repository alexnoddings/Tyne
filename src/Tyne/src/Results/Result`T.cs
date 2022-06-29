using System.Diagnostics.Contracts;
using Tyne.Results;

namespace Tyne.Results;

/// <summary>
///     Represents a result with a <see cref="Value"/>, a <see cref="Success"/>, and <see cref="Metadata"/>.
/// </summary>
/// <typeparam name="T">The type of the result's value.</typeparam>
public class Result<T>
{
	private readonly T? _value;

    /// <summary>
    ///     Gets the value of the result. This will throw an <see cref="InvalidOperationException"/> when <see cref="Success"/> is <see langword="false"/>.
    /// </summary>
	public T Value =>
		Success
		? _value!
		: throw new InvalidOperationException($"{nameof(Value)} cannot be accessed when {nameof(Success)} is false");

    /// <summary>
    ///     Whether the result was successful.
    /// </summary>
	public bool Success { get; }

    /// <summary>
    ///     Metadata associated with the result. May be empty.
    /// </summary>
	public List<IMetadata> Metadata { get; }

	protected internal Result(bool success, T? value, params IMetadata[] metadata)
	{
		if (metadata is null)
			throw new ArgumentNullException(nameof(metadata));

		if (success && value is null)
			throw new ArgumentNullException(nameof(value), $"A {nameof(value)} should be passed when {nameof(success)} is true.");

		// We don't throw an error when success is false but value is not null.
		// Non-reference types (e.g. int, Unit) can't be null.

		_value = value;
		Success = success;
		Metadata = metadata.ToList();
	}

	protected internal Result(bool success, T? value, IEnumerable<IMetadata> metadata)
	{
		if (metadata is null)
			throw new ArgumentNullException(nameof(metadata));

		if (success && value is null)
			throw new ArgumentNullException(nameof(value), $"A {nameof(value)} should be passed when {nameof(success)} is true.");
		
		// We don't throw an error when success is false but value is not null.
		// Non-reference types (e.g. int, Unit) can't be null.

		_value = value;
		Success = success;
		Metadata = metadata.ToList();
    }

    #region Successful
    /// <summary>
    ///     Creates a successful <see cref="Result{T}"/> with a <see cref="SuccessMetadata"/> from <paramref name="successMessage"/>.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="successMessage">A success message to add as a <see cref="SuccessMetadata"/>.</param>
    /// <param name="metadata">Optionally, extra metadata parameters to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.</returns>
    [Pure]
	public static Result<T> Successful(T value, string successMessage, params IMetadata[] metadata) =>
		Successful(value, successMessage, (IEnumerable<IMetadata>)metadata);

    /// <summary>
    ///     Creates a successful <see cref="Result{T}"/> with a <see cref="SuccessMetadata"/> from <paramref name="successMessage"/>.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="successMessage">A success message to add as a <see cref="SuccessMetadata"/>.</param>
    /// <param name="metadata">Extra metadata to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.
    [Pure]
	public static Result<T> Successful(T value, string successMessage, IEnumerable<IMetadata> metadata) =>
		Successful(value, metadata.Prepend(new SuccessMetadata(successMessage)));

    /// <summary>
    ///     Creates a successful <see cref="Result{T}"/> with optional metadata.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="metadata">Optionally, metadata parameters to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.
    [Pure]
	public static Result<T> Successful(T value, params IMetadata[] metadata) =>
		Successful(value, (IEnumerable<IMetadata>)metadata);

    /// <summary>
    ///     Creates a successful <see cref="Result{T}"/> with metadata.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="metadata">Metadata to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="true"/>.
    [Pure]
	public static Result<T> Successful(T value, IEnumerable<IMetadata> metadata) =>
		new(true, value, metadata);
    #endregion

    #region Failure
    /// <summary>
    ///     Creates a failure <see cref="Result{T}"/> with an <see cref="ErrorMetadata"/> from <paramref name="errorMessage"/>.
    /// </summary>
    /// <param name="errorMessage">A failure message to add as a <see cref="ErrorMetadata"/>.</param>
    /// <param name="metadata">Optionally, extra metadata parameters to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
    [Pure]
	public static Result<T> Failure(string errorMessage, params IMetadata[] metadata) =>
		Failure(errorMessage, (IEnumerable<IMetadata>)metadata);

    /// <summary>
    ///     Creates a failure <see cref="Result{T}"/> with an <see cref="ErrorMetadata"/> from <paramref name="errorMessage"/>.
    /// </summary>
    /// <param name="errorMessage">A failure message to add as a <see cref="ErrorMetadata"/>.</param>
    /// <param name="metadata">Extra metadata to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
    [Pure]
	public static Result<T> Failure(string errorMessage, IEnumerable<IMetadata> metadata) =>
		Failure(metadata.Prepend(new ErrorMetadata(errorMessage)));

    /// <summary>
    ///     Creates a failure <see cref="Result{T}"/> with optional metadata.
    /// </summary>
    /// <param name="metadata">Optionally, metadata parameters to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
    [Pure]
	public static Result<T> Failure(params IMetadata[] metadata) =>
		Failure((IEnumerable<IMetadata>)metadata);

    /// <summary>
    ///     Creates a failure <see cref="Result{T}"/> with an <see cref="ErrorMetadata"/> from <paramref name="errorMessage"/>.
    /// </summary>
    /// <param name="metadata">Metadata to add.</param>
    /// <returns>A <see cref="Result{T}"/> with <see cref="Success"/> set as <see langword="false"/>.</returns>
    [Pure]
	public static Result<T> Failure(IEnumerable<IMetadata> metadata) =>
		new(false, default, metadata);
	#endregion
}
