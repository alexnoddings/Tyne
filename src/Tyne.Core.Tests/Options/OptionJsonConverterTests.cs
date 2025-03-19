using System.Text.Json;

namespace Tyne;

public class OptionJsonConverterTests
{
    private static readonly JsonSerializerOptions _jsonOptions = JsonSerializerOptions.Default;

    [Test]
    public async Task Serialise_SomeOption_Works()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var json = JsonSerializer.Serialize(option, _jsonOptions);

        // Assert
        var expectedJson = "42";
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    public async Task Serialise_NoneOption_Works()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var json = JsonSerializer.Serialize(option, _jsonOptions);

        // Assert
        var expectedJson = "null";
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    public async Task Deserialise_SomeOption_ValidJson_Works()
    {
        // Arrange
        var expectedOption = Option.Some(42);
        var json = "42";

        // Act
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualOption).IsNotNull();
        await Assert.That(actualOption).IsEqualTo(expectedOption);
    }

    [Test]
    public async Task Deserialise_NoneOption_ValidJson_Works()
    {
        // Arrange
        var expectedOption = Option.None<int>();
        var json = "null";

        // Act
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualOption).IsNotNull();
        await Assert.That(actualOption).IsEqualTo(expectedOption);
    }

    [Test]
    public async Task Roundtrip_SomeOption_Works()
    {
        // Arrange
        var expectedOption = Option.Some(42);

        // Act
        var json = JsonSerializer.Serialize(expectedOption, _jsonOptions);
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualOption).IsNotNull();
        await Assert.That(actualOption!).IsSome(42);
    }

    [Test]
    public async Task Roundtrips_NoneOption_Works()
    {
        // Arrange
        var expectedOption = Option.None<int>();

        // Act
        var json = JsonSerializer.Serialize(expectedOption, _jsonOptions);
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualOption).IsNotNull();
        await Assert.That(actualOption!).IsNone();
    }
}
