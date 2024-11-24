namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A filter value which supports selection from <see cref="SelectItems"/>.
/// </summary>
/// <typeparam name="TValue">The type which the value manages.</typeparam>
public interface IFilterSelectValue<TValue>
{
    /// <summary>
    ///     The <see cref="IFilterSelectItem{TValue}"/>s which have been registered.
    /// </summary>
    public ICollection<IFilterSelectItem<TValue?>>? SelectItems { get; }
}
