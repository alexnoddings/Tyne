using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.CompilerServices;

namespace Tyne.HttpMediator;

public static partial class HttpResultCodesExtensions
{
    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <see cref="HttpStatusCode.BadRequest"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="_">The <see cref="HttpResultCodes"/>.</param>
    /// <param name="message">The error message to construct the <see cref="Result{T}.Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="HttpResult{T}"/> whose error is constructed using <paramref name="message"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> BadRequest<T>(this HttpResultCodes _, string message) =>
        HttpResult.Error<T>(message, HttpStatusCode.BadRequest);

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <see cref="HttpStatusCode.BadRequest"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="_">The <see cref="HttpResultCodes"/>.</param>
    /// <param name="code">The error code to construct the <see cref="Error"/> with.</param>
    /// <param name="message">The error message to construct the <see cref="Error"/> with.</param>
    /// <returns>An <c>Error</c> <see cref="HttpResult{T}"/> whose error is constructed using <paramref name="code"/> and <paramref name="message"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> BadRequest<T>(this HttpResultCodes _, string code, string message) =>
        HttpResult.Error<T>(code, message, HttpStatusCode.BadRequest);

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <see cref="HttpStatusCode.BadRequest"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="_">The <see cref="HttpResultCodes"/>.</param>
    /// <param name="error">The <see cref="Error"/> to use.</param>
    /// <returns>An <c>Error</c> <see cref="HttpResult{T}"/> whose error is constructed using <paramref name="error"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> BadRequest<T>(this HttpResultCodes _, in Error error) =>
        HttpResult.Error<T>(in error, HttpStatusCode.BadRequest);
}
