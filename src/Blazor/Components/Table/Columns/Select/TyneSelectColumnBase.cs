using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace Tyne.Blazor;

public abstract class TyneSelectColumnBase<TRequest, TResponse, TValue> : TyneFilteredColumnBase<TRequest, TResponse>, ITyneSelectColumn<TValue>
{
    private readonly HashSet<TyneSelectValue<TValue?>> _registeredValues = new();
    protected IEnumerable<TyneSelectValue<TValue?>> RegisteredValues => _registeredValues.AsEnumerable();

    [Parameter, EditorRequired]
    public RenderFragment Values { get; set; } = EmptyRenderFragment.Instance;

    [Parameter]
    public string? ContentWidth { get; set; }

    protected string ContentWidthStyle =>
        new StyleBuilder()
        .AddStyle("width", ContentWidth, !string.IsNullOrWhiteSpace(ContentWidth))
        .Build();

    public IDisposable RegisterValue(TyneSelectValue<TValue?> value)
    {
        if (_registeredValues.Contains(value))
            throw new ArgumentException($"{nameof(TyneSelectValue<TValue>)} is already registered.");

        _registeredValues.Add(value);
        return new DisposableAction(() => _registeredValues.Remove(value));
    }
}
