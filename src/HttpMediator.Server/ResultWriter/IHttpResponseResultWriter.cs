using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Tyne.Internal.HttpMediator;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     Handles writing <see cref="HttpResult{T}"/>s to <see cref="HttpContext"/>s.
/// </summary>
/// <remarks>
///     <para>
///         While this can be achieved through <see cref="JsonConverter{T}"/>,
///         this way writes <c>Ok(T)</c> results directly,
///         and transforms <c>Error</c> results into <see href="https://datatracker.ietf.org/doc/html/rfc7807"/> (<see cref="ProblemDetails"/>).
///         It also handles setting the <see cref="HttpResponse.StatusCode"/>.
///     </para>
///     <para>
///         Since this is designed for servers to write to <see cref="HttpResponse"/>, no read method is provided.
///     </para>
/// </remarks>
public interface IHttpResponseResultWriter
{
    /// <summary>
    ///     Writes <paramref name="result"/> to the <paramref name="httpContext"/>.
    /// </summary>
    /// <typeparam name="T">The inner <see cref="HttpResult{T}"/> type.</typeparam>
    /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
    /// <param name="result">The <see cref="HttpResult{T}"/> to write.</param>
    /// <param name="options">Optionally, <see cref="JsonSerializerOptions"/>. If <see langword="null"/>, injected options are used instead.</param>
    /// <param name="cancellationToken">Optionally, a <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="Task"/> representing the write.</returns>
    public Task WriteAsync<T>(HttpContext httpContext, HttpResult<T> result, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default);
}
