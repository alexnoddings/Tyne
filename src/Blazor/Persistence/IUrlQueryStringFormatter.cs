using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Persistence;

/// <summary>
///     Handles formatting data to URLs in their query string.
/// </summary>
[SuppressMessage("Design", "CA1054: URI-like parameters should not be strings", Justification = "This is consistent with AspNetCore's NavigationManager methods/extensions.")]
[SuppressMessage("Design", "CA1055:URI-like return values should not be strings", Justification = "This is consistent with AspNetCore's NavigationManager methods/extensions.")]
public interface IUrlQueryStringFormatter
{
    /// <summary>
    ///     Gets a <typeparamref name="T"/> value from <paramref name="uri"/>'s query string parameter <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="T">The type to get.</typeparam>
    /// <param name="uri">The uri to get the query string from.</param>
    /// <param name="key">The query string parameter key.</param>
    /// <returns>An <see cref="Option{T}"/> possibly containing the value.</returns>
    /// <remarks>
    ///     The returned <see cref="Option{T}"/> will be <c>Some(<typeparamref name="T"/>)</c> if <paramref name="key"/>
    ///     was present in <paramref name="uri"/>'s query string and could be de-serialised to a valid <typeparamref name="T"/>.
    ///     Otherwise, it will be <c>None</c>. An empty <paramref name="key"/> will also return <c>None</c>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="uri"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException">When <paramref name="key"/> is <see langword="null"/>.</exception>
    public Option<T> GetValue<T>(string uri, string key);

    /// <summary>
    ///     Gets the URI after <paramref name="uri"/>'s query string parameter <paramref name="key"/> has been updated with <paramref name="value"/>.
    /// </summary>
    /// <param name="uri">The URI to update.</param>
    /// <param name="key">The query string parameter key.</param>
    /// <param name="value">The query string parameter value.</param>
    /// <returns>A new URI, whose query string has been updated.</returns>
    /// <remarks>
    ///     If <paramref name="uri"/>'s query string does not contain <paramref name="key"/>, it will be added.
    ///     If it contains <paramref name="key"/> but <paramref name="value"/> is null or empty, then the query parameter will be removed.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="uri"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException">When <paramref name="key"/> is <see langword="null"/>.</exception>
    [RequiresUnreferencedCode($"This API is not trim safe. It relies on reflecting on {nameof(NavigationManagerExtensions)}.")]
    public string GetUriWithQueryParameter(string uri, string key, object? value);

    /// <summary>
    ///     Gets the URI after <paramref name="uri"/>'s query string parameters has been updated with <paramref name="parameters"/>.
    /// </summary>
    /// <param name="uri">The URI to update.</param>
    /// <param name="parameters">A dictionary of parameters to update the query string with.</param>
    /// <returns>A new URI, whose query string has been updated.</returns>
    /// <remarks>
    ///     This modifies the URI using the same behaviour described in <see cref="GetUriWithQueryParameter(string, string, object?)"/>'s remarks.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="uri"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException">When <paramref name="parameters"/> is <see langword="null"/>.</exception>
    public string GetUriWithQueryParameters(string uri, IReadOnlyDictionary<string, object?> parameters);

    /// <summary>
    ///     Gets the URI after <paramref name="uri"/>'s query string parameters has been updated with the public properties from <paramref name="parameters"/>.
    /// </summary>
    /// <param name="uri">The URI to update.</param>
    /// <param name="parameters">An object whose public properties will be read and used to update the query string with.</param>
    /// <returns>A new URI, whose query string has been updated.</returns>
    /// <remarks>
    ///     <para>
    ///         This method reflects across <paramref name="parameters"/>' public properties to form query string parameters.
    ///     </para>
    ///     <para>
    ///         This modifies the URI using the same behaviour described in <see cref="GetUriWithQueryParameter(string, string, object?)"/>'s remarks.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="uri"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException">When <paramref name="parameters"/> is <see langword="null"/>.</exception>
    [RequiresUnreferencedCode($"This API is not trim safe. It relies on reflecting on {nameof(parameters)}'s properties.")]
    public string GetUriWithQueryParameters(string uri, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] object parameters);
}
