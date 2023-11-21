using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     An <see cref="IFilterSelectItem{TValue}"/> component
///     whose value and content are provided through parameters.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
/// <remarks>
///     This must be used inside of a <see cref="IFilterSelectValueContext{TValue}"/>,
///     such as <see cref="TyneFilterSelectSingleValue{TRequest, TValue}"/>
///     or <see cref="TyneFilterSelectMultiValue{TRequest, TValue}"/>.
/// </remarks>
public sealed class TyneFilterSelectItem<TValue> : ComponentBase, IFilterSelectItem<TValue, RenderFragment>, IDisposable
{
    /// <summary>
    ///     The <typeparamref name="TValue"/> of this item.
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Value { get; set; }

    [Parameter]
    public string? AsString { get; set; }

    /// <summary>
    ///     Renders <see cref="Value"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    RenderFragment IFilterSelectItem<TValue, RenderFragment>.Metadata =>
        ChildContent ?? EmptyRenderFragment.Instance;

    [CascadingParameter]
    private IFilterSelectValueContext<TValue> Context { get; init; } = null!;

    private ISelectValueItemHandle? _handle;

    /// <summary>
    ///     Attaches this item to the <see cref="IFilterSelectValueContext{TValue}"/>.
    /// </summary>
    protected override void OnInitialized()
    {
        _handle = Context.Attach(this);
        base.OnInitialized();
    }

    /// <summary>
    ///     Disposes of this item, detaching it from the value context.
    /// </summary>
    public void Dispose()
    {
        _handle?.Dispose();
        _handle = null;
    }
}
