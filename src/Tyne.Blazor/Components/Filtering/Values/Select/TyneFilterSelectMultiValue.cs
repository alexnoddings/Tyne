using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A <see cref="TyneFilterValue{TRequest, TValue}"/> which supports multi-item selection through <see cref="IFilterSelectValue{TValue}"/>.
/// </summary>
/// <inheritdoc/>
[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TValue))]
public class TyneFilterSelectMultiValue<TRequest, TValue> : TyneFilterSelectValue<TRequest, HashSet<TValue>, TValue>
{
}
