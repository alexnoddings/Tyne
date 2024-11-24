namespace Tyne;

public class OptionOrExtensionTests
{
    [Fact]
    public void OrValue_None_Null_Throws()
    {
        // Arrange
        var option = Option.None<int?>();
        int? value = null;

        // Act
        void act() => _ = option.Or(value);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void OrValue_None_ReturnsValue()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var or = option.Or(101);

        // Assert
        Assert.Equal(101, or);
    }

    [Fact]
    public void OrValue_Some_Null_Throws()
    {
        // Arrange
        var option = Option.Some<int?>(42);
        int? value = null;

        // Act
        void act() => _ = option.Or(value);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void OrValue_Some_ReturnsSome()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var or = option.Or(101);

        // Assert
        Assert.Equal(42, or);
    }

    [Fact]
    public void OrValueFactory_None_Null_Throws()
    {
        // Arrange
        var option = Option.None<int>();
        Func<int> valueFactory = null!;

        // Act
        void act() => _ = option.Or(valueFactory);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void OrValueFactory_None_ReturnsValueFactory()
    {
        // Arrange
        var option = Option.None<int>();

        var valueFactory = Substitute.For<Func<int>>();
        _ = valueFactory.Invoke().Returns(101);

        // Act
        var or = option.Or(valueFactory);

        // Assert
        Assert.Equal(101, or);
        _ = valueFactory.Received(1).Invoke();
    }

    [Fact]
    public void OrValueFactory_Some_Null_Throws()
    {
        // Arrange
        var option = Option.Some(42);
        Func<int> valueFactory = null!;

        // Act
        void act() => _ = option.Or(valueFactory);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void OrValueFactory_Some_ReturnsSome()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var or = option.Or(() => throw new InvalidOperationException("Value factory should not be invoked for Some option."));

        // Assert
        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_None_ValueType_ReturnsZero()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var or = option.OrDefault();

        // Assert
        Assert.Equal(0, or);
    }

    [Fact]
    public void OrDefault_None_NullableValueType_ReturnsNull()
    {
        // Arrange
        var option = Option.None<int?>();

        // Act
        var or = option.OrDefault();

        // Assert
        Assert.Null(or);
    }

    [Fact]
    public void OrDefault_None_ReferenceType_ReturnsNull()
    {
        // Arrange
        var option = Option.None<string?>();

        // Act
        var or = option.OrDefault();

        // Assert
        Assert.Null(or);
    }

    [Fact]
    public void OrDefault_Some_ValueType_ReturnsSome()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var or = option.OrDefault();

        // Assert
        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_Some_NullableValueType_ReturnsSome()
    {
        // Arrange
        var option = Option.Some<int?>(42);

        // Act
        var or = option.OrDefault();

        // Assert
        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_Some_ReferenceType_ReturnsSome()
    {
        // Arrange
        var option = Option.Some<string?>("abc");

        // Act
        var or = option.OrDefault();

        // Assert
        Assert.Equal("abc", or);
    }
}
