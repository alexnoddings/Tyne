using System.Web;

namespace Tyne.Blazor.Persistence;

public class UrlQueryStringFormatter_GetUriWithQueryParameterTests
{
    private const string QueryParameterKey = "testSetValue";

    [Test]
    [MethodDataSource<UrlUtilities_TestHelpers>(nameof(UrlUtilities_TestHelpers.GetValueToStringData))]
    public async Task GetUriWithQueryParameterTests_Works(object? input, string? expectedQueryParameterValue)
    {
        var uri = "https://localhost/test/page";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, input);

        var query = new Uri(newUri).Query;
        var actualQueryParameterValue = HttpUtility.ParseQueryString(query).Get(QueryParameterKey);

        await Assert.That(expectedQueryParameterValue).IsEqualTo(actualQueryParameterValue);
    }

    [Test]
    public async Task SetValue_UpdatesParameter()
    {
        var uri = $"https://localhost/test/page?{QueryParameterKey}=42";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, 101);

        await Assert.That(newUri).IsEqualTo($"https://localhost/test/page?{QueryParameterKey}=101");
    }

    [Test]
    public async Task SetValue_Null_RemovesParameter()
    {
        var uri = $"https://localhost/test/page?{QueryParameterKey}=42";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, null);

        await Assert.That(newUri).IsEqualTo("https://localhost/test/page");
    }
}
