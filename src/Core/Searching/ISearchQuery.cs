namespace Tyne.Searching;

/// <summary>
///		A search query. Provides pagination and ordering by default.
/// </summary>
public interface ISearchQuery : ISearchQueryPage, ISearchQueryOrder
{
}
