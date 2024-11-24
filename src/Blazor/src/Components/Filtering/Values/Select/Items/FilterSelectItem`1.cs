namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A simple implementation of <see cref="IFilterSelectItem{TValue}"/>
///     which provides a value and content through the constructor.
///     This allows it to be created directly through code,
///     rather than with a component.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
public class FilterSelectItem<TValue> : IFilterSelectItem<TValue>
{
    /// <inheritdoc/>
    public TValue? Value { get; }

    /// <inheritdoc/>
    public string? AsString { get; }

    /// <summary>
    ///     Constructs a new <see cref="FilterSelectItem{TValue}"/>.
    /// </summary>
    /// <param name="value">The item's <typeparamref name="TValue"/>.</param>
    /// <param name="asString">The item's <see cref="string"/> representation.</param>
    public FilterSelectItem(TValue? value, string asString)
    {
        Value = value;
        AsString = asString;
    }
}
