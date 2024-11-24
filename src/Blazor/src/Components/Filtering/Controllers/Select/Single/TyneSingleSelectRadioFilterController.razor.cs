using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A single-selection controller which renders <see cref="IFilterSelectValue{TValue}"/>s as radio options in a list.
/// </summary>
/// <inheritdoc/>
public partial class TyneSingleSelectRadioFilterController<TRequest, TValue>
{
    /// <summary>
    ///     The <see cref="FlexDirection"/> of the radio options.
    /// </summary>
    [Parameter]
    public FlexDirection Layout { get; set; }

    /// <summary>
    ///     The class name for the radio options container.
    /// </summary>
    /// <remarks>
    ///     This handles transforming <see cref="Layout"/> into CSS classes.
    /// </remarks>
    protected string RadioOptionContainerClassName =>
        new CssBuilder("d-flex")
        .AddClass("flex-row", Layout is FlexDirection.Row)
        .AddClass("flex-column", Layout is FlexDirection.Column)
        .Build();
}
