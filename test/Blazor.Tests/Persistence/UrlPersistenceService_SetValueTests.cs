using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Web;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_SetValueTests : TestContext
{
    private const string QueryParameterKey = "testSetValue";

    private static readonly MethodInfo SetValueT_MethodInfo = Get_SetValueT_MethodInfo();
    private static MethodInfo Get_SetValueT_MethodInfo()
    {
        const string methodName = nameof(SetValueT_Works);
        const BindingFlags methodFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var method =
            typeof(UrlPersistenceService_SetValueTests)
            .GetMethod(methodName, methodFlags);

        if (method is null)
            throw new InvalidOperationException($"Could not load method info for generic test method '{methodName}'.");

        return method;
    }

    public UrlPersistenceService_SetValueTests() : base()
    {
        Services.AddSingleton<UrlPersistenceService>();
    }

    public static object?[][] SetValue_Data { get; } =
        [
            [typeof(bool), true, "true"],
            [typeof(bool), false, "false"],
            [typeof(bool?), null, null],
            [typeof(bool?), true, "true"],
            [typeof(bool?), false, "false"],

            [typeof(string), null, null],
            [typeof(string), "", null],
            [typeof(string), "   ", null],
            [typeof(string), "SomeValue", "SomeValue"],
            [typeof(string), " SomeValue  ", "SomeValue"],

            [typeof(char), 'a', "a"],
            [typeof(char), '*', "*"],

            [typeof(int), -1, "-1"],
            [typeof(int), 0, "0"],
            [typeof(int), 42, "42"],

            [typeof(Guid), Guid.Parse("035859e0-748e-4a54-95c9-3d5428296c6a"), "035859e0-748e-4a54-95c9-3d5428296c6a"],

            [typeof(DateTime), DateTime.Parse("2023-10-21 10:42:08", CultureInfo.InvariantCulture), "20231021104208"],

            [typeof(SomeEnumType), SomeEnumType.ValueOne, SomeEnumType.ValueOne.ToString()],
            [typeof(SomeEnumType), SomeEnumType.ValueTwo, SomeEnumType.ValueTwo.ToString()],

            [typeof(SerialisableData), new SerialisableData(101, null, null), @"{""x"":101}"],
            [typeof(SerialisableData), new SerialisableData(101, true, "aBc"), @"{""x"":101,""y"":true,""z"":""aBc""}"],
        ];

    [Theory]
    [MemberData(nameof(SetValue_Data))]
    [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters", Justification = "False positive, expectedValue is used.")]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Assertions are handled by the generic method invoked.")]
    public void SetValue_Works(Type targetType, object actualValue, string? expectedQueryParameterValue)
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo("/test/page");

        SetValueT_MethodInfo
            .MakeGenericMethod(targetType)
            .Invoke(this, [actualValue, expectedQueryParameterValue]);
    }

    private void SetValueT_Works<T>(T? value, string? expectedQueryParameterValue)
    {
        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue(QueryParameterKey, value);

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

        var query = new Uri(navigationManager.Uri).Query;
        var actualQueryParameterValue = HttpUtility.ParseQueryString(query).Get(QueryParameterKey);

        Assert.Equal(expectedQueryParameterValue, actualQueryParameterValue);
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

    [Fact]
    public void SetValue_UpdatesParameter()
    {
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo($"/test/page?{QueryParameterKey}=42");

        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        persistenceService.SetValue(QueryParameterKey, 101);

        Assert.Equal($"http://localhost/test/page?{QueryParameterKey}=101", navigationManager.Uri);
    }
}
