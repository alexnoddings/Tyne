namespace Tyne.Blazor;

public interface ITyneForm
{
    public ITyneFormState State { get; }
    public IDisposable WatchForStateChanges(TyneFormStateChanged stateChangedCallback);
}

public delegate Task TyneLoadForm<in TInput>(TInput input);
