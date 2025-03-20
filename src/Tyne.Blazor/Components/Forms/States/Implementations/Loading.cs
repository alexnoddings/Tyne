namespace Tyne.Blazor.Forms.StateMachine;

internal class TyneFormStateLoading<TParams, TModel, TError> :
    TyneFormStateBase<TParams, TModel, TError>,
    TyneFormState.Loading<TParams>
{
    public required TParams Params { get; internal init; }

    public Task MakeInactiveAsync() =>
        StateMachine.TransitionToInactiveAsync();
}
