namespace Tyne;

public class OptionSelectExtensionTests
{
    [Fact]
    public void Select_NullSelector_Throws()
    {
        var option = Option.Some(42);
        Func<int, int> selector = null!;

        AssertExt.ThrowsArgumentNullException(() => option.Select(selector));
    }

    [Fact]
    public void Select_None_ReturnsNone()
    {
        var option = Option.None<int>();

        var selector = Substitute.For<Func<int, int>>();

        var selected = option.Select(selector);

        AssertOption.IsNone(selected);
        selector.DidNotReceive().Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Select_Some_ReturnsSomeValue()
    {
        var option = Option.Some(42);

        var selector = Substitute.For<Func<int, int>>();
        selector.Invoke(42).Returns(101);

        var selected = option.Select(selector);

        AssertOption.IsSome(101, selected);
        selector.Received(1).Invoke(42);
    }
}
