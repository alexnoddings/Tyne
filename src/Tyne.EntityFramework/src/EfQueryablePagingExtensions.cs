using Tyne.Queries;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
///		Extension methods for paginating <see cref="IQueryable{T}"/>s.
/// </summary>
public static class EfQueryablePagingExtensions
{
	public static Task<List<TSource>> ToPageAsync<TSource>(this IQueryable<TSource> source, ISearchQuery query) =>
        ToPageAsync(source, query.Page.Index, query.Page.Size);

	public static async Task<List<TSource>> ToPageAsync<TSource>(this IQueryable<TSource> source, int index, int size) =>
        await source.Page(index, size).ToListAsync();
}
