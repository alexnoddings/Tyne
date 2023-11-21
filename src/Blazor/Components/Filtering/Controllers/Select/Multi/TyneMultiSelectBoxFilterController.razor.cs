using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A multi-selection controller which renders <see cref="IFilterSelectValue{TValue}"/>s in a dropdown box.
/// </summary>
/// <inheritdoc/>
public partial class TyneMultiSelectBoxFilterController<TRequest, TValue>
{
    private string ConvertValueToString(TValue value) =>
        SelectItems
        ?.FirstOrDefault(item => item.Value?.Equals(value) == true)
        ?.AsString
        ?? string.Empty;
}
