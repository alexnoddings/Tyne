using System.Reflection;
using System.Resources;

namespace Tyne;

public class ExceptionMessageTests
{
    private const BindingFlags FieldBindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

    public static IEnumerable<object[]> GetExceptionMessageFieldNames() =>
        typeof(ExceptionMessages)
        .GetFields(FieldBindingFlags)
        .Where(field => field.FieldType == typeof(string))
        .Select(field => new object[] { field.Name })
        .ToList();

    [Theory]
    [MemberData(nameof(GetExceptionMessageFieldNames))]
    public void AllFields_HaveValue(string fieldName)
    {
        var field = typeof(ExceptionMessages).GetField(fieldName, FieldBindingFlags);
        if (field is null)
            Assert.Fail($"Could not find field '{fieldName}' on '{nameof(ExceptionMessages)}'.");

        var exceptionMessage = field.GetValue(null) as string;
        Assert_ExceptionMessageIsValid(fieldName, exceptionMessage);
    }

    [Fact]
    public void JsonConversionForTypeNotSupported_HasValue()
    {
        var type = typeof(ExceptionMessageTests);
        var exceptionMessage = ExceptionMessages.JsonConversionForTypeNotSupported(type);
        Assert_ExceptionMessageIsValid(nameof(ExceptionMessages.JsonConversionForTypeNotSupported), exceptionMessage);
    }

    private static void Assert_ExceptionMessageIsValid(string resourceName, string? exceptionMessage)
    {
        // Ensure the message isn't null or whitespace
        Assert.NotNull(exceptionMessage);
        Assert.False(string.IsNullOrWhiteSpace(exceptionMessage), $"Exception message '{resourceName}' is empty or whitespace.");

        // And that it isn't equal to the default 'not found' resource
        var notFoundResourceValue = EmbeddedResourceManager.GetNotFoundResourceValue(resourceName);
        if (notFoundResourceValue == exceptionMessage)
            Assert.Fail($"No resource found for exception message '{resourceName}'.");
    }
}
