using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Tables.Columns;

/// <summary>
///     A column for use in a <see cref="TyneTable2Base{TRequest, TResponse}"/>.
/// </summary>
/// <typeparam name="TResponse">The type of response.</typeparam>
public partial class TyneColumnHeader<TResponse> : ComponentBase
{
    /// <summary>
    ///     Which property on <typeparamref name="TResponse"/> to order by when the column is interacted with.
    /// </summary>
    /// <remarks>
    ///     Use <see langword="null"/> (or do not supply a parameter) to not enable ordering on this column header.
    /// </remarks>
    [Parameter]
    public string? OrderBy { get; set; }
    private TyneKey OrderByKey => TyneKey.From(OrderBy);

    /// <summary>
    ///     The <see cref="RenderFragment"/> to display in the column header.
    /// </summary>
    /// <remarks>
    ///     This content is always shown.
    /// </remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
