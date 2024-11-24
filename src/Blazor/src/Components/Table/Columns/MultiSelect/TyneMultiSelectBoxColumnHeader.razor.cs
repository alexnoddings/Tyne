using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Filtering.Controllers;

namespace Tyne.Blazor.Tables.Columns;

/// <summary>
///     A table column header which renders a
///     <see cref="TyneMultiSelectBoxFilterController{TRequest,TValue}"/>.
/// </summary>
/// <inheritdoc/>
public partial class TyneMultiSelectBoxColumnHeader<TRequest, TResponse, TValue>
{
    /// <summary>
    ///     The property on <typeparamref name="TResponse"/> to order by.
    /// </summary>
    /// <remarks>
    ///     Use <see langword="null"/> to not apply ordering, or <c>"*"</c> to use the name of the property
    ///     which <see cref="TyneMultiSelectFilterControllerBase{TRequest, TValue}.For"/> points to.
    /// </remarks>
    [Parameter]
    public string? OrderBy { get; set; }
    private TyneKey OrderByKey => TyneKey.From(OrderBy, ForCache.PropertyInfo);

    /// <summary>
    ///     The content to show in the column header.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
