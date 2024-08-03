using System.Diagnostics.CodeAnalysis;
using Tyne.Blazor.Filtering.Controllers;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     An implementation of <see cref="IFilterControllerHandle{TValue}"/>
///     designed for use with <see cref="TyneFilterContext{TRequest}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The value type which the controller accesses.</typeparam>
/// <remarks>
///     Detaches the <see cref="FilterController"/> when disposed.
/// </remarks>
internal sealed class FilterControllerHandle<TRequest, TValue> : FilterControllerHandle, IFilterControllerHandle<TValue>
{
    private sealed record HandleState(
        TyneFilterContext<TRequest> Context,
        IFilterValue<TValue> FilterValue,
        IFilterController<TValue> FilterController
    );
    private HandleState? _state;

    [MemberNotNullWhen(false, nameof(_state))]
    private bool IsDisposed => _state is null;

    private readonly TyneKey _key;
    public ref readonly TyneKey Key => ref _key;

    public IFilterValue<TValue> FilterValue
    {
        get
        {
            EnsureNotDisposed();
            return _state.FilterValue;
        }
    }

    internal IFilterController<TValue> FilterController
    {
        get
        {
            EnsureNotDisposed();
            return _state.FilterController;
        }
    }

    internal override IFilterController FilterControllerBase => FilterController;

    /// <summary>
    ///     Constructs a new <see cref="FilterControllerHandle{TRequest, TValue}"/>.
    /// </summary>
    /// <param name="context">The filter context.</param>
    /// <param name="key">The <see cref="TyneKey"/> the <paramref name="filterController"/> has attached to the <paramref name="context"/> with.</param>
    /// <param name="filterValue">The filter value the <paramref name="filterController"/> is attached to.</param>
    /// <param name="filterController">The filter controller.</param>
    public FilterControllerHandle(
        TyneFilterContext<TRequest> context,
        TyneKey key,
        IFilterValue<TRequest, TValue> filterValue,
        IFilterController<TValue> filterController
    )
    {
        _state = new(context, filterValue, filterController);
        _key = key;
    }

    [MemberNotNull(nameof(_state))]
    private void EnsureNotDisposed()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(FilterValueHandle<TRequest, TValue>), "Handle is disposed.");
    }

    /// <summary>
    ///     Detaches the filter controller.
    /// </summary>
    public override void Dispose()
    {
        // Assume we're already disposed if state is null
        if (_state is null)
            return;

        _state.Context.DetachFilterController(this);
        _state = null;
    }
}
