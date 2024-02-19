using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A controller which attaches to a <see cref="string"/> property.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <remarks>
///     This controller renders as a <see cref="MudTextField{T}"/>.
/// </remarks>
public partial class TyneStringFilterController<TRequest>
{
    /// <summary>
    ///     A class to apply to the rendered <see cref="MudTextField{T}"/>.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    ///     A label to show on the rendered <see cref="MudTextField{T}"/>.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    ///     An <see cref="Expression"/> for the <see cref="string"/> property to attach to.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TRequest, string>> For { get; set; } = null!;
    protected TynePropertyKeyCache<TRequest, string> ForCache { get; } = new();
    protected override TyneKey ForKey => ForCache.Update(For);
}
