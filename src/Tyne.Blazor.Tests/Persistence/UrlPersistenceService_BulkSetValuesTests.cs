using System.Web;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_BulkSetValuesTests : TestContext
{
    public UrlPersistenceService_BulkSetValuesTests()
    {
        _ = Services
            .AddSingleton<IUrlQueryStringFormatter, UrlQueryStringFormatter>()
            .AddScoped<UrlPersistenceService>();
    }

    [Fact]
    public void BulkSetValues_Dictionary_Works()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

        var uri = "/test/page?param1=123&param2=456&param3=789";
        navigationManager.NavigateTo(uri);

        var queryParameters = new Dictionary<string, object?>
        {
            { "param2", null },
            { "param3", "aBc" },
            { "param4", nameof(SomeEnumType.ValueTwo) },
        };

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.BulkSetValues(queryParameters);

        AssertParamsUpdated(navigationManager.Uri);
    }

    [Fact]
    public void BulkSetValues_Object_Works()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

        var uri = "/test/page?param1=123&param2=456&param3=789";
        navigationManager.NavigateTo(uri);

        var queryParameters = new
        {
            param2 = (object?)null,
            param3 = "aBc",
            param4 = nameof(SomeEnumType.ValueTwo),
        };

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.BulkSetValues(queryParameters);

        AssertParamsUpdated(navigationManager.Uri);
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
