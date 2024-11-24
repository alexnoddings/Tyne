namespace Tyne.MediatorEndpoints;

[Collection(TestWebAppCollection.Name)]
public class MediatorProxySendTests
{
    private TestWebAppFactory TestWebApp { get; }

    public MediatorProxySendTests(TestWebAppFactory testWebAppFactory)
    {
        TestWebApp = testWebAppFactory;
    }

    [Fact]
    public async Task Send_ValidRequest_Works()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        var request = new CountRequest { Count = 101 };

        // Act
        var response = await mediatorProxy.Send(request);

        // Assert
        Assert.Equal(102, response.NewCount);
    }

    [Fact]
    public async Task Send_NullRequest_Throws_UnwrapException()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        CountRequest request = null!;

        // Act
        Task act() =>
            mediatorProxy.Send(request);

        // Assert
        _ = await Assert.ThrowsAsync<UnwrapResultValueException>(act);
    }

    [Fact]
    public async Task Send_NoContentResponse_Throws_UnwrapException()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        var request = new NoContentRequest();

        // Act
        Task act() =>
            mediatorProxy.Send(request);

        // Assert
        _ = await Assert.ThrowsAsync<UnwrapResultValueException>(act);
    }

    [Fact]
    public async Task Send_InvalidResultResponse_ReturnsDefault()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        var request = new InvalidResultRequest();

        // Act
        var result = await mediatorProxy.Send(request);

        // Assert
        Assert.NotNull(result);
    }
}
