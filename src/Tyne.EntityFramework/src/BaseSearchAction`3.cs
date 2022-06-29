using Microsoft.EntityFrameworkCore;
using Tyne.Actions;
using Tyne.Queries;
using Tyne.Results;

namespace Tyne.EntityFramework;

/// <summary>
///		The base class for an <see cref="ISearchAction{TQuery, TResult}"/>.
///		Handles pagination and leaveas filtering/mapping/ordering up to the implementor.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public abstract class BaseSearchAction<TQuery, TResult, TEntity>
    : ISearchAction<TQuery, TResult>
    where TQuery : ISearchQuery
    where TEntity : class
{
    /// <summary>
    ///		The default property on <see cref="TEntity"/> to order by.
    /// </summary>
    /// <remarks>
    ///		This is used as a fallback to sort ascending when <see cref="ISearchQuery.Order"/> or <see cref="SearchQueryOrder.By"/> are <see langword="null"/>.
    ///		If <see langword="null"/> or whitespace, no ordering will be applied.
    /// </remarks>
    protected virtual string? DefaultOrderBy { get; }

    public abstract Task<Result<SearchResults<TResult>>> RunAsync(TQuery model);

    protected async Task<Result<SearchResults<TResult>>> RunAsync(TQuery query, IQueryable<TEntity> entities)
    {
        IQueryable<TEntity> queryableFiltered = Filter(entities, query);
        IQueryable<TResult> queryableMapped = Map(queryableFiltered, query);
        IQueryable<TResult> queryableOrdered = Order(queryableMapped, query);

        // We only use queryableFiltered as mapping/ordering is irrelevant for counts.
        // We still execute after these to guarantee that all user code for these has ran already.
        int count = await queryableFiltered
            // Ignores warnings that data isn't sorted as we don't care for counts
            .OrderBy(e => e)
            .CountAsync();
        List<TResult> results = await queryableOrdered.ToPageAsync(query);

        return SearchResults<TResult>.AsResult(count, results);
    }

    /// <summary>
    ///		Filters <paramref name="queryable"/> using the <paramref name="query"/>.
    /// </summary>
    /// <param name="queryable">The input <see cref="IQueryable{T}"/> to filter.</param>
    /// <param name="query">The query being executed.</param>
    /// <returns>A filtered <see cref="IQueryable{T}"/>.</returns>
    protected abstract IQueryable<TEntity> Filter(IQueryable<TEntity> queryable, TQuery query);

    /// <summary>
    ///		Maps an <see cref="IQueryable{T}"/> of <typeparamref name="TEntity"/> to <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="queryable">The input <see cref="IQueryable{T}"/> to map.</param>
    /// <param name="query">The query being executed.</param>
    /// <returns>A mapped <see cref="IQueryable{T}"/>.</returns>
    protected abstract IQueryable<TResult> Map(IQueryable<TEntity> queryable, TQuery query);

    /// <summary>
    ///		Orders <paramref name="queryable"/> using the <paramref name="query"/>.
    /// </summary>
    /// <param name="queryable">The input <see cref="IQueryable{T}"/> to order.</param>
    /// <param name="query">The query being executed.</param>
    /// <returns>An ordered <see cref="IQueryable{T}"/>.</returns>
    /// <remarks>
    ///		The default implementation uses the <paramref name="query"/>'s <see cref="ISearchQuery.Order"/>,
    ///		and falls back to <see cref="DefaultOrderBy"/>. If both are <see langword="null"/> or whitespace,
    ///		no ordering will be applied.
    /// </remarks>
    protected virtual IQueryable<TResult> Order(IQueryable<TResult> queryable, TQuery query)
    {
        if (!string.IsNullOrWhiteSpace(query.Order?.By))
            return queryable.OrderBy(query.Order.By, query.Order.IsDescending);

        if (!string.IsNullOrWhiteSpace(DefaultOrderBy))
            return queryable.OrderBy(DefaultOrderBy, false);

        return queryable;
    }
}
