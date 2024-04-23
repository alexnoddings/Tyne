using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tyne.Blazor.Persistence;

public class UrlQueryStringFormatter_GetValueTests
{
    private const string QueryParameterKey = "testGetValue";

    private static readonly MethodInfo GetValueT_MethodInfo = Get_GetValueT_MethodInfo();
    private static MethodInfo Get_GetValueT_MethodInfo()
    {
        const string methodName = nameof(GetValueT_Works);
        const BindingFlags methodFlags = BindingFlags.NonPublic | BindingFlags.Static;

        return typeof(UrlQueryStringFormatter_GetValueTests)
            .GetMethod(methodName, methodFlags)
            ?? throw new InvalidOperationException($"Could not load method info for generic test method '{methodName}'.");
    }

    public static IEnumerable<object?[]> GetValue_Data => UrlUtilities_TestHelpers.StringToValue_Data;

    [Theory]
    [MemberData(nameof(GetValue_Data))]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Assertions are handled by the generic method invoked.")]
    public void GetValue_Works(string queryParameterValue, object expectedOption)
    {
        ArgumentNullException.ThrowIfNull(expectedOption);

        var expectedOptionType = expectedOption.GetType();
        if (!expectedOptionType.IsGenericType || expectedOptionType.GetGenericTypeDefinition() != typeof(Option<>))
            throw new ArgumentException("Value was not an Option<>.", nameof(expectedOption));

        var queryParameterEncodedValue =
            queryParameterValue is not null
            ? Uri.EscapeDataString(queryParameterValue)
            : null;

        var uri = $"https://localhost/test/page?{QueryParameterKey}={queryParameterEncodedValue}";

        var optionType = expectedOptionType.GenericTypeArguments[0];
        _ = GetValueT_MethodInfo
            .MakeGenericMethod(optionType)
            .Invoke(null, [uri, expectedOption]);
    }

    private static void GetValueT_Works<T>(string uri, Option<T> expectedOption)
    {
        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var actualValue = urlQueryStringFormatter.GetValue<T>(uri, QueryParameterKey);
        Assert.Equal(expectedOption, actualValue);
    }
}
