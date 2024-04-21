using System.Net;

namespace Tyne.AspNetCore.Endpoints;

[Collection(TestWebAppCollection.Name)]
public class NotFoundEndpointTests
{
    private TestWebAppFactory TestWebApp { get; }

    public NotFoundEndpointTests(TestWebAppFactory testWebAppFactory)
    {
        TestWebApp = testWebAppFactory;
    }

    public static TheoryData<HttpMethod> SupportedMethodsData => new(SupportedMethods);
    public static IEnumerable<HttpMethod> SupportedMethods
    {
        get
        {
            yield return HttpMethod.Put;
            yield return HttpMethod.Post;
            yield return HttpMethod.Patch;
            yield return HttpMethod.Options;
            yield return HttpMethod.Head;
            yield return HttpMethod.Get;
            yield return HttpMethod.Delete;
        }
    }

    public static TheoryData<HttpMethod, Uri> GetUris(string type)
    {
        var theoryData = new TheoryData<HttpMethod, Uri>();
        foreach (var uri in EnumerateUris())
        {
            foreach (var method in SupportedMethods)
            {
                theoryData.Add(method, new Uri(uri, UriKind.Relative));
            }
        }
        return theoryData;

        IEnumerable<string> EnumerateUris()
        {
            yield return $"/test/notfound/{type}";
            yield return $"/test/notfound/{type}/";
            yield return $"/test/notfound/{type}/test";
            yield return $"/test/notfound/{type}/test/with/path";
            yield return $"/test/notfound/{type}/test?with=query&and=a#fragment";
        }
    }

    public static TheoryData<HttpMethod, Uri> GetDefaultUris() =>
        GetUris("default");

    public static TheoryData<HttpMethod, Uri> GetSyncHandlerUris() =>
        GetUris("sync-handler");

    public static TheoryData<HttpMethod, Uri> GetAsyncHandlerUris() =>
        GetUris("async-handler");

    [Theory]
    [MemberData(nameof(GetDefaultUris))]
    public async Task NoHandler_ReturnsEmpty404(HttpMethod method, Uri uri)
    {
        // Arrange
        var httpClient = TestWebApp.CreateClient();

        // Act
        using var request = CreateRequest(method, uri);
        var response = await httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseContent = await response.Content.ReadAsByteArrayAsync();
        Assert.Empty(responseContent);
    }

    [Theory]
    [MemberData(nameof(GetSyncHandlerUris))]
    public async Task SyncHandler_ReturnsEmpty404WithHeader(HttpMethod method, Uri uri)
    {
        // Arrange
        var httpClient = TestWebApp.CreateClient();

        // Act
        using var request = CreateRequest(method, uri);
        var response = await httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseContent = await response.Content.ReadAsByteArrayAsync();
        Assert.Empty(responseContent);
        Assert.Contains(response.Headers, header => header.Key == TestWebAppHost.NotFoundSyncHandlerHeaderKey);
    }

    [Theory]
    [MemberData(nameof(GetAsyncHandlerUris))]
    public async Task AsyncHandler_Returns404WithCustomContent(HttpMethod method, Uri uri)
    {
        // Arrange
        var httpClient = TestWebApp.CreateClient();

        // Act
        using var request = CreateRequest(method, uri);
        var response = await httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(TestWebAppHost.NotFoundAsyncHandlerBodyMessage, responseContent);
    }

    // This tests that the web app host returns a BadRequest
    // for invalid 'not found' endpoints rather than
    // using the default ASP.NET fallback behaviour
    [Theory]
    [MemberData(nameof(SupportedMethodsData))]
    public async Task Unhandled_ReturnsBadRequest(HttpMethod method)
    {
        // Arrange
        var httpClient = TestWebApp.CreateClient();

        // Act
        using var request = CreateRequest(method, new Uri("/_meta/unmapped/path", UriKind.Relative));
        var response = await httpClient.SendAsync(request);

        Assert.Equal(TestWebAppHost.InvalidTestRequestStatusCode, (int)response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(TestWebAppHost.InvalidTestRequestBodyMessage, responseContent);
    }

    private static HttpRequestMessage CreateRequest(HttpMethod method, Uri uri) =>
        new HttpRequestMessage(method, uri);
}
