namespace Tyne.Blazor;

public interface ITyneFormSaveContext
{
    public bool CanSave { get; }
    public Task SaveAsync();

    public void Deconstruct(out bool canSave, out SaveForm save)
    {
        canSave = CanSave;
        save = SaveAsync;
    }
}
