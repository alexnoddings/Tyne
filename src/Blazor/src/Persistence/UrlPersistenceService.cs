using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Persistence;

/// <summary>
///     Handles persisting data to the URL in the query string using <see cref="NavigationManager"/>.
/// </summary>
/// <remarks>
///     <para>
///         This is designed to persist page data between sessions.
///     </para>
///     <para>
///         Values are formatted according to <see cref="IUrlQueryStringFormatter"/>.
///     </para>
/// </remarks>
internal sealed class UrlPersistenceService : IUrlPersistenceService
{
    // This class doesn't do anything clever itself, it's primarily here as a QoL
    // feature to coordinate between the navigation manager and url formatter service
    private readonly IUrlQueryStringFormatter _urlQueryStringFormatter;
    private readonly NavigationManager _navigationManager;

    /// <summary>
    ///     Initialises a new <see cref="UrlPersistenceService"/>.
    /// </summary>
    /// <param name="urlQueryStringFormatter">
    ///     An <see cref="IUrlQueryStringFormatter"/>.
    /// </param>
    /// <param name="navigationManager">
    ///     A <see cref="NavigationManager"/>.
    /// </param>
    public UrlPersistenceService(IUrlQueryStringFormatter urlQueryStringFormatter, NavigationManager navigationManager)
    {
        _urlQueryStringFormatter = urlQueryStringFormatter ?? throw new ArgumentNullException(nameof(urlQueryStringFormatter));
        _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
    }

    /// <inheritdoc/>
    public Option<T> GetValue<T>(string key) =>
        _urlQueryStringFormatter.GetValue<T>(_navigationManager.Uri, key);

    /// <inheritdoc/>
    public void SetValue<T>(string key, T? value)
    {
        var newUri = _urlQueryStringFormatter.GetUriWithQueryParameter(_navigationManager.Uri, key, value);
        _navigationManager.NavigateTo(newUri, forceLoad: false, replace: true);
    }

    /// <inheritdoc/>
    public void BulkSetValues(IReadOnlyDictionary<string, object?> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var newUri = _urlQueryStringFormatter.GetUriWithQueryParameters(_navigationManager.Uri, parameters);
        _navigationManager.NavigateTo(newUri, forceLoad: false, replace: true);
    }

    /// <inheritdoc/>
    [RequiresUnreferencedCode($"This API is not trim safe. It relies on reflecting on {nameof(parameters)}'s properties.")]
    public void BulkSetValues([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] object parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var newUri = _urlQueryStringFormatter.GetUriWithQueryParameters(_navigationManager.Uri, parameters);
        _navigationManager.NavigateTo(newUri, forceLoad: false, replace: true);
    }
}
