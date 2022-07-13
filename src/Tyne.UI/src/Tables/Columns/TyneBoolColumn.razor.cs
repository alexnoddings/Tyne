using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public sealed partial class TyneBoolColumn<TResult, TSearch> : TyneFilteredColumn<TResult, TSearch> where TSearch : ISearchQuery
{
	protected override bool IsFilterEnabled => Value is not null;
	private bool? Value { get; set; }

	private PropertyInfo? ForProperty { get; set; }
	private Expression<Func<TSearch, bool?>>? _for;
	[Parameter]
	public Expression<Func<TSearch, bool?>>? For
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
	public RenderFragment True { get; set; } = builder => builder.AddContent(0, "True");

	[Parameter, EditorRequired]
	public RenderFragment False { get; set; } = builder => builder.AddContent(0, "False");

	[Parameter, EditorRequired]
	public RenderFragment Neither { get; set; } = builder => builder.AddContent(0, "Neither");

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

	public async Task SetValueAsync(bool? newValue)
	{
		if (Value == newValue) return;

		Value = newValue;
		await OnFilterUpdatedAsync();
	}

	protected override async Task ClearAsync() =>
		await SetValueAsync(null);
}
