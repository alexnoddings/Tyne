using System.Net;
using System.Text.Json;
using Tyne.HttpMediator;

namespace Tyne;

public class HttpResultJsonConverterTests
{
    // Neither of these are official, they're used as they're unlikely to be used as a default anywhere
    private const int OkStatusInt = 242;
    private const HttpStatusCode OkStatusCode = (HttpStatusCode)OkStatusInt;

    private const int ErrorStatusInt = 418;
    private const HttpStatusCode ErrorStatusCode = (HttpStatusCode)ErrorStatusInt;

    [Fact]
    public void Ok_int_Serialises()
    {
        // Arrange
        var expectedResult = HttpResult.Ok(42, OkStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@$"{{""StatusCode"":{OkStatusInt},""IsOk"":true,""Value"":42,""Error"":null}}", json);
    }

    [Fact]
    public void Ok_int_Deserialises()
    {
        // Arrange
        var expectedResult = HttpResult.Ok(42, OkStatusCode);
        var json = @$"{{""StatusCode"":{OkStatusInt},""IsOk"":true,""Value"":42,""Error"":null}}";

        // Act
        var actualResult = JsonSerializer.Deserialize<HttpResult<int>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Ok_int_Roundtrips()
    {
        // Arrange
        var expectedResult = HttpResult.Ok(42, OkStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var output = JsonSerializer.Deserialize<HttpResult<int>>(json);

        // Assert
        Assert.NotNull(output);
        AssertHttpResult.AreEqual(expectedResult, output);
    }

    [Fact]
    public void Ok_string_Serialises()
    {
        // Arrange
        var expectedResult = HttpResult.Ok("101", OkStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@$"{{""StatusCode"":{OkStatusInt},""IsOk"":true,""Value"":""101"",""Error"":null}}", json);
    }

    [Fact]
    public void Ok_string_Deserialises()
    {
        // Arrange
        var expectedResult = HttpResult.Ok("101", OkStatusCode);
        var json = @$"{{""StatusCode"":{OkStatusInt},""IsOk"":true,""Value"":""101"",""Error"":null}}";

        // Act
        var actualResult = JsonSerializer.Deserialize<HttpResult<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Ok_string_Roundtrips()
    {
        // Arrange
        var expectedResult = HttpResult.Ok("101", OkStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var actualResult = JsonSerializer.Deserialize<HttpResult<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_int_Serialises()
    {
        // Arrange
        var expectedResult = HttpResult.Error<int>(TestError.Instance, ErrorStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@$"{{""StatusCode"":{ErrorStatusInt},""IsOk"":false,""Value"":0,""Error"":{TestError.Json}}}", json);
    }

    [Fact]
    public void Error_int_Deserialises()
    {
        // Arrange
        var json = @$"{{""StatusCode"":{ErrorStatusInt},""IsOk"":false,""Value"":0,""Error"":{TestError.Json}}}";
        var expectedResult = HttpResult.Error<int>(TestError.Instance, ErrorStatusCode);

        // Act
        var actualResult = JsonSerializer.Deserialize<HttpResult<int>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_int_Roundtrips()
    {
        // Arrange
        var expectedResult = HttpResult.Error<int>(TestError.Instance, ErrorStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var actualResult = JsonSerializer.Deserialize<HttpResult<int>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_string_Serialises()
    {
        // Arrange
        var expectedResult = HttpResult.Error<string>(TestError.Instance, ErrorStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);

        // Assert
        Assert.Equal(@$"{{""StatusCode"":{ErrorStatusInt},""IsOk"":false,""Value"":null,""Error"":{TestError.Json}}}", json);
    }

    [Fact]
    public void Error_string_Deserialises()
    {
        // Arrange
        var json = @$"{{""StatusCode"":{ErrorStatusInt},""IsOk"":false,""Value"":null,""Error"":{TestError.Json}}}";
        var expectedResult = HttpResult.Error<string>(TestError.Instance, ErrorStatusCode);

        // Act
        var actualResult = JsonSerializer.Deserialize<HttpResult<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public void Error_string_Roundtrips()
    {
        // Arrange
        var expectedResult = HttpResult.Error<string>(TestError.Instance, ErrorStatusCode);

        // Act
        var json = JsonSerializer.Serialize(expectedResult);
        var actualResult = JsonSerializer.Deserialize<HttpResult<string>>(json);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }
}
