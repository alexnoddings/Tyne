using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A simple implementation of <see cref="IFilterSelectItem<TValue>"/>
///     which provides a value and content through the constructor.
///     This allows it to be created directly through code,
///     rather than with a component.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
public sealed class FilterSelectItem<TValue> : IFilterSelectItem<TValue>
{
    /// <inheritdoc/>
    public TValue? Value { get; }

    /// <summary>
    ///     Renders <see cref="Value"/>.
    /// </summary>
    public RenderFragment Content { get; }

    /// <summary>
    ///     Constructs a new <see cref="FilterSelectItem{TValue}"/> for the <paramref name="value"/> and <paramref name="content"/>.
    /// </summary>
    /// <param name="value">The <see cref="Value"/>.</param>
    /// <param name="content">The <see cref="Content"/>.</param>
    public FilterSelectItem(TValue? value, RenderFragment content)
    {
        Value = value;
        Content = content;
    }
}
