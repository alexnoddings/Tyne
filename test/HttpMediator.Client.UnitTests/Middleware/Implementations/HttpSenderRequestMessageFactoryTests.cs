using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tyne.HttpMediator.Client;

public class HttpSenderRequestMessageFactoryTests
{
    [Fact]
    public void HttpDelete()
    {
        AssertMethodIsCorrect<HttpDeleteRequest, HttpDeleteResponse>();
        AssertUriIsCorrect<HttpDeleteRequest, HttpDeleteResponse>();
        AssertHasNoContent<HttpDeleteRequest, HttpDeleteResponse>();
    }

    [Fact]
    public void HttpGet()
    {
        AssertMethodIsCorrect<HttpGetRequest, HttpGetResponse>();
        AssertUriIsCorrect<HttpGetRequest, HttpGetResponse>();
        AssertHasNoContent<HttpGetRequest, HttpGetResponse>();
    }

    [Fact]
    public void HttpPatch()
    {
        AssertMethodIsCorrect<HttpPatchRequest, HttpPatchResponse>();
        AssertUriIsCorrect<HttpPatchRequest, HttpPatchResponse>();
        AssertHasContent<HttpPatchRequest, HttpPatchResponse>();
    }

    [Fact]
    public void HttpPost()
    {
        AssertMethodIsCorrect<HttpPostRequest, HttpPostResponse>();
        AssertUriIsCorrect<HttpPostRequest, HttpPostResponse>();
        AssertHasContent<HttpPostRequest, HttpPostResponse>();
    }

    [Fact]
    public void HttpPut()
    {
        AssertMethodIsCorrect<HttpPutRequest, HttpPutResponse>();
        AssertUriIsCorrect<HttpPutRequest, HttpPutResponse>();
        AssertHasContent<HttpPutRequest, HttpPutResponse>();
    }

    private static void AssertHasNoContent<TRequest, TResponse>()
        where TRequest : IHttpRequestBase<TResponse>, new()
    {
        using var scope = HttpMediatorClientTestScope.Create();

        // Arrange
        var requestMessageFactory = scope.Services.GetRequiredService<HttpSenderRequestMessageFactory>();

        // Act
        var requestMessage = requestMessageFactory.CreateHttpRequestMessage(new TRequest());

        // Assert
        Assert.NotNull(requestMessage);
        Assert.Null(requestMessage.Content);
    }
    private static void AssertHasContent<TRequest, TResponse>()
        where TRequest : IHttpRequestBase<TResponse>, new()
    {
        using var scope = HttpMediatorClientTestScope.Create();

        // Arrange
        var requestMessageFactory = scope.Services.GetRequiredService<HttpSenderRequestMessageFactory>();

        // Act
        var requestMessage = requestMessageFactory.CreateHttpRequestMessage(new TRequest());

        // Assert
        Assert.NotNull(requestMessage);
        Assert.NotNull(requestMessage.Content);
    }

    private static void AssertMethodIsCorrect<TRequest, TResponse>()
        where TRequest : IHttpRequestBase<TResponse>, new()
    {
        using var scope = HttpMediatorClientTestScope.Create();

        // Arrange
        var requestMessageFactory = scope.Services.GetRequiredService<HttpSenderRequestMessageFactory>();

        // Act
        var requestMessage = requestMessageFactory.CreateHttpRequestMessage(new TRequest());

        // Assert
        Assert.NotNull(requestMessage);
        Assert.Equal(TRequest.Method, requestMessage.Method);
    }

    private static void AssertUriIsCorrect<TRequest, TResponse>()
        where TRequest : IHttpRequestBase<TResponse>, new()
    {
        using var scope = HttpMediatorClientTestScope.Create();

        // Arrange
        var httpMediatorOptions = scope.Services.GetRequiredService<IOptions<HttpMediatorOptions>>().Value;
        var requestMessageFactory = scope.Services.GetRequiredService<HttpSenderRequestMessageFactory>();

        var expectedUrl = httpMediatorOptions.GetFullApiUri<TRequest>().ToString();

        // Act
        var requestMessage = requestMessageFactory.CreateHttpRequestMessage(new TRequest());
        var actualUrl = requestMessage.RequestUri?.ToString();

        // Assert
        Assert.NotNull(requestMessage);
        // StartsWith rather than Equal as requests which URL-encode the request will be /api/path/endpoint?somequerystring
        Assert.StartsWith(expectedUrl, actualUrl, StringComparison.Ordinal);
    }
}
