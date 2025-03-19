using System.Diagnostics.CodeAnalysis;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     An implementation of <see cref="IFilterValueHandle{TValue}"/>
///     designed for use with <see cref="TyneFilterContext{TRequest}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type which the value manages.</typeparam>
internal sealed class FilterValueHandle<TRequest, TValue> : FilterValueHandle<TRequest>, IFilterValueHandle<TValue>
{
    private sealed record HandleState(TyneFilterContext<TRequest> Context, IFilter<TRequest> FilterInstance);
    private HandleState? _state;

    [MemberNotNullWhen(false, nameof(_state))]
    private bool IsDisposed => _state is null;

    private readonly TyneKey _key;
    public ref readonly TyneKey Key => ref _key;
    internal override IFilter<TRequest> FilterInstance
    {
        get
        {
            EnsureNotDisposed();
            return _state.FilterInstance;
        }
    }

    /// <summary>
    ///     Constructs a new <see cref="FilterValueHandle{TRequest, TValue}"/>.
    /// </summary>
    /// <param name="context">The filter context.</param>
    /// <param name="key">The <see cref="TyneKey"/> which <paramref name="filterInstance"/> attached to <paramref name="context"/> with.</param>
    /// <param name="filterInstance">The filter.</param>
    public FilterValueHandle(TyneFilterContext<TRequest> context, TyneKey key, IFilter<TRequest> filterInstance)
    {
        _state = new(context, filterInstance);
        _key = key;
    }

    /// <summary>
    ///     Triggers a reload of the data inside this context.
    /// </summary>
    /// <returns>
    ///     See <see cref="TyneFilterContext{TRequest}.ReloadDataAsync"/>.
    /// </returns>
    public Task ReloadDataAsync()
    {
        if (IsDisposed)
            return Task.CompletedTask;

        return _state.Context.ReloadDataAsync();
    }

    /// <summary>
    ///     Notifies the context of this filter values handle being updated to <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the value notification.</returns>
    public Task NotifyValueUpdatedAsync(TValue? newValue)
    {
        if (IsDisposed)
            return Task.CompletedTask;

        return _state.Context.NotifyValueUpdatedAsync(_key, newValue);
    }

    /// <summary>
    ///     Notifies the context of this filter value's state having changed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the state change notification.</returns>
    public Task NotifyStateChangedAsync()
    {
        if (IsDisposed)
            return Task.CompletedTask;

        return _state.Context.NotifyStateChangedAsync(_key);
    }

    [MemberNotNull(nameof(_state))]
    private void EnsureNotDisposed()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(FilterValueHandle<TRequest, TValue>), "Handle is disposed.");
    }

    /// <summary>
    ///     Disposes of the handle, detaching the filter value from the context.
    /// </summary>
    public override void Dispose()
    {
        // Assume we're already disposed if state is null
        if (_state is null)
            return;

        _state.Context.DetachFilterValue(this);
        _state = null;
    }
}
