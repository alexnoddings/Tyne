namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A filter which supports searching for <typeparamref name="TSearchValue"/>s via <see cref="SearchAsync(string)"/>.
/// </summary>
/// <typeparam name="TSearchValue">The type being searched.</typeparam>
public interface IFilterSearchValue<TSearchValue>
{
    /// <summary>
    ///     Searches a data source for <typeparamref name="TSearchValue"/>s which match <paramref name="search"/>.
    /// </summary>
    /// <param name="search">Optionally, a parameter used to search <typeparamref name="TSearchValue"/>s.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> whose result is a list of <typeparamref name="TSearchValue"/>s found.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is vague to allow implementers to decide how searching is implemented, and against what data source.
    ///     </para>
    ///     <para>
    ///         This may be data stored in memory but too large to show in one list, or it may be loaded from a remote API.
    ///     </para>
    /// </remarks>
    public Task<List<TSearchValue>> SearchAsync(string search);
}
