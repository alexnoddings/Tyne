using System.Web;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_SetValueTests : TestContext
{
    private const string QueryParameterKey = "testSetValue";

    public UrlPersistenceService_SetValueTests() : base()
    {
        Services.AddSingleton<IUrlQueryStringFormatter, UrlQueryStringFormatter>();
        Services.AddSingleton<UrlPersistenceService>();
    }

    public static IEnumerable<object?[]> SetValue_Data => UrlUtilities_TestHelpers.ValueToString_Data;

    [Theory]
    [MemberData(nameof(SetValue_Data))]
    public void SetValue_Works(object? input, string? expectedQueryParameterValue)
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo("/test/page");

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue(QueryParameterKey, input);

        var query = new Uri(navigationManager.Uri).Query;
        var actualQueryParameterValue = HttpUtility.ParseQueryString(query).Get(QueryParameterKey);

        Assert.Equal(expectedQueryParameterValue, actualQueryParameterValue);
    }

    [Fact]
    public void SetValue_UpdatesParameter()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo($"/test/page?{QueryParameterKey}=42");

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue(QueryParameterKey, 101);

        Assert.Equal($"http://localhost/test/page?{QueryParameterKey}=101", navigationManager.Uri);
    }

    [Fact]
    public void SetValue_Null_RemovesParameter()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo($"/test/page?{QueryParameterKey}=42");

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue<int?>(QueryParameterKey, null);

        Assert.Equal("http://localhost/test/page", navigationManager.Uri);
    }
}
