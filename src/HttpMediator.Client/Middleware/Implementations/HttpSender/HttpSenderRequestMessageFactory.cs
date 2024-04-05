using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Options;

namespace Tyne.HttpMediator.Client;

/// <summary>
///     Factory for generating <see cref="HttpRequestMessage"/>s for the <see cref="HttpSenderMiddleware"/>.
/// </summary>
internal sealed class HttpSenderRequestMessageFactory
{
    public const string QueryParameterName = "request";

    private readonly IOptions<HttpMediatorOptions> _httpMediatorOptions;
    private readonly IOptions<JsonSerializerOptions> _jsonSerialiserOptions;

    public HttpSenderRequestMessageFactory(IOptions<HttpMediatorOptions> httpMediatorOptions, IOptions<JsonSerializerOptions> jsonSerialiserOptions)
    {
        ArgumentNullException.ThrowIfNull(httpMediatorOptions);
        ArgumentNullException.ThrowIfNull(jsonSerialiserOptions);

        _httpMediatorOptions = httpMediatorOptions;
        _jsonSerialiserOptions = jsonSerialiserOptions;
    }

    /// <summary>
    ///     Creates a <see cref="HttpRequestMessage"/> for the <paramref name="request"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of response which <paramref name="request"/> will generate.</typeparam>
    /// <param name="request">The <see cref="IHttpRequestBase{TResponse}"/> to send.</param>
    /// <returns>A <see cref="HttpRequestMessage"/> which contains the <paramref name="request"/>.</returns>
    /// <exception cref="NotSupportedException">When an invalid <see cref="IHttpRequestMetadata.Method"/> is present on <paramref name="request"/>.</exception>
    /// <remarks>
    ///     This will generate a <see cref="HttpRequestMessage"/> with:
    ///     <list type="bullet">
    ///         <item>The method set</item>
    ///         <item>The URL set</item>
    ///         <item>The body content set (if appropriate)</item>
    ///     </list>
    /// </remarks>
    public HttpRequestMessage CreateHttpRequestMessage<TResponse>(IHttpRequestBase<TResponse> request)
    {
        var requestData = GetRequestData(request);
        return requestData.Method.Method.ToUpperInvariant() switch
        {
            "GET" or "DELETE" => CreateUrlSerialisedRequest(request, requestData),
            "POST" or "PUT" or "PATCH" => CreateContentSerialisedRequest(request, requestData),
            var other => throw new NotSupportedException($"Unsupported API HTTP method {other}.")
        };
    }

    private HttpRequestMessage CreateUrlSerialisedRequest<TResponse>(IHttpRequestBase<TResponse> request, RequestData requestData)
    {
        var serialisedRequestBytes = JsonSerializer.SerializeToUtf8Bytes(request, requestData.RequestType, _jsonSerialiserOptions.Value);
        var encodedRequest = HttpUtility.UrlEncode(serialisedRequestBytes);
        var queryString = $"?{QueryParameterName}={encodedRequest}";
        var requestUri = requestData.Uri + queryString;

        return new HttpRequestMessage(requestData.Method, requestUri);
    }

    private static readonly MediaTypeHeaderValue JsonMediaTypeHeader = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);
    private HttpRequestMessage CreateContentSerialisedRequest<TResponse>(IHttpRequestBase<TResponse> request, RequestData requestData)
    {
        var content = JsonContent.Create(request, requestData.RequestType, mediaType: JsonMediaTypeHeader, options: _jsonSerialiserOptions.Value);
        return new HttpRequestMessage(requestData.Method, requestData.Uri)
        {
            Content = content
        };
    }

    private sealed record RequestData(Type RequestType, string Uri, HttpMethod Method);

    // Using reflection here makes the Send API surface much easier to consume.
    // Currently, the API uses:
    //      Send<TResponse>(IApiRequest<TResponse>)
    // Which allows a caller to call:
    //      mediatorProxy.Send(request)
    // If the API was:
    //      Send<TRequest, TResponse>(TRequest) where TRequest : IApiRequest<TResponse>
    // Then the TResponse can't be inferred by the compiler, requiring callers to use:
    //      mediatorProxy.Send<SomeRequest, SomeResponse>(request)
    // Which is very ugly and unwieldy.
    // Instead, the caller passes in an object of type IApiRequest<TResponse>,
    // and we use reflection to work backwards to the missing TRequest argument
    private RequestData GetRequestData(IHttpRequestMetadata request)
    {
        var genericMethodInfo = GetRequestDataCoreMethodInfo.MakeGenericMethod(request.GetType());

        object? requestDataObj;
        try
        {
            requestDataObj = genericMethodInfo.Invoke(this, []);
        }
        catch (TargetInvocationException invocationException)
        {
            // Reflection wraps exceptions in a TargetInvocationException,
            // unwrap the base exception to expose for the stack trace
            throw invocationException.GetBaseException();
        }

        return (RequestData)requestDataObj!;
    }

    [SuppressMessage("Major Code Smell", "S3011: Reflection should not be used to increase accessibility of classes, methods, or fields.", Justification = "We are reflecting on a method private to this class.")]
    private static readonly MethodInfo GetRequestDataCoreMethodInfo =
        typeof(HttpSenderRequestMessageFactory)
        .GetMethod(nameof(GetRequestDataCore), BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"No \"{nameof(GetRequestDataCore)}\" method found on \"{nameof(HttpSenderRequestMessageFactory)}\".");

    private RequestData GetRequestDataCore<TRequest>() where TRequest : IHttpRequestMetadata
    {
        var requestType = typeof(TRequest);
        var uri = _httpMediatorOptions.Value.GetFullApiUri<TRequest>();
        var method = TRequest.Method;
        return new(requestType, uri.ToString(), method);
    }
}
