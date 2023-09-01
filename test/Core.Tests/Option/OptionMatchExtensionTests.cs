namespace Tyne;

public class OptionApplyExtensionTests
{
    [Fact]
    public void MatchT_NullSome_Throws()
    {
        var option = Option.None<int>();
        Func<int, int> some = null!;
        Func<int> none = () => 0;

        AssertExt.ThrowsArgumentNullException(() => option.Match(some, none));
    }

    [Fact]
    public void MatchT_NullNone_Throws()
    {
        var option = Option.None<int>();
        Func<int, int> some = (_) => 0;
        Func<int> none = null!;

        AssertExt.ThrowsArgumentNullException(() => option.Match(some, none));
    }

    [Fact]
    public void MatchT_None_ReturnsNone()
    {
        var option = Option.None<int>();

        var some = Substitute.For<Func<int, int>>();

        var none = Substitute.For<Func<int>>();
        none.Invoke().Returns(101);

        var match = option.Match(some.Invoke, none.Invoke);

        Assert.Equal(101, match);
        some.DidNotReceive().Invoke(Arg.Any<int>());
        none.Received(1).Invoke();
    }

    [Fact]
    public void MatchT_Some_ReturnsSome()
    {
        var option = Option.Some(42);

        var some = Substitute.For<Func<int, int>>();
        some.Invoke(42).Returns(101);

        var none = Substitute.For<Func<int>>();

        var match = option.Match(some.Invoke, none.Invoke);

        Assert.Equal(101, match);
        some.Received(1).Invoke(42);
        none.DidNotReceive().Invoke();
    }
}
