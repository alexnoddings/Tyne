namespace Tyne.Queries;

/// <summary>
///		A search query. Provides a <see cref="SearchQueryPage"/> and <see cref="SearchQueryOrder"/> by default.
/// </summary>
public interface ISearchQuery
{
	/// <summary>
	///		The page to search for.
	/// </summary>
	/// 
	public SearchQueryPage Page { get; set; }
	/// <summary>
	///		Optionally, how to order the results.
	/// </summary>
	/// <remarks>
	///		May be null, in which case default (or no) ordering should be applied.
	/// </remarks>
	public SearchQueryOrder? Order { get; set; }
}
