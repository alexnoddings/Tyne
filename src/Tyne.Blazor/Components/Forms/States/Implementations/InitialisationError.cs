namespace Tyne.Blazor.Forms.StateMachine;

internal class TyneFormStateInitialisationError<TParams, TModel, TError> :
    TyneFormStateBase<TParams, TModel, TError>,
    TyneFormState.InitialisationError<TError>
{
    public required TError Error { get; internal init; }
}
