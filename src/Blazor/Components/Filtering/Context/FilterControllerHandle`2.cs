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
    private TyneFilterContext<TRequest>? _context;
    private readonly TyneKey _key;
    public ref readonly TyneKey Key => ref _key;

    private IFilterValue<TValue>? _filter;
    public IFilterValue<TValue> Filter => EnsureNotDisposed(_filter);

    private IFilterController<TValue>? _filterController;
    internal IFilterController<TValue> FilterController => EnsureNotDisposed(_filterController);

    internal override IFilterController FilterControllerBase => FilterController;

    /// <summary>
    ///     Constructs a new <see cref="FilterControllerHandle{TRequest, TValue}"/>.
    /// </summary>
    /// <param name="context">The filter context.</param>
    /// <param name="key">The <see cref="TyneKey"/> the <paramref name="filterController"/> has attached to the <paramref name="context"/> with.</param>
    /// <param name="filterValue">The filter value the <paramref name="filterController"/> is attached to.</param>
    /// <param name="filterController">The filter controller.</param>
    public FilterControllerHandle(
        TyneFilterContext<TRequest>? context,
        TyneKey key,
        IFilterValue<TRequest, TValue> filterValue,
        IFilterController<TValue> filterController
    )
    {
        _context = context;
        _key = key;
        _filter = filterValue;
        _filterController = filterController;
    }

    [return: NotNull]
    private T EnsureNotDisposed<T>(T? prop) =>
        prop ?? throw new ObjectDisposedException(nameof(FilterValueHandle<TRequest>), "Handle is disposed.");

    /// <summary>
    ///     Detaches the filter controller.
    /// </summary>
    public override void Dispose()
    {
        // Assume we're already disposed if context is null
        if (_context is null)
            return;

        // Detach this handle then null out every field to indicate we're disposed
        _context.DetachFilterController(this);
        _context = null;
        _filter = null;
        _filterController = null;
    }
}
