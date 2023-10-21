using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     Represents a select-able <see cref="Value"/> which can be rendered with <see cref="Content"/>.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
public interface IFilterSelectItem<out TValue>
{
    /// <summary>
    ///     The <typeparamref name="TValue"/> this item represents.
    /// </summary>
    public TValue? Value { get; }

    /// <summary>
    ///     Renders the <see cref="Value"/>.
    /// </summary>
    public RenderFragment Content { get; }
}
