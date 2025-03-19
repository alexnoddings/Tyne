using System.Text.Json;

namespace Tyne;

public class ResultJsonConverterTests
{
    private static readonly JsonSerializerOptions _jsonOptions = JsonSerializerOptions.Default;

    [Test]
    public async Task Serialise_OkResult_Works()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);

        // Act
        var json = JsonSerializer.Serialize(result, _jsonOptions);

        // Assert
        var expectedJson = """{"$":"ok","Value":42,"Error":null}""";
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    public async Task Serialise_ErrorResult_Works()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");

        // Act
        var json = JsonSerializer.Serialize(result, _jsonOptions);

        // Assert
        var expectedJson = """{"$":"error","Value":0,"Error":"some error"}""";
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    [Arguments("""{"Value":42,"Error":null}""")]
    [Arguments("""{"Value":0,"Error":"some error"}""")]
    [Arguments("""{"$":null,"Value":42,"Error":null}""")]
    [Arguments("""{"$":null,"Value":0,"Error":"some error"}""")]
    [Arguments("""{"$":"","Value":42,"Error":null}""")]
    [Arguments("""{"$":"","Value":0,"Error":"some error"}""")]
    public async Task Deserialise_NoType_Throws_JsonException(string json)
    {
        // Arrange
        var expectedErrorMessage = ExceptionMessages.Result_JsonConverter_NoResultType;

        // Act
        Result<int, string>? Act() => JsonSerializer.Deserialize<Result<int, string>>(json, _jsonOptions);

        // Assert
        await Assert.That(Act).Throws<JsonException>().WithMessage(expectedErrorMessage);
    }

    [Test]
    [Arguments("""{"$":"invalid","Value":42,"Error":null}""")]
    [Arguments("""{"$":"invalid","Value":0,"Error":"some error"}""")]
    public async Task Deserialise_InvalidType_Throws_JsonException(string json)
    {
        // Arrange
        var expectedErrorMessage = ExceptionMessages.Result_JsonConverter_InvalidType("invalid");

        // Act
        Result<int, string>? Act() => JsonSerializer.Deserialize<Result<int, string>>(json, _jsonOptions);

        // Assert
        await Assert.That(Act).Throws<JsonException>().WithMessage(expectedErrorMessage);
    }

    [Test]
    [Arguments("""{"$":"ok","Value":[],"Error":null}""")]
    [Arguments("""{"$":"ok","Value":null,"Error":null}""")]
    [Arguments("""{"$":"ok","Value":"forty two","Error":null}""")]
    [Arguments("""{"$":"error","Value":0,"Error":[]}""")]
    [Arguments("""{"$":"error","Value":0,"Error":0}""")]
    public async Task Deserialise_InvalidValueOrError_Throws_JsonException(string json)
    {
        // Act
        Result<int, string>? Act() => JsonSerializer.Deserialize<Result<int, string>>(json, _jsonOptions);

        // Assert
        // No expected message, since this exception should be raised by STJ.
        await Assert.That(Act).Throws<JsonException>();
    }

    [Test]
    public async Task Deserialise_OkResult_ValidJson_Works()
    {
        // Arrange
        var expectedResult = Result.Ok<int, string>(42);
        var json = """{"$":"ok","Value":42,"Error":null}""";

        // Act
        var actualResult = JsonSerializer.Deserialize<Result<int, string>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualResult).IsNotNull();
        await Assert.That(actualResult).IsEqualTo(expectedResult);
    }

    [Test]
    public async Task Deserialise_ErrorResult_ValidJson_Works()
    {
        // Arrange
        var expectedResult = Result.Error<int, string>("some error");
        var json = """{"$":"error","Value":0,"Error":"some error"}""";

        // Act
        var actualResult = JsonSerializer.Deserialize<Result<int, string>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualResult).IsNotNull();
        await Assert.That(actualResult).IsEqualTo(expectedResult);
    }

    [Test]
    public async Task Roundtrip_OkResult_Works()
    {
        // Arrange
        var expectedResult = Result.Ok<int, string>(42);

        // Act
        var json = JsonSerializer.Serialize(expectedResult, _jsonOptions);
        var actualResult = JsonSerializer.Deserialize<Result<int, string>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualResult).IsNotNull();
        await Assert.That(actualResult!).IsOk(42);
    }

    [Test]
    public async Task Roundtrips_ErrorResult_Works()
    {
        // Arrange
        var expectedResult = Result.Error<int, string>("some error");

        // Act
        var json = JsonSerializer.Serialize(expectedResult, _jsonOptions);
        var actualResult = JsonSerializer.Deserialize<Result<int, string>>(json, _jsonOptions);

        // Assert
        await Assert.That(actualResult).IsNotNull();
        await Assert.That(actualResult!).IsError("some error");
    }
}
