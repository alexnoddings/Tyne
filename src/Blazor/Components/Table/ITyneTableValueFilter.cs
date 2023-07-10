namespace Tyne.Blazor;

public interface ITyneTableValueFilter<TValue>
{
    public TValue? Value { get; }

    public Task<bool> SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default);
    public Task<bool> ClearValueAsync(CancellationToken cancellationToken = default);
}
