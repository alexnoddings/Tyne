namespace Tyne.Blazor.Forms.StateMachine;

internal class TyneFormStateUninitialised<TParams, TModel, TError> :
    TyneFormStateBase<TParams, TModel, TError>,
    TyneFormState.Uninitialised<TParams, TError>
{
    public async Task InitialiseAsync(InitialiseForm<TParams, TError> initialiseForm)
    {
        await StateMachine
            .PerformStateTransitionAsync(this, TransitionToInitialisedAsync)
            .ConfigureAwait(false);

        async Task TransitionToInitialisedAsync(ChangeFormState changeFormState, CancellationToken ct)
        {
            var initialising = new TyneFormStateInitialising<TParams, TModel, TError>
            {
                StateMachine = StateMachine
            };
            await changeFormState(initialising).ConfigureAwait(false);

            Result<TParams, TError> initResult;
            try
            {
                initResult = await initialiseForm().ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Initialisation was cancelled
                // Don't bother doing anything as the cancellation thread will transition the state
                return;
            }
            // Intentionally don't catch any other exceptions here.
            // We don't know how to translate exceptions into user-domain TErrors.
            // Implementors should wrap their operations to prevent exceptions from bubbling up.

            var newState = initResult.Match(
                ok: ITyneFormState (initParams) => new TyneFormStateInactive<TParams, TModel, TError> { StateMachine = StateMachine, Params = initParams },
                error: ITyneFormState (error) => new TyneFormStateInitialisationError<TParams, TModel, TError> { StateMachine = StateMachine, Error = error }
            );
            await changeFormState(newState).ConfigureAwait(false);
        }
    }
}
