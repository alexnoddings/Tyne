using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

/// <summary>
///     Terminal middleware which sends requests to the <see cref="HttpClient"/>.
/// </summary>
internal sealed class HttpSenderMiddleware : IHttpMediatorMiddleware
{
    private readonly ILogger _logger;
    private readonly HttpSenderRequestMessageFactory _httpRequestMessageFactory;
    private readonly HttpClient _httpClient;
    private readonly IHttpResponseResultReader _httpResponseResultReader;

    public HttpSenderMiddleware(
        ILogger<HttpSenderMiddleware> logger,
        HttpSenderRequestMessageFactory httpRequestMessageFactory,
        HttpClient httpClient,
        IHttpResponseResultReader httpResponseResultReader
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpRequestMessageFactory = httpRequestMessageFactory ?? throw new ArgumentNullException(nameof(httpRequestMessageFactory));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpResponseResultReader = httpResponseResultReader ?? throw new ArgumentNullException(nameof(httpResponseResultReader));
    }

    public async Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>
    {
        using var requestMessage = _httpRequestMessageFactory.CreateHttpRequestMessage(request);
        var requestUri = requestMessage.RequestUri switch
        {
            { IsAbsoluteUri: true } uri => uri.PathAndQuery,
            { IsAbsoluteUri: false } uri => uri.ToString(),
            _ => null
        };
        _logger.LogSendingHttpRequest(requestUri, request);
        using var responseMessage = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        return await _httpResponseResultReader.ReadAsync<TResponse>(responseMessage).ConfigureAwait(false);
    }
}
