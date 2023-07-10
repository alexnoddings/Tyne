namespace Tyne.Blazor;

public interface ITyneTableFilterWrapper<TValue>
{
    public TValue? Value { get; }

    public Task SetValueAsync(TValue? newValue);
    public Task SetValueAsync(TValue? newValue, CancellationToken cancellationToken);
}
