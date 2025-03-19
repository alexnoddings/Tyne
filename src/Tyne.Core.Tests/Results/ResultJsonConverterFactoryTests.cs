using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne;

public class ResultJsonConverterFactoryTests
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Default;

    private static readonly Type[] _validResultTypes =
    [
        typeof(Result<int, int>),
        typeof(Result<Unit, string>),
        typeof(Result<object[], object[]>),
    ];

    private static readonly Type[] _nonResultTypes =
    [
        typeof(int),
        typeof(string),
        typeof(object[]),
        typeof(Result<,>), // Non-concrete type
        typeof(ResultJsonConverterFactoryTests),
    ];

    private static Func<Type> CreateTypeFunc(Type type) => () => type;

    public static IEnumerable<Func<Type>> GetValidResultTypes() =>
        _validResultTypes.Select(CreateTypeFunc);

    public static IEnumerable<Func<Type>> GetNonResultTypes() =>
        _nonResultTypes.Select(CreateTypeFunc);

    [Test]
    public async Task CanConvert_Null_Throws_ArgumentNullException()
    {
        // Arrange
        var factory = new ResultJsonConverterFactory();

        // Act
        bool Act() => factory.CanConvert(null!);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    [MethodDataSource(nameof(GetValidResultTypes))]
    public async Task CanConvert_ResultTypes_True(Type type)
    {
        // Arrange
        var factory = new ResultJsonConverterFactory();

        // Act
        var canConvert = factory.CanConvert(type);

        // Assert
        await Assert.That(canConvert).IsTrue();
    }

    [Test]
    [MethodDataSource(nameof(GetNonResultTypes))]
    public async Task CanConvert_OtherTypes_False(Type type)
    {
        // Arrange
        var factory = new ResultJsonConverterFactory();

        // Act
        var canConvert = factory.CanConvert(type);

        // Assert
        await Assert.That(canConvert).IsFalse();
    }

    [Test]
    public async Task CreateConverter_NullType_Throws_ArgumentNullException()
    {
        // Arrange
        var factory = new ResultJsonConverterFactory();

        // Act
        JsonConverter Act() => factory.CreateConverter(null!, _jsonSerializerOptions);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task CreateConverter_NullOptions_Throws_ArgumentNullException()
    {
        // Arrange
        var factory = new ResultJsonConverterFactory();

        // Act
        JsonConverter Act() => factory.CreateConverter(typeof(Result<int, string>), null!);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    [MethodDataSource(nameof(GetNonResultTypes))]
    public async Task CreateConverter_OtherTypes_ThrowsNotSupportedException(Type type)
    {
        // Arrange
        var factory = new ResultJsonConverterFactory();
        var expectedErrorMessage = ExceptionMessages.JsonConverter_ConversionForTypeNotSupported(type);

        // Act
        JsonConverter Act() => factory.CreateConverter(type, _jsonSerializerOptions);

        // Assert
        await Assert.That(Act).Throws<NotSupportedException>().WithMessage(expectedErrorMessage);
    }
}
