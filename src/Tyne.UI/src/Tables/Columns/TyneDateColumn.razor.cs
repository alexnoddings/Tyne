using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public sealed partial class TyneDateColumn<TResult, TSearch> : TyneFilteredColumn<TResult, TSearch> where TSearch : ISearchQuery
{
	protected override bool IsFilterEnabled => Date is not null;

	private DateTime? Date { get; set; }

	private PropertyInfo? ForProperty { get; set; }
	private Expression<Func<TSearch, object?>>? _for;
	[Parameter]
	public Expression<Func<TSearch, object?>>? For
	{
		get => _for;
		set
		{
			_for= value;
			if (value is null)
				ForProperty = null;
			else
				ForProperty = ExpressionHelper.GetPropertyInfo(value);
		}
	}

	protected override void OnParametersSet()
	{
		if (_for is null)
			throw new InvalidOperationException();
	}

	public override void Prepare(TSearch search)
	{
		if (!IsFilterEnabled)
			return;

		ForProperty?.SetValue(search, Date);
	}

	public async Task SetValueAsync(DateTime? newValue)
	{
		if (Date == newValue) return;

		Date = newValue;
		await OnFilterUpdatedAsync();
	}

	protected override async Task ClearAsync()
	{
		if (Date is null) return;

		Date = null;
		await OnFilterUpdatedAsync();
	}
}
