namespace Tyne.Searching;

/// <summary>
///     A base implementation for <see cref="ISearchQuery"/>.
/// </summary>
public class SearchQuery : ISearchQuery
{
    /// <inheritdoc/>
    public int PageIndex { get; set; }

    /// <inheritdoc/>
    public int PageSize { get; set; } = 10;

    /// <inheritdoc/>
    public string? OrderBy { get; set; }

    /// <inheritdoc/>
    public bool OrderByDescending { get; set; }
}
