using Microsoft.AspNetCore.Components;
using Tyne.Queries;

namespace Tyne.UI.Tables;

public abstract partial class TyneFilteredColumn<TResult, TSearch> : TyneColumn<TResult>, IFilteredColumn<TSearch>, IDisposable where TSearch : ISearchQuery
{
	protected abstract bool IsFilterEnabled { get; }

	protected virtual bool IsFilterVisible { get; set; }

	protected virtual void ToggleFilterVisibility() => IsFilterVisible = !IsFilterVisible;

	[Parameter]
	public string? FilterOpenIcon { get; set; }

	protected string FilterOpenIconOrDefault =>
		string.IsNullOrWhiteSpace(FilterOpenIcon)
		? Icons.Material.Filled.SearchOff
		: FilterOpenIcon;

	[Parameter]
	public string? FilterClosedIcon { get; set; }

	protected string FilterClosedIconOrDefault =>
		string.IsNullOrWhiteSpace(FilterClosedIcon)
		? Icons.Material.Filled.Search
		: FilterClosedIcon;

	[Parameter]
	public string? FilterEnabledIcon { get; set; }

	protected string FilterEnabledIconOrDefault =>
		string.IsNullOrWhiteSpace(FilterEnabledIcon)
		? Icons.Material.Filled.SavedSearch
		: FilterEnabledIcon;

	protected virtual string FilterIcon
	{
		get
		{
			if (IsFilterEnabled)
				return FilterEnabledIconOrDefault;

			if (IsFilterVisible)
				return FilterOpenIconOrDefault;

			return FilterClosedIconOrDefault;
		}
	}

	protected virtual Color FilterIconColor =>
		IsFilterEnabled
		? Color.Primary
		: Color.Default;

	protected abstract Task ClearAsync();

	public abstract void Prepare(TSearch search);

	[CascadingParameter]
	private ITyneTableFacade<TSearch> TyneTableFacade { get; set; } = default!;

	private IDisposable ColumnRegistration { get; set; } = default!;

	protected override void OnInitialized()
	{
		if (TyneTableFacade is null)
			throw new ArgumentNullException(nameof(TyneTableFacade), "Cascading facade parameter missing.");

		ColumnRegistration = TyneTableFacade.RegisterColumn(this);
	}

	protected async Task OnFilterUpdatedAsync() =>
		await TyneTableFacade.ReloadDataAsync();

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing) =>
		ColumnRegistration.Dispose();
}
