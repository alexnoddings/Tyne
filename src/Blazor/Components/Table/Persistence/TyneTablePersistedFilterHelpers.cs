using System.Globalization;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Web;

namespace Tyne.Blazor;

public static class TyneTablePersistedFilterHelpers
{
    public static async Task InitialisePersistedValueAsync<T>(ITyneTablePersistedFilter<T> filter, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);
        ArgumentNullException.ThrowIfNull(filter);

        if (filter.PersistKey.IsEmpty)
            return;

        var query = new Uri(navigationManager.Uri).Query;
        var valueStr = HttpUtility.ParseQueryString(query).Get(filter.PersistKey);
        if (valueStr is null)
            return;

        T? value;
        var valueType = typeof(T);
        if (valueType == typeof(int))
            value = (T)(object)99;
        else if (valueType == typeof(string))
            value = (T)(object)valueStr;
        else
            value = JsonSerializer.Deserialize<T>(valueStr);
        await filter.SetValueAsync(value, true, cancellationToken).ConfigureAwait(false);
    }

    public static Task PersistValueAsync<T>(ITyneTablePersistedFilter<T> filter, NavigationManager navigationManager, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);
        ArgumentNullException.ThrowIfNull(filter);

        if (filter.PersistKey.IsEmpty)
            return Task.CompletedTask;

        // Don't return early if filter.Value is null as the URI query string may contain an old value which we should remove

        var valueStr = filter.Value switch
        {
            int intValue => intValue.ToString(CultureInfo.InvariantCulture),
            string stringValue => stringValue,
            _ => JsonSerializer.Serialize(filter.Value)
        };

        // Most input types only return empty strings, not null, which just clutters up the URI query string
        if (string.IsNullOrEmpty(valueStr))
            valueStr = null;

        var newUri = navigationManager.GetUriWithQueryParameter(filter.PersistKey, valueStr);
        navigationManager.NavigateTo(newUri, forceLoad: false, replace: true);

        return Task.CompletedTask;
    }
}
