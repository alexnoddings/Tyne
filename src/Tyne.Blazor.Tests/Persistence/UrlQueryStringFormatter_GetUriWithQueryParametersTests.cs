using System.Web;

namespace Tyne.Blazor.Persistence;

public class UrlQueryStringFormatter_GetUriWithQueryParametersTests
{
    private const string QueryParameterKey = "testSetValue";

    [Test]
    public async Task SetValue_Null_RemovesParameter()
    {
        var uri = $"https://localhost/test/page?{QueryParameterKey}=42";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, null);

        await Assert.That(newUri).IsEqualTo("https://localhost/test/page");
    }

    [Test]
    public async Task BulkSetValues_Dictionary_Works()
    {
        var uri = "https://localhost/test/page?param1=123&param2=456&param3=789";

        var queryParameters = new Dictionary<string, object?>
        {
            { "param2", null },
            { "param3", "aBc" },
            { "param4", nameof(SomeEnumType.ValueTwo) },
        };

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameters(uri, queryParameters);

        await AssertParamsUpdated(newUri);
    }

    [Test]
    public async Task BulkSetValues_Object_Works()
    {
        var uri = "https://localhost/test/page?param1=123&param2=456&param3=789";

        var queryParameters = new
        {
            param2 = (object?)null,
            param3 = "aBc",
            param4 = nameof(SomeEnumType.ValueTwo),
        };

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameters(uri, queryParameters);

        await AssertParamsUpdated(newUri);
    }

    private static async Task AssertParamsUpdated(string uri)
    {
        var newQueryString = new Uri(uri).Query;
        var newQuery = HttpUtility.ParseQueryString(newQueryString);

        // Param2 should be removed
        await Assert.That(newQuery.Keys.Count).IsEqualTo(3);
        // Param1 shouldn't be changed
        await Assert.That(newQuery["param1"]).IsEqualTo("123");
        // Param3 should be updated
        await Assert.That(newQuery["param3"]).IsEqualTo("aBc");
        // Param4 should be added
        await Assert.That(newQuery["param4"]).IsEqualTo(nameof(SomeEnumType.ValueTwo));
    }
}
