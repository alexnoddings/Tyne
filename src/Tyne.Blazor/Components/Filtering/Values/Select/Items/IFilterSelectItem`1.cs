namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     Represents a select-able <see cref="Value"/>.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
public interface IFilterSelectItem<out TValue>
{
    /// <summary>
    ///     The <typeparamref name="TValue"/> this item represents.
    /// </summary>
    public TValue? Value { get; }

    /// <summary>
    ///     A <see cref="string"/> representation of <see cref="Value"/>.
    /// </summary>
    public string? AsString { get; }
}
