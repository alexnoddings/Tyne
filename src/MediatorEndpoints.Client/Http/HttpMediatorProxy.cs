using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Options;

namespace Tyne.MediatorEndpoints.Http;

/// <summary>
///		Proxies <see cref="IApiRequest{TResponse}"/>s to the server over a <see cref="System.Net.Http.HttpClient"/>.
/// </summary>
public class HttpMediatorProxy : IHttpMediatorProxy
{
    private HttpClient HttpClient { get; }
    private JsonSerializerOptions JsonSerialiserOptions { get; }

    public HttpMediatorProxy(HttpClient httpClient, IOptions<JsonSerializerOptions> jsonSerialiserOptions)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(jsonSerialiserOptions);

        HttpClient = httpClient;
        JsonSerialiserOptions = jsonSerialiserOptions.Value;
    }

    public async Task<HttpMediatorResult<TResponse>> SendHttp<TResponse>(IApiRequest<TResponse> request, HttpMediatorResponseBehaviour behaviour = HttpMediatorResponseBehaviour.Automatic, CancellationToken cancellationToken = default) =>
        await Send(request, behaviour, cancellationToken).ConfigureAwait(false);

    public async Task<TResponse> Send<TResponse>(IApiRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var result = await Send(request, HttpMediatorResponseBehaviour.Automatic, cancellationToken).ConfigureAwait(false);
        return result.Response ?? throw new InvalidOperationException($"Could not read response. De-serialising returned null, not {nameof(TResponse)}.");
    }

    private async Task<HttpMediatorResult<TResponse>> Send<TResponse>(IApiRequest<TResponse> request, HttpMediatorResponseBehaviour behaviour, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var apiRequestInfo = GetApiRequestInfo(request);

#if Tyne_MediatorEndpoints_GetSupport
        return apiRequestInfo.ApiMethod switch
        {
            ApiRequestMethod.Get => await SendGet(request, apiRequestInfo, behaviour, cancellationToken).ConfigureAwait(false),
            ApiRequestMethod.Post => await SendPost(request, apiRequestInfo, behaviour, cancellationToken).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(request), $"Invalid {nameof(ApiRequestMethod)}."),
        };
#else
        return await SendPost(request, apiRequestInfo, behaviour, cancellationToken).ConfigureAwait(false);
#endif
    }

#if Tyne_MediatorEndpoints_GetSupport
    private async Task<HttpMediatorResult<TResponse>> SendGet<TResponse>(IApiRequest<TResponse> request, ApiRequestInfo apiRequestInfo, HttpMediatorResponseBehaviour behaviour, CancellationToken cancellationToken)
    {
        var requestJson = JsonSerializer.Serialize(request, request.GetType(), JsonSerialiserOptions);
        var uriQuery = QueryString.Create("query", requestJson);

        var uriBase = "/api/" + apiRequestInfo.ApiUri;
        var uri = new Uri(uriBase + uriQuery, UriKind.Relative);

        return await SendCore<TResponse>(HttpMethod.Get, uri, null, behaviour, cancellationToken).ConfigureAwait(false);
    }
#endif

    private async Task<HttpMediatorResult<TResponse>> SendPost<TResponse>(IApiRequest<TResponse> request, ApiRequestInfo apiRequestInfo, HttpMediatorResponseBehaviour behaviour, CancellationToken cancellationToken)
    {
        var uri = new Uri("/api/" + apiRequestInfo.ApiUri, UriKind.Relative);

        using var httpContent = JsonContent.Create(request, apiRequestInfo.RequestType, options: JsonSerialiserOptions);
        return await SendCore<TResponse>(HttpMethod.Post, uri, httpContent, behaviour, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpMediatorResult<TResponse>> SendCore<TResponse>(HttpMethod method, Uri uri, HttpContent? content, HttpMediatorResponseBehaviour behaviour, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(method, uri)
        {
            Content = content
        };
        var httpResponse = await HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

        TResponse? response = default;
        if (behaviour is HttpMediatorResponseBehaviour.Automatic)
            response = await httpResponse.Content.ReadFromJsonAsync<TResponse>(JsonSerialiserOptions, cancellationToken).ConfigureAwait(false);

        return new(response, httpResponse);
    }

    // We use reflection here to make the Send API nicer.
    // Currently, the request parameter doesn't have a specific implementation type.
    // We could update the generic signature to <TRequest, TResponse> which would give us a specific impl type,
    // but then the compiler can't implicitly fill the generics, which would lead to ugly Mediator.Send<SomeRequest, SomeResonse>(...) calls.
    [SuppressMessage("Major Code Smell", "S3011: Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "We are reflecting on a method private to this class.")]
    private static readonly MethodInfo GetApiRequestInfoMethodInfo =
        typeof(HttpMediatorProxy).GetMethod(nameof(GetApiRequestInfoCore), BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"No \"{nameof(GetApiRequestInfoCore)}\" method found on \"{nameof(HttpMediatorProxy)}\".");
    private static ApiRequestInfo GetApiRequestInfo(IApiRequestMetadata request)
    {
        var methodInfo = GetApiRequestInfoMethodInfo.MakeGenericMethod(request.GetType());

        var apiRequestInfoObj = methodInfo.Invoke(null, new object[] { request });
        if (apiRequestInfoObj is ApiRequestInfo apiRequestInfo)
            return apiRequestInfo;

        throw new InvalidOperationException("Generic method invocation failed - result was null or invalid.");
    }

#if Tyne_MediatorEndpoints_GetSupport
    private sealed record ApiRequestInfo(Type RequestType, string ApiUri, ApiRequestMethod ApiMethod);
    private static ApiRequestInfo GetApiRequestInfoCore<TApiRequestMetadata>(TApiRequestMetadata metadata) where TApiRequestMetadata : IApiRequestMetadata =>
        new(metadata.GetType(), TApiRequestMetadata.Uri, TApiRequestMetadata.Method);
#else
    private sealed record ApiRequestInfo(Type RequestType, string ApiUri);
    private static ApiRequestInfo GetApiRequestInfoCore<TApiRequestMetadata>(TApiRequestMetadata metadata) where TApiRequestMetadata : IApiRequestMetadata =>
        new(metadata.GetType(), TApiRequestMetadata.Uri);
#endif
}
