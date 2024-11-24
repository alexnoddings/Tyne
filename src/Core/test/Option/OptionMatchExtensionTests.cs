namespace Tyne;

public class OptionApplyExtensionTests
{
    [Fact]
    public void MatchT_NullSome_Throws()
    {
        // Arrange
        var option = Option.None<int>();

        Func<int, int> some = null!;
        static int none() => 0;

        // Act
        void act() => option.Match(some, none);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void MatchT_NullNone_Throws()
    {
        // Arrange
        var option = Option.None<int>();

        static int some(int _) => 0;
        Func<int> none = null!;

        // Act
        void act() => option.Match(some, none);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void MatchT_None_ReturnsNone()
    {
        // Arrange
        var option = Option.None<int>();

        var some = Substitute.For<Func<int, int>>();

        var none = Substitute.For<Func<int>>();
        _ = none.Invoke().Returns(101);

        // Act
        var match = option.Match(some.Invoke, none.Invoke);

        // Assert
        Assert.Equal(101, match);
        _ = some.DidNotReceive().Invoke(Arg.Any<int>());
        _ = none.Received(1).Invoke();
    }

    [Fact]
    public void MatchT_Some_ReturnsSome()
    {
        // Arrange
        var option = Option.Some(42);

        var some = Substitute.For<Func<int, int>>();
        _ = some.Invoke(42).Returns(101);

        var none = Substitute.For<Func<int>>();

        // Act
        var match = option.Match(some.Invoke, none.Invoke);

        // Assert
        Assert.Equal(101, match);
        _ = some.Received(1).Invoke(42);
        _ = none.DidNotReceive().Invoke();
    }
}
