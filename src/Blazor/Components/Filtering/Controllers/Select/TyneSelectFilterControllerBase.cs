using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A base implementation of a selection controller.
/// </summary>
/// <typeparam name="TSelectItem">The type which the filter value is expected to provide with <see cref="IFilterSelectValue{TValue}"/>.</typeparam>
/// <inheritdoc/>
public abstract partial class TyneSelectFilterControllerBase<TRequest, TValue, TSelectItem> : TyneFilterControllerBase<TRequest, TValue>
{
    /// <summary>
    ///     A label for the UI to indicate what this controller is for.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    ///     Gets the <see cref="IFilterSelectItem{TSelectItem}"/>s registered to the attached filter value.
    /// </summary>
    /// <remarks>
    ///     If the attached filter value is not compatible with selection (i.e. does not implement <see cref="IFilterSelectValue{TSelectItem}"/>), an <see cref="InvalidOperationException"/> will be thrown.
    /// </remarks>
    /// <exception cref="InvalidOperationException">When the attached filter does not support <see cref="IFilterSelectValue{TSelectItem}"/>.</exception>
    protected ICollection<IFilterSelectItem<TSelectItem?>>? SelectItems =>
            EnsureFilterSupportsSelection().SelectItems;

    private IFilterSelectValue<TSelectItem> EnsureFilterSupportsSelection()
    {
        if (Handle.Filter is not IFilterSelectValue<TSelectItem> filterSelectValue)
            throw new InvalidOperationException($"{nameof(TyneSingleSelectBoxFilterController<TRequest, TValue>)} is not compatible with filter value for '{ForKey}'; filter value does not support {nameof(IFilterSelectValue<TSelectItem>)}<{typeof(TSelectItem).Name}>.");

        return filterSelectValue;
    }

    /// <summary>
    ///     Initialises the controller and ensures that the attached filter value supports selection (i.e. implements <see cref="IFilterSelectValue{TSelectItem}"/>).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the initialisation.</returns>
    protected override Task OnInitializedAsync()
    {
        // Check the handle filter is valid.
        _ = EnsureFilterSupportsSelection();
        return Task.CompletedTask;
    }
}