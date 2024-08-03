using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Filtering.Context;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A base controller which attaches to a min and a max <typeparamref name="TValue"/> on <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type the filter value manages.</typeparam>
/// <remarks>
///     <para>
///         This handles attaching this instance to the cascading <see cref="IFilterContext{TRequest}"/>
///         and exposes the <see cref="MinHandle"/> and <see cref="MaxHandle"/>.
///         These handles are automatically detached when disposed.
///     </para>
///     <para>
///         It also provides <see cref="SetFilterValuesAsync(TValue?, TValue?)"/> which
///         handles starting <see cref="IFilterContext{TRequest}.BatchUpdateValuesAsync(Func{Task})"/> for you.
///     </para>
///     <para>
///         Inheritors only need to implement <see cref="ForMinKey"/> and <see cref="ForMaxKey"/>
///         so this knows what <see cref="TyneKey"/>s to attach to during initialisation.
///     </para>
/// </remarks>
// We can't simply inherit from TyneFilterControllerBase as it needs a handle for more than one value (one for min, one for max).
// This could be simplified by using one type (e.g. MudBlazor's DateRange), but that would require the shared and server projects
// to also know about the DateRange type, which introduces a dependency on MudBlazor to the server.
public abstract partial class TyneMinMaxFilterControllerBase<TRequest, TValue> : ComponentBase, IFilterController<TValue?>, IDisposable
{
    /// <summary>
    ///     The cascading filtering context this controller is running in.
    /// </summary>
    [CascadingParameter]
    protected IFilterContext<TRequest> Context { get; init; } = null!;

    /// <summary>
    ///     The <see cref="TyneKey"/> of the minimum property to attach to on the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    protected abstract TyneKey ForMinKey { get; }

    /// <summary>
    ///     The handle returned when we attached to the min property on the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    protected IFilterControllerHandle<TValue?> MinHandle { get; private set; } = null!;

    /// <summary>
    ///     The <typeparamref name="TValue"/> of the attached minimum filter value.
    /// </summary>
    /// <remarks>
    ///     This is a convenient shorthand to access <see cref="MinHandle"/>.
    /// </remarks>
    protected TValue? Min =>
        MinHandle.FilterValue.Value;

    /// <summary>
    ///     The <see cref="TyneKey"/> of the maximum property to attach to on the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    protected abstract TyneKey ForMaxKey { get; }

    /// <summary>
    ///     The handle returned when we attached to the max property on the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    // The handle is attached during OnInitialized, which is the first
    // method executed, so we don't expect any callers to access it before then.
    protected IFilterControllerHandle<TValue?> MaxHandle { get; private set; } = null!;

    /// <summary>
    ///     The <typeparamref name="TValue"/> of the attached minimum filter value.
    /// </summary>
    /// <remarks>
    ///     This is a convenient shorthand to access <see cref="MaxHandle"/>.
    /// </remarks>
    protected TValue? Max =>
        MaxHandle.FilterValue.Value;

    /// <summary>
    ///     Attaches this instance to the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    /// <remarks>
    ///     If overriding this, you need to call it before attempting to access <see cref="MinHandle"/> or <see cref="MaxHandle"/>.
    /// </remarks>
    protected override void OnInitialized()
    {
        MinHandle = Context.AttachController(ForMinKey, this);
        MaxHandle = Context.AttachController(ForMaxKey, this);
    }

    /// <summary>
    ///     Sets the values of the attached min and max filter's <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="min">The new minimum <typeparamref name="TValue"/>.</param>
    /// <param name="max">The new maximum <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the values being set.</returns>
    /// <remarks>
    ///     This wraps value setting in a batch update on the context.
    ///     This is required to ensure values are properly set - you should not
    ///     update the min or max value at the same time outside of a batch update.
    /// </remarks>
    protected virtual Task SetFilterValuesAsync(TValue? min, TValue? max)
    {
        return Context.BatchUpdateValuesAsync(async () =>
        {
            await MinHandle.FilterValue.SetValueAsync(min).ConfigureAwait(true);
            await MaxHandle.FilterValue.SetValueAsync(max).ConfigureAwait(true);
        });
    }

    /// <summary>
    ///     Invoked by <see cref="IFilterContext{TRequest}"/> when the
    ///     min or max <see cref="IFilterValue{TValue}"/> this instance
    ///     is attached to are updated with a <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <remarks>
    ///     <para>
    ///         The default implementation of this is to invoke <see cref="ComponentBase.StateHasChanged"/>.
    ///     </para>
    ///     <para>
    ///         This method is not always called on the associated renderer's synchronisation context.
    ///         Any calls to the Blazor state (e.g. <see cref="ComponentBase.StateHasChanged"/>) should be
    ///         executed using <see cref="ComponentBase.InvokeAsync(System.Action)"/>.
    ///     </para>
    /// </remarks>
    public virtual Task OnValueUpdatedAsync(TValue? newValue) =>
        InvokeAsync(StateHasChanged);

    /// <summary>
    ///     Invoked by <see cref="IFilterContext{TRequest}"/> when the
    ///     min or max <see cref="IFilterValue{TValue}"/> this instance
    ///     is attached to has a state change.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The default implementation of this is to invoke <see cref="ComponentBase.StateHasChanged"/>.
    ///     </para>
    ///     <para>
    ///         This method is not always called on the associated renderer's synchronisation context.
    ///         Any calls to the Blazor state (e.g. <see cref="ComponentBase.StateHasChanged"/>) should be
    ///         executed using <see cref="ComponentBase.InvokeAsync(System.Action)"/>.
    ///     </para>
    /// </remarks>
    public Task OnStateChangedAsync() =>
        InvokeAsync(StateHasChanged);

    /// <summary>
    ///     Disposes of the filter controller's resources and
    ///     detaches the <see cref="MinHandle"/> and <see cref="MaxHandle"/>.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        MinHandle?.Dispose();
        MaxHandle?.Dispose();
    }

    /// <summary>
    ///     Disposes of the filter controller.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
