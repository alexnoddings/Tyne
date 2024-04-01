using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.CompilerServices;

namespace Tyne.HttpMediator;

/// <summary>
///     Static methods for creating <see cref="HttpResult{T}"/>s.
/// </summary>
/// <seealso cref="HttpResult{T}"/>
public static class HttpResult
{
    /// <summary>
    ///     Provides access to extension helper methods for creating <see cref="HttpResult{T}"/>s for specific <see cref="HttpStatusCode"/>s.
    /// </summary>
    public static readonly HttpResultCodes Codes = new();

    /// <summary>
    ///     Creates an <c>Ok(<typeparamref name="T"/>)</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="true"/>) with <paramref name="statusCode"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <param name="statusCode">
    ///     The <see cref="HttpStatusCode"/> to use.
    ///     See <see cref="HttpResult{T}.StatusCode"/> for the valid status code range.
    /// </param>
    /// <returns>An <c>Ok(<typeparamref name="T"/>)</c> <see cref="HttpResult{T}"/> which wraps <paramref name="value"/>.</returns>
    /// <remarks>
    ///     A <see cref="BadResultException"/> will be thrown if <paramref name="value"/> is <see langword="null"/>,
    ///     or if the <paramref name="statusCode"/> is not valid.
    ///     See <see cref="HttpResult{T}.StatusCode"/> for the valid status code range.
    /// </remarks>
    /// <exception cref="BadResultException">
    ///     When <paramref name="value"/> is <see langword="null"/>,
    ///     or when <paramref name="statusCode"/> is not valid.
    /// </exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> Ok<T>(in T value, HttpStatusCode statusCode) =>
        // Result's constructor handles null values
        // HttpResult's constructor handles invalid status codes
        new(value, statusCode);

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <paramref name="statusCode"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <param name="statusCode">
    ///     The <see cref="HttpStatusCode"/> to use.
    ///     See <see cref="HttpResult{T}.StatusCode"/> for the valid status code range.
    /// </param>
    /// <returns>An <c>Error</c> <see cref="HttpResult{T}"/> whose error is constructed using <paramref name="message"/>.</returns>
    /// <exception cref="BadResultException">
    ///     When <paramref name="statusCode"/> is not valid.
    /// </exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> Error<T>(string message, HttpStatusCode statusCode)
    {
        // Let Error handle a potentially null message
        var error = Tyne.Error.From(Tyne.Error.DefaultCode, message);
        // HttpResult's constructor handles invalid status codes
        return new(error, statusCode);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <paramref name="statusCode"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="code">The error code to construct the <see cref="Tyne.Error"/> with.</param>
    /// <param name="message">The error message to construct the <see cref="Tyne.Error"/> with.</param>
    /// <param name="statusCode">
    ///     The <see cref="HttpStatusCode"/> to use.
    ///     See <see cref="HttpResult{T}.StatusCode"/> for the valid status code range.
    /// </param>
    /// <returns>An <c>Error</c> <see cref="HttpResult{T}"/> whose error is constructed using <paramref name="code"/> and <paramref name="message"/>.</returns>
    /// <exception cref="BadResultException">
    ///     When <paramref name="statusCode"/> is not valid.
    /// </exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> Error<T>(int code, string message, HttpStatusCode statusCode)
    {
        // Let Error handle a potentially null message
        var error = Tyne.Error.From(code, message);
        // HttpResult's constructor handles invalid status codes
        return new(error, statusCode);
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <paramref name="statusCode"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="error">The error to use as <see cref="Result{T}.Error"/>.</param>
    /// <param name="statusCode">
    ///     The <see cref="HttpStatusCode"/> to use.
    ///     See <see cref="HttpResult{T}.StatusCode"/> for the valid status code range.
    /// </param>
    /// <returns>An <c>Error</c> <see cref="HttpResult{T}"/> whose error is constructed using <paramref name="error"/>.</returns>
    /// <exception cref="BadResultException">
    ///     When <paramref name="statusCode"/> is not valid.
    /// </exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> Error<T>(in Error error, HttpStatusCode statusCode) =>
        // HttpResult's constructor handles invalid status codes
        new(error, statusCode);

    internal const HttpStatusCode DefaultOkStatusCode = HttpStatusCode.OK;
    internal const HttpStatusCode DefaultErrorStatusCode = HttpStatusCode.BadRequest;

    // OK status codes must be 2XX (successful)
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsValidOkStatusCode(HttpStatusCode statusCode) =>
        (HttpStatusCode)200 <= statusCode && statusCode <= (HttpStatusCode)299;

    // Error status codes must be 4XX (client error) or 5XX (server error)
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsValidErrorStatusCode(HttpStatusCode statusCode) =>
        (HttpStatusCode)400 <= statusCode && statusCode <= (HttpStatusCode)599;
}
