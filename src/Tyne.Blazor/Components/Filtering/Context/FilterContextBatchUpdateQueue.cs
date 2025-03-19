namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     Tracks calls made to a <see cref="TyneFilterContext{TRequest}"/> during a batch update,
///     so that they can be applied together once the batch update is completing.
/// </summary>
internal sealed class FilterContextBatchUpdateQueue<TRequest>
{
    private readonly TyneFilterContext<TRequest> _filterContext;

    /// <summary>
    ///     Whether a call to <see cref="TyneFilterContext{TRequest}.ReloadDataAsync()"/> has been made.
    /// </summary>
    /// <remarks>
    ///     Calls to <see cref="TyneFilterContext{TRequest}.ReloadDataAsync()"/> will be ignored and queued here during a batch update.
    ///     This ensures that updating N values will only result in one data reload once all values are updated, rather than reloading per value update.
    /// </remarks>
    public bool IsReloadDataQueued { get; set; }

    /// <summary>
    ///     Parameters which were persisted during a batch update.
    /// </summary>
    /// <remarks>
    ///     Parameters need to be queued and applied in bulk as WASM's NavigationManager asynchronously dispatches a task to update the URL.
    ///     Because of this, multiple calls in quick succession (such as a batch update) are likely to fail as the current URL will not have updated yet.
    /// </remarks>
    /// <seealso href="https://github.com/dotnet/aspnetcore/blob/main/src/Components/WebAssembly/WebAssembly/src/Services/WebAssemblyNavigationManager.cs#L57"/>
    public Dictionary<string, object?> PersistenceParameterQueue { get; } = [];

    public FilterContextBatchUpdateQueue(TyneFilterContext<TRequest> filterContext)
    {
        _filterContext = filterContext ?? throw new ArgumentNullException(nameof(filterContext));
    }

    /// <summary>
    ///     FLushes the operations queued during the batch update.
    /// </summary>
    /// <remarks>
    ///     This should be executed once the batch has completed and set to <see langword="null"/>.
    ///     Otherwise, the calls being flushed will be queued up again here, then lost once the queue is cleared.
    /// </remarks>
    public async Task FlushAsync()
    {
        if (PersistenceParameterQueue.Count > 0)
            _filterContext.Persistence.BulkSetValues(PersistenceParameterQueue);

        if (IsReloadDataQueued)
            await _filterContext.ReloadDataAsync().ConfigureAwait(false);

        PersistenceParameterQueue.Clear();
        IsReloadDataQueued = false;
    }
}
