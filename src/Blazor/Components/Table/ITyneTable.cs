namespace Tyne.Blazor;

public interface ITyneTable
{
    public Task ReloadServerDataAsync(CancellationToken cancellationToken = default);
}
