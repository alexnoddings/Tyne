using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace Tyne.HttpMediator.Client;

public class FluentValidationMiddlewareTests
{
    [Fact]
    public async Task Send_InvalidRequest_Fails()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create(middleware: b => b.UseFluentValidation());

        var middleware = scope.Services.GetRequiredService<FluentValidationMiddleware>();
        var next = CreateAssertFailNextDelegate();
        var request = new ValidatedRequest() { Message = ValidatedRequest.NotValidMessage };

        // Act
        var actualResult = await middleware.InvokeAsync(request, next);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.IsError(HttpStatusCode.BadRequest, actualResult);
    }

    [Fact]
    public async Task Send_ValidRequest_DefersToNext()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create(middleware: b => b.UseFluentValidation());

        var middleware = scope.Services.GetRequiredService<FluentValidationMiddleware>();
        var request = new ValidatedRequest() { Message = ValidatedRequest.ValidMessage };

        var nextCounter = 0;
        Task<HttpResult<ValidatedResponse>> Next(ValidatedRequest request)
        {
            Interlocked.Increment(ref nextCounter);
            var response = new ValidatedResponse { Message = request.Message };
            return HttpResult.Codes.OK(response).ToTask();
        }

        // Act
        var actualResult = await middleware.InvokeAsync(request, Next);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.IsOk(HttpStatusCode.OK, actualResult);
        Assert.Equal(request.Message, actualResult.Value.Message);
        Assert.Equal(1, nextCounter);
    }

    private static HttpMediatorDelegate<ValidatedRequest, ValidatedResponse> CreateAssertFailNextDelegate() =>
        _ => throw new FailException($"{nameof(FluentValidationMiddleware)} should be terminal for invalid requests, and not invoke the next delegate.");
}
