using System.Text.Json;

namespace Tyne;

public class OptionJsonTests
{
    [Fact]
    public void Some_int_Serialises()
    {
        // Arrange
        var expectedOption = Option.Some(42);

        // Act
        var json = JsonSerializer.Serialize(expectedOption);

        // Assert
        Assert.Equal("42", json);
    }

    [Fact]
    public void Some_int_Deserialises()
    {
        // Arrange
        var expectedOption = Option.Some(42);
        var json = "42";

        // Act
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void Some_int_Roundtrips()
    {
        // Arrange
        var expectedOption = Option.Some(42);

        // Act
        var json = JsonSerializer.Serialize(expectedOption);
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void Some_string_Serialises()
    {
        // Arrange
        var expectedOption = Option.Some("101");

        // Act
        var json = JsonSerializer.Serialize(expectedOption);

        // Assert
        Assert.Equal(@"""101""", json);
    }

    [Fact]
    public void Some_string_Deserialises()
    {
        // Arrange
        var expectedOption = Option.Some("101");
        var json = @"""101""";

        // Act
        var actualOption = JsonSerializer.Deserialize<Option<string>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void Some_string_Roundtrips()
    {
        // Arrange
        var expectedOption = Option.Some("101");

        // Act
        var json = JsonSerializer.Serialize(expectedOption);
        var actualOption = JsonSerializer.Deserialize<Option<string>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void None_int_Serialises()
    {
        // Arrange
        var expectedOption = Option.None<int>();

        // Act
        var json = JsonSerializer.Serialize(expectedOption);

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void None_int_Deserialises()
    {
        // Arrange
        var expectedOption = Option.None<int>();
        var json = "null";

        // Act
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void None_int_Roundtrips()
    {
        // Arrange
        var expectedOption = Option.None<int>();

        // Act
        var json = JsonSerializer.Serialize(expectedOption);
        var actualOption = JsonSerializer.Deserialize<Option<int>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void None_string_Serialises()
    {
        // Arrange
        var expectedOption = Option.None<string>();

        // Act
        var json = JsonSerializer.Serialize(expectedOption);

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void None_string_Deserialises()
    {
        // Arrange
        var expectedOption = Option.None<string>();
        var json = "null";

        // Act
        var actualOption = JsonSerializer.Deserialize<Option<string>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void None_string_Roundtrips()
    {
        // Arrange
        var expectedOption = Option.None<string>();

        // Act
        var json = JsonSerializer.Serialize(expectedOption);
        var actualOption = JsonSerializer.Deserialize<Option<string>>(json);

        // Assert
        AssertOption.AreEqual(expectedOption, actualOption);
    }
}
