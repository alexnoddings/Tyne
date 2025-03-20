namespace Tyne.Blazor.Forms.StateMachine;

internal class TyneFormStateActive<TParams, TModel, TError> :
    TyneFormStateBase<TParams, TModel, TError>,
    TyneFormState.Active<TParams, TModel, TError>
{
    public required TParams Params { get; internal init; }
    public required TModel Model { get; internal init; }

    public TError? Error { get; private set; }

    public async Task UpdateErrorAsync(TError? error)
    {
        Error = error;
        await StateMachine.NotifyStateChangedAsync().ConfigureAwait(false);
    }

    public Task MakeInactiveAsync() =>
        StateMachine.TransitionToInactiveAsync();

    public async Task SaveAsync()
    {
        await StateMachine
            .PerformStateTransitionAsync(this, TransitionToSavingAsync)
            .ConfigureAwait(false);

        async Task TransitionToSavingAsync(ChangeFormState changeFormState, CancellationToken ct)
        {
            var saving = new TyneFormStateSaving<TParams, TModel, TError> { StateMachine = StateMachine, Params = Params, Model = Model };
            await changeFormState(saving).ConfigureAwait(false);

            Result<Unit, TError> savingResult;
            try
            {
                savingResult = await StateMachine.SaveModelAsync(Model).WaitAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Saving was cancelled
                // Don't bother doing anything as the cancellation thread will transition the state
                return;
            }
            // Intentionally don't catch any other exceptions here.
            // We don't know how to translate exceptions into user-domain TErrors.
            // Implementors should wrap their operations to prevent exceptions from bubbling up.

            var newState = savingResult.Match(
                ok: ITyneFormState (_) =>
                    new TyneFormStateActive<TParams, TModel, TError> { StateMachine = StateMachine, Params = Params, Model = Model },
                error: ITyneFormState (error) =>
                    new TyneFormStateActive<TParams, TModel, TError> { StateMachine = StateMachine, Params = Params, Model = Model, Error = error }
            );
            await changeFormState(newState).ConfigureAwait(false);
        }
    }
}
