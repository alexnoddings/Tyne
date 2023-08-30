using System.Diagnostics.Contracts;

namespace Tyne;

/// <summary>
///     Static methods for creating <see cref="Result{T}"/>s.
/// </summary>
/// <seealso cref="Result{T}"/>
public static class Result
{
    /// <summary>
    ///     Creates an <c>Ok(<typeparamref name="T"/>)</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="true"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <returns>An <c>Ok(<typeparamref name="T"/>)</c> <see cref="Result{T}"/> which wraps <paramref name="value"/>.</returns>
    /// <remarks>
    ///     A <see cref="BadOptionException"/> will be thrown if <paramref name="value"/> is <see langword="null"/>.
    /// </remarks>
    /// <exception cref="BadOptionException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    public static Result<T> Ok<T>(T value)
    {
        if (value is null)
            throw new BadResultException(ExceptionMessages.Result_OkMustHaveValue);

        return new(value);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="message"/>.</returns>
    [Pure]
    public static Result<T> Error<T>(string message)
    {
        // Let Error handle a potentially null message
        var error = new Error(Tyne.Error.DefaultCode, message, null);
        return new Result<T>(error);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="code">The error code to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="code"/> and <paramref name="message"/>.</returns>
    [Pure]
    public static Result<T> Error<T>(int code, string message)
    {
        // Let Error handle a potentially null message
        var error = new Error(code, message, null);
        return new Result<T>(error);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="code">The error code to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <param name="causedBy">The exception to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="code"/>, <paramref name="message"/>, and <paramref name="causedBy"/>.</returns>
    [Pure]
    public static Result<T> Error<T>(int code, string message, Exception causedBy)
    {
        // Let Error handle potential nulls
        var error = new Error(code, message, causedBy);
        return new Result<T>(error);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/> (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="error">The error to use as <see cref="Result{T}.Error"/>.</param>
    /// <returns>An <c>Error</c> <see cref="Result{T}"/> whose error is constructed using <paramref name="error"/>.</returns>
    [Pure]
    public static Result<T> Error<T>(in Error error)
    {
        return new Result<T>(error);
    }
}
