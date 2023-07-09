namespace Tyne.Blazor;

public interface ITyneTableValueFilter<TValue>
{
    public TValue? Value { get; }
    public Task SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default);
    public Task ClearValueAsync(CancellationToken cancellationToken = default);
}
