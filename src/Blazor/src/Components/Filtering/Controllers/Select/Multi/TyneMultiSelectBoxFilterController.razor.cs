using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A multi-selection controller which renders <see cref="IFilterSelectValue{TValue}"/>s in a dropdown box.
/// </summary>
/// <inheritdoc/>
public partial class TyneMultiSelectBoxFilterController<TRequest, TValue>
{
    /// <inheritdoc cref="MudSelect{T}.MaxHeight"/>
    [Parameter]
    public int MaxHeight { get; set; } = 300;

    /// <inheritdoc cref="MudSelect{T}.AnchorOrigin"/>
    [Parameter]
    public Origin AnchorOrigin { get; set; } = Origin.TopCenter;

    /// <inheritdoc cref="MudSelect{T}.TransformOrigin"/>
    [Parameter]
    public Origin TransformOrigin { get; set; } = Origin.TopCenter;

    /// <inheritdoc cref="MudSelect{T}.Clearable"/>
    [Parameter]
    public bool Clearable { get; set; }

    /// <inheritdoc cref="MudSelect{T}.Dense"/>
    [Parameter]
    public bool Dense { get; set; }

    /// <summary>
    ///     Whether the select box's content should be sized based on it's content.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="true"/>.
    /// </remarks>
    [Parameter]
    public bool SizeToContent { get; set; } = true;

    private string ConvertValueToString(TValue value) =>
        SelectItems
        ?.FirstOrDefault(item => item.Value?.Equals(value) == true)
        ?.AsString
        ?? string.Empty;
}
