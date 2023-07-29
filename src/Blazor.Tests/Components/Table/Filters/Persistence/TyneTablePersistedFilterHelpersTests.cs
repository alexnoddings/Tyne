using System.Reflection;
using System.Web;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor;

public class TyneTablePersistedFilterHelpersTests : TestContext
{
    public static object?[][] InitialisePersistedValueAsync_Data { get; } =
        new object?[][]
        {
            new object?[] { typeof(bool), null, false },
            new object?[] { typeof(bool), "true", true },
            new object?[] { typeof(bool), "false", false },
            new object?[] { typeof(bool?), null, null },
            new object?[] { typeof(bool?), "", null },
            new object?[] { typeof(bool?), "   ", null },

            new object?[] { typeof(string), "", null },
            new object?[] { typeof(string), "   ", null },
            new object?[] { typeof(string), "SomeValue", "SomeValue" },
            new object?[] { typeof(string), " SomeValue\t ", "SomeValue" },

            new object?[] { typeof(char), "", default(char) },
            new object?[] { typeof(char), "a", 'a' },
            new object?[] { typeof(char), "*", '*' },
            new object?[] { typeof(char?), "", null },
            new object?[] { typeof(char?), "aaa", null },
            new object?[] { typeof(char?), "a", 'a' },
            new object?[] { typeof(char?), "*", '*' },

            new object?[] { typeof(int), "", default(int) },
            new object?[] { typeof(int), "101", 101 },
            new object?[] { typeof(int), " 101\t ", 101 },
            new object?[] { typeof(int?), "", null },
            new object?[] { typeof(int?), "x", null },
            new object?[] { typeof(int?), "9999999999", null },

            new object?[] { typeof(SerialisableData), "", null },
            new object?[] { typeof(SerialisableData), "{}", new SerialisableData(0, null, null) },
            new object?[] { typeof(SerialisableData), @"{""X"":101}", new SerialisableData(101, null, null) },
            new object?[] { typeof(SerialisableData), @"{""X"":101,""Y"":true,""Z"":""aBc""}", new SerialisableData(101, true, "aBc") },
        };

    [Theory]
    [MemberData(nameof(InitialisePersistedValueAsync_Data))]
    public async Task InitialisePersistedValueAsync_CorrectlyParses(Type filterType, string queryValue, object expectedValue)
    {
        const string methodName = nameof(InitialisePersistedValueAsync_CorrectlyParsesT);
        const BindingFlags methodFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var genericMethod = GetType().GetMethod(methodName, methodFlags);
        if (genericMethod is null)
            Assert.Fail($"Could not load method info for generic test method '{methodName}'.");

        var taskObject = genericMethod.MakeGenericMethod(filterType).Invoke(this, new object?[] { queryValue, expectedValue });
        await (Task)taskObject!;
    }

    private async Task InitialisePersistedValueAsync_CorrectlyParsesT<T>(string queryValue, T expectedValue)
    {
        const string uriQueryKey = "queryPersistKey";

        // Arrange
        var uri = $"https://example/route/page?{uriQueryKey}={queryValue}";

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo(uri);

        var filter = new PersistedFilterImplementation<T>
        {
            PersistKey = TyneTableKey.From(uriQueryKey)
        };

        // Act
        await TyneTablePersistedFilterHelpers.InitialisePersistedValueAsync(filter, navigationManager);

        // Assert
        Assert.Equal(expectedValue, filter.Value);
    }

