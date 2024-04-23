using System.Net;

namespace Tyne.HttpMediator;

[Collection(TestWebAppCollection.Name)]
public class HttpMediatorSendAsync
{
    private TestWebAppFactory TestWebApp { get; }

    public HttpMediatorSendAsync(TestWebAppFactory testWebAppFactory)
    {
        TestWebApp = testWebAppFactory;
    }

    [Fact]
    public async Task Send_Simple_ValidRequest_Works()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (httpMediator, _) = testScope;

        // Arrange
        var request = new SimpleRequest { Count = 101 };

        // Act
        var httpResult = await httpMediator.SendAsync(request);

        // Assert
        var value = AssertHttpResult.IsOk(HttpStatusCode.OK, httpResult);
        Assert.Equal(102, value.NewCount);
    }

    [Fact]
    public async Task Send_Validated_ValidRequest_Works()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (httpMediator, _) = testScope;

        // Arrange
        var request = new ValidatedRequest { Message = ValidatedRequest.ValidMessage };

        // Act
        var httpResult = await httpMediator.SendAsync(request);

        // Assert
        var value = AssertHttpResult.IsOk(HttpStatusCode.OK, httpResult);
        Assert.Equal(request.Message, value.Message);
    }

    [Fact]
    public async Task Send_Validated_InvalidRequest_ReturnsBadRequest()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (httpMediator, _) = testScope;

        // Arrange
        var request = new ValidatedRequest { Message = ValidatedRequest.NotValidMessage };

        // Act
        var httpResult = await httpMediator.SendAsync(request);

        // Assert
        _ = AssertHttpResult.IsError(HttpStatusCode.BadRequest, httpResult);
    }

    [Fact]
    public async Task Send_GET() =>
        await SendRequestAsync<HttpGetRequest, HttpGetResponse>();

    [Fact]
    public async Task Send_DELETE() =>
        await SendRequestAsync<HttpDeleteRequest, HttpDeleteResponse>();

    [Fact]
    public async Task Send_POST() =>
        await SendRequestAsync<HttpPostRequest, HttpPostResponse>();

    [Fact]
    public async Task Send_PUT() =>
        await SendRequestAsync<HttpPutRequest, HttpPutResponse>();

    [Fact]
    public async Task Send_PATCH() =>
        await SendRequestAsync<HttpPatchRequest, HttpPatchResponse>();

    private async Task SendRequestAsync<TRequest, TResponse>()
        where TRequest : TestHttpRequestBase, IHttpRequest<TResponse>, new()
        where TResponse : TestHttpResponseBase
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (httpMediator, _) = testScope;

        // Arrange
        var request = new TRequest()
        {
            Count = 42
        };

        // Act
        var httpResult = await httpMediator.SendAsync(request);

        // Assert
        _ = AssertHttpResult.IsOk(HttpStatusCode.OK, httpResult);
        Assert.Equal(43, httpResult.Value.NewCount);
    }
}
