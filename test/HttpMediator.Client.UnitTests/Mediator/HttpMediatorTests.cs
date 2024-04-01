using System.Net;

namespace Tyne.HttpMediator.Client;

public class HttpMediatorTests
{
    [Fact]
    public async Task NullRequest_ReturnsBadRequest()
    {
        using var scope = HttpMediatorClientTestScope.Create();

        // Arrange
        var httpMediator = scope.Mediator;
        SimpleRequest request = null!;

        // Act
        var result = await httpMediator.SendAsync(request);

        // Assert
        AssertHttpResult.IsError(HttpStatusCode.BadRequest, result);
    }
}
