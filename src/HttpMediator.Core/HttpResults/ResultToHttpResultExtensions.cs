using System.Net;

namespace Tyne.HttpMediator;

/// <summary>
///     Extension methods for creating <see cref="HttpResult{T}"/>s from <see cref="Result{T}"/>s.
/// </summary>
public static class ResultToHttpResultExtensions
{
    /// <summary>
    ///     Creates an <see cref="HttpResult{T}"/> from <paramref name="result"/>
    ///     whose <see cref="HttpResult{T}.StatusCode"/> is <paramref name="statusCode"/>.
    /// </summary>
    /// <typeparam name="T">The type of <c>Ok(<typeparamref name="T"/>)</c> value the <paramref name="result"/> encapsulates.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}"/>.
    /// </param>
    /// <param name="statusCode">
    ///     The <see cref="HttpStatusCode"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="HttpResult{T}"/> from <paramref name="result"/> and <paramref name="statusCode"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">When <paramref name="result"/> is <see langword="null"/>.</exception>
    /// <exception cref="BadResultException">
    ///     When <paramref name="result"/> is <c>Ok(<typeparamref name="T"/>)</c> and <paramref name="statusCode"/> indicates an error;
    ///     or <paramref name="result"/> is Error and <paramref name="statusCode"/> indicates an OK result.
    /// </exception>
    public static HttpResult<T> ToHttpResult<T>(this Result<T> result, HttpStatusCode statusCode)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.IsOk)
            return HttpResult.Ok(result.Value, statusCode);

        return HttpResult.Error<T>(result.Error, statusCode);
    }
}
