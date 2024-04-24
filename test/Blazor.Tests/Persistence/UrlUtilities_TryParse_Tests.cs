using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tyne.Blazor.Persistence;

public class UrlUtilities_TryParse_Tests
{
    private static readonly MethodInfo TryParseT_MethodInfo = Get_TryParseT_MethodInfo();
    private static MethodInfo Get_TryParseT_MethodInfo()
    {
        const string methodName = nameof(TryParseT);
        const BindingFlags methodFlags = BindingFlags.NonPublic | BindingFlags.Static;

        var method =
            typeof(UrlUtilities_TryParse_Tests)
            .GetMethod(methodName, methodFlags)
            ?? throw new InvalidOperationException($"Could not load method info for generic test method '{methodName}'.");
        return method;
    }

    public static IEnumerable<object?[]> TryParse_Data => UrlUtilities_TestHelpers.StringToValue_Data;

    [Theory]
    [MemberData(nameof(TryParse_Data))]
    [SuppressMessage("Blocker Code Smell", "S2699: Tests should include assertions", Justification = "Assertions are handled by the generic method invoked.")]
    public void TryParse_ProducesCorrectValue(string input, object expectedOption)
    {
        ArgumentNullException.ThrowIfNull(expectedOption);

        var expectedOptionType = expectedOption.GetType();
        if (!expectedOptionType.IsGenericType || expectedOptionType.GetGenericTypeDefinition() != typeof(Option<>))
            throw new ArgumentException("Value was not an Option<>.", nameof(expectedOption));

        var optionType = expectedOptionType.GenericTypeArguments[0];
        _ = TryParseT_MethodInfo
            .MakeGenericMethod(optionType)
            .Invoke(null, [input, expectedOption]);
    }

    private static void TryParseT<T>(string input, Option<T> expectedOption)
    {
        var actualOption = UrlUtilities.TryParse<T>(input);
        AssertOption.AreEqual(expectedOption, actualOption);
    }

    [Fact]
    public void TryParse_IntArray_ProducesCorrectValue()
    {
        var expected = new int[] { 0, 101 };
        var actualOption = UrlUtilities.TryParse<int[]>("[0, 101]");

        AssertOption.IsSome(actualOption);
        Assert.Equal(expected, actualOption.Value);
    }

    [Fact]
    public void TryParse_IntList_ProducesCorrectValue()
    {
        var expected = new List<int> { 0, 101 };
        var actualOption = UrlUtilities.TryParse<List<int>>("[0, 101]");

        AssertOption.IsSome(actualOption);
        Assert.Equal(expected, actualOption.Value);
    }

    [Fact]
    public void TryParse_IntHashSet_ProducesCorrectValue()
    {
        var expected = new HashSet<int> { 0, 101 };
        var actualOption = UrlUtilities.TryParse<HashSet<int>>("[0, 101]");

        AssertOption.IsSome(actualOption);
        Assert.Equal(expected, actualOption.Value);
    }
}
