using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tyne.Blazor.Persistence;

public class UrlUtilities_TryParse_Tests
{
    private static readonly MethodInfo _tryParseTMethodInfo = Get_TryParseT_MethodInfo();
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

    [Test]
    [MethodDataSource<UrlUtilities_TestHelpers>(nameof(UrlUtilities_TestHelpers.GetStringToValueData))]
    [SuppressMessage("Blocker Code Smell", "S2699: Tests should include assertions", Justification = "Assertions are handled by the generic method invoked.")]
    public async Task TryParse_ProducesCorrectValue(string input, object expectedOption)
    {
        ArgumentNullException.ThrowIfNull(expectedOption);

        var expectedOptionType = expectedOption.GetType();
        if (!expectedOptionType.IsGenericType || expectedOptionType.GetGenericTypeDefinition() != typeof(Option<>))
            throw new ArgumentException("Value was not an Option<>.", nameof(expectedOption));

        var optionType = expectedOptionType.GenericTypeArguments[0];
        var taskObj = _tryParseTMethodInfo
            .MakeGenericMethod(optionType)
            .Invoke(null, [input, expectedOption]);

        await (Task)taskObj!;
    }

    private static async Task TryParseT<T>(string input, Option<T> expectedOption)
    {
        var actualOption = UrlUtilities.TryParse<T>(input);
        await Assert.That(actualOption).IsEqualTo(expectedOption);
    }

    [Test]
    public async Task TryParse_IntArray_ProducesCorrectValue()
    {
        var expected = new[] { 0, 101 };
        var actualOption = UrlUtilities.TryParse<int[]>("[0, 101]");

        await Assert.That(actualOption).IsSome(expected);
    }

    [Test]
    public async Task TryParse_IntList_ProducesCorrectValue()
    {
        var expected = new List<int> { 0, 101 };
        var actualOption = UrlUtilities.TryParse<List<int>>("[0, 101]");

        await Assert.That(actualOption).IsSome(expected);
    }

    [Test]
    public async Task TryParse_IntHashSet_ProducesCorrectValue()
    {
        var expected = new HashSet<int> { 0, 101 };
        var actualOption = UrlUtilities.TryParse<HashSet<int>>("[0, 101]");

        await Assert.That(actualOption).IsSome(expected);
    }
}
