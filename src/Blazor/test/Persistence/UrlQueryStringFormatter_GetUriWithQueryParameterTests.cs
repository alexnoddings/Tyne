using System.Web;

namespace Tyne.Blazor.Persistence;

public class UrlQueryStringFormatter_GetUriWithQueryParameterTests
{
    private const string QueryParameterKey = "testSetValue";

    public static IEnumerable<object?[]> GetUriWithQueryParameterTests_Data => UrlUtilities_TestHelpers.ValueToString_Data;

    [Theory]
    [MemberData(nameof(GetUriWithQueryParameterTests_Data))]
    public void GetUriWithQueryParameterTests_Works(object? input, string? expectedQueryParameterValue)
    {
        var uri = "https://localhost/test/page";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, input);

        var query = new Uri(newUri).Query;
        var actualQueryParameterValue = HttpUtility.ParseQueryString(query).Get(QueryParameterKey);

        Assert.Equal(expectedQueryParameterValue, actualQueryParameterValue);
    }

    [Fact]
    public void SetValue_UpdatesParameter()
    {
        var uri = $"https://localhost/test/page?{QueryParameterKey}=42";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, 101);

        Assert.Equal($"https://localhost/test/page?{QueryParameterKey}=101", newUri);
    }

    [Fact]
    public void SetValue_Null_RemovesParameter()
    {
        var uri = $"https://localhost/test/page?{QueryParameterKey}=42";

        var urlQueryStringFormatter = new UrlQueryStringFormatter();
        var newUri = urlQueryStringFormatter.GetUriWithQueryParameter(uri, QueryParameterKey, null);

        Assert.Equal("https://localhost/test/page", newUri);
    }
}
