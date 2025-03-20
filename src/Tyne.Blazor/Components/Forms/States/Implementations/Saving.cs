namespace Tyne.Blazor.Forms.StateMachine;

internal class TyneFormStateSaving<TParams, TModel, TError> :
    TyneFormStateBase<TParams, TModel, TError>,
    TyneFormState.Saving<TParams, TModel, TError>
{
    public required TParams Params { get; internal init; }
    public required TModel Model { get; internal init; }

    // Does not contain a value while saving
    public TError? Error { get; }

    // Ignore any errors occuring during saving, they'll be reset when we transition to Active
    public Task UpdateErrorAsync(TError? error) => Task.CompletedTask;
}
