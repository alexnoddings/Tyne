namespace Tyne.Blazor.Forms.StateMachine;

internal abstract class TyneFormStateBase<TParams, TModel, TError>
    : ITyneFormState
{
    protected internal required FormStateMachine<TParams, TModel, TError> StateMachine { get; init; }
}
