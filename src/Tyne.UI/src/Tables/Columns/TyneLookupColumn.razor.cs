using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

public partial class TyneLookupColumn<TResult, TSearch, TLookup, TLookupKey> : TyneFilteredColumn<TResult, TSearch> where TSearch : ISearchQuery
{
	protected MudBaseInput<TLookup>? InputRef { get; set; }

	protected override bool IsFilterEnabled => Value is not null;
	private TLookup? Value { get; set; }

	private PropertyInfo? ForProperty { get; set; }
	private Expression<Func<TSearch, TLookupKey?>>? _for;
	[Parameter]
	public Expression<Func<TSearch, TLookupKey?>>? For
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

	private PropertyInfo? KeyProperty { get; set; }
	private Expression<Func<TLookup, TLookupKey?>>? _key;
	[Parameter]
	public Expression<Func<TLookup, TLookupKey?>>? Key
	{
		get => _key;
		set
		{
			_key = value;
			if (value is null)
				KeyProperty = null;
			else
				KeyProperty = ExpressionHelper.GetPropertyInfo(value);
		}
	}

	[Parameter, EditorRequired]
	public Func<string?, Task<IEnumerable<TLookup>>> LookupFunc { get; set; } = default!;

	[Parameter]
	public Func<TLookup, string>? LookupToStringFunc { get; set; }

	[Parameter]
	public RenderFragment<TLookup>? ItemTemplate { get; set; }

	[Parameter]
	public RenderFragment<TLookup>? ItemSelectedTemplate { get; set; }

	protected override void OnParametersSet()
	{
		if (_for is null)
			throw new InvalidOperationException();

		if (_key is null)
			throw new InvalidOperationException();

		if (LookupFunc is null)
			throw new InvalidOperationException();
	}

	public override void Prepare(TSearch search)
	{
		if (Value is null || !IsFilterEnabled || KeyProperty is null || ForProperty is null)
			return;

		var lookupValue = KeyProperty.GetValue(Value);
		ForProperty.SetValue(search, lookupValue);
	}

	public async Task SetValueAsync(TLookup? newValue)
	{
		if (Value?.Equals(newValue) == true) return;

		Value = newValue;
		await OnFilterUpdatedAsync();
	}

	protected override async Task ClearAsync()
	{
		await SetValueAsync(default);
		if (InputRef is not null && IsFilterVisible)
			await InputRef.FocusAsync();
	}

	private string LookupToString(TLookup? lookup)
	{
		if (lookup is null)
			return string.Empty;

		if (LookupToStringFunc is null)
			return lookup.ToString() ?? string.Empty;

		return LookupToStringFunc(lookup);
	}
}
