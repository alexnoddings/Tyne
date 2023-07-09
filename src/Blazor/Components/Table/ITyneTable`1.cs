namespace Tyne.Blazor;

public interface ITyneTable<out TRequest> : ITyneTable
{
    public IDisposable RegisterFilter(ITyneTableRequestFilter<TRequest> filter);
}
