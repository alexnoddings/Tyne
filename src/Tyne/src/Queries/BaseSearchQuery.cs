namespace Tyne.Queries;

/// <summary>
///		Base implementatino of <see cref="ISearchQuery"/>.
/// </summary>
public class BaseSearchQuery : ISearchQuery
{
	public SearchQueryPage Page { get; set; } = SearchQueryPage.Default;
	public SearchQueryOrder? Order { get; set; }
}
