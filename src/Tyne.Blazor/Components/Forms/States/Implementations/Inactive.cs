namespace Tyne.Blazor.Forms.StateMachine;

internal class TyneFormStateInactive<TParams, TModel, TError> :
    TyneFormStateBase<TParams, TModel, TError>,
    TyneFormState.Inactive<TParams, TModel, TError>
{
    public required TParams Params { get; internal init; }

    public async Task MakeActiveAsync(MakeFormActive<TModel, TError> makeFormActive)
    {
        await StateMachine
            .PerformStateTransitionAsync(this, TransitionToActiveAsync)
            .ConfigureAwait(false);

        async Task TransitionToActiveAsync(ChangeFormState changeFormState, CancellationToken ct)
        {
            var loading = new TyneFormStateLoading<TParams, TModel, TError> { StateMachine = StateMachine, Params = Params };
            await changeFormState(loading).ConfigureAwait(false);

            Result<TModel, TError> loadingResult;
            try
            {
                loadingResult = await makeFormActive(ct).WaitAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Loading was cancelled
                // Don't bother doing anything as the cancellation thread will transition the state
                return;
            }
            // Intentionally don't catch any other exceptions here.
            // We don't know how to translate exceptions into user-domain TErrors.
            // Implementors should wrap their operations to prevent exceptions from bubbling up.

            var newState = loadingResult.Match(
                ok: ITyneFormState (model) =>
                    new TyneFormStateActive<TParams, TModel, TError> { StateMachine = StateMachine, Params = Params, Model = model },
                error: ITyneFormState (error) =>
                    new TyneFormStateLoadingError<TParams, TModel, TError> { StateMachine = StateMachine, Params = Params, Error = error }
            );
            await changeFormState(newState).ConfigureAwait(false);
        }
    }
}
