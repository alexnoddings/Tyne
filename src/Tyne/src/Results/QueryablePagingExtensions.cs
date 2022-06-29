using Tyne.Queries;

namespace System.Linq;

/// <summary>
///		Extension methods for paginating <see cref="IQueryable{T}"/>s.
/// </summary>
public static class QueryablePagingExtensions
{
	public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, ISearchQuery query) =>
		Page(source, query.Page.Index, query.Page.Size);

	public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int index, int size)
	{
		if (index < 0)
			index = 0;

		if (size < 0)
			size = 1;

		return source.Skip(index * size).Take(size);
	}
}
