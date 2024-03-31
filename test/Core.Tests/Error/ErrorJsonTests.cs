using System.Text.Json;

namespace Tyne;

public class ErrorJsonTests
{
    [Fact]
    public void Serialise_CodeAndMessageAreSerialised()
    {
        // Arrange
        var expectedError = TestError.Instance;

        // Act
        var json = JsonSerializer.Serialize(expectedError);

        // Assert
        Assert.Equal(TestError.Json, json);
    }

    [Fact]
    public void Serialise_CausedByIsNotSerialised()
    {
        // Arrange
        var expectedError = Error.From(TestError.Code, TestError.Message, new InvalidOperationException("Some exception."));

        // Act
        var json = JsonSerializer.Serialize(expectedError);

        // Assert
        Assert.Equal(TestError.Json, json);
    }

    [Fact]
    public void Deserialise_CodeAndMessageAreDeserialised()
    {
        // Arrange
        var expectedError = TestError.Instance;
        var json = TestError.Json;

        // Act
        var actualError = JsonSerializer.Deserialize<Error>(json);

        // Assert
        Assert.NotNull(actualError);
        AssertError.AreEqual(expectedError, actualError);
    }

    [Fact]
    public void Deserialise_CausedBy_Exception_IsNotDeserialised()
    {
        // Arrange
        var expectedError = TestError.Instance;
        var json = @$"{{""Code"":{TestError.Code},""Message"":""{TestError.Message}"",""CausedBy"":{{""Message"":""Some exception.""}}}}";

        // Act
        var actualError = JsonSerializer.Deserialize<Error>(json);

        // Assert
        Assert.NotNull(actualError);
        AssertError.AreEqual(expectedError, actualError);
    }

    [Fact]
    public void Deserialise_CausedBy_OptionException_IsNotDeserialised()
    {
        // Arrange
        var expectedError = TestError.Instance;
        var json = @$"{{""Code"":{TestError.Code},""Message"":""{TestError.Message}"",""CausedBy"":{{""HasValue"":true,""Value"":{{""Message"":""Some exception.""}}}}}}";

        // Act
        var actualError = JsonSerializer.Deserialize<Error>(json);

        // Assert
        Assert.NotNull(actualError);
        AssertError.AreEqual(expectedError, actualError);
    }

    [Fact]
    public void RoundTrips_WithoutException()
    {
        // Arrange
        var expectedError = Error.From(TestError.Code, TestError.Message, new InvalidOperationException("Some exception."));

        // Act
        var json = JsonSerializer.Serialize(expectedError);
        var actualError = JsonSerializer.Deserialize<Error>(json);

        // Assert
        Assert.NotNull(actualError);
        Assert.Equal(expectedError.Code, actualError.Code);
        Assert.Equal(expectedError.Message, actualError.Message);
        AssertOption.IsNone(actualError.CausedBy);
    }
}
