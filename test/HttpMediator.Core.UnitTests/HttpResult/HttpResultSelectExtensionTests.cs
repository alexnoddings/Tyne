using System.Net;

namespace Tyne.HttpMediator;

public class ResultSelectExtensionTests
{
    [Fact]
    public void Select_NullSelector_Throws()
    {
        // Arrange
        var result = HttpResult.Ok(42, HttpStatusCode.OK);
        Func<int, int> selector = null!;

        // Act and assert
        AssertExt.ThrowsArgumentNullException(() => result.Select(selector));
    }

    [Fact]
    public void Select_Error_ReturnsError()
    {
        // Arrange
        var result = HttpResult.Error<int>(TestError.Instance, TestError.StatusCode);
        var selector = Substitute.For<Func<int, int>>();

        // Act
        var selected = result.Select(selector);

        // Assert
        AssertResult.IsError(TestError.Instance, selected);
        Assert.Equal(TestError.StatusCode, selected.StatusCode);
    }

    [Fact]
    public void Select_Ok_ReturnsOkValue()
    {
        // Arrange
        var result = HttpResult.Ok(42, HttpStatusCode.OK);
        var selector = Substitute.For<Func<int, int>>();
        selector.Invoke(42).Returns(101);

        // Act
        var selected = result.Select(selector);

        // Assert
        AssertResult.IsOk(101, selected);
        selector.Received(1).Invoke(42);
        Assert.Equal(HttpStatusCode.OK, selected.StatusCode);
    }
}
