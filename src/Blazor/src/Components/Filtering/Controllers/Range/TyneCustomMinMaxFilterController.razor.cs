using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A controller which attaches to a min and a max <typeparamref name="TValue"/> on <typeparamref name="TRequest"/>.
///     The values to attach to are provided by <see cref="TyneMinMaxFilterController{TRequest, TValue}.ForMin"/> and <see cref="TyneMinMaxFilterController{TRequest, TValue}.ForMax"/>.
///     This controller does not render it's own UI. Instead, it has a <see cref="ChildContent"/>
///     which interacts with the min and max values through <see cref="ITyneCustomMinMaxFilterContext{TValue}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type the filter values manage.</typeparam>
public partial class TyneCustomMinMaxFilterController<TRequest, TValue> : ITyneCustomMinMaxFilterContext<TValue>
{
    /// <summary>
    ///     A <see cref="RenderFragment{TValue}"/> to render the controller's custom content.
    /// </summary>
    /// <remarks>
    ///     This takes a <see cref="ITyneCustomMinMaxFilterContext{TValue}"/>
    ///     which provides access to the attached values.
    /// </remarks>
    [Parameter]
    public RenderFragment<ITyneCustomMinMaxFilterContext<TValue>> ChildContent { get; set; } =
        EmptyRenderFragment.For<ITyneCustomMinMaxFilterContext<TValue>>();

    // These are explicit interface implementations as they need to be public,
    // which clashes with existing members of the same name.
    TValue? ITyneCustomMinMaxFilterContext<TValue>.Min => Min;
    IFilterControllerHandle<TValue?> ITyneCustomMinMaxFilterContext<TValue>.MinHandle => MinHandle;
    TValue? ITyneCustomMinMaxFilterContext<TValue>.Max => Max;
    IFilterControllerHandle<TValue?> ITyneCustomMinMaxFilterContext<TValue>.MaxHandle => MaxHandle;

    public Task SetValuesAsync(TValue? min, TValue? max) => SetFilterValuesAsync(min, max);
}
