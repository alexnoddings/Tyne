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
///         <see cref="TyneFilterValueCore{TRequest, TValue}.SetValueAsync(TValue)"/>
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

    /// <summary>
    ///     Updates <see cref="SelectItems"/> with the new values returned by <see cref="LoadAvailableValuesAsync"/>,
    ///     and notifies the context via <see cref="IFilterValueHandle{TValue}.NotifyStateChangedAsync"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the available value update.</returns>
    /// <remarks>
    ///     If <see cref="LoadAvailableValuesAsync"/> executes asynchronously,
    ///     <see cref="SelectItems"/> will be temporarily updated with <see langword="null"/>.
    /// </remarks>
    protected async Task UpdateAvailableValuesAsync()
    {
        // Kick off loading task
        var availableValuesTask = LoadAvailableValuesAsync();
        List<IFilterSelectItem<TSelectValue?>> availableValues;

        // If the task completes synchronously,
        // then we don't need to bother updating the context with the
        // intermediate 'no values' state (setting SelectItems to null)
        if (availableValuesTask.IsCompleted)
        {
            // CA1849: Call async methods when in an async method
            // REASON: We only run this branch when we know the task has already completed.
#pragma warning disable CA1849
            availableValues = availableValuesTask.Result;
#pragma warning restore CA1849
        }
        else
        {
            // While we wait for the new values, update the context that there are no available values
            // This prevents stale values from being select-able if LoadAvailableValuesAsync is slow
            if (SelectItems is not null)
            {
                SelectItems = null;
                await NotifyContextOfSelectItemsUpdatedAsync().ConfigureAwait(false);
            }

            availableValues = await availableValuesTask.ConfigureAwait(false);
        }

        SelectItems = availableValues?.ToList();
        await NotifyContextOfSelectItemsUpdatedAsync().ConfigureAwait(false);
    }

    private Task NotifyContextOfSelectItemsUpdatedAsync()
    {
        if (Context.IsInitialisationStarted)
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
