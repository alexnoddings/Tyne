using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.HttpMediator.Client;

public class ExceptionHandlerMiddlewareTests
{
    [Fact]
    public async Task Send_SimpleRequest_Works()
    {
        using var scope = HttpMediatorClientTestScope.Create(middleware: b => b.UseExceptionHandler());

        // Arrange
        scope.Http.Handle<SimpleRequest, SimpleResponse>(request => new SimpleResponse { NewCount = request.Count + 1 });

        var middleware = scope.Services.GetRequiredService<ExceptionHandlerMiddleware>();
        var exception = new InvalidOperationException("Some exception from an inner middleware.");
        Task<HttpResult<SimpleResponse>> next(SimpleRequest _) => throw exception;
        var request = new SimpleRequest() { Count = 101 };

        // Act
        var actualResult = await middleware.InvokeAsync(request, next);

        // Assert
        Assert.NotNull(actualResult);
        var actualError = AssertHttpResult.IsError(HttpStatusCode.BadRequest, actualResult);
        Assert.Equal(exception, actualError.CausedBy);
    }
}
