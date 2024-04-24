using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace Tyne.HttpMediator.Client;

public class HttpSenderMiddlewareTests
{
    [Fact]
    public async Task Send_SimpleRequest_Works()
    {
        using var scope = HttpMediatorClientTestScope.Create();

        // Arrange
        _ = scope.Http.Handle<SimpleRequest, SimpleResponse>(request => new SimpleResponse { NewCount = request.Count + 1 });

        var middleware = scope.Services.GetRequiredService<HttpSenderMiddleware>();
        var next = CreateAssertFailNextDelegate<SimpleRequest, SimpleResponse>();
        var request = new SimpleRequest() { Count = 101 };

        // Act
        var actualResult = await middleware.InvokeAsync(request, next);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsOk(HttpStatusCode.OK, actualResult);
        Assert.Equal(102, actualResult.Value.NewCount);
    }

    [Fact]
    public Task Send_HttpDelete_Works() =>
        Send_HttpRequest_Works<HttpDeleteRequest, HttpDeleteResponse>();

    [Fact]
    public Task Send_HttpGet_Works() =>
        Send_HttpRequest_Works<HttpGetRequest, HttpGetResponse>();

    [Fact]
    public Task Send_HttpPatch_Works() =>
        Send_HttpRequest_Works<HttpPatchRequest, HttpPatchResponse>();

    [Fact]
    public Task Send_HttpPost_Works() =>
        Send_HttpRequest_Works<HttpPostRequest, HttpPostResponse>();

    [Fact]
    public Task Send_HttpPut_Works() =>
        Send_HttpRequest_Works<HttpPutRequest, HttpPutResponse>();

    private static async Task Send_HttpRequest_Works<TRequest, TResponse>()
        where TRequest : TestHttpRequestBase, IHttpRequestBase<TResponse>, new()
        where TResponse : TestHttpResponseBase, new()
    {
        using var scope = HttpMediatorClientTestScope.Create();

        // Arrange
        _ = scope.Http.Handle<TRequest, TResponse>(request => new TResponse { NewCount = request.Count + 1 });

        var middleware = scope.Services.GetRequiredService<HttpSenderMiddleware>();
        var next = CreateAssertFailNextDelegate<TRequest, TResponse>();
        var request = new TRequest() { Count = 101 };

        // Act
        var actualResult = await middleware.InvokeAsync(request, next);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsOk(HttpStatusCode.OK, actualResult);
        Assert.Equal(102, actualResult.Value.NewCount);
    }

    private static HttpMediatorDelegate<TRequest, TResponse> CreateAssertFailNextDelegate<TRequest, TResponse>()
        where TRequest : IHttpRequestBase<TResponse> =>
        _ => throw FailException.ForFailure($"{nameof(HttpSenderMiddleware)} should be terminal and not invoke the next delegate.");
}
