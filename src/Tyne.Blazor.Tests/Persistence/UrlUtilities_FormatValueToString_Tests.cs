namespace Tyne.Blazor.Persistence;

public class UrlUtilities_FormatValueToString_Tests
{
    [Test]
    [MethodDataSource<UrlUtilities_TestHelpers>(nameof(UrlUtilities_TestHelpers.GetValueToStringData))]
    public async Task FormatValueToString_ProducesCorrectString(object? input, string? expected)
    {
        var actual = UrlUtilities.FormatValueToString(input);
        await Assert.That(expected).IsEqualTo(actual);
    }
}
