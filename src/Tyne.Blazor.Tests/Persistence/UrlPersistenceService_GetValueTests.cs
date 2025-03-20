using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_GetValueTests : Bunit.TestContext
{
    private const string QueryParameterKey = "testGetValue";

    private static readonly MethodInfo _getValueTMethodInfo = Get_GetValueT_MethodInfo();
    private static MethodInfo Get_GetValueT_MethodInfo()
    {
        const string methodName = nameof(GetValueT_Works);
        const BindingFlags methodFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var method =
            typeof(UrlPersistenceService_GetValueTests)
            .GetMethod(methodName, methodFlags)
            ?? throw new InvalidOperationException($"Could not load method info for generic test method '{methodName}'.");
        return method;
    }

    public UrlPersistenceService_GetValueTests()
    {
        Services
            .AddSingleton<IUrlQueryStringFormatter, UrlQueryStringFormatter>()
            .AddScoped<UrlPersistenceService>();
    }

    [Test]
    [MethodDataSource<UrlUtilities_TestHelpers>(nameof(UrlUtilities_TestHelpers.GetStringToValueData))]
    [SuppressMessage("Blocker Code Smell", "S2699: Tests should include assertions.", Justification = "Assertions are handled by the generic method invoked.")]
    public async Task GetValue_Works(string queryParameterValue, object expectedOption)
    {
        ArgumentNullException.ThrowIfNull(expectedOption);

        var expectedOptionType = expectedOption.GetType();
        if (!expectedOptionType.IsGenericType || expectedOptionType.GetGenericTypeDefinition() != typeof(Option<>))
            throw new ArgumentException("Value was not an Option<>.", nameof(expectedOption));

        var queryParameterEncodedValue =
            queryParameterValue is not null
            ? Uri.EscapeDataString(queryParameterValue)
            : null;

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo($"/test/page?{QueryParameterKey}={queryParameterEncodedValue}");

        var optionType = expectedOptionType.GenericTypeArguments[0];
        var task =
            _getValueTMethodInfo
            .MakeGenericMethod(optionType)
            .Invoke(this, [expectedOption]);

        await (Task)task!;
    }

    private async Task GetValueT_Works<T>(Option<T> expectedOption)
    {
        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        var actualValue = persistenceService.GetValue<T>(QueryParameterKey);
        await Assert.That(expectedOption).IsEqualTo(actualValue);
    }
}
