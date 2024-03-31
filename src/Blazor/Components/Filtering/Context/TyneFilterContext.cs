using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Tyne.Blazor.Filtering.Controllers;
using Tyne.Blazor.Filtering.Values;
using Tyne.Blazor.Persistence;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     The default implementation of a context for Tyne's rich interactive filtering to run inside of.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <seealso cref="IFilterContext{TRequest}"/>
/// <seealso href="/docs/packages/Blazor/Tables/Lifecycle.html">The docs on filtering for a more comprehensive overview.</seealso>
public sealed class TyneFilterContext<TRequest> : IFilterContext<TRequest>, IDisposable
{
    // Logger instance
    private readonly ILogger _logger;
    // Delegate which will trigger the data this context is filtering to reload
    private readonly ReloadContextData _reloadData;

    // Stores the Task which is initialising the context, or null if init hasn't begun
    private Task? _initTask;

    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(_initTask))]
    public bool IsInitialisationStarted => _initTask is not null;

    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(_initTask))]
    public bool IsInitialised => _initTask?.IsCompletedSuccessfully is true;

    /// <summary>
    ///     <see langword="true"/> if context initialisation did not complete successfully; otherwise, <see langword="false"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This will be <see langword="false"/> if the call to <see cref="InitialiseAsync"/> threw an exception.
    ///         This may happen if an attached value fails during <see cref="IFilter{TRequest}.EnsureInitialisedAsync"/>.
    ///     </para>
    ///     <para>
    ///         Consumers of this context should not use the context if it is faulted.
    ///         Use of a faulted context is not defined behaviour.
    ///     </para>
    /// </remarks>
    [MemberNotNullWhen(true, nameof(_initTask))]
    public bool IsFaulted => _initTask?.IsCompleted is true && !_initTask.IsCompletedSuccessfully;

    // Used during hot reloading to allow filters to overwrite existing ones which haven't disposed yet
    private bool _allowValueOverwriting;

    /// <inheritdoc/>
    public IUrlPersistenceService Persistence { get; }

    // Attached handles for values and controllers
    private readonly Dictionary<TyneKey, FilterValueHandle<TRequest>> _valueHandles = new();
    private readonly Dictionary<TyneKey, HashSet<FilterControllerHandle>> _controllerHandles = new();

    /// <summary>
    ///     Constructs an instance of <see cref="TyneFilterContext{TRequest}"/>.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> for this context.</param>
    /// <param name="persistence">A <see cref="IUrlPersistenceService"/> for the context to use.</param>
    /// <param name="reloadDataFunc">A function which causes the data in this context to be reloaded.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="logger"/>, <paramref name="persistence"/>, or <paramref name="reloadDataFunc"/> are <see langword="null"/>.</exception>
    public TyneFilterContext(ILogger<TyneFilterContext<TRequest>> logger, IUrlPersistenceService persistence, ReloadContextData reloadDataFunc)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ArgumentNullException.ThrowIfNull(persistence);
        Persistence = new FilterContextPersistenceWrapper<TRequest>(this, persistence);
        _reloadData = reloadDataFunc ?? throw new ArgumentNullException(nameof(reloadDataFunc));

        // Hook into Tyne's static Hot Reload functions to notify the context of a hot reload.
        // This lets us temporarily loosen restrictions to allow values and controllers to re-register.
        TyneHotReloadWatcher.OnClearCache += OnHotReloadClearCache;
        TyneHotReloadWatcher.OnUpdateApplication += OnHotReloadUpdateApplication;
    }

    /// <summary>
    ///     Initialises the context.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the context initialising.</returns>
    /// <remarks>
    ///     This handles calling <see cref="IFilter{TRequest}.EnsureInitialisedAsync"/>
    ///     for attached filter values.
    /// </remarks>
    /// <exception cref="InvalidOperationException">When initialising has already began.</exception>
    /// <exception cref="Exception">Any exception thrown during initialisation will be rethrown by the returned <see cref="Task"/>.</exception>
    public Task InitialiseAsync()
    {
        _logger.LogFilterContextInitialising();

        if (IsInitialisationStarted)
        {
            _logger.LogFilterContextAlreadyInitialising();
            throw new InvalidOperationException("Context has already began initialising.");
        }

        // Pre-fill _initTask with an empty, running task.
        // This ensures no race conditions where InitialiseCoreAsync
        // hasn't returned and set _initTask before a value
        // tries to call the context and finds _initTask to be null.
        var placeholderInitTaskCompletionSource = new TaskCompletionSource();
        _initTask = placeholderInitTaskCompletionSource.Task;

        try
        {
            _initTask = InitialiseCoreAsync();
        }
        catch (Exception exception)
        {
            // A lot of logic relies on checking _initTask, but if InitialiseCoreAsync
            // throws synchronously then _initTask will never be set.
            // So we catch synchronous exceptions, capture them in initTask, and then rethrow.
            _logger.LogFilterContextErrorInitialising(exception);
            _initTask = Task.FromException(exception);
            placeholderInitTaskCompletionSource.SetException(exception);
            // We don't wrap the exception, otherwise a sync vs async exception
            // thrown by InitialiseCoreAsync would behave differently.
            throw;
        }

        // Continue the initialisation with a method to complete the placeholder init task once it completes.
        // We want to complete synchronously as it's very short lived and we want the placeholder to complete as soon as possible.
        return _initTask.ContinueWith(CompletePlaceholderInitTask, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);

        void CompletePlaceholderInitTask(Task actualInitTask)
        {
            if (actualInitTask.IsCompletedSuccessfully)
            {
                placeholderInitTaskCompletionSource.SetResult();
                return;
            }

            if (actualInitTask.IsCanceled)
            {
                placeholderInitTaskCompletionSource.SetCanceled();
                return;
            }

            // This method should only ever be called when actualInitTask has completed,
            // which means we must have faulted to reach here.
            var exception =
                (Exception?)actualInitTask.Exception
                ?? new InvalidOperationException("Unknown exception occurred during context initialisation.");

            placeholderInitTaskCompletionSource.SetException(exception);
        }
    }

    /// <summary>
    ///     Waits for the context to be initialised.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> representing the context initialising.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This assumes that you have already began context initialisation with <see cref="InitialiseAsync"/>.
    ///         Trying to call this prior to initialisation indicates incorrect usage by the caller.
    ///     </para>
    ///     <para>
    ///         You only need to call this if <see cref="IsInitialised"/> is <see langword="false"/>.
    ///         Calling it once initialised will just return a <see cref="Task"/> whose status is <see cref="TaskStatus.RanToCompletion"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When this is called before <see cref="InitialiseAsync"/>.</exception>
    /// <exception cref="Exception">Any exceptions thrown during <see cref="InitialiseAsync"/> will be captured in the returned <see cref="Task"/>.</exception>
    public Task WaitForInitialisedAsync()
    {
        if (!IsInitialisationStarted)
            throw new InvalidOperationException("Cannot wait for initialisation, context has not yet began initialisation.");

        return _initTask;
    }

    // Performs the core initialisation routines for the context.
    private Task InitialiseCoreAsync()
    {
        _logger.LogFilterContextInitialisingValues();

        // Value initialisation is ran in parallel so multiple async values can initialise at the same time (e.g. calling an API)
        var filterInitialisationTasks = _valueHandles.Values.Select(valueHandle => valueHandle.FilterInstance.EnsureInitialisedAsync());

        // Wait for all filters to initialise.
        // This will throw an for any init task faults.
        return Task.WhenAll(filterInitialisationTasks);
    }

    private void EnsureReadyToDispatchNotifications(TyneKey key)
    {
        // Init doesn't have to be completed, only started.
        // This is so that values may notify controllers of updates during init.
        // This isn't important for synchronous/fast values, but is important
        // for values which don't notify til after the first render has completed
        // (e.g. loading values from an API)
        if (!IsInitialisationStarted)
            throw new InvalidOperationException("Notifications cannot be dispatched before initialisation has begun.");

        if (key.IsEmpty)
            throw new KeyEmptyException("Cannot notify for for an empty key.");
    }

    /// <summary>
    ///     Notifies the context that the value for <paramref name="key"/> has been updated with <paramref name="newValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The type the filter value for <paramref name="key"/> manages.</typeparam>
    /// <param name="key">The <see cref="TyneKey"/> of the filter value which has updated.</param>
    /// <param name="newValue">The new <typeparamref name="TValue"/> which has been updated.</param>
    /// <returns>A <see cref="Task"/> representing the value update notification(s).</returns>
    /// <remarks>
    ///     <para>
    ///         This calls <see cref="IFilterController{TValue}.OnValueUpdatedAsync(TValue)"/>
    ///         for the controllers attached for <paramref name="key"/>.
    ///     </para>
    ///     <para>
    ///         This is internal as it's designed to be called through handles, not directly by values.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When called prior to <see cref="InitialiseAsync"/>.</exception>
    /// <exception cref="KeyEmptyException">When <paramref name="key"/> is empty.</exception>
    internal async Task NotifyValueUpdatedAsync<TValue>(TyneKey key, TValue? newValue)
    {
        EnsureReadyToDispatchNotifications(key);

        _logger.LogFilterContextNotifyingControllersOfValueUpdate(key, newValue);

        // Don't fail if no controller handles are found, having none attached is valid
        if (!_controllerHandles.TryGetValue(key, out var controllerHandles))
            return;

        foreach (var controllerHandle in controllerHandles)
        {
            // This should never fail as the controller attaching code should only add <TRequest, Value> handles
            if (controllerHandle is FilterControllerHandle<TRequest, TValue> valueControllerHandle)
                await valueControllerHandle.FilterController.OnValueUpdatedAsync(newValue).ConfigureAwait(false);
            else
                Debug.Fail("Controller handle is not of the expected type.", $"Controller handle was expected to be of type {nameof(FilterControllerHandle)}<{typeof(TRequest).Name}, {typeof(TValue).Name}>, but was of type {controllerHandle.GetType().Name}.");
        }
    }

    internal async Task NotifyStateChangedAsync(TyneKey key)
    {
        EnsureReadyToDispatchNotifications(key);

        _logger.LogFilterContextNotifyingControllersOfStateChange(key);

        // Don't fail if no controller handles are found, having none attached is valid
        if (!_controllerHandles.TryGetValue(key, out var controllerHandles))
            return;

        foreach (var controllerHandle in controllerHandles)
        {
            await controllerHandle.FilterControllerBase.OnStateChangedAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Configures <paramref name="request"/> using the attached values' <see cref="IFilter{TRequest}.ConfigureRequestAsync(TRequest)"/>.
    /// </summary>
    /// <param name="request">The <typeparamref name="TRequest"/> to configure.</param>
    /// <returns>A <see cref="Task"/> representing the request configuration.</returns>
    /// <exception cref="InvalidOperationException">When the task is not initialised yet, or if <see cref="IsFaulted"/> is <see langword="true"/>.</exception>
    public async Task ConfigureRequestAsync(TRequest request)
    {
        if (IsFaulted)
            throw new InvalidOperationException("Unable to configure request, context failed to initialise.");

        if (!IsInitialised)
            throw new InvalidOperationException("Request can only be configured once context is initialised.");

        _logger.LogFilterContextConfiguringRequest();

        foreach (var valueHandle in _valueHandles.Values)
        {
            var configureRequestTask = valueHandle.FilterInstance.ConfigureRequestAsync(request);
            if (configureRequestTask.IsCompletedSuccessfully)
                continue;

            await configureRequestTask.ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Triggers a reload of the data inside this context.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> representing the data reloading.
    ///     If a batch is active, this will instead queue a reload and
    ///     synchronously return <see cref="Task.CompletedTask"/>.
    /// </returns>
    /// <remarks>
    ///     If a batch update is in progress (i.e. this method is called during a call to <see cref="BatchUpdateValuesAsync(Func{Task})"/>),
    ///     then the reload will instead be queued to be executed once the batch is finished,
    ///     and this will synchronously return <see cref="Task.CompletedTask"/>.
    ///     It will not wait for the batch to flush and reload the data.
    /// </remarks>
    public Task ReloadDataAsync()
    {
        // If no batch is active then reload the data.
        if (CurrentBatchUpdate is null)
        {
            _logger.LogFilterContextReloadingData();
            return _reloadData.Invoke();
        }

        // If a batch is active then queue a data reload up and return a completed task.
        _logger.LogFilterContextQueueingReloadingDataForBatch();
        CurrentBatchUpdate.IsReloadDataQueued = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public IFilterValueHandle<TValue> AttachValue<TValue>(TyneKey key, IFilterValue<TRequest, TValue> filter)
    {
        if (IsInitialisationStarted)
            throw new InvalidOperationException("Filter values can only be registered prior to context initialisation.");

        KeyEmptyException.ThrowIfEmpty(key, exceptionMessage: "Cannot attach a value for an empty key.");

        if (_valueHandles.TryGetValue(key, out var existingHandle))
        {
            // We allow overwriting values if the runtime is hot reloading
            // to account for values being disposed of and re-created after
            // the context has already started.
            if (_allowValueOverwriting)
            {
                // If allowing the overwrite, then remove and detach the old handle
                _valueHandles.Remove(key);
                existingHandle.Dispose();
            }
            else
            {
                // If not allowing the overwrite, then throw an exception
                throw new ArgumentException($"Context already has a filter attached for key '{key}'.");
            }
        }

        _logger.LogFilterContextAttachingFilterValue(key, typeof(TValue));
        var valueHandle = new FilterValueHandle<TRequest, TValue>(this, key, filter);
        _valueHandles.Add(key, valueHandle);
        return valueHandle;
    }

    /// <summary>
    ///     Detaches a filter value from the context via it's <paramref name="valueHandle"/>.
    /// </summary>
    /// <typeparam name="TValue">The type the filter value manages.</typeparam>
    /// <param name="valueHandle">The value's handle.</param>
    internal void DetachFilterValue<TValue>(FilterValueHandle<TRequest, TValue> valueHandle)
    {
        var key = valueHandle.Key;
        if (!_valueHandles.TryGetValue(key, out var existingValueHandle))
        {
            _logger.LogFilterContextCantDetachFilterValue(key);
            return;
        }

        Debug.Assert(existingValueHandle == valueHandle, $"Attached value for key '{key}' differs from the handle trying to detach it.");

        _logger.LogFilterContextDetachingFilterValue(key, typeof(TValue));
        _valueHandles.Remove(key);
    }

    /// <inheritdoc/>
    public IFilterControllerHandle<TValue> AttachController<TValue>(TyneKey key, IFilterController<TValue> controller)
    {
        ArgumentNullException.ThrowIfNull(controller);
        KeyEmptyException.ThrowIfEmpty(key, exceptionMessage: "Cannot attach a controller for an empty key.");

        if (!_valueHandles.TryGetValue(key, out var valueHandle))
            throw new ArgumentException($"No value attached for key '{key}'. Are you missing an '{nameof(IFilterValue<object>)}'?");

        _logger.LogFilterContextAttachingFilterController(key, controller.GetType());

        // We can't guarantee that e.g. a StringController tries to attach to an Int value,
        // so we need to type-check it first to make sure.
        if (valueHandle.FilterInstance is not IFilterValue<TRequest, TValue> filterValue)
        {
            var filterActualValueType = valueHandle.FilterInstance
                .GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == typeof(IFilterValue<,>))
                .Select(i => i.GetGenericArguments()[1])
                .FirstOrDefault()
                // We shouldn't have filter values registered which don't implement IFilterValue<,>
                ?? throw new ArgumentException($"Value attached for key '{key}' is not compatible with '{typeof(TValue)}'");

            // With Value Types, Interface<TValue?> becomes Interface<Nullable<T>>, which will fail a type-test against Interface<T>.
            // So we check if our TValue and IFilterValue's TValue are similar, but one is a Nullable<T> version of the other.
            if (IsNullableOf(typeof(TValue), filterActualValueType) || IsNullableOf(filterActualValueType, typeof(TValue)))
                throw new ArgumentException($"Value attached for key '{key}' is '{filterActualValueType}', but the controller is '{typeof(TValue)}', which is incompatible. Ensure nullable annotations match.");

            throw new ArgumentException($"Value attached for key '{key}' is '{filterActualValueType}', but the controller is '{typeof(TValue)}', which is incompatible.");
        }

        // Create a new set of handles if one hasn't already been attached
        if (!_controllerHandles.TryGetValue(key, out var controllerHandles))
            controllerHandles = _controllerHandles[key] = new();

        var controllerHandle = new FilterControllerHandle<TRequest, TValue>(this, key, filterValue, controller);
        controllerHandles.Add(controllerHandle);
        return controllerHandle;
    }

    private static bool IsNullableOf(Type maybeNullableType, Type maybeValueType)
    {
        if (!maybeNullableType.IsValueType)
            return false;

        if (!maybeNullableType.IsGenericType || maybeNullableType.GetGenericTypeDefinition() != typeof(Nullable<>))
            return false;

        if (Nullable.GetUnderlyingType(maybeNullableType) != maybeValueType)
            return false;

        return true;
    }

    /// <summary>
    ///     Detaches a filter controller from the context via it's <paramref name="controllerHandle"/>.
    /// </summary>
    /// <typeparam name="TValue">The type the filter value manages.</typeparam>
    /// <param name="controllerHandle">The controller's handle.</param>
    internal void DetachFilterController<TValue>(FilterControllerHandle<TRequest, TValue> controllerHandle)
    {
        var key = controllerHandle.Key;
        if (!_controllerHandles.TryGetValue(key, out var controllerHandles))
        {
            _logger.LogFilterContextCantDetachFilterController(key);
            return;
        }

        _logger.LogFilterContextDetachingFilterController(key, controllerHandle.FilterController.GetType());
        var didRemoveHandle = controllerHandles.Remove(controllerHandle);
        Debug.Assert(didRemoveHandle, "Tried to detach unattached controller Handle.");

        // Clear out emptied handle sets
        if (controllerHandles.Count == 0)
            _controllerHandles.Remove(key);
    }

    /// <summary>
    ///     The currently active batch update, or <see langword="null"/> if one is not active.
    /// </summary>
    internal FilterContextBatchUpdateQueue<TRequest>? CurrentBatchUpdate { get; private set; }

    /// <inheritdoc/>
    public async Task BatchUpdateValuesAsync(Func<Task> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        if (CurrentBatchUpdate is not null)
            throw new InvalidOperationException("Context only supports one active batch at a time.");

        using var loggerScope = _logger.BeginScope("Context batch update");
        _logger.LogFilterContextBatchUpdateValuesStart();

        var currentBatchUpdate = CurrentBatchUpdate = new(this);
        try
        {
            await func().ConfigureAwait(false);
        }
        finally
        {
            CurrentBatchUpdate = null;
        }

        using (var flushLogScope = _logger.BeginScope("Flushing context batch update"))
        {
            _logger.LogFilterContextBatchUpdateValuesFlush();
            await currentBatchUpdate.FlushAsync().ConfigureAwait(false);
        }

        _logger.LogFilterContextBatchUpdateValuesFinished();
    }

    private Task OnHotReloadClearCache()
    {
        _logger.LogFilterContextHotReloadResetState();

        // Reset the init task so values and controllers may re-attach
        _initTask = null;

        // Allow values to be overwritten with hot-reloaded versions
        _allowValueOverwriting = true;

        // Handles aren't disposed as not all children may have updated,
        // in which case they won't re-register themselves.

        return Task.CompletedTask;
    }

    private async Task OnHotReloadUpdateApplication()
    {
        using var logScope = _logger.BeginScope("Hot reload re-initialisation");
        _logger.LogFilterContextHotReloadReinitialise();

        // Components should have re-initialised by now, lock overwriting again
        _allowValueOverwriting = false;

        // Re-run initialisation
        await InitialiseAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Disposes of the context.
    /// </summary>
    public void Dispose()
    {
        // Remove hot reload event handlers
        TyneHotReloadWatcher.OnClearCache -= OnHotReloadClearCache;
        TyneHotReloadWatcher.OnUpdateApplication -= OnHotReloadUpdateApplication;
    }
}
