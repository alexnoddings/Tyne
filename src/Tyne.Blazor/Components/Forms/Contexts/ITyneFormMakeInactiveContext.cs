namespace Tyne.Blazor;

public interface ITyneFormMakeInactiveContext
{
    public bool CanMakeInactive { get; }
    public Task MakeInactiveAsync();

    public void Deconstruct(out bool canMakeInactive, out MakeFormInactive makeInactive)
    {
        canMakeInactive = CanMakeInactive;
        makeInactive = MakeInactiveAsync;
    }
}
