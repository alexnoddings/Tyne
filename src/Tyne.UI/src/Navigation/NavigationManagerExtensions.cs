using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;

namespace Microsoft.AspNetCore.Components;

/// <summary>
///     Extension methods for <see cref="NavigationManager"/>.
/// </summary>
public static class NavigationManagerExtensions
{
    /// <summary>
    ///     Tries to load a parameter from <see cref="NavigationManager.Uri"/>.
    /// </summary>
    /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
    /// <param name="name">The name of the parameter to get.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <remarks>
    ///     If the parameter is not found, or does not have a value, <paramref name="value"/> will be initialised as <see langword="null"/>, and <see langword="false"/> will be returned.
    /// </remarks>
    public static bool TryGetQueryParameter(this NavigationManager navigationManager, string name, [NotNullWhen(true)] out string? value)
    {
        var query = new Uri(navigationManager.Uri).Query;
        if (QueryHelpers.ParseQuery(query).TryGetValue(name, out var values) && values.Count > 0)
        {
            value = values[0];
            return true;
        }

        value = null;
        return false;
    }

    /// <summary>
    ///     Tries to load a parameter from <see cref="NavigationManager.Uri"/>.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
    /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
    /// <param name="name">The name of the parameter to get.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <remarks>
    ///     If the parameter is not found, or does not have a value, <paramref name="value"/> will be initialised as <see langword="null"/>, and <see langword="false"/> will be returned.
    /// </remarks>
    [RequiresPreviewFeatures]
    public static bool TryGetQueryParameter<T>(this NavigationManager navigationManager, string name, out T? value) where T : IParseable<T>
    {
        var query = new Uri(navigationManager.Uri).Query;
        if (QueryHelpers.ParseQuery(query).TryGetValue(name, out var values) && values.Count > 0 && T.TryParse(values[0], CultureInfo.InvariantCulture, out value))
            return true;

        value = default;
        return false;
    }

    /// <summary>
    ///     Adds, updates, or removes a single parameter in <see cref="NavigationManager.Uri"/>.
    /// </summary>
    /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
    /// <param name="name">The name of the parameter to add or update.</param>
    /// <param name="value">The value of the parameter to add or update.</param>
    /// <param name="replace">If true, replaces the current entry in the history stack. If false, appends the new entry to the history stack.</param>
    /// <remarks>If <paramref name="value"/> is <see langword="null"/>, the parameter will be removed if it exists in the URI. Otherwise, it will be added or updated.</remarks>
    public static void SetQueryParameter(this NavigationManager navigationManager, string name, string? value, bool replace = false)
    {
        var newUri = navigationManager.GetUriWithQueryParameter(name, value);
        navigationManager.NavigateTo(newUri, replace: replace);
    }

    /// <summary>
    ///     Adds, updates, or removes a single parameter in <see cref="NavigationManager.Uri"/>.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
    /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
    /// <param name="name">The name of the parameter to add or update.</param>
    /// <param name="value">The value of the parameter to add or update.</param>
    /// <param name="replace">If true, replaces the current entry in the history stack. If false, appends the new entry to the history stack.</param>
    /// <remarks>
    ///     If <paramref name="value"/> is <see langword="null"/>, the parameter will be removed if it exists in the URI. Otherwise, it will be added or updated.
    ///     This method naively calls <see cref="object.ToString()"/> on <paramref name="value"/>. If you require custom formatting or serialisation,
    ///     consider implementing that and calling <see cref="SetQueryParameter(NavigationManager, string, string, bool)"/> instead.
    /// </remarks>
    public static void SetQueryParameter<T>(this NavigationManager navigationManager, string name, T? value, bool replace = false)
    {
        var valueStr = value?.ToString();
        var newUri = navigationManager.GetUriWithQueryParameter(name, valueStr);
        navigationManager.NavigateTo(newUri, replace: replace);
    }
}
