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

    public static async Task InitialisePersistedValueAsync<T>(ITyneTablePersistedFilter<T> filter, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);
        ArgumentNullException.ThrowIfNull(filter);

        var persistKey = filter.PersistKey;
        if (persistKey.IsEmpty)
            return;

        var query = new Uri(navigationManager.Uri).Query;
        var valueStr = HttpUtility.ParseQueryString(query).Get(persistKey);

        if (valueStr is null)
            return;

        // Both strings and chars are serialised with ""s, which looks ugly in URLs
        // So as special cases, they are both encoded as literals
        T? value;
        if (typeof(T) == typeof(string))
        {
            if (string.IsNullOrWhiteSpace(valueStr))
                value = default;
            else
                value = (T)(object)valueStr.Trim();
        }
        else if (typeof(T) == typeof(char) || Nullable.GetUnderlyingType(typeof(T)) == typeof(char))
        {
            if (valueStr.Length == 1)
                value = (T)(object)valueStr[0];
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
                return;
            }
        }

        await filter.SetValueAsync(value, true, cancellationToken).ConfigureAwait(false);
    }

    public static Task PersistValueAsync<T>(ITyneTablePersistedFilter<T> filter, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        // This is Task-returning even though it is entirely synchronous so
        // that the implementation is open to modification later without
        // breaking existing usage.
        ArgumentNullException.ThrowIfNull(navigationManager);
        ArgumentNullException.ThrowIfNull(filter);

        var persistKey = filter.PersistKey;
        if (persistKey.IsEmpty)
            return Task.CompletedTask;

        var valueStr = filter.Value switch
        {
            // Don't return early if filter.Value is null as the URI query string may contain an old value which we should remove
            null => null,
            char value => value.ToString(),
            string value => value,
            _ => JsonSerializer.Serialize(filter.Value, JsonOptions)
        };

        if (string.IsNullOrEmpty(valueStr))
            valueStr = null;

        var newUri = navigationManager.GetUriWithQueryParameter(persistKey, valueStr);
        navigationManager.NavigateTo(newUri, forceLoad: false, replace: true);

        return Task.CompletedTask;
    }
}
