using Tyne.Queries;

namespace Tyne.UI.Tables;

public interface ITyneTableFacade<out TSearch> where TSearch : ISearchQuery
{
	public IDisposable RegisterColumn(IFilteredColumn<TSearch> column);
	public Task ReloadDataAsync();
}
