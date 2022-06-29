namespace Tyne.Results;

public class SearchResults<T> : List<T>
{
	public int TotalCount { get; }

	public SearchResults(int totalCount, IEnumerable<T> collection) : base(collection)
	{
		TotalCount = totalCount;
	}

	public static Result<SearchResults<T>> AsResult(int totalCount, IEnumerable<T> collection) =>
		Result<SearchResults<T>>.Successful(new SearchResults<T>(totalCount, collection));
}
