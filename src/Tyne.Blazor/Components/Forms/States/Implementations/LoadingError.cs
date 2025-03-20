namespace Tyne.Blazor.Forms.StateMachine;

internal class TyneFormStateLoadingError<TParams, TModel, TError> :
    TyneFormStateBase<TParams, TModel, TError>,
    TyneFormState.LoadingError<TParams, TError>
{
    public required TParams Params { get; internal init; }
    public required TError Error { get; internal init; }

    public Task MakeInactiveAsync() =>
        StateMachine.TransitionToInactiveAsync();
}
