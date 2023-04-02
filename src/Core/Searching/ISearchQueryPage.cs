namespace Tyne.Searching;

/// <summary>
///     How to paginate search query results.
/// </summary>
public interface ISearchQueryPage
{
    /// <summary>
    ///     The <c>0</c>-based page index.
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    ///     The size of the page.
    /// </summary>
    public int PageSize { get; set; }
}
