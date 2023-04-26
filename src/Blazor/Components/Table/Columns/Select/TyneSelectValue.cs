using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public sealed class TyneSelectValue<TValue> : ComponentBase, IDisposable
{
    [Parameter]
    public TValue? Value { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = EmptyRenderFragment.Instance;

    [Parameter]
    public string? AsString { get; set; }

    [CascadingParameter]
    private ITyneSelectColumn<TValue> Column { get; init; } = null!;

    private IDisposable? _registration;

    protected override void OnInitialized()
    {
        if (Column is null)
        {
            // For value types, ITyneSelectColumn<T?> becomes ITyneSelectColumn<Nullable<T>>, which is a different type to ITyneSelectColumn<T>.
            if (typeof(TValue).IsValueType)
            {
                throw new InvalidOperationException(
                    $"{nameof(TyneSelectValue<TValue>)} requires a cascading parameter of type {nameof(ITyneSelectColumn<TValue>)}<{typeof(TValue).Name}>." +
                    $" You may need to use `{nameof(TValue)}=\"{typeof(TValue).Name}?\"` for the parameter to cascade properly." +
                    " Are you trying to create a value outside of a Tyne select column?"
                );
            }

            // For non-value types, ITyneSelectColumn<T?> becomes ITyneSelectColumn<[Nullable] T>, which is the same type as ITyneSelectColumn<T>
            throw new InvalidOperationException(
                $"{nameof(TyneSelectValue<TValue>)} requires a cascading parameter of type {nameof(ITyneSelectColumn<TValue>)}<{typeof(TValue).Name}>." +
                " Are you trying to create a value outside of a Tyne select column?"
            );
        }

        _registration = Column.RegisterValue(this);
    }

    public void Dispose()
    {
        _registration?.Dispose();
        _registration = null;
    }

    public override string ToString()
    {
        // It's valid for StringDisplay to be empty here (e.g. for a null value)
        if (AsString is not null)
            return AsString;

        if (Value is not null)
            return Value.ToString() ?? string.Empty;

        return string.Empty;
    }
}
