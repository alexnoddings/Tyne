using System.Web;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_BulkSetValuesTests : Bunit.TestContext
{
    public UrlPersistenceService_BulkSetValuesTests()
    {
        _ = Services
            .AddSingleton<IUrlQueryStringFormatter, UrlQueryStringFormatter>()
            .AddScoped<UrlPersistenceService>();
    }

    [Test]
    public async Task BulkSetValues_Dictionary_Works()
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

        await AssertParamsUpdated(navigationManager.Uri);
    }

    [Test]
    public async Task BulkSetValues_Object_Works()
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

        await AssertParamsUpdated(navigationManager.Uri);
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
