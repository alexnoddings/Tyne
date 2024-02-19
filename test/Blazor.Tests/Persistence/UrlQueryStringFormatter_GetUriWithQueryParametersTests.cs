using System.Web;

namespace Tyne.Blazor.Persistence;

public class UrlQueryStringFormatter_GetUriWithQueryParametersTests
{
    private const string QueryParameterKey = "testSetValue";

    [Fact]
    public void SetValue_Null_RemovesParameter()
    {
        var uri = $"https://localhost/test/page?{QueryParameterKey}=42";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, null);

        Assert.Equal($"https://localhost/test/page", newUri);
    }

    [Fact]
    public void BulkSetValues_Dictionary_Works()
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

        AssertParamsUpdated(newUri);
    }

    [Fact]
    public void BulkSetValues_Object_Works()
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

        AssertParamsUpdated(newUri);
    }

    private static void AssertParamsUpdated(string uri)
    {
        var newQueryString = new Uri(uri).Query;
        var newQuery = HttpUtility.ParseQueryString(newQueryString);

        // Param2 should be removed
        Assert.Equal(3, newQuery.Keys.Count);
        // Param1 shouldn't be changed
        Assert.Equal("123", newQuery["param1"]);
        // Param3 should be updated
        Assert.Equal("aBc", newQuery["param3"]);
        // Param4 should be added
        Assert.Equal(nameof(SomeEnumType.ValueTwo), newQuery["param4"]);
    }
}
