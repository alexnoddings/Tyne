using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A context representing an <see cref="IFilterSelectValue{TValue}"/>
///     in which <see cref="IFilterSelectItem{TValue}"/>s can be attached.
/// </summary>
/// <typeparam name="TValue">The type which the value manages.</typeparam>
/// <remarks>
///     This will usually be provided as a <see cref="CascadingValue{TValue}"/>
///     inside of a select value's content for select items to attach to.
/// </remarks>
public interface IFilterSelectValueContext<in TValue>
{
    /// <summary>
    ///     Attaches an <paramref name="item"/> to the select value.
    /// </summary>
    /// <param name="item">The <see cref="IFilterSelectItem{TValue}"/> to attach.</param>
    /// <returns>
    ///     An <see cref="ISelectValueItemHandle"/>.
    ///     This is how items should interact with the value once attached,
    ///     and should be disposed of to detach the <paramref name="item"/>
    ///     once it goes out of scope.
    /// </returns>
    public ISelectValueItemHandle Attach(IFilterSelectItem<TValue> item);
}
