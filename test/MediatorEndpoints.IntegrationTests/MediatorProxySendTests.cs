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
    public async Task Send_Simple_ValidRequest_Works()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        var request = new SimpleRequest { Count = 101 };

        // Act
        var response = await mediatorProxy.Send(request);

        // Assert
        Assert.Equal(102, response.NewCount);
    }

    [Fact]
    public async Task Send_Null_Throws_UnwrapException()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        SimpleRequest request = null!;

        // Act
        Task act() =>
            mediatorProxy.Send(request);

        // Assert
        _ = await Assert.ThrowsAsync<UnwrapResultValueException>(act);
    }

    [Fact]
    public async Task Send_NoResponse_Throws_UnwrapException()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        var request = new SimpleRequest { Count = SimpleRequest.CountToReturnNoResponse };

        // Act
        Task act() =>
            mediatorProxy.Send(request);

        // Assert
        _ = await Assert.ThrowsAsync<UnwrapResultValueException>(act);
    }

    [Fact]
    public async Task Send_WrongResponse_ReturnsDefault()
    {
        using var testScope = TestWebApp.CreateTestScope();
        var (mediatorProxy, _) = testScope;

        // Arrange
        var request = new SimpleRequest { Count = SimpleRequest.CountToReturnWrongResponse };

        // Act
        var result = await mediatorProxy.Send(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SimpleResponse.DefaultNewCount, result.NewCount);
    }
}
