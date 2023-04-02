using MudBlazor;
using Microsoft.AspNetCore.Components;
using Tyne.MediatorEndpoints;
using Tyne.Searching;

namespace Tyne.Blazor;

public partial class TyneTable<TRequest, TResponse> : MudTable<TResponse>, IServerDataTable where TRequest : IApiRequest<SearchResults<TResponse>>, ISearchQuery, new()
{
	[Inject]
	private IMediatorProxy Mediator { get; init; } = null!;

	public TyneTable()
	{
		UserAttributes.Add("aria-role", "table");
		ServerData = LoadDataAsync;
	}

	private async Task<TableData<TResponse>> LoadDataAsync(TableState state)
	{
		var searchResults = await Mediator
			.Send(new TRequest
			{
				PageIndex = state.Page,
				PageSize = state.PageSize,
				OrderBy = state.SortLabel,
				OrderByDescending = state.SortDirection is SortDirection.Descending
			})
			.ConfigureAwait(true);

		return new TableData<TResponse>
		{
			TotalItems = searchResults.TotalCount,
			Items = searchResults
		};
	}
}
