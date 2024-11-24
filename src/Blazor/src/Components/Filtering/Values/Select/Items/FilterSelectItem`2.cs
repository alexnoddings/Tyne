namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     An extension of <see cref="FilterSelectItem{TValue}"/> with <see cref="Metadata"/>.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
/// <typeparam name="TMetadata">The type of metadata on the item.</typeparam>
public class FilterSelectItem<TValue, TMetadata> : FilterSelectItem<TValue>, IFilterSelectItem<TValue, TMetadata>
{
    /// <inheritdoc/>
    public TMetadata? Metadata { get; }

    /// <summary>
    ///     Constructs a new <see cref="FilterSelectItem{TValue, TMetadata}"/>.
    /// </summary>
    /// <param name="value">The item's <typeparamref name="TValue"/>.</param>
    /// <param name="asString">The item's <see cref="string"/> representation.</param>
    /// <param name="metadata">The item's <typeparamref name="TMetadata"/>.</param>
    public FilterSelectItem(TValue? value, string asString, TMetadata? metadata) : base(value, asString)
    {
        Metadata = metadata;
    }
}
