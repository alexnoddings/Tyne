namespace Tyne.Blazor;

public abstract partial class TyneFilteredColumnBase<TRequest, TResponse, TValue> :
    TyneFilteredColumnBase<TRequest, TResponse>,
    ITyneTableValueFilter<TValue>
{
    public TValue? Value { get; protected set; }

    public virtual async Task SetValueAsync(TValue? newValue, bool isSilent, CancellationToken cancellationToken = default)
    {
        if (EqualityComparer<TValue>.Default.Equals(newValue, Value))
            return;

        Value = newValue;

        if (!isSilent)
            await OnUpdatedAsync(cancellationToken).ConfigureAwait(true);
    }

    public override Task ClearValueAsync(CancellationToken cancellationToken = default) =>
        SetValueAsync(default, false, cancellationToken);
}
