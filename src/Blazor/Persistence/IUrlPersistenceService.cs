using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Persistence;

/// <summary>
///     Handles persisting data to the URL in the query string.
/// </summary>
/// <remarks>
///     Designed to persist page data between sessions.
/// </remarks>
public interface IUrlPersistenceService
{
    /// <summary>
    ///     Gets a <typeparamref name="T"/> value from the current URI's query string parameter <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="T">The type to get.</typeparam>
    /// <param name="key">The query string parameter key.</param>
    /// <returns>An <see cref="Option{T}"/> possibly containing the value.</returns>
    /// <remarks>
    ///     The returned <see cref="Option{T}"/> will be <c>Some(<typeparamref name="T"/>)</c> if <paramref name="key"/>
    ///     was present in the query string, and could be de-serialised to a valid <typeparamref name="T"/>.
    ///     Otherwise, it will be <c>None</c>. An empty <paramref name="key"/> will also return <c>None</c>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="key"/> is <see langword="null"/>.</exception>
    public Option<T> GetValue<T>(string key);

    /// <summary>
    ///     Updates the current URI, setting the <paramref name="key"/> parameter to <paramref name="value"/> in the query string.
    /// </summary>
    /// <typeparam name="T">The type to save.</typeparam>
    /// <param name="key">The query string parameter key.</param>
    /// <param name="value">
    ///     The <typeparamref name="T"/> value to save.
    ///     If this is <see langword="null"/>, the query parameter will be removed.
    /// </param>
    /// <exception cref="ArgumentNullException">When <paramref name="key"/> is <see langword="null"/>.</exception>
    public void SetValue<T>(string key, T? value);

    /// <summary>
    ///     Updates the current URI, setting multiple query string parameters based on <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">A dictionary of query string parameter keys/values.</param>
    /// <remarks>
    ///     Updating more than one query string parameter in quick succession requires using this rather than <see cref="SetValue{T}(string, T)"/>.
    ///     <see cref="NavigationManager"/> returns before the URL has been updated asynchronously,
    ///     so setting multiple values simultaneously won't use the updated URL correctly.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="parameters"/> is <see langword="null"/>.</exception>
    public void BulkSetValues(IReadOnlyDictionary<string, object?> parameters);

    /// <summary>
    ///     Updates the current URI, setting multiple query string parameters based on <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">An object whose public properties will be read and parsed into <see cref="BulkSetValues(IReadOnlyDictionary{string, object?})"/>.</param>
    /// <inheritdoc cref="BulkSetValues(IReadOnlyDictionary{string, object?})"/>
    /// <example>
    ///     <code lang="cs">
    ///         _urlPersistenceService.BulkSetValues(new { ParamA = 42, ParamB = "101" });
    ///     </code>
    /// </example>
    [RequiresUnreferencedCode($"This API is not trim safe. It relies on reflecting on {nameof(parameters)}'s properties.")]
    public void BulkSetValues([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] object parameters);
}
