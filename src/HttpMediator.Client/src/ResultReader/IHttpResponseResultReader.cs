using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne.HttpMediator.Client;

/// <summary>
///     Handles reading <see cref="HttpResult{T}"/>s from <see cref="HttpResponseMessage"/>s.
/// </summary>
/// <remarks>
///     <para>
///         This should be used instead of going through <see cref="JsonConverter{T}"/>s
///         as the server implementation serialises <see cref="HttpResult{T}"/>s
///         as their raw value, or as a problem details.
///     </para>
///     <para>
///         Since this is designed for clients to read from <see cref="HttpResponseMessage"/>s, no write method is provided.
///     </para>
/// </remarks>
public interface IHttpResponseResultReader
{
    /// <summary>
    ///     Reads a <see cref="HttpResult{T}"/> from the <paramref name="response"/>.
    /// </summary>
    /// <typeparam name="T">The inner <see cref="HttpResult{T}"/> type.</typeparam>
    /// <param name="response">The <see cref="HttpResponseMessage"/>.</param>
    /// <param name="options">Optionally, <see cref="JsonSerializerOptions"/>. If <see langword="null"/>, injected options are used instead.</param>
    /// <param name="cancellationToken">Optionally, a <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="Task"/> whose result is the <see cref="HttpResult{T}"/>.</returns>
    public Task<HttpResult<T>> ReadAsync<T>(HttpResponseMessage response, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default);
}
