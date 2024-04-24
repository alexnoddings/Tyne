namespace Tyne;

public class ResultSelectExtensionTests
{
    [Fact]
    public void Select_NullSelector_Throws()
    {
        var result = Result.Ok(42);
        Func<int, int> selector = null!;

        _ = AssertExt.ThrowsArgumentNullException(() => result.Select(selector));
    }

    [Fact]
    public void Select_Error_ReturnsError()
    {
        var result = Result.Error<int>(TestError.Instance);

        var selector = Substitute.For<Func<int, int>>();

        var selected = result.Select(selector);

        _ = AssertResult.IsError(TestError.Instance, selected);
    }

    [Fact]
    public void Select_Ok_ReturnsOkValue()
    {
        var result = Result.Ok(42);

        var selector = Substitute.For<Func<int, int>>();
        _ = selector.Invoke(42).Returns(101);

        var selected = result.Select(selector);

        _ = AssertResult.IsOk(101, selected);
        _ = selector.Received(1).Invoke(42);
    }
}
