using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne;

public class OptionJsonConverterFactoryTests
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Default;

    private static readonly Type[] _validOptionTypes =
    [
        typeof(Option<int>),
        typeof(Option<Unit>),
        typeof(Option<object[]>),
    ];

    private static readonly Type[] _nonOptionTypes =
    [
        typeof(int),
        typeof(string),
        typeof(object[]),
        typeof(Option<>), // Non-concrete type
        typeof(OptionJsonConverterFactoryTests),
    ];

    private static Func<Type> CreateTypeFunc(Type type) => () => type;

    public static IEnumerable<Func<Type>> GetValidOptionTypes() =>
        _validOptionTypes.Select(CreateTypeFunc);

    public static IEnumerable<Func<Type>> GetNonOptionTypes() =>
        _nonOptionTypes.Select(CreateTypeFunc);

    [Test]
    public async Task CanConvert_Null_Throws_ArgumentNullException()
    {
        // Arrange
        var factory = new OptionJsonConverterFactory();

        // Act
        bool Act() => factory.CanConvert(null!);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    [MethodDataSource(nameof(GetValidOptionTypes))]
    public async Task CanConvert_OptionTypes_True(Type type)
    {
        // Arrange
        var factory = new OptionJsonConverterFactory();

        // Act
        var canConvert = factory.CanConvert(type);

        // Assert
        await Assert.That(canConvert).IsTrue();
    }

    [Test]
    [MethodDataSource(nameof(GetNonOptionTypes))]
    public async Task CanConvert_OtherTypes_False(Type type)
    {
        // Arrange
        var factory = new OptionJsonConverterFactory();

        // Act
        var canConvert = factory.CanConvert(type);

        // Assert
        await Assert.That(canConvert).IsFalse();
    }

    [Test]
    public async Task CreateConverter_NullType_Throws_ArgumentNullException()
    {
        // Arrange
        var factory = new OptionJsonConverterFactory();

        // Act
        JsonConverter Act() => factory.CreateConverter(null!, _jsonSerializerOptions);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task CreateConverter_NullOptions_Throws_ArgumentNullException()
    {
        // Arrange
        var factory = new OptionJsonConverterFactory();

        // Act
        JsonConverter Act() => factory.CreateConverter(typeof(Option<int>), null!);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    [MethodDataSource(nameof(GetNonOptionTypes))]
    public async Task CreateConverter_OtherTypes_ThrowsNotSupportedException(Type type)
    {
        // Arrange
        var factory = new OptionJsonConverterFactory();
        var expectedNoneMessage = ExceptionMessages.JsonConverter_ConversionForTypeNotSupported(type);

        // Act
        JsonConverter Act() => factory.CreateConverter(type, _jsonSerializerOptions);

        // Assert
        await Assert.That(Act).Throws<NotSupportedException>().WithMessage(expectedNoneMessage);
    }
}
