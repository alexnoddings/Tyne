using Tyne.Blazor.Filtering.Context;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A handle given to a <see cref="IFilterValue{TValue}"/> when it attaches to an <see cref="IFilterContext{TRequest}"/>.
/// </summary>
/// <typeparam name="TValue">The type the filter value manages.</typeparam>
/// <remarks>
///     This handle should be disposed once the <see cref="IFilterValue{TValue}"/>
///     goes out of scope to ensure it is detached from the <see cref="IFilterContext{TRequest}"/>.
/// </remarks>
public interface IFilterValueHandle<in TValue> : IDisposable
{
    /// <summary>
    ///     Instructs the <see cref="IFilterContext{TRequest}"/> to reload the data being filtered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the data being reloaded.</returns>
    public Task ReloadDataAsync();

    /// <summary>
    ///     Notifies the <see cref="IFilterContext{TRequest}"/>
    ///     (and any controllers attached to this value)
    ///     that this value has a <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the new value notification being handled.</returns>
    /// <remarks>
    ///     The context must have started initialisation prior to calling this.
    ///     This can be checked with <see cref="IFilterContext{TRequest}.IsInitialisationStarted"/>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When called prior to the context starting initialisation.</exception>
    public Task NotifyValueUpdatedAsync(TValue? newValue);

    /// <summary>
    ///     Notifies the <see cref="IFilterContext{TRequest}"/>
    ///     (and any controllers attached to this value)
    ///     that this value's state has changed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the state change notification being handled.</returns>
    /// <remarks>
    ///     <para>
    ///         Use <see cref="NotifyValueUpdatedAsync(TValue?)"/> if the <typeparamref name="TValue"/> has been updated.
    ///         Otherwise, use this to notify of a non-value change to the filter state (e.g. remote data has loaded).
    ///     </para>
    ///     <para>
    ///         The context must have started initialisation prior to calling this.
    ///         This can be checked with <see cref="IFilterContext{TRequest}.IsInitialisationStarted"/>
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When called prior to the context starting initialisation.</exception>
    public Task NotifyStateChangedAsync();
}
