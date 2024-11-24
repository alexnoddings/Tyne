using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A filter controller which is controller through <see cref="ChildContent"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type the filter value manages.</typeparam>
/// <example lang="razor">
/// ```razor
///     &lt;TyneCustomFilterController TRequest="SearchParts.Request" TValue="PartSize?" For="m =&gt; m.Size1" Context="Filter"&gt;
///         @if (Filter.Value is PartSize.Small)
///         {
///             &lt;MudButton Variant="Variant.Outlined" Color="Color.Primary" OnClick="Filter.ClearFilterValueAsync"&gt;
///                 Show all sizes
///             &lt;/MudButton&gt;
///         }
///         else
///         {
///             &lt;MudButton Variant="Variant.Outlined" Color="Color.Primary" OnClick="@(() =&gt; Filter.SetFilterValueAsync(PartSize.Small))"&gt;
///                 Show only small parts
///             &lt;/MudButton&gt;
///         }
///     &lt;/TyneCustomFilterController&gt;
/// ```
/// </example>
public sealed partial class TyneCustomFilterController<TRequest, TValue> : TyneFilterControllerBase<TRequest, TValue>, ITyneCustomFilterContext<TValue>
{
    /// <summary>
    ///     A <see cref="RenderFragment{TValue}"/> to render the controller's custom content.
    /// </summary>
    /// <remarks>
    ///     This takes a <see cref="ITyneCustomFilterContext{TValue}"/>
    ///     which provides access to the attached value.
    /// </remarks>
    [Parameter]
    public RenderFragment<ITyneCustomFilterContext<TValue>> ChildContent { get; set; } = EmptyRenderFragment.For<ITyneCustomFilterContext<TValue>>();

    /// <summary>
    ///     An <see cref="Expression"/> for the <typeparamref name="TValue"/> property to attach to.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TRequest, TValue>> For { get; set; } = null!;
    private readonly TynePropertyKeyCache<TRequest, TValue> _forCache = new();
    protected override TyneKey ForKey => _forCache.Update(For);

    // These are explicit interface implementations as they need to be public,
    // which clashes with existing members of the same name.
    TValue? ITyneCustomFilterContext<TValue>.Value => Value;
    IFilterControllerHandle<TValue> ITyneCustomFilterContext<TValue>.Handle => Handle;

    Task ITyneCustomFilterContext<TValue>.SetFilterValueAsync(TValue? newValue) =>
        Handle.FilterValue.SetValueAsync(newValue);

    Task ITyneCustomFilterContext<TValue>.ClearFilterValueAsync() =>
        Handle.FilterValue.ClearValueAsync();
}
