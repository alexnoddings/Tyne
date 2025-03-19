namespace Tyne;

public class OptionMiscellaneousTests
{
    [Test]
    public async Task HashCode_Some_EqualsValueHashCode()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var hashCode = option.GetHashCode();

        // Assert
        await Assert.That(hashCode).IsEqualTo(42.GetHashCode());
    }

    [Test]
    public async Task HashCode_Some_CallsHashCodeOnce()
    {
        // Arrange
        var obj = new MockObject();
        var option = Option.Some(obj);

        // Act
        var hashCode = option.GetHashCode();

        // Assert
        await Assert.That(hashCode).IsEqualTo(MockObject.HashCode);
        await Assert.That(obj.GetHashCodeInvocationCount).IsEqualTo(1);
    }

    [Test]
    public async Task HashCode_None_ReturnsZero()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var hashCode = option.GetHashCode();

        // Assert
        await Assert.That(hashCode).IsEqualTo(0);
    }

    [Test]
    public async Task ToString_Some()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var str = option.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("Some(42)");
    }

    [Test]
    public async Task ToString_Some_CallsValueToStringOnce()
    {
        // Arrange
        var obj = new MockObject();
        var option = Option.Some(obj);

        // Act
        var str = option.ToString();

        // Assert
        await Assert.That(str).IsEqualTo($"Some({MockObject.AsString})");
        await Assert.That(obj.ToStringInvocationCount).IsEqualTo(1);
    }

    [Test]
    public async Task ToString_None_ReturnsNoneLiteral()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var str = option.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("None");
    }
}
