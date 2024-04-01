using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.CompilerServices;

namespace Tyne.HttpMediator;

public static partial class HttpResultCodesExtensions
{
    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <see cref="HttpStatusCode.InternalServerError"/>.
    /// </summary>
    /// <inheritdoc cref="BadRequest{T}(HttpResultCodes, string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> InternalServerError<T>(this HttpResultCodes _, string message) =>
        HttpResult.Error<T>(message, HttpStatusCode.InternalServerError);

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <see cref="HttpStatusCode.InternalServerError"/>.
    /// </summary>
    /// <inheritdoc cref="BadRequest{T}(HttpResultCodes, int, string)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> InternalServerError<T>(this HttpResultCodes _, int code, string message) =>
        HttpResult.Error<T>(code, message, HttpStatusCode.InternalServerError);

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="false"/>)
    ///     with <see cref="HttpStatusCode.InternalServerError"/>.
    /// </summary>
    /// <inheritdoc cref="BadRequest{T}(HttpResultCodes, in Error)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> InternalServerError<T>(this HttpResultCodes _, in Error error) =>
        HttpResult.Error<T>(error, HttpStatusCode.InternalServerError);
}
