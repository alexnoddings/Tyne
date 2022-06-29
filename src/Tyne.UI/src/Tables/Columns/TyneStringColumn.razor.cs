using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public sealed partial class TyneStringColumn<TResult, TSearch> : TyneFilteredColumn<TResult, TSearch> where TSearch : ISearchQuery
{
	private MudBaseInput<string>? InputRef { get; set; }

	protected override bool IsFilterEnabled => !string.IsNullOrWhiteSpace(Value);
	private string? Value { get; set; }

	private PropertyInfo? ForProperty { get; set; }
	private Expression<Func<TSearch, string?>>? _for;
	[Parameter]
	public Expression<Func<TSearch, string?>>? For
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

	public async Task SetValueAsync(string? newValue)
	{
		if (Value == newValue) return;

		Value = newValue;
		await OnFilterUpdatedAsync();
	}

	protected override async Task ClearAsync()
	{
		await SetValueAsync(null);
		if (InputRef is not null)
			await InputRef.FocusAsync();
	}
}
