using System.Web;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_SetValueTests : Bunit.TestContext
{
    private const string QueryParameterKey = "testSetValue";

    public UrlPersistenceService_SetValueTests()
    {
        Services
        .AddSingleton<IUrlQueryStringFormatter, UrlQueryStringFormatter>()
        .AddScoped<UrlPersistenceService>();
    }

    [Test]
    [MethodDataSource<UrlUtilities_TestHelpers>(nameof(UrlUtilities_TestHelpers.GetValueToStringData))]
    public async Task SetValue_Works(object? input, string? expectedQueryParameterValue)
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo("/test/page");

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue(QueryParameterKey, input);

        var query = new Uri(navigationManager.Uri).Query;
        var actualQueryParameterValue = HttpUtility.ParseQueryString(query).Get(QueryParameterKey);

        await Assert.That(expectedQueryParameterValue).IsEqualTo(actualQueryParameterValue);
    }

    [Test]
    public async Task SetValue_UpdatesParameter()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo($"/test/page?{QueryParameterKey}=42");

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue(QueryParameterKey, 101);

        await Assert.That(navigationManager.Uri).IsEqualTo($"http://localhost/test/page?{QueryParameterKey}=101");
    }

    [Test]
    public async Task SetValue_Null_RemovesParameter()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo($"/test/page?{QueryParameterKey}=42");

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue<int?>(QueryParameterKey, null);

        await Assert.That(navigationManager.Uri).IsEqualTo("http://localhost/test/page");
    }
}
