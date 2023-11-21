using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Filtering.Context;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     Base implementation of <see cref="IFilterController{TValue}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type the filter value manages.</typeparam>
/// <remarks>
///     <para>
///         This handles attaching this instance to the cascading
///         <see cref="IFilterContext{TRequest}"/> and exposes the <see cref="Handle"/>.
///         This handle is automatically detached when disposed.
///     </para>
///     <para>
///         It also provides some convenient shorthands getting/setting the filter value.
///     </para>
///     <para>
///         Inheritors only need to implement <see cref="ForKey"/> so this
///         knows what <see cref="TyneKey"/> to attach to during initialisation.
///     </para>
/// </remarks>
public abstract class TyneFilterControllerBase<TRequest, TValue> : ComponentBase, IFilterController<TValue>, IDisposable
{
    [CascadingParameter]
    private IFilterContext<TRequest> Context { get; init; } = null!;

    /// <summary>
    ///     The <see cref="TyneKey"/> to attach to on the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    /// <remarks>
    ///     This should NOT change for the lifetime of the controller.
    /// </remarks>
    protected abstract TyneKey ForKey { get; }

    private IFilterControllerHandle<TValue>? _handle;
    /// <summary>
    ///     The handle returned when we attached to the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    /// <remarks>
    ///     Accessing this after the instance has been disposed will throw an <see cref="ObjectDisposedException"/>.
    /// </remarks>
    protected IFilterControllerHandle<TValue> Handle
    {
        get
        {
            // We only account for the handle being null after being disposed.
            // The handle is attached during OnInitialized, which is the first
            // method executed, so we don't expect any callers to access it before then.
            if (_handle is null)
                throw new ObjectDisposedException(nameof(TyneFilterControllerBase<TRequest, TValue>), "Cannot access handle after value has been disposed.");

            return _handle;
        }
    }

    /// <summary>
    ///     The <typeparamref name="TValue"/> of the attached filter value.
    /// </summary>
    /// <remarks>
    ///     This is a convenient shorthand to access <see cref="Handle"/>.
    /// </remarks>
    protected TValue? Value =>
        Handle.Filter.Value;

    /// <summary>
    ///     Attaches this instance to the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    /// <remarks>
    ///     If overriding this, you need to call it before attempting to access <see cref="Handle"/>.
    /// </remarks>
    protected override void OnInitialized()
    {
        if (ForKey.IsEmpty)
            throw new KeyEmptyException($"Controller can't attach to empty {nameof(ForKey)}. Are you missing a For property?");

        _handle = Context.AttachController(ForKey, this);
    }

    /// <summary>
    ///     Sets the value of the attached filter's <typeparamref name="TValue"/> to <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the value being set.</returns>
    /// <remarks>
    ///     <para>
    ///         This is a convenient shorthand to access <see cref="Handle"/>.
    ///     </para>
    /// </remarks>
    protected Task SetFilterValueAsync(TValue? newValue) =>
        Handle.Filter.SetValueAsync(newValue);

    /// <summary>
    ///     Clears the value of the attached filter's <typeparamref name="TValue"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the value being cleared.</returns>
    /// <remarks>
    ///     <para>
    ///         This is a convenient shorthand to access <see cref="Handle"/>.
    ///     </para>
    /// </remarks>
    protected Task ClearFilterValueAsync() =>
        Handle.Filter.ClearValueAsync();

    /// <summary>
    ///     Invoked by <see cref="IFilterContext{TRequest}"/>
    ///     when the <see cref="IFilterValue{TValue}"/> this instance
    ///     is attached to is updated with a <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>
    ///     The default implementation of this is to just invoke <see cref="ComponentBase.StateHasChanged"/>.
    /// </returns>
    public virtual Task OnValueUpdatedAsync(TValue? newValue)
    {
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Disposes of the filter controller's resources
    ///     and detaches the <see cref="Handle"/>.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        _handle?.Dispose();
        _handle = null;
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
