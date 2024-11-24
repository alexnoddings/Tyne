using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A <see cref="TyneFilterValue{TRequest, TValue}"/> which supports single-item selection through <see cref="IFilterSelectValue{TValue}"/>.
/// </summary>
/// <inheritdoc/>
[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TValue))]
public class TyneFilterSelectSingleValue<TRequest, TValue> : TyneFilterSelectValue<TRequest, TValue, TValue>
{
}
