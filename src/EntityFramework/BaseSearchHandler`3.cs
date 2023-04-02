using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Tyne.Searching;

namespace Tyne.EntityFramework;

/// <summary>
///		Base implementation for an <see cref="IHandler{TIn, TOut, E}"/> which returns <see cref="SearchResults{T}"/>.
/// </summary>
/// <remarks>
///		Inherits from <see cref="BaseHandler{TIn, TOut, E}"/> and provides extensibility points to control filtering/projection/ordering.
/// </remarks>
/// <typeparam name="TQuery">The input <see cref="ISearchQuery"/> type.</typeparam>
/// <typeparam name="TResult">The output of <see cref="SearchResults{T}"/>.</typeparam>
/// <typeparam name="TEntity">The type of entity being loaded.</typeparam>
/// <inheritdoc/>
public abstract class BaseSearchHandler<TQuery, TResult, TEntity>
    where TQuery : ISearchQuery
    where TEntity : class
{
    /// <inheritdoc/>
    protected async Task<SearchResults<TResult>> ExecuteAsync(TQuery request, IQueryable<TEntity> source, CancellationToken cancellationToken)
    {
        var filtered = Filter(source, request);
        var projected = Project(filtered, request);
        var ordered = Order(projected, request);
        var paginated = Paginate(ordered, request);

        // totalCount only cares about filtered results, not projections or orderings
        // The OrderBy ignores warnings about unsorted data
        var totalCount = await filtered.OrderBy(e => e).CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await paginated.ToListAsync(cancellationToken).ConfigureAwait(false);
        var transformedItems = Transform(items);

        return new SearchResults<TResult>(transformedItems, totalCount);
    }

    /// <summary>
    ///     Filters the <paramref name="source"/> by the <paramref name="query"/>.
    /// </summary>
    /// <param name="source">The sequence to filter.</param>
    /// <param name="query">The <typeparamref name="TQuery"/> to filter the <paramref name="source"/> with.</param>
    /// <returns>An <see cref="IQueryable{T}"/> which has been filtered.</returns>
    protected abstract IQueryable<TEntity> Filter(IQueryable<TEntity> source, TQuery query);

    /// <summary>
    ///     Projects the data in <paramref name="source"/> from <typeparamref name="TEntity"/>s to <typeparamref name="TResult"/>s.
    /// </summary>
    /// <param name="source">The sequence to filter.</param>
    /// <param name="query">The <typeparamref name="TQuery"/> to project the <paramref name="source"/> with.</param>
    /// <returns>An <see cref="IQueryable{T}"/> of <typeparamref name="TResult"/>s projected from <paramref name="source"/>.</returns>
    protected abstract IQueryable<TResult> Project(IQueryable<TEntity> source, TQuery query);

    /// <summary>
    ///     Orders the <paramref name="source"/> by the <paramref name="query"/>.
    /// </summary>
    /// <param name="source">The sequence to order.</param>
    /// <param name="query">The <typeparamref name="TQuery"/> to order the <paramref name="source"/> with.</param>
    /// <returns>An <see cref="IQueryable{T}"/> which may have been ordered.</returns>
    /// <remarks>
    ///     The default implementation is to return <see cref="ISearchQueryOrder.OrderBy"/>, or by <see cref="OrderDefault(IQueryable{TResult})"/> if the given order isn't valid.
    /// </remarks>
    protected virtual IQueryable<TResult> Order(IQueryable<TResult> source, TQuery query)
    {
        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            // If Order.By is a valid property on TResult, then OrderByProperty will return an IOrderedQueryable.
            // Otherwise, it will return source unmodified. If maybeOrdered isn't ordered, fall back to default ordering.
            var maybeOrdered = source.OrderByProperty(query.OrderBy, query.OrderByDescending);
            if (maybeOrdered is IOrderedQueryable<TResult>)
                return maybeOrdered;
        }

        return OrderDefault(source);
    }

    /// <summary>
    ///     Orders the <paramref name="source"/> when ordering by the <typeparamref name="TQuery"/> has failed.
    /// </summary>
    /// <param name="source">The sequence to order.</param>
    /// <returns>An <see cref="IQueryable{T}"/> which may have been ordered.</returns>
    /// <remarks>
    ///     The default implementation is to simply return <paramref name="source"/> unaltered.
    ///     This can be modified to order by a property by default.
    /// </remarks>
    protected abstract IQueryable<TResult> OrderDefault(IQueryable<TResult> source);

    /// <summary>
    ///     Paginates the <paramref name="source"/> by the <paramref name="query"/>.
    /// </summary>
    /// <param name="source">The sequence to paginate.</param>
    /// <param name="query">The <typeparamref name="TQuery"/> to paginate the <paramref name="source"/> with.</param>
    /// <returns>An <see cref="IQueryable{T}"/> which has been paginated.</returns>
    /// <remarks>
    ///     The default implementation is to call <see cref="QueryablePaginationExtensions.Paginate{T}(IQueryable{T}, ISearchQuery)"/>.
    /// </remarks>
    protected virtual IQueryable<TResult> Paginate(IQueryable<TResult> source, TQuery query) =>
        source.Paginate(query);

    /// <summary>
    ///     Transforms the materialised results.
    /// </summary>
    /// <param name="results">The materialised results.</param>
    /// <returns>The <paramref name="results"/> transformed in some way.</returns>
    /// <remarks>
    ///     This is an optional extensibility point to give callers a way of modifying data once materialised.
    ///     The default implementation is a NOP, which is fine to leave unless you need to modify the materialised data.
    /// </remarks>
    [SuppressMessage("Design", "CA1002: Do not expose generic lists", Justification = "Inheritance with TResult isn't relevant here, and passing a different generic collection will incur an overhead in non-overridden Transforms.")]
    protected virtual List<TResult> Transform(List<TResult> results) =>
        // Does not transform results by default, this is an extensibility point for implementers
        results;
}
