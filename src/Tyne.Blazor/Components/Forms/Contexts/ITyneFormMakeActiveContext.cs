namespace Tyne.Blazor;

public interface ITyneFormMakeActiveContext<TModel, TError>
{
    public bool CanMakeActive { get; }
    public Task MakeActiveAsync(MakeFormActive<TModel, TError> makeFormActive);

    public void Deconstruct(out bool canMakeActive, out Func<MakeFormActive<TModel, TError>, Task> makeActive)
    {
        canMakeActive = CanMakeActive;
        makeActive = MakeActiveAsync;
    }
}
