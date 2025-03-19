namespace Tyne;

public class OptionExtensionsMatchTests
{
    [Test]
    public async Task Match_NullSome_Throws_ArgumentNullException()
    {
        // Arrange
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();
        Func<int, string> some = null!;
        var none = Substitute.For<Func<string>>();

        // Act
        void ActSome() => _ = someOption.Match(some, none);
        void ActNone() => _ = noneOption.Match(some, none);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_NullNone_Throws_ArgumentNullException()
    {
        // Arrange
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();
        var some = Substitute.For<Func<int, string>>();
        Func<string> none = null!;

        // Act
        void ActSome() => _ = someOption.Match(some, none);
        void ActNone() => _ = noneOption.Match(some, none);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_SomeOption_ExecutesSome()
    {
        // Arrange
        const int someValue = 42;
        var option = Option.Some(someValue);
        var some = Substitute.For<Func<int, int>>();
        some.Invoke(someValue).Returns(101);
        var none = Substitute.For<Func<int>>();

        // Act
        var value = option.Match(some.Invoke, none.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        some.Received(1).Invoke(someValue);
        none.DidNotReceive().Invoke();
    }

    [Test]
    public async Task Match_NoneOption_ExecutesNone()
    {
        // Arrange
        var option = Option.None<int>();
        var some = Substitute.For<Func<int, int>>();
        var none = Substitute.For<Func<int>>();
        _ = none.Invoke().Returns(101);

        // Act
        var value = option.Match(some.Invoke, none.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        some.DidNotReceive().Invoke(Arg.Any<int>());
        none.Received(1).Invoke();
    }
}
