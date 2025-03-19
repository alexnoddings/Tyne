using System.Reflection;
using System.Resources;

namespace Tyne;

public class ExceptionMessageTests
{
    private const BindingFlags FieldBindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

    public static IEnumerable<Func<string>> GetExceptionMessageFieldNames() =>
        typeof(ExceptionMessages)
        .GetFields(FieldBindingFlags)
        .Where(field => field.FieldType == typeof(string))
        .Select(field => field.Name)
        .Select(fieldName => (Func<string>)(() => fieldName));

    [Test]
    [MethodDataSource(nameof(GetExceptionMessageFieldNames))]
    public async Task AllFields_HaveValue(string fieldName)
    {
        var field = typeof(ExceptionMessages).GetField(fieldName, FieldBindingFlags);
        if (field is null)
            Assert.Fail($"Could not find field '{fieldName}' on '{nameof(ExceptionMessages)}'.");

        var exceptionMessage = field.GetValue(null) as string;
        await Assert_ExceptionMessageIsValid(fieldName, exceptionMessage);
    }

    [Test]
    public async Task JsonConverter_ConversionForTypeNotSupported_HasValue()
    {
        var type = typeof(ExceptionMessageTests);
        var exceptionMessage = ExceptionMessages.JsonConverter_ConversionForTypeNotSupported(type);
        await Assert_ExceptionMessageIsValid(nameof(ExceptionMessages.JsonConverter_ConversionForTypeNotSupported), exceptionMessage);
    }

    [Test]
    public async Task JsonConverter_InvalidType_HasValue()
    {
        var type = "invalid result type";
        var exceptionMessage = ExceptionMessages.Result_JsonConverter_InvalidType(type);
        await Assert_ExceptionMessageIsValid(nameof(ExceptionMessages.Result_JsonConverter_InvalidType), exceptionMessage);
    }

    private static async Task Assert_ExceptionMessageIsValid(string resourceName, string? exceptionMessage)
    {
        // Ensure the message isn't null or whitespace
        await Assert.That(exceptionMessage).IsNotNullOrWhitespace();

        // And that it isn't equal to the default 'not found' resource
        var notFoundResourceValue = EmbeddedResourceManager.GetNotFoundResourceValue(resourceName);
        await Assert.That(exceptionMessage)
            .IsNotEqualTo(notFoundResourceValue)
            .Because("it should have a resource value");
    }
}
