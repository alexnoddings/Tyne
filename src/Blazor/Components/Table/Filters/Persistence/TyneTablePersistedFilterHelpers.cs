using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Tyne.Blazor;

public static class TyneTablePersistedFilterHelpers
{
    public static JsonSerializerOptions JsonOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public delegate Task<bool> SetFilterValue<in T>(T? newValue, bool isSilent, CancellationToken cancellationToken = default);

    public static Task<bool> InitialisePersistedValueAsync<T>(ITyneTablePersistedFilter<T> filter, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return InitialisePersistedValueAsync<T>(filter.PersistKey, filter.SetValueAsync, navigationManager, cancellationToken);
    }

    public static async Task<bool> InitialisePersistedValueAsync<T>(TyneTableKey key, SetFilterValue<T> setFilterValue, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);
        ArgumentNullException.ThrowIfNull(setFilterValue);

        if (key.IsEmpty)
            return false;

        var query = new Uri(navigationManager.Uri).Query;
        var valueStr = HttpUtility.ParseQueryString(query).Get(key);

        if (valueStr is null)
            return false;

        // Normally, everything is JSON serialised with ""s, which can look ugly in URLs
        // Some special cases are encoded as literals instead
        T? value;
        if (IsTargetType<string, T>())
        {
            if (string.IsNullOrWhiteSpace(valueStr))
                value = default;
            else
                value = (T)(object)valueStr.Trim();
        }
        else if (IsTargetType<char, T>())
        {
            if (valueStr.Length == 1)
                value = (T)(object)valueStr[0];
            else
                value = default;
        }
        else if (IsTargetType<Guid, T>())
        {
            if (Guid.TryParse(valueStr, out var guid))
                value = (T)(object)guid;
            else
                value = default;
        }
        else
        {
            try
            {
                value = JsonSerializer.Deserialize<T>(valueStr, JsonOptions);
            }
            catch (JsonException)
            {
                return false;
            }
        }

        return await setFilterValue(value, true, cancellationToken).ConfigureAwait(false);
    }

    private static bool IsTargetType<TExpected, TActual>()
    {
        var expected = typeof(TExpected);
        var actual = typeof(TActual);
        var isType = expected == actual;
        if (expected.IsValueType)
            isType |= Nullable.GetUnderlyingType(actual) == expected;

        return isType;
    }

    public static Task PersistValueAsync<T>(ITyneTablePersistedFilter<T> filter, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return PersistValueAsync(filter.PersistKey, filter.Value, navigationManager, cancellationToken);
    }

    public static Task PersistValueAsync<T>(TyneTableKey key, T value, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        // This is Task-returning even though it is entirely synchronous so
        // that the implementation is open to modification later without
        // breaking existing usage.
        ArgumentNullException.ThrowIfNull(navigationManager);

        if (key.IsEmpty)
            return Task.CompletedTask;

        var valueStr = value switch
        {
            // Don't return early if filter.Value is null as the URI query string may contain an old value which we should remove
            null => null,
            string str => str,
            char chr => chr.ToString(),
            Guid guid => guid.ToString("D"),
            _ => JsonSerializer.Serialize(value, JsonOptions)
        };

        if (string.IsNullOrEmpty(valueStr))
            valueStr = null;

        var newUri = navigationManager.GetUriWithQueryParameter(key, valueStr);
        navigationManager.NavigateTo(newUri, forceLoad: false, replace: true);

        return Task.CompletedTask;
    }
}
