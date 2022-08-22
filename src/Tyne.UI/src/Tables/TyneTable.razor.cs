using Microsoft.AspNetCore.Components;
using Tyne.Queries;
using Tyne.Results;

namespace Tyne.UI.Tables;

[CascadingTypeParameter(nameof(TResult))]
[CascadingTypeParameter(nameof(TSearch))]
public partial class TyneTable<TResult, TSearch> : ITyneTable where TSearch : ISearchQuery, new()
{
	[Parameter]
	public RenderFragment? Toolbar { get; set; }

	[Parameter]
	public RenderFragment Headers { get; set; } = default!;

	[Parameter]
	public RenderFragment<TResult> RowTemplate { get; set; } = default!;

	[Parameter]
	public RenderFragment? NoData { get; set; }

	[Parameter]
	public RenderFragment? Loading { get; set; }

	[Parameter]
	public RenderFragment? Footer { get; set; }

	[Parameter]
	public RenderFragment<Result<SearchResults<TResult>>>? Result { get; set; }

	/// <inheritdoc cref="MudTableBase.Dense" />
	[Parameter]
	public bool Dense { get; set; } = true;

	/// <inheritdoc cref="MudTableBase.Hover" />
	[Parameter]
	public bool Hover { get; set; } = true;

	/// <inheritdoc cref="MudTableBase.Striped" />
	[Parameter]
	public bool Striped { get; set; } = true;

	[Parameter, EditorRequired]
	public Func<TSearch, Task<Result<SearchResults<TResult>>>> Search { get; set; } = default!;

	[Parameter]
	public EventCallback<TableRowClickEventArgs<TResult>> OnRowClicked { get; set; } = default!;

	private MudTable<TResult>? Table { get; set; }

	private TyneTableFacade<TSearch> Facade { get; }

	[Parameter]
	public TableLoadingBehaviour LoadingBehaviour { get; set; } = TableLoadingBehaviour.LockControlsWithOverlay;

	private bool IsLoading { get; set; } = true;

	private Result<SearchResults<TResult>>? LoadDataResult { get; set; }

	public TyneTable()
	{
		Facade = new TyneTableFacade<TSearch>(this);
	}

	public async Task ReloadAsync() =>
		await Table!.ReloadServerData();

	private async Task<TableData<TResult>> LoadDataAsync(TableState tableState)
	{
		IsLoading = true;
		StateHasChanged();

		try
		{
			var searchQuery = new TSearch
			{
				Page = new SearchQueryPage(tableState.Page, tableState.PageSize),
				Order =
					string.IsNullOrEmpty(tableState.SortLabel) || tableState.SortDirection == SortDirection.None
					? null
					: new SearchQueryOrder(tableState.SortLabel, tableState.SortDirection == SortDirection.Descending)
			};

			foreach (IFilteredColumn<TSearch> column in Facade.Columns)
				column.Prepare(searchQuery);

			LoadDataResult = await Search(searchQuery);
			if (!LoadDataResult.Success)
				return new TableData<TResult> { TotalItems = 0, Items = Enumerable.Empty<TResult>() };

			SearchResults<TResult> searchResults = LoadDataResult.Value;
			return new TableData<TResult> { TotalItems = searchResults.TotalCount, Items = searchResults };
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}
}
