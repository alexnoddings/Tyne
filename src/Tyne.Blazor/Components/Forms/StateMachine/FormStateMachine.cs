using System.Diagnostics;

namespace Tyne.Blazor.Forms.StateMachine;

internal sealed class FormStateMachine<TParams, TModel, TError> : IDisposable
{
    internal ITyneFormState State { get; private set; }

    private readonly SemaphoreSlim _stateTransitionSemaphore = new(1, 1);
    private readonly SemaphoreSlim _inactiveTransitionSemaphore = new(1, 1);
    private CancellationTokenSource _inactiveCts = new();

    private readonly SaveFormModel<TModel, TError> _saveFormModel;
    public Task<Result<Unit, TError>> SaveModelAsync(TModel model) => _saveFormModel(model);

    public FormStateMachine(SaveFormModel<TModel, TError> saveFormModel)
    {
        _saveFormModel = saveFormModel;

        State = new TyneFormStateUninitialised<TParams, TModel, TError> { StateMachine = this };
    }

    public async Task PerformStateTransitionAsync(ITyneFormState oldState, Func<ChangeFormState, CancellationToken, Task> transitionState)
    {
        if (oldState != State)
        {
            Debug.Fail("Invalid state transition. Was an old state captured?");
            return;
        }

        var inactiveToken = _inactiveCts.Token;
        try
        {
            await _stateTransitionSemaphore.WaitAsync(inactiveToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Request to transition was cancelled by MakeInactive
            return;
        }

        try
        {
            // State was transitioned while we waited
            // Gracefully return as this is not exceptional
            if (oldState != State)
                return;

            await transitionState(ChangeStateAsync, inactiveToken).ConfigureAwait(false);
        }
        finally
        {
            _stateTransitionSemaphore.Release();
        }
    }

    public async Task TransitionToInactiveAsync()
    {
        // Non-initialised states don't support transitioning to inactive.
        // The initialising state handles transitioning to inactive itself.
        if (State is not TyneFormState.Has.Initialised<TParams> initialised)
            return;

        // Already inactive, exit
        if (State is TyneFormState.Inactive)
            return;

        // We explicitly don't observe the inactive cancellation token.
        // If we did, it would cancel and cause us to return before the transition to inactive was complete
        // since it's cancelled early to give other transitions more time to bail
        await _inactiveTransitionSemaphore.WaitAsync().ConfigureAwait(false);

        // State transitioned to inactive while we waited, nothing for us to do now
        if (State is TyneFormState.Inactive)
        {
            _inactiveTransitionSemaphore.Release();
            return;
        }

        try
        {
            // Cancel any active state transitions
            // This must come BEFORE entering the state transition critical region,
            // otherwise we'll just sit and wait for the current transition to end.
            // Note that this token isn't always observed.
            // For example, an end developer may choose to not observe it during loading.
            // It's also not passed down to save methods to ensure they complete fully,
            // though the state machine will still cancel awaiting the save operation to transition into cancelled.
            await _inactiveCts.CancelAsync().ConfigureAwait(false);

            // Acquire the state transition semaphore.
            // We don't need it, but we acquire it to prevent any other callers from
            // entering the critical region while we're making inactive.
            // This should free up quickly as any other transitions should get cancelled.
            await _stateTransitionSemaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                // We explicitly only dispose/recycle the token source AFTER we've acquired the state transition semaphore.
                // Otherwise, another thread could enter the critical state transition region during disposal,
                // and block our acquisition of the semaphore above
                _inactiveCts.Dispose();
                _inactiveCts = new();

                var inactive = new TyneFormStateInactive<TParams, TModel, TError> { StateMachine = this, Params = initialised.Params };
                await ChangeStateAsync(inactive).ConfigureAwait(false);
            }
            finally
            {
                _stateTransitionSemaphore.Release();
            }
        }
        finally
        {
            _inactiveTransitionSemaphore.Release();
        }
    }

    private Task ChangeStateAsync(ITyneFormState newState)
    {
        State = newState;
        return NotifyStateChangedAsync();
    }

    internal async Task NotifyStateChangedAsync()
    {
        foreach (var stateChangedCallback in StateChangedCallbacks)
            await stateChangedCallback.Invoke().ConfigureAwait(true);
    }

    private List<TyneFormStateChanged> StateChangedCallbacks { get; } = [];

    public IDisposable WatchForChanges(TyneFormStateChanged stateChangedCallback)
    {
        StateChangedCallbacks.Add(stateChangedCallback);
        return new DisposableAction(() => StateChangedCallbacks.Remove(stateChangedCallback));
    }

    public void Dispose()
    {
        StateChangedCallbacks.Clear();
        _stateTransitionSemaphore.Dispose();
        _inactiveTransitionSemaphore.Dispose();
        _inactiveCts.Dispose();
    }
}
