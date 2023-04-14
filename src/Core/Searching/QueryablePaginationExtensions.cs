using Tyne.Searching;

namespace System.Linq;

/// <summary>
///     Pagination extensions for <see cref="IQueryable{T}"/>s.
/// </summary>
public static class QueryablePaginationExtensions
{
    /// <summary>
    ///     Paginates <paramref name="source"/>. Skips <paramref name="pageIndex"/> pages of size <paramref name="pageSize"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data in the data source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="pageIndex">
    ///     The page index to skip to.
    ///     This is <c>0</c>-based, so page <c>0</c> is the first page.
    ///     Negative values will be clamped to <c>0</c>.
    /// </param>
    /// <param name="pageSize">The size of pages.</param>
    /// <returns>An <see cref="IQueryable{T}" /> that contains the specified page of elements from the <paramref name="source" /> sequence.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageIndex, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (pageIndex < 0)
            pageIndex = 0;

        if (pageSize < 1)
            pageSize = 1;

        if (pageSize > 100)
            pageSize = 100;

        return source.Skip(pageIndex * pageSize).Take(pageSize);
    }

    /// <summary>
    ///     Paginates <paramref name="source"/> using <paramref name="searchQueryPage"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data in the data source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="searchQueryPage">The <see cref="ISearchQueryPage"/> to order by.</param>
    /// <returns>An <see cref="IQueryable{T}" /> that contains the specified page of elements from the <paramref name="source" /> sequence.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, ISearchQueryPage searchQueryPage)
    {
        ArgumentNullException.ThrowIfNull(searchQueryPage);

        return source.Paginate(searchQueryPage.PageIndex, searchQueryPage.PageSize);
    }
}
