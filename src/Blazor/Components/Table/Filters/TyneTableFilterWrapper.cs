namespace Tyne.Blazor;

internal sealed class TyneTableFilterWrapper<TValue> : ITyneTableFilterWrapper<TValue>
{
    private readonly Func<TValue?> _valueGetter;
    public TValue? Value => _valueGetter.Invoke();

    private readonly Func<TValue?, CancellationToken, Task> _valueSetter;
    public Task SetValueAsync(TValue? newValue) =>
        SetValueAsync(newValue, default);

    public Task SetValueAsync(TValue? newValue, CancellationToken cancellationToken) =>
        _valueSetter.Invoke(newValue, cancellationToken);

    public TyneTableFilterWrapper(Func<TValue?> valueGetter, Func<TValue?, CancellationToken, Task> valueSetter)
    {
        _valueGetter = valueGetter ?? throw new ArgumentNullException(nameof(valueGetter));
        _valueSetter = valueSetter ?? throw new ArgumentNullException(nameof(valueSetter));
    }
}
