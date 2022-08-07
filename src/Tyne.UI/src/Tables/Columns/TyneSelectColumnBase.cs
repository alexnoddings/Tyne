using Microsoft.AspNetCore.Components;
using Tyne.Queries;
using Tyne.Utilities;

namespace Tyne.UI.Tables;

[CascadingTypeParameter(nameof(TValue))]
public abstract class TyneSelectColumnBase<TResult, TSearch, TValue> : TyneFilteredColumn<TResult, TSearch>, ITyneSelectColumn<TValue> where TSearch : ISearchQuery
{
	private readonly List<TyneSelectValue<TValue?>> availableSelectValues = new();
	protected IReadOnlyList<TyneSelectValue<TValue?>> AvailableSelectValues => availableSelectValues;

	[Parameter]
	public TyneSelectColumnType Type { get; set; }

	public IDisposable RegisterSelectValue(TyneSelectValue<TValue?> selectValue)
	{
		if (availableSelectValues.Contains(selectValue))
			throw new ArgumentException("Value is already registered.", nameof(selectValue));

		availableSelectValues.Add(selectValue);

		return new DisposableAction(() => availableSelectValues.Remove(selectValue));
	}
}
