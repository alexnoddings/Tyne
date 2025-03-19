namespace Tyne;

public class OptionExtensionsOrTests
{
    [Test]
    public async Task OrValue_NullValue_Throws_ArgumentNullException()
    {
        // Arrange
        var someOption = Option.Some("some");
        var noneOption = Option.None<string>();
        string value = null!;

        // Act
        void ActSome() => _ = someOption.Or(value);
        void ActNone() => _ = noneOption.Or(value);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrValue_SomeOption_ReturnsOptionValue()
    {
        // Arrange
        var option = Option.Some(42);
        var value = 101;

        // Act
        var or = option.Or(value);

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrValue_NoneOption_ReturnsOrValue()
    {
        // Arrange
        var option = Option.None<int>();
        var value = 101;

        // Act
        var or = option.Or(value);

        // Assert
        await Assert.That(or).IsEqualTo(101);
    }

    [Test]
    public async Task OrValueFactory_NullValueFactory_Throws_ArgumentNullException()
    {
        // Arrange
        var someOption = Option.Some("some");
        var noneOption = Option.None<string>();
        Func<string> valueFactory = null!;

        // Act
        void ActSome() => _ = someOption.Or(valueFactory);
        void ActNone() => _ = noneOption.Or(valueFactory);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrValueFactory_SomeOption_ReturnsOptionValue()
    {
        // Arrange
        var option = Option.Some(42);
        var valueFactory = Substitute.For<Func<int>>();

        // Act
        var or = option.Or(valueFactory);

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrValueFactory_NoneOption_ReturnsValueFactory()
    {
        // Arrange
        var option = Option.None<int>();
        var valueFactory = Substitute.For<Func<int>>();
        valueFactory.Invoke().Returns(101);

        // Act
        var or = option.Or(valueFactory);

        // Assert
        await Assert.That(or).IsEqualTo(101);
        valueFactory.Received(1).Invoke();
    }

    [Test]
    public async Task OrDefault_SomeOption_ReturnsOptionValue()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var or = option.OrDefault();

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrDefault_NoneOption_ReturnsDefault()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var or = option.OrDefault();

        // Assert
        await Assert.That(or).IsEqualTo(0);
    }

    [Test]
    public async Task OrNull_SomeOption_ReturnsOptionValue()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var or = option.OrNull();

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrNull_NoneOption_ReturnsNull()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var or = option.OrNull();

        // Assert
        await Assert.That(or).IsEqualTo(null);
    }
}
