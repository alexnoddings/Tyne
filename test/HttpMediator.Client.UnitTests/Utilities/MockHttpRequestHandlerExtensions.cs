using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using RichardSzalay.MockHttp;
using System.Web;
using System.Text.Json;

namespace Tyne.HttpMediator.Client;

public static class MockHttpRequestHandlerExtensions
{
    public static MockedRequest Handle<TRequest, TResponse>(this MockHttpMessageHandler mockHttp, Func<TRequest, TResponse> requestHandler) where TRequest : IHttpRequestBase<TResponse>
    {
        var apiUri = GetApiUri<TRequest>();
        var method = TRequest.Method;

        Func<HttpRequestMessage, HttpContent> handler = TRequest.Method.Method.ToUpperInvariant() switch
        {
            "GET" or "DELETE" => HandleUrlEncodedRequest,
            "POST" or "PUT" or "PATCH" => HandleContentEncodedRequest,
            var other => throw new NotSupportedException($"Unsupported HTTP method {other}.")
        };

        return mockHttp.Expect(method, apiUri).Respond(handler);

        [SuppressMessage("AsyncUsage", "AsyncFixer02: Long-running or blocking operations inside an async method.", Justification = "Async isn't available in the handler.")]
        HttpContent HandleUrlEncodedRequest(HttpRequestMessage http)
        {
            var requestUri = http.RequestUri!.Query;
            var queryString = HttpUtility.ParseQueryString(requestUri);
            var requestStr = queryString[HttpSenderRequestMessageFactory.QueryParameterName];
            if (requestStr is null)
                throw new InvalidOperationException("Test HTTP handler could not get request from query string.");

            var request = JsonSerializer.Deserialize<TRequest>(requestStr, options: HttpMediatorClientTestScope.JsonSerializerOptions)
                ?? throw new InvalidOperationException("Test HTTP handler could not parse request from URI.");

            var response = requestHandler(request);
            return JsonContent.Create(response, options: HttpMediatorClientTestScope.JsonSerializerOptions);
        }

        [SuppressMessage("AsyncUsage", "AsyncFixer02: Long-running or blocking operations inside an async method.", Justification = "Async isn't available in the handler.")]
        HttpContent HandleContentEncodedRequest(HttpRequestMessage http)
        {
            if (http.Content is null)
                throw new InvalidOperationException("Test HTTP handler expects not-null content.");

            var request = http.Content.ReadFromJsonAsync<TRequest>(options: HttpMediatorClientTestScope.JsonSerializerOptions).Result
                ?? throw new InvalidOperationException("Test HTTP handler could not parse request body.");

            var response = requestHandler(request);
            return JsonContent.Create(response, options: HttpMediatorClientTestScope.JsonSerializerOptions);
        }
    }

    // Used to add the "**" wildcard to the end of GETs or DELETEs so that requests with query parameters are properly mapped
    private static string GetMethodWildcard<TRequest>() where TRequest : IHttpRequestMetadata =>
        TRequest.Method.Method.ToUpperInvariant() is "GET" or "DELETE"
        ? "**"
        : string.Empty;

    private static string GetApiUri<TRequest>() where TRequest : IHttpRequestMetadata =>
        HttpMediatorClientTestScope.BaseAddress + HttpMediatorClientTestScope.ApiBase + TRequest.Uri + GetMethodWildcard<TRequest>();

}
