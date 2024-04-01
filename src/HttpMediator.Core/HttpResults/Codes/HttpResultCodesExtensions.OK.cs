using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.CompilerServices;

namespace Tyne.HttpMediator;

public static partial class HttpResultCodesExtensions
{
    /// <summary>
    ///     Creates an <c>Ok(<typeparamref name="T"/>)</c> <see cref="HttpResult{T}"/>
    ///     (i.e. <see cref="Result{T}.IsOk"/> is <see langword="true"/>)
    ///     with <see cref="HttpStatusCode.OK"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the result encapsulates.</typeparam>
    /// <param name="_">The <see cref="HttpResultCodes"/>.</param>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <returns>An <c>Ok(<typeparamref name="T"/>)</c> <see cref="HttpResult{T}"/> which wraps <paramref name="value"/>.</returns>
    /// <remarks>
    ///     A <see cref="BadResultException"/> will be thrown if <paramref name="value"/> is <see langword="null"/>.
    /// </remarks>
    /// <exception cref="BadResultException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HttpResult<T> OK<T>(this HttpResultCodes _, in T value) =>
        new(value, HttpStatusCode.OK);
}