    [Fact]
    public async Task InitialisePersistedValueAsync_Throws_ArgumentNullException_ForNullFilter()
    {
        // Arrange
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => TyneTablePersistedFilterHelpers.InitialisePersistedValueAsync<int>(null!, navigationManager)
        );
    }

    [Fact]
    public async Task InitialisePersistedValueAsync_Throws_ArgumentNullException_ForNullNavigationManager()
    {
        const string uriQueryKey = "queryPersistKey";

        // Arrange
        var filter = new PersistedFilterImplementation<int>
        {
            PersistKey = TyneTableKey.From(uriQueryKey)
        };

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => TyneTablePersistedFilterHelpers.InitialisePersistedValueAsync(filter, null!)
        );
    }

    public static object?[][] PersistValueAsync_Data { get; } =
        new object?[][]
        {
            new object?[] { typeof(bool), false, "false" },
            new object?[] { typeof(bool), true, "true" },
            new object?[] { typeof(bool?), null, null },
            new object?[] { typeof(bool?), false, "false" },
            new object?[] { typeof(bool?), true, "true" },

            new object?[] { typeof(string), null, null },
            new object?[] { typeof(string), "", null },
            new object?[] { typeof(string), " ", " " },
            new object?[] { typeof(string), " SomeValue\t ", " SomeValue\t " },

            new object?[] { typeof(char), 'a', "a" },
            new object?[] { typeof(char), '*', "*" },
            new object?[] { typeof(char), default(char), default(char).ToString() },
            new object?[] { typeof(char?), 'a', "a" },
            new object?[] { typeof(char?), '*', "*" },
            new object?[] { typeof(char?), null, null },
            new object?[] { typeof(char?), default(char), default(char).ToString() },

            new object?[] { typeof(int), 0, "0" },
            new object?[] { typeof(int), 101, "101" },
            new object?[] { typeof(int?), 0, "0" },
            new object?[] { typeof(int?), 101, "101" },
            new object?[] { typeof(int?), null, null },

            new object?[] { typeof(SerialisableData), new SerialisableData(101, null, null), @"{""x"":101}", },
            new object?[] { typeof(SerialisableData), new SerialisableData(101, true, "aBc"), @"{""x"":101,""y"":true,""z"":""aBc""}" },
            new object?[] { typeof(SerialisableData), new SerialisableData(101, true, " aBc\t "), @"{""x"":101,""y"":true,""z"":"" aBc\t ""}" },
            new object?[] { typeof(SerialisableData?), null, null, },
            new object?[] { typeof(SerialisableData?), new SerialisableData(101, null, null), @"{""x"":101}", },
            new object?[] { typeof(SerialisableData?), new SerialisableData(101, true, "aBc"), @"{""x"":101,""y"":true,""z"":""aBc""}" },
            new object?[] { typeof(SerialisableData?), new SerialisableData(101, true, " aBc\t "), @"{""x"":101,""y"":true,""z"":"" aBc\t ""}" },
        };

    [Theory]
    [MemberData(nameof(PersistValueAsync_Data))]
    public async Task PersistValueAsync_CorrectlyPersists(Type filterType, object value, string? expectedQueryValue)
    {
        const string methodName = nameof(PersistValueAsync_CorrectlyPersistsT);
        const BindingFlags methodFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var genericMethod = GetType().GetMethod(methodName, methodFlags);
        if (genericMethod is null)
            Assert.Fail($"Could not load method info for generic test method '{methodName}'.");

        var taskObject = genericMethod.MakeGenericMethod(filterType).Invoke(this, new object?[] { value, expectedQueryValue });
        await (Task)taskObject!;
    }

    private async Task PersistValueAsync_CorrectlyPersistsT<T>(T value, string? expectedQueryValue)
    {
        const string uriQueryKey = "queryPersistKey";

        // Arrange
        var uri = $"https://example/route/page";

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        navigationManager.NavigateTo(uri);

        var filter = new PersistedFilterImplementation<T>
        {
            PersistKey = TyneTableKey.From(uriQueryKey)
        };
        await filter.SetValueAsync(value, false);

        // Act
        await TyneTablePersistedFilterHelpers.PersistValueAsync(filter, navigationManager);

        // Assert
        if (expectedQueryValue is null)
        {
            // The URI should not have changed
            Assert.Equal(uri, navigationManager.Uri);
        }
        else
        {
            // The URI should be updated with the new query parameter
            var queryString = new Uri(navigationManager.Uri).Query;
            var queryValue = HttpUtility.ParseQueryString(queryString).Get(uriQueryKey);
            Assert.Equal(expectedQueryValue, queryValue);
        }
    }

    [Fact]
    public async Task PersistValueAsync_Throws_ArgumentNullException_ForNullFilter()
    {
        // Arrange
        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => TyneTablePersistedFilterHelpers.PersistValueAsync<int>(null!, navigationManager)
        );
    }

    [Fact]
    public async Task PersistValueAsync_Throws_ArgumentNullException_ForNullNavigationManager()
    {
        const string uriQueryKey = "queryPersistKey";

        // Arrange
        var filter = new PersistedFilterImplementation<int>
        {
            PersistKey = TyneTableKey.From(uriQueryKey)
        };

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => TyneTablePersistedFilterHelpers.PersistValueAsync(filter, null!)
        );
    }

    private record struct SerialisableData(int X, bool? Y, string? Z);

    private sealed class PersistedFilterImplementation<TValue> : ITyneTablePersistedFilter<TValue>
    {
        public TValue? Value { get; set; }

        public TyneTableKey PersistKey { get; set; }

        public Task<bool> SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default)
        {
            if (EqualityComparer<TValue>.Default.Equals(newValue, Value))
                return Task.FromResult(false);

            Value = newValue;
            return Task.FromResult(true);
        }

        public Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
            SetValueAsync(default, false, cancellationToken);
    }
}
