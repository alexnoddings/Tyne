using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

public class UrlPersistenceService_GetValueTests : TestContext
{
    private const string QueryParameterKey = "testGetValue";

    private static readonly MethodInfo GetValueT_MethodInfo = Get_GetValueT_MethodInfo();
    private static MethodInfo Get_GetValueT_MethodInfo()
    {
        const string methodName = nameof(GetValueT_Works);
        const BindingFlags methodFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var method =
            typeof(UrlPersistenceService_GetValueTests)
            .GetMethod(methodName, methodFlags);

        if (method is null)
            throw new InvalidOperationException($"Could not load method info for generic test method '{methodName}'.");

        return method;
    }

    public UrlPersistenceService_GetValueTests() : base()
    {
        Services.AddSingleton<UrlPersistenceService>();
    }

    public static object?[][] GetValue_Data { get; } =
        [
            [typeof(bool), Option.None<bool>(), null],
            [typeof(bool), Option.Some(true), "true"],
            [typeof(bool), Option.Some(false), "false"],
            [typeof(bool?), Option.None<bool?>(), null],
            [typeof(bool?), Option.None<bool?>(), ""],
            [typeof(bool?), Option.None<bool?>(), "   "],
            [typeof(bool?), Option.Some<bool?>(true), "true"],
            [typeof(bool?), Option.Some<bool?>(false), "false"],

            [typeof(string), Option.None<string>(), null],
            [typeof(string), Option.None<string>(), ""],
            [typeof(string), Option.None<string>(), "  "],
            [typeof(string), Option.Some("SomeValue"), "SomeValue"],

            [typeof(char), Option.None<char>(), null],
            [typeof(char), Option.None<char>(), ""],
            [typeof(char), Option.None<char>(), "aaa"],
            [typeof(char), Option.Some('a'), "a"],
            [typeof(char), Option.Some('*'), "*"],

            [typeof(int), Option.None<int>(), null],
            [typeof(int), Option.None<int>(), ""],
            [typeof(int), Option.Some(101), "101"],
            [typeof(int), Option.None<int>(), "9999999999"],

            [typeof(Guid), Option.None<Guid>(), null],
            [typeof(Guid), Option.None<Guid>(), "invalid guid"],
            [typeof(Guid), Option.Some(Guid.Parse("14b344cc-15bd-474d-a871-a6a71a58bea2")), "14b344cc-15bd-474d-a871-a6a71a58bea2"],

            [typeof(DateTime), Option.None<DateTime>(), "invalid datetime"],
            [typeof(DateTime), Option.Some(DateTime.Parse("2023-10-21 10:42:08", CultureInfo.InvariantCulture)), "20231021104208"],

            [typeof(SomeEnumType), Option.None<SomeEnumType>(), null],
            [typeof(SomeEnumType), Option.None<SomeEnumType>(), ""],
            [typeof(SomeEnumType), Option.Some(SomeEnumType.ValueOne), ((int)SomeEnumType.ValueOne).ToString(provider: null)],
            [typeof(SomeEnumType), Option.Some(SomeEnumType.ValueTwo), ((int)SomeEnumType.ValueTwo).ToString(provider: null)],
            [typeof(SomeEnumType), Option.Some(SomeEnumType.ValueOne), SomeEnumType.ValueOne.ToString()],
            [typeof(SomeEnumType), Option.Some(SomeEnumType.ValueTwo), SomeEnumType.ValueTwo.ToString()],

            [typeof(SerialisableData), Option.None<SerialisableData>(), ""],
            [typeof(SerialisableData), Option.Some(new SerialisableData()), "{}"],
            [typeof(SerialisableData), Option.Some(new SerialisableData(101, null, null)), @"{""X"":101}"],
            [typeof(SerialisableData), Option.Some(new SerialisableData(101, true, "aBc")), @"{""X"":101,""Y"":true,""Z"":""aBc""}"],
        ];

    [Theory]
    [MemberData(nameof(GetValue_Data))]
    [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters", Justification = "False positive, expectedValue is used.")]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Assertions are handled by the generic method invoked.")]
    public void GetValue_Works(Type targetType, object expectedValue, string? queryParameterValue)
    {
        var queryParameterEncodedValue =
            queryParameterValue is not null
            ? Uri.EscapeDataString(queryParameterValue)
            : null;

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo($"/test/page?{QueryParameterKey}={queryParameterEncodedValue}");

        GetValueT_MethodInfo
            .MakeGenericMethod(targetType)
            .Invoke(this, [expectedValue]);
    }

    private void GetValueT_Works<T>(Option<T> expectedValue)
    {
        var persistenceService = Services.GetRequiredService<UrlPersistenceService>();
        var actualValue = persistenceService.GetValue<T>(QueryParameterKey);
        Assert.Equal(expectedValue, actualValue);
    }
}
