namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     Represents a select-able <see cref="Value"/> which can be rendered with <see cref="Content"/>.
/// </summary>
/// <typeparam name="TValue">The type of value the select is for.</typeparam>
public interface IFilterSelectItem<out TValue, TMetadata> : IFilterSelectItem<TValue>
{
    /// <summary>
    ///     Some <typeparamref name="TMetadata"/> which represents <see cref="IFilterSelectItem{TValue}.Value"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is designed to allow advanced usage scenarios where <typeparamref name="TValue"/>
    ///         is a simple type (e.g. <see cref="Guid"/>) to allow the value to be rendered based on extra data.
    ///     </para>
    ///     <para>
    ///         For example, you may load these values from an API, where it returns (Guid Id, string Name, Color Color).
    ///         The regular <see cref="IFilterSelectItem{TValue}"/> loses this color metadata, whereas this type
    ///         can retain it for rendering by a controller.
    ///     </para>
    /// </remarks>
    public TMetadata? Metadata { get; }
}
