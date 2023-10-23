using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A multi-selection controller which renders <see cref="IFilterSelectValue{TValue}"/>s as checkboxes in a list.
/// </summary>
/// <inheritdoc/>
public partial class TyneMultiSelectCheckboxFilterController<TRequest, TValue>
{
    /// <summary>
    ///     The <see cref="FlexDirection"/> of the checkboxes.
    /// </summary>
    [Parameter]
    public FlexDirection Layout { get; set; }

    /// <summary>
    ///     The class name for the checkbox container.
    /// </summary>
    /// <remarks>
    ///     This handles transforming <see cref="Layout"/> into CSS classes.
    /// </remarks>
    protected string CheckboxContainerClassName =>
        new CssBuilder("d-flex")
        .AddClass("flex-row", Layout is FlexDirection.Row)
        .AddClass("flex-column", Layout is FlexDirection.Column)
        .Build();

    private Task UpdateSelectedAsync(TValue? value, bool isSelected) =>
        isSelected
        ? AddValueAsync(value)
        : RemoveValueAsync(value);
}
