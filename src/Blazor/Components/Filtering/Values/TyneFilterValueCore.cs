using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Tyne.Blazor.Filtering.Context;
using Tyne.Blazor.Filtering.Controllers;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     The core of the <see cref="IFilterValue{TRequest, TValue}"/> implementation.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type which the value manages.</typeparam>
public abstract class TyneFilterValueCore<TRequest, TValue> : ComponentBase, IFilterValue<TRequest, TValue>, IDisposable
{
    /// <summary>
    ///     Behaviour to use when calling <see cref="SetValueAsync(TValue?, SetValueBehaviour)"/>.
    /// </summary>
    /// <remarks>
    ///     These are flags - they should be combined as appropriate for the desired behaviour.
    /// </remarks>
    [Flags]
    protected enum SetValueBehaviour
    {
        /// <summary>
        ///     Does nothing.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Sets the <see cref="Value"/>.
        /// </summary>
        SetValue = 1 << 0,

        /// <summary>
        ///     Notifies the <see cref="Context"/> that this value has updated.
        /// </summary>
        NotifyContext = 1 << 1,

        /// <summary>
        ///     Notifies the <see cref="Context"/> that the data being filtered should be reloaded.
        /// </summary>
        ReloadData = 1 << 2,

        /// <summary>
        ///     Persists the new value.
        /// </summary>
        Persist = 1 << 3,

        /// <summary>
        ///     The default behaviour.
        /// </summary>
        /// <remarks>
        ///     This will:
        ///     <list type="bullet">
        ///         <item><see cref="SetValue"/></item>
        ///         <item><see cref="NotifyContext"/></item>
        ///         <item><see cref="ReloadData"/></item>
        ///         <item><see cref="Persist"/></item>
        ///     </list>
        /// </remarks>
        Default = SetValue | NotifyContext | ReloadData | Persist
    }

    /// <inheritdoc/>
    public TValue? Value { get; private set; }

    /// <summary>
    ///     The <see cref="TyneKey"/> to attach to the <see cref="IFilterContext{TRequest}"/> with.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This cannot be <see cref="TyneKey.Empty"/>, or it can't be attached to the <see cref="Context"/>.
    ///     </para>
    ///     <para>
    ///         This should NOT change for the lifetime of the value.
    ///     </para>
    /// </remarks>
    protected abstract TyneKey ForKey { get; }

    /// <summary>
    ///     The <see cref="TyneKey"/> to persist the <see cref="Value"/>
    ///     with using <see cref="TryPersistValueAsync(TValue)"/>,
    ///     and load with <see cref="TryInitialiseValueFromPersistedAsync()"/>.
    /// </summary>
    /// <remarks>
    ///     Using <see cref="TyneKey.Empty"/> will disable persistence completely.
    /// </remarks>
    protected abstract TyneKey PersistKey { get; }

    /// <summary>
    ///     The default <typeparamref name="TValue"/> to use if one was not loaded from persistence by
    ///     <see cref="TryInitialiseValueFromDefaultAsync()"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This value will not be automatically persisted.
    ///     </para>
    ///     <para>
    ///         By default, this value is only used during initialisation.
    ///         Clearing the filter value will reset <see cref="Value"/> to <see langword="default"/>.
    ///         This may be overridden with <see cref="ClearValueAsync()"/>.
    ///     </para>
    /// </remarks>
    protected abstract TValue? DefaultValue { get; }

    /// <summary>
    ///     The cascading <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    [CascadingParameter]
    protected IFilterContext<TRequest> Context { get; init; } = null!;

    private IFilterValueHandle<TValue>? _handle;
    /// <summary>
    ///     The handle from attaching to the <see cref="Context"/>.
    /// </summary>
    protected IFilterValueHandle<TValue> Handle
    {
        get
        {
            // We only account for the handle being null after being disposed.
            // The handle is attached during OnInitialized, which is the first
            // method executed, so we don't expect any callers to access it before then.
            if (_handle is null)
                throw new ObjectDisposedException(nameof(TyneFilterValueBase<TRequest, TValue>), "Cannot access handle after value has been disposed.");

            return _handle;
        }
    }

    [Inject]
    private ILoggerFactory LoggerFactory { get; init; } = null!;
    private ILogger? _logger;

    /// <summary>
    ///     An <see cref="ILogger"/> instance named after the current type.
    /// </summary>
    protected ILogger Logger => _logger ??= LoggerFactory.CreateLogger(GetType());

    /// <summary>
    ///     Attaches this value to the <see cref="Context"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">When <see cref="ForKey"/> is empty.</exception>
    protected override void OnInitialized()
    {
        // ComponentBase runs sync versions before async versions,
        // so this should always be the first method called
        var forKey = ForKey;
        if (forKey.IsEmpty)
            throw new KeyEmptyException($"Value can't be attached to empty {nameof(ForKey)}. Are you missing a For property?");

        if (Context is null)
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {nameof(IFilterContext<object>)}<{typeof(TRequest).Name}>.");

        // Always attach a handle, regardless of if we could generate a setter.
        // We won't be able to configure the request later, but it will stop as many
        // errors where controllers try to attach to invalid values.
        _handle = Context.AttachValue(forKey, this);
    }

    // Used to check if init has started, and to await it completing
    private Task? _initTask;

