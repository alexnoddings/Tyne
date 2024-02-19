namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A <see cref="TyneFilterValue{TRequest, TValue}"/> which supports
///     value selection through <see cref="IFilterSelectValue{TValue}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type which the value manages.</typeparam>
/// <typeparam name="TSelectValue">The type of value which is select-able.</typeparam>
/// <remarks>
///     <para>
///         <typeparamref name="TValue"/> and <typeparamref name="TSelectValue"/> may differ
///         to support multi-selection by having <typeparamref name="TValue"/> be a
///         <see cref="HashSet{T}"/> of <typeparamref name="TSelectValue"/>.
///     </para>
///     <para>
///         By default, this does not enforce that any values set through
///         <see cref="TyneFilterValueBase{TRequest, TValue}.SetValueAsync(TValue)"/>
///         appears in <see cref="SelectItems"/>.
///     </para>
/// </remarks>
public abstract class TyneFilterSelectValueBase<TRequest, TValue, TSelectValue> : TyneFilterValue<TRequest, TValue>, IFilterSelectValue<TSelectValue>
{
    /// <summary>
    ///     The item available for selection.
    /// </summary>
    public ICollection<IFilterSelectItem<TSelectValue?>>? SelectItems { get; private set; }

    /// <summary>
    ///     Initialises the value.
    /// </summary>
    /// <remarks/>
    /// <inheritdoc/>
    protected override async Task InitialiseAsync()
    {
        await UpdateAvailableValuesAsync().ConfigureAwait(false);

        // base handles initialising the value, which we only want to try once available values is set
        await base.InitialiseAsync().ConfigureAwait(false);
    }

    protected async Task UpdateAvailableValuesAsync()
    {
        if (SelectItems is not null)
        {
            SelectItems = null;
            await NotifyContextOfSelectItemsUpdatedAsync().ConfigureAwait(false);
        }

        var availableValues = await LoadAvailableValuesAsync().ConfigureAwait(false);

        SelectItems = availableValues?.ToList();
        await NotifyContextOfSelectItemsUpdatedAsync().ConfigureAwait(false);
    }

    private Task NotifyContextOfSelectItemsUpdatedAsync()
    {
        if (IsInitialised)
            return Handle.NotifyStateChangedAsync();

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Loads the available <see cref="IFilterSelectItem{TValue}"/>s.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> whose result is the available <see cref="IFilterSelectItem{TValue}"/>s.
    /// </returns>
    protected abstract Task<List<IFilterSelectItem<TSelectValue?>>> LoadAvailableValuesAsync();
}
