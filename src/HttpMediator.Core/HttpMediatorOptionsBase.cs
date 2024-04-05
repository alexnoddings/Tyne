using System.Diagnostics.CodeAnalysis;

namespace Tyne.HttpMediator;

/// <summary>
///     Provides options to be used with HTTP Mediator which are common across the client and server.
/// </summary>
public abstract class HttpMediatorOptionsBase
{
    private string _apiBase = "/api/";

    /// <summary>
    ///     The base API path.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This should be well-formed such that it both starts and ends with a '/'.
    ///         If it does not, it will be formatted appropriately.
    ///         Additionally, <see cref="string.Empty"/> is not a valid API base; '/' will be used instead.
    ///     </para>
    ///     <para>
    ///         Defaults to <c>"/api/"</c>.
    ///     </para>
    /// </remarks>
    public string ApiBase
    {
        get => _apiBase;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _apiBase = NormaliseApiBase(value);
        }
    }

    /// <summary>
    ///     Forms a relative <see cref="Uri"/> based on <see cref="ApiBase"/> and <paramref name="partialUri"/>.
    /// </summary>
    /// <param name="partialUri">The partial URI after the base.</param>
    /// <returns>A relative <see cref="Uri"/> based on <see cref="ApiBase"/> and <paramref name="partialUri"/>.</returns>
    [SuppressMessage("Design", "CA1054: URI-like parameters should not be strings.", Justification = "This is only called by APIs which store the URI as a string.")]
    public Uri GetFullApiUri(string partialUri)
    {
        ArgumentNullException.ThrowIfNull(partialUri);

        var fullUri = ApiBase + partialUri.Trim('/');
        return new Uri(fullUri, UriKind.Relative);
    }

    /// <summary>
    ///     Forms a relative <see cref="Uri"/> based on <see cref="ApiBase"/> and <typeparamref name="TRequest"/>'s <see cref="IHttpRequestMetadata.Uri"/>.
    /// </summary>
    /// <typeparam name="TRequest">The <see cref="IHttpRequestMetadata"/>.</typeparam>
    /// <returns>A relative <see cref="Uri"/> based on <see cref="ApiBase"/> and <typeparamref name="TRequest"/>'s <see cref="IHttpRequestMetadata.Uri"/>.</returns>
    public Uri GetFullApiUri<TRequest>() where TRequest : IHttpRequestMetadata =>
        GetFullApiUri(TRequest.Uri);

    private static string NormaliseApiBase(string apiBase)
    {
        if (apiBase.Length == 0)
        {
            return "/";
        }

        if (!apiBase.StartsWith('/'))
            apiBase = "/" + apiBase;

        if (!apiBase.EndsWith('/'))
            apiBase += "/";

        return apiBase;
    }
}
