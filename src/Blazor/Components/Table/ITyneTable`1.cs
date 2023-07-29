namespace Tyne.Blazor;

public interface ITyneTable<out TRequest> : ITyneTable
{
    public IDisposable RegisterFilter(ITyneTableRequestFilter<TRequest> filter);
    public Task NotifySyncedFilterChangedAsync<TValue>(ITyneTableSyncedFilter<TValue> filterInstance, TValue? newValue, CancellationToken cancellationToken = default);
}
