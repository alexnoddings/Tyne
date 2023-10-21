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
    private TyneFilterContext<TRequest>? _context;
    private readonly TyneKey _key;
    public ref readonly TyneKey Key => ref _key;

    private IFilter<TRequest>? _filterInstance;
    internal override IFilter<TRequest> FilterInstance
    {
        get
        {
            EnsureNotDisposed();
            return _filterInstance!;
        }
    }

    /// <summary>
    ///     Constructs a new <see cref="FilterValueHandle{TRequest, TValue}"/>.
    /// </summary>
    /// <param name="context">The filter context.</param>
    /// <param name="key">The <see cref="TyneKey"/> which <paramref name="filterInstance"/> attached to <paramref name="context"/> with.</param>
    /// <param name="filterInstance">The filter.</param>
    public FilterValueHandle(TyneFilterContext<TRequest>? context, TyneKey key, IFilter<TRequest> filterInstance)
    {
        _context = context;
        _key = key;
        _filterInstance = filterInstance;
    }

    /// <summary>
    ///     Triggers a reload of the data inside this context.
    /// </summary>
    /// <returns>
    ///     See <see cref="TyneFilterContext{TRequest}.ReloadDataAsync"/>.
    /// </returns>
    public Task ReloadDataAsync()
    {
        EnsureNotDisposed();
        return _context.ReloadDataAsync();
    }

    /// <summary>
    ///     Notifies the context of this filter values handle being updated to <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the value notification.</returns>
    public Task NotifyValueUpdatedAsync(TValue? newValue)
    {
        EnsureNotDisposed();
        return _context.NotifyValueUpdatedAsync(_key, newValue);
    }

    [MemberNotNull(nameof(_context))]
    private void EnsureNotDisposed()
    {
        if (_context is null)
            throw new ObjectDisposedException(nameof(FilterValueHandle<TRequest, TValue>), "Handle is disposed.");
    }

    /// <summary>
    ///     Disposes of the handle, detaching the filter value from the context.
    /// </summary>
    public override void Dispose()
    {
        if (_context is null)
            return;

        _context.DetachFilterValue(this);
        _context = null;
        _filterInstance = null;
    }
}
