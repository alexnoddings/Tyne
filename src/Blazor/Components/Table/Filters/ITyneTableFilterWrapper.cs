namespace Tyne.Blazor;

public interface ITyneTableFilterWrapper<TValue>
{
    public TValue? Value { get; }
    public Task UpdateValueAsync(TValue? newValue);
}
