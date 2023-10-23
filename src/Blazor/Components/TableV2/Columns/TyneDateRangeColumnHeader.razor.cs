using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Filtering.Controllers;

namespace Tyne.Blazor.Tables.Columns;

/// <summary>
///     A table column header which renders a
///     <see cref="TyneDateRangeFilterController{TRequest}"/>.
/// </summary>
/// <inheritdoc/>
public partial class TyneDateRangeColumnHeader<TRequest, TResponse>
{
    /// <summary>
    ///     The property on <typeparamref name="TResponse"/> to order by.
    /// </summary>
    [Parameter]
    public string? OrderBy { get; set; }
    private TyneKey OrderByKey => TyneKey.From(OrderBy);

    /// <summary>
    ///     The content to show in the column header.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
