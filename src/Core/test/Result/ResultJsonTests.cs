using System.Text.Json;

namespace Tyne;

public class ResultJsonTests
{
    [Fact]
    public void Ok_int_Serialises()
    {
        // Arrange
        var expectedResult = Result.Ok(42);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@"{""IsOk"":true,""Value"":42,""Error"":null}", json);
    }

    [Fact]
    public void Ok_int_Deserialises()
    {
        // Arrange
        var expectedResult = Result.Ok(42);
        var json = @"{""IsOk"":true,""Value"":42,""Error"":null}";

        // Act
        var actualResult = JsonSerializer.Deserialize<Result<int>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Ok_int_Roundtrips()
    {
        // Arrange
        var expectedResult = Result.Ok(42);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var output = JsonSerializer.Deserialize<Result<int>>(json);

        // Assert
        Assert.NotNull(output);
        AssertResult.AreEqual(expectedResult, output);
    }

    [Fact]
    public void Ok_string_Serialises()
    {
        // Arrange
        var expectedResult = Result.Ok("101");

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@"{""IsOk"":true,""Value"":""101"",""Error"":null}", json);
    }

    [Fact]
    public void Ok_string_Deserialises()
    {
        // Arrange
        var expectedResult = Result.Ok("101");
        var json = @"{""IsOk"":true,""Value"":""101"",""Error"":null}";

        // Act
        var actualResult = JsonSerializer.Deserialize<Result<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Ok_string_Roundtrips()
    {
        // Arrange
        var expectedResult = Result.Ok("101");

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var actualResult = JsonSerializer.Deserialize<Result<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_int_Serialises()
    {
        // Arrange
        var expectedResult = Result.Error<int>(TestError.Instance);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@$"{{""IsOk"":false,""Value"":0,""Error"":{TestError.Json}}}", json);
    }

    [Fact]
    public void Error_int_Deserialises()
    {
        // Arrange
        var json = @$"{{""IsOk"":false,""Value"":0,""Error"":{TestError.Json}}}";
        var expectedResult = Result.Error<int>(TestError.Instance);

        // Act
        var actualResult = JsonSerializer.Deserialize<Result<int>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_int_Roundtrips()
    {
        // Arrange
        var expectedResult = Result.Error<int>(TestError.Instance);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var actualResult = JsonSerializer.Deserialize<Result<int>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_string_Serialises()
    {
        // Arrange
        var expectedResult = Result.Error<string>(TestError.Instance);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@$"{{""IsOk"":false,""Value"":null,""Error"":{TestError.Json}}}", json);
    }

    [Fact]
    public void Error_string_Deserialises()
    {
        // Arrange
        var json = @$"{{""IsOk"":false,""Value"":null,""Error"":{TestError.Json}}}";
        var expectedResult = Result.Error<string>(TestError.Instance);

        // Act
        var actualResult = JsonSerializer.Deserialize<Result<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_string_Roundtrips()
    {
        // Arrange
        var expectedResult = Result.Error<string>(TestError.Instance);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var actualResult = JsonSerializer.Deserialize<Result<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertResult.AreEqual(expectedResult, actualResult);
    }
}
