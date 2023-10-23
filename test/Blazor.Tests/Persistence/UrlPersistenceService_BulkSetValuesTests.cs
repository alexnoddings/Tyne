using System.Web;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_BulkSetValuesTests : TestContext
{
    public UrlPersistenceService_BulkSetValuesTests() : base()
    {
        Services.AddSingleton<UrlPersistenceService>();
    }

    [Fact]
    public void BulkSetValues_Works()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

        var uri = "/test/page?param1=123&param2=456&param3=789";
        navigationManager.NavigateTo(uri);

        var queryParameters = new Dictionary<string, object?>
        {
            { "param2", null },
            { "param3", "aBc" },
            { "param4", SomeEnumType.ValueTwo.ToString() },
        };

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.BulkSetValues(queryParameters);

        var newQueryString = new Uri(navigationManager.Uri).Query;
        var newQuery = HttpUtility.ParseQueryString(newQueryString);

        // Param2 should be removed
        Assert.Equal(3, newQuery.Keys.Count);
        // Param1 shouldn't be changed
        Assert.Equal("123", newQuery["param1"]);
        // Param3 should be updated
        Assert.Equal("aBc", newQuery["param3"]);
        // Param4 should be added
        Assert.Equal(SomeEnumType.ValueTwo.ToString(), newQuery["param4"]);
    }
}