    /// <summary>
    ///     Initialises the value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the value initialising.</returns>
    /// <remarks>
    ///     This is <see langword="sealed"/> as it handles caching the initialisation <see cref="Task"/>.
    ///     See <see cref="InitialiseAsync"/> for what initialisation takes place.
    /// </remarks>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The exception is rethrown when the returned Task is awaited.")]
    protected override sealed Task OnInitializedAsync()
    {
        // A lot of logic relies on checking _initTask,
        // but if InitialiseAsync throws synchronously
        // then _initTask will never be set.
        // So we catch synchronous exceptions
        // to set them as the initTask.
        // Note the exception isn't immediately rethrown
        // as we return the _initTask and leave awaiting
        // the exception to the caller.
        try
        {
            _initTask = InitialiseAsync();
        }
        catch (Exception exception)
        {
            _initTask = Task.FromException(exception);
        }

        return _initTask;
    }

    /// <summary>
    ///     Ensures the value has been initialised, and notifies the context
    ///     that we have set our initial <typeparamref name="TValue"/>.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> representing the initialisation.
    /// </returns>
    /// <remarks>
    ///     This may only be called after <see cref="OnInitializedAsync"/> has been called by Blazor.
    /// </remarks>
    /// <exception cref="InvalidOperationException">When called before initialisation has begun.</exception>
    public async Task EnsureInitialisedAsync()
    {
        if (_initTask is null)
            throw new InvalidOperationException();

        await _initTask.ConfigureAwait(false);
        // If initialisation yields (e.g. loading values from an API), then controllers will have
        // been initialised, so we need to notify them that we now have a value available.
        // This is done here rather when the value is first loaded as controllers
        // may not be initialised by the time this method is called.
        await Handle.NotifyValueUpdatedAsync(Value).ConfigureAwait(false);
    }

    /// <summary>
    ///     Initialises the value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the value initialising.</returns>
    /// <remarks>
    ///     The base implementation handles value initialisation from persistence or <see cref="DefaultValue"/> as a fallback.
    /// </remarks>
    protected virtual async Task InitialiseAsync()
    {
        var didInitialiseFromPersisted = await TryInitialiseValueFromPersistedAsync().ConfigureAwait(false);
        if (!didInitialiseFromPersisted)
            await TryInitialiseValueFromDefaultAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Tries to initialise <see cref="Value"/> with a default value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the value being initialised.</returns>
    /// <remarks>
    ///     <para>
    ///         This is normally called during <see cref="InitialiseAsync"/> if no value was loaded by <see cref="TryInitialiseValueFromPersistedAsync()"/>.
    ///     </para>
    ///     <para>
    ///         The base implementation sets <see cref="Value"/> to be <see cref="DefaultValue"/>.
    ///     </para>
    /// </remarks>
    protected virtual Task TryInitialiseValueFromDefaultAsync()
    {
        var value = DefaultValue;
        return SetValueAsync(value, SetValueBehaviour.SetValue);
    }

    /// <summary>
    ///     Tries to initialise <see cref="Value"/> from a persisted <typeparamref name="TValue"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the value being initialised.</returns>
    /// <remarks>
    ///     <para>
    ///         This is normally called during <see cref="InitialiseAsync"/>.
    ///     </para>
    /// </remarks>
    protected virtual async Task<bool> TryInitialiseValueFromPersistedAsync()
    {
        var persistAs = PersistKey;
        if (persistAs.IsEmpty)
            return false;

        var valueOption = Context.Persistence.GetValue<TValue>(persistAs.Key);
        if (!valueOption.HasValue)
            return false;

        var value = valueOption.Value;
        await SetValueAsync(value, SetValueBehaviour.SetValue).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Tries to persist <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <typeparamref name="TValue"/> to persist.</param>
    /// <returns>A <see cref="Task"/> representing the value being persisted.</returns>
    protected virtual Task TryPersistValueAsync(TValue? value)
    {
        var persistAs = PersistKey;
        if (!persistAs.IsEmpty)
            Context.Persistence.SetValue(persistAs.Key, value);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task SetValueAsync(TValue? newValue) =>
        SetValueAsync(newValue, SetValueBehaviour.Default);

    /// <inheritdoc/>
    public virtual Task ClearValueAsync() =>
        SetValueAsync(default, SetValueBehaviour.Default);

    /// <summary>
    ///     Sets <see cref="Value"/> to <paramref name="newValue"/> according to <paramref name="behaviour"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the value being set.</returns>
    /// <remarks>
    ///     This will notify all controllers attached to the same <see cref="ForKey"/> on <see cref="Context"/>
    ///     via <see cref="IFilterController{TValue}.OnValueUpdatedAsync(TValue)"/>.
    /// </remarks>
    protected virtual async Task SetValueAsync(TValue? newValue, SetValueBehaviour behaviour)
    {
        var handle = Handle;

        if ((behaviour & SetValueBehaviour.SetValue) != 0)
            Value = newValue;

        if ((behaviour & SetValueBehaviour.Persist) != 0)
            await TryPersistValueAsync(newValue).ConfigureAwait(false);

        if ((behaviour & SetValueBehaviour.NotifyContext) != 0)
            await handle.NotifyValueUpdatedAsync(newValue).ConfigureAwait(false);

        if ((behaviour & SetValueBehaviour.ReloadData) != 0)
            await handle.ReloadDataAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Configures <paramref name="request"/> by setting the property
    ///     matching <see cref="ForKey"/> to <see cref="Value"/>.
    /// </summary>
    /// <param name="request">The <typeparamref name="TRequest"/> to configure.</param>
    /// <returns>A <see cref="ValueTask"/> representing the <paramref name="request"/> being configured.</returns>
    public abstract ValueTask ConfigureRequestAsync(TRequest request);

    /// <summary>
    ///     Disposes of the value, detaching it from the <see cref="Context"/>.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        _handle?.Dispose();
        _handle = null;
    }

    /// <summary>
    ///     Disposes of the value, detaching it from the <see cref="IFilterContext{TRequest}"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
