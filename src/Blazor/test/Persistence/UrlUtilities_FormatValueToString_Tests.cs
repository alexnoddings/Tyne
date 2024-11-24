namespace Tyne.Blazor.Persistence;

public class UrlUtilities_FormatValueToString_Tests
{
    public static IEnumerable<object?[]> FormatValueToString_Data => UrlUtilities_TestHelpers.ValueToString_Data;

    [Theory]
    [MemberData(nameof(FormatValueToString_Data))]
    public void FormatValueToString_ProducesCorrectString(object? input, string? expected)
    {
        var actual = UrlUtilities.FormatValueToString(input);
        Assert.Equal(expected, actual);
    }
}
