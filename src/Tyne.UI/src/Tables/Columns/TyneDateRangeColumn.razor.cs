using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public sealed partial class TyneDateRangeColumn<TResult, TSearch> : TyneFilteredColumn<TResult, TSearch> where TSearch : ISearchQuery
{
	protected override bool IsFilterEnabled => DateRange.Start is not null || DateRange.End is not null;

	private DateRange DateRange { get; set; } = new(null, null);

	private PropertyInfo? ForMinProperty { get; set; }
	private Expression<Func<TSearch, object?>>? _forMin;
	[Parameter]
	public Expression<Func<TSearch, object?>>? ForMin
	{
		get => _forMin;
		set
		{
			_forMin = value;
			if (value is null)
				ForMinProperty = null;
			else
				ForMinProperty = ExpressionHelper.GetPropertyInfo(value);
		}
	}

	private PropertyInfo? ForMaxProperty { get; set; }
	private Expression<Func<TSearch, object?>>? _forMax;
	[Parameter]
	public Expression<Func<TSearch, object?>>? ForMax
	{
		get => _forMax;
		set
		{
			_forMax = value;
			if (value is null)
				ForMaxProperty = null;
			else
				ForMaxProperty = ExpressionHelper.GetPropertyInfo(value);
		}
	}

	protected override void OnParametersSet()
	{
		if (_forMin is null && _forMax is null)
			throw new InvalidOperationException($"One or both properties {nameof(ForMin)} and {nameof(ForMax)} must be set.");
	}

	public override void Prepare(TSearch search)
	{
		if (!IsFilterEnabled)
			return;

		ForMinProperty?.SetValue(search, DateRange.Start);
		ForMaxProperty?.SetValue(search, DateRange.End);
	}

	public async Task SetMinValueAsync(DateTime? newMinValue)
	{
		if (DateRange.Start == newMinValue) return;

		DateRange.Start = newMinValue;
		await OnFilterUpdatedAsync();
	}

	public async Task SetMaxValueAsync(DateTime? newMaxValue)
	{
		if (DateRange.End == newMaxValue) return;

		DateRange.End = newMaxValue;
		await OnFilterUpdatedAsync();
	}

	private async Task SetDateRangeAsync(DateRange? newValue)
	{
		if (DateRange.Start == newValue?.Start && DateRange.End == newValue?.End) return;

		DateRange = newValue ?? new(null, null);
		await OnFilterUpdatedAsync();
	}

	protected override async Task ClearAsync()
	{
		if (DateRange.Start is null && DateRange.End is null) return;

		DateRange.Start = DateRange.End = null;
		await OnFilterUpdatedAsync();
	}
}
