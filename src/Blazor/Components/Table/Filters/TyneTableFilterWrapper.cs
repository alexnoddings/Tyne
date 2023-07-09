namespace Tyne.Blazor;

internal sealed class TyneTableFilterWrapper<TValue> : ITyneTableFilterWrapper<TValue>
{
    private readonly Func<TValue?> _valueGetter;
    public TValue? Value => _valueGetter.Invoke();

    private readonly Func<TValue?, Task> _valueSetter;
    public Task UpdateValueAsync(TValue? newValue) =>
        _valueSetter.Invoke(newValue);

    public TyneTableFilterWrapper(Func<TValue?> valueGetter, Func<TValue?, Task> valueSetter)
    {
        _valueGetter = valueGetter ?? throw new ArgumentNullException(nameof(valueGetter));
        _valueSetter = valueSetter ?? throw new ArgumentNullException(nameof(valueSetter));
    }
}
