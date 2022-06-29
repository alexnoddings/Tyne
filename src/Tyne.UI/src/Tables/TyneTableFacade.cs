using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

internal class TyneTableFacade<TSearch> : ITyneTableFacade<TSearch> where TSearch : ISearchQuery
{
	private ITyneTable TyneTable { get; }

	public TyneTableFacade(ITyneTable tyneTable)
	{
		TyneTable = tyneTable ?? throw new ArgumentNullException(nameof(tyneTable));
	}

	private List<IFilteredColumn<TSearch>> AllColumns { get; } = new();
	public IEnumerable<IFilteredColumn<TSearch>> Columns => AllColumns.AsEnumerable();

	public IDisposable RegisterColumn(IFilteredColumn<TSearch> column)
	{
		if (AllColumns.Contains(column))
			throw new ArgumentException("Column is already registered.", nameof(column));

		AllColumns.Add(column);

		return new DisposableAction(() => AllColumns.Remove(column));
	}

	public Task ReloadDataAsync() => TyneTable.ReloadAsync();
}
