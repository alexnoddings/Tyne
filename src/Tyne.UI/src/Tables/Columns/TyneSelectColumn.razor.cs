using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public sealed partial class TyneSelectColumn<TResult, TSearch, TValue> : TyneSelectColumnBase<TResult, TSearch, TValue> where TSearch : ISearchQuery
{
	protected override bool IsFilterEnabled => Value is not null;
	private TValue? Value { get; set; }

	private PropertyInfo? ForProperty { get; set; }
	private Expression<Func<TSearch, TValue?>>? _for;
	[Parameter]
	public Expression<Func<TSearch, TValue?>>? For
	{
		get => _for;
		set
		{
			_for = value;
			if (value is null)
				ForProperty = null;
			else
				ForProperty = ExpressionHelper.GetPropertyInfo(value);
		}
	}

	[Parameter, EditorRequired]
	public RenderFragment Values { get; set; } = _ => { };

	protected override void OnParametersSet()
	{
		if (_for is null)
			throw new InvalidOperationException();
	}

	public override void Prepare(TSearch search)
	{
		if (!IsFilterEnabled)
			return;

		ForProperty?.SetValue(search, Value);
	}

	private async Task SetValueAsync(TValue? newValue)
	{
		if (Value?.Equals(newValue) == true) return;

		Value = newValue;
		await OnFilterUpdatedAsync();
	}

	protected override async Task ClearAsync()
	{
		await SetValueAsync(default);
	}
}
