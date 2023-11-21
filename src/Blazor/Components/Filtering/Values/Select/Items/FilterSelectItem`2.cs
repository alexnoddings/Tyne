namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A simple implementation of <see cref="IFilterSelectItem<TValue>"/>
///     which provides a value and content through the constructor.
///     This allows it to be created directly through code,
///     rather than with a component.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
public class FilterSelectItem<TValue, TMetadata> : FilterSelectItem<TValue>, IFilterSelectItem<TValue, TMetadata>
{
    /// <inheritdoc/>
    public TMetadata? Metadata { get; }

    /// <summary>
    ///     Constructs a new <see cref="FilterSelectItem{TValue, TMetadata}"/> for the <paramref name="value"/>, <paramref name="content"/>, and <paramref name="metadata"/>.
    /// </summary>
    /// <param name="value">The <see cref="Value"/>.</param>
    /// <param name="asString">The <see cref="AsString"/>.</param>
    /// <param name="metadata">The <see cref="Metadata"/>.</param>
    public FilterSelectItem(TValue? value, string asString, TMetadata? metadata) : base(value, asString)
    {
        Metadata = metadata;
    }
}
