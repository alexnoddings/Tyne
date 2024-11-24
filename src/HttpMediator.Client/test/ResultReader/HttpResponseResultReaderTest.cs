using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tyne.Internal.HttpMediator;

namespace Tyne.HttpMediator.Client;

public class HttpResponseResultReaderTest
{
    private sealed class TestObject
    {
        public static readonly TestObject Instance = new() { Value = 451 };

        public int Value { get; set; }

        public override bool Equals(object? obj) =>
            obj is TestObject testObj && Value == testObj.Value;

        public override int GetHashCode() =>
            Value.GetHashCode();
    }

    // Neither of these are official, they're used as they're unlikely to be used as a default anywhere
    private const HttpStatusCode OkStatusCode = (HttpStatusCode)242;
    private const HttpStatusCode ErrorStatusCode = (HttpStatusCode)418;

    [Fact]
    public async Task ReadInt_Ok_WithIntContent_ReturnsOk()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        var expectedResult = HttpResult.Ok(42, OkStatusCode);
        using var responseMessage = CreateHttpResponseMessage(expectedResult, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<int>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public async Task ReadInt_Ok_WithNoContent_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        using var responseMessage = CreateHttpResponseMessage<object>(null, HttpStatusCode.OK, jsonOptions);
        responseMessage.Content = null;
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<int>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsError(HttpStatusCode.InternalServerError, actualResult);
        Assert.NotNull(actualResult.Error.Message);
        Assert.NotEmpty(actualResult.Error.Message);
    }

    [Fact]
    public async Task ReadInt_Ok_WithNullContent_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        using var responseMessage = CreateHttpResponseMessage<object>(null, HttpStatusCode.OK, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<int>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsError(HttpStatusCode.BadRequest, actualResult);
        Assert.NotNull(actualResult.Error.Message);
        Assert.NotEmpty(actualResult.Error.Message);
    }

    [Fact]
    public async Task ReadInt_Ok_WithObjectContent_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        // An object will be serialised as "{...}", whereas an int will expect a number token
        using var responseMessage = CreateHttpResponseMessage(new TestObject(), HttpStatusCode.OK, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<int>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsError(HttpStatusCode.BadRequest, actualResult);
        Assert.NotNull(actualResult.Error.Message);
        Assert.NotEmpty(actualResult.Error.Message);
    }

    [Fact]
    public async Task ReadInt_Ok_WithArrayContent_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        // A string[] will be serialised as "[]", whereas an int will expect a number token
        using var responseMessage = CreateHttpResponseMessage(Array.Empty<string>(), HttpStatusCode.OK, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<int>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsError(HttpStatusCode.BadRequest, actualResult);
        Assert.NotNull(actualResult.Error.Message);
        Assert.NotEmpty(actualResult.Error.Message);
    }

    [Fact]
    public async Task ReadInt_Error_WithError_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        var expectedError = Error.From("Tyne.HttpMediator.Client.UnitTests.Error", "Response result reader test error.");
        var expectedResult = HttpResult.Error<int>(expectedError, ErrorStatusCode);
        using var responseMessage = CreateHttpResponseMessage(expectedResult, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<int>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public async Task ReadObject_Ok_WithObjectContent_ReturnsOk()
    {

        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        var expectedResult = HttpResult.Ok(TestObject.Instance, OkStatusCode);
        using var responseMessage = CreateHttpResponseMessage(expectedResult, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<TestObject>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        AssertHttpResult.AreEqual(expectedResult, actualResult);
    }

    [Fact]
    public async Task ReadObject_Ok_WithNoContent_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        using var responseMessage = CreateHttpResponseMessage<object>(null, HttpStatusCode.OK, jsonOptions);
        responseMessage.Content = null;
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<TestObject>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsError(HttpStatusCode.InternalServerError, actualResult);
        Assert.NotNull(actualResult.Error.Message);
        Assert.NotEmpty(actualResult.Error.Message);
    }

    [Fact]
    public async Task ReadObject_Ok_WithNullContent_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        using var responseMessage = CreateHttpResponseMessage<object>(null, HttpStatusCode.OK, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<TestObject>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsError(HttpStatusCode.BadRequest, actualResult);
        Assert.NotNull(actualResult.Error.Message);
        Assert.NotEmpty(actualResult.Error.Message);
    }

    [Fact]
    public async Task ReadObject_Ok_WithIntContent_ReturnsError()
    {
        // Arrange
        using var scope = HttpMediatorClientTestScope.Create();
        var jsonOptions = scope.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

        // A string[] will be serialised as "[]", whereas an int will expect a number token
        using var responseMessage = CreateHttpResponseMessage(42, HttpStatusCode.OK, jsonOptions);
        var resultReader = scope.Services.GetRequiredService<IHttpResponseResultReader>();

        // Act
        var actualResult = await resultReader.ReadAsync<TestObject>(responseMessage, jsonOptions);

        // Assert
        Assert.NotNull(actualResult);
        _ = AssertHttpResult.IsError(HttpStatusCode.BadRequest, actualResult);
        Assert.NotNull(actualResult.Error.Message);
        Assert.NotEmpty(actualResult.Error.Message);
    }

    private static ProblemDetails CreateProblemDetails(Error error)
    {
        var problemDetails = new ProblemDetails
        {
            Title = error.Message,
        };
        problemDetails.Extensions.Add(nameof(Error.Code), error.Code);
        return problemDetails;
    }

    private static HttpResponseMessage CreateHttpResponseMessage<T>(HttpResult<T> result, JsonSerializerOptions jsonSerializerOptions)
    {
        if (result.IsOk)
            return CreateHttpResponseMessage(result.Value, result.StatusCode, jsonSerializerOptions);

        return CreateHttpResponseMessage(CreateProblemDetails(result.Error), result.StatusCode, jsonSerializerOptions);
    }

    private static HttpResponseMessage CreateHttpResponseMessage<TContent>(TContent? content, HttpStatusCode statusCode, JsonSerializerOptions jsonSerializerOptions) =>
        new(statusCode)
        {
            Content = JsonContent.Create(content, options: jsonSerializerOptions)
        };
}
