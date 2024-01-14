using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;

namespace Tyne.Blazor.Persistence;

/// <inheritdoc/>
internal sealed class UrlQueryStringFormatter : IUrlQueryStringFormatter
{
    /// <inheritdoc/>
    public Option<T> GetValue<T>(string uri, string key)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(key);

        if (string.IsNullOrWhiteSpace(key))
            return Option.None<T>();

        var queryString = new Uri(uri).Query;
        var valueStr = HttpUtility.ParseQueryString(queryString).Get(key);
        if (valueStr is null)
            return Option.None<T>();

        return UrlUtilities.TryParse<T>(valueStr);
    }

    /// <inheritdoc/>
    [RequiresUnreferencedCode($"This API is not trim safe.")]
    public string GetUriWithQueryParameter(string uri, string key, object? value)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(key);

        if (string.IsNullOrWhiteSpace(key))
            return uri.ToString();

        var valueStr = UrlUtilities.FormatValueToString(value);
        var newUri =
            valueStr is not null
            ? NavigationManagerHelper.GetUriWithUpdatedQueryParameter(uri, key, valueStr)
            : NavigationManagerHelper.GetUriWithRemovedQueryParameter(uri, key);

        return newUri;
    }

    /// <inheritdoc/>
    public string GetUriWithQueryParameters(string uri, IReadOnlyDictionary<string, object?> parameters)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(parameters);

        var formattedParameters = parameters.ToDictionary(
            kv => kv.Key,
            kv => ValueToString(kv.Value)
        );

        return NavigationManagerHelper.GetUriWithQueryParameters(uri, formattedParameters);
    }

    /// <inheritdoc/>
    [RequiresUnreferencedCode($"This API is not trim safe. It relies on reflecting on {nameof(parameters)}'s properties.")]
    public string GetUriWithQueryParameters(string uri, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] object parameters)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(parameters);

        var formattedParameters =
            parameters
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(propertyInfo => propertyInfo.CanRead)
            .Select(propertyInfo => (
                Key: propertyInfo.Name,
                Value: propertyInfo.GetValue(parameters))
            ).ToDictionary(
                kv => kv.Key,
                kv => ValueToString(kv.Value)
            );

        return NavigationManagerHelper.GetUriWithQueryParameters(uri, formattedParameters);
    }

    // This is just convenience to avoid ugly long casting inline
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? ValueToString(object? value) =>
        UrlUtilities.FormatValueToString(value);
}
