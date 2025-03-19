namespace Tyne;

public class OptionExtensionsApplyTests
{
    [Test]
    public async Task Apply1_NullSome_Throws_ArgumentNullException()
    {
        // Arrange
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();
        Action<int> some = null!;

        // Act
        void ActSome() => someOption.Apply(some);
        void ActNone() => noneOption.Apply(some);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public void Apply1_SomeOption_ExecutesSome()
    {
        // Arrange
        var option = Option.Some(42);
        var some = Substitute.For<Action<int>>();
        some.Invoke(Arg.Is(42));

        // Act
        option.Apply(some);

        // Assert
        some.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public void Apply1_NoneOption_DoesNotExecuteSome()
    {
        // Arrange
        var option = Option.None<int>();
        var some = Substitute.For<Action<int>>();

        // Act
        option.Apply(some);

        // Assert
        some.DidNotReceive().Invoke(Arg.Any<int>());
    }
}
