namespace Tyne;

public class OptionCreationTests
{
#pragma warning disable TYNE001
    [Test]
    public async Task EmptyConstructor_ValueType_IsNone()
    {
        // Act
        var option = new Option<int>();

        // Assert
        await Assert.That(option).IsNone();
    }

    [Test]
    public async Task EmptyConstructor_NullableValueType_IsNone()
    {
        // Act
        var option = new Option<int?>();

        // Assert
        await Assert.That(option).IsNone();
    }

    [Test]
    public async Task EmptyConstructor_ReferenceType_IsNone()
    {
        // Act
        var option = new Option<string>();

        // Assert
        await Assert.That(option).IsNone();
    }

    [Test]
    public async Task Uninitialized_ValueType_IsNone()
    {
        // Act
        Option<int> option = default;

        // Assert
        await Assert.That(option).IsNone();
    }

    [Test]
    public async Task Uninitialized_NullableValueType_IsNone()
    {
        // Act
        Option<int?> option = default;

        // Assert
        await Assert.That(option).IsNone();
    }

    [Test]
    public async Task Uninitialized_ReferenceType_IsNone()
    {
        // Act
        Option<string> option = default;

        // Assert
        await Assert.That(option).IsNone();
    }
#pragma warning restore TYN0001

    [Test]
    public async Task Some_Null_Throws_ArgumentNullException()
    {
        // Arrange
        int? value = null;

        // Act
        Action act = () => _ = Option.Some(value!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Some_Cached_Unit()
    {
        // Arrange
        var value = Unit.Value;

        // Act
        var option = Option.Some(value);

        // Assert
        await Assert.That(option).IsSome(value);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Some_Cached_Bool(bool value)
    {
        // Act
        var option = Option.Some(value);

        // Assert
        await Assert.That(option).IsSome(value);
    }

    [Test]
    public async Task Some_Cached_Int()
    {
        // Arrange
        var value = 0;

        // Act
        var option = Option.Some(value);

        // Assert
        await Assert.That(option).IsSome(value);
    }

    [Test]
    public async Task Some_ValueType_ReturnsSome()
    {
        // Arrange
        var value = 42;

        // Act
        var option = Option.Some(value);

        // Assert
        await Assert.That(option).IsSome(value);
    }

    [Test]
    public async Task Some_NullableValueType_ReturnsSome()
    {
        // Arrange
        int? value = 42;

        // Act
        var option = Option.Some(value);

        // Assert
        await Assert.That(option).IsSome(value);
    }

    [Test]
    public async Task Some_ReferenceType_ReturnsSome()
    {
        // Arrange
        var value = "some value";

        // Act
        var option = Option.Some(value);

        // Assert
        await Assert.That(option).IsSome(expectedValue: value);
    }

    [Test]
    public async Task None_ValueType_ReturnsNone()
    {
        // Act
        var option = Option.None<int>();

        // Assert
        await Assert.That(option).IsNone();
    }

    [Test]
    public async Task None_NullableValueType_ReturnsNone()
    {
        // Act
        var option = Option.None<int?>();

        // Assert
        await Assert.That(option).IsNone();
    }

    [Test]
    public async Task Some_ReferenceType_ReturnsNone()
    {
        // Act
        var option = Option.None<string>();

        // Assert
        await Assert.That(option).IsNone();
    }
}
