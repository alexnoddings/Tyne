namespace Tyne.Blazor;

public abstract partial class TyneFilteredColumnBase<TRequest, TResponse, TValue> :
    TyneFilteredColumnBase<TRequest, TResponse>,
    ITyneTableValueFilter<TValue>
{
    public TValue? Value { get; private set; }

    public virtual async Task<bool> SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        if (EqualityComparer<TValue>.Default.Equals(newValue, Value))
            return false;

        Value = newValue;

        if (!isSilent)
            await OnUpdatedAsync(cancellationToken).ConfigureAwait(true);

        return true;
    }

    public override Task<bool> ClearValueAsync(CancellationToken cancellationToken = default) =>
        SetValueAsync(default, false, cancellationToken);
}
