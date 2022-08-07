using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public sealed partial class TyneMultiSelectColumn<TResult, TSearch, TValue> : TyneSelectColumnBase<TResult, TSearch, TValue> where TSearch : ISearchQuery
{
	protected override bool IsFilterEnabled => SelectedValues.Count > 0;
	private HashSet<TValue?> SelectedValues { get; } = new();

	private PropertyInfo? ForProperty { get; set; }
	private Expression<Func<TSearch, List<TValue?>?>>? _for;
	[Parameter]
	public Expression<Func<TSearch, List<TValue?>?>>? For
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

		var valuesList = SelectedValues.ToList();
		ForProperty?.SetValue(search, valuesList);
	}

	private bool IsValueSelected(TValue? value) => SelectedValues.Contains(value);

	private async Task SetValueSelectedAsync(TValue? value, bool isSelected)
	{
		var isCurrentlySelected = IsValueSelected(value);
		if (isCurrentlySelected == isSelected) return;

		if (isSelected)
			SelectedValues.Add(value);
		else
			SelectedValues.Remove(value);
		
		await OnFilterUpdatedAsync();
	}

	private async Task SetValuesAsync(IEnumerable<TValue?> values)
	{
		SelectedValues.Clear();
		SelectedValues.UnionWith(values);
		await OnFilterUpdatedAsync();
	}

	protected override async Task ClearAsync()
	{
		SelectedValues.Clear();
		await OnFilterUpdatedAsync();
	}
}
