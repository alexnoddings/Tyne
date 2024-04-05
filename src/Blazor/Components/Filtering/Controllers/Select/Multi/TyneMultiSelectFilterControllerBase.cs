using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     Base class for multi-selection controllers.
/// </summary>
/// <remarks>
///     This multi-selection column requires the value to be a <see cref="HashSet{T}"/> of <typeparamref name="TValue"/>s.
/// </remarks>
/// <inheritdoc/>
public abstract partial class TyneMultiSelectFilterControllerBase<TRequest, TValue> : TyneSelectFilterControllerBase<TRequest, HashSet<TValue>, TValue>
{
    /// <summary>
    ///     An <see cref="Expression"/> for the <typeparamref name="TValue"/> property to attach to.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TRequest, HashSet<TValue?>>> For { get; set; } = null!;
    /// <summary>
    ///     A <see cref="TynePropertyKeyCache{TSource, TProperty}"/> which caches the <see cref="TyneKey"/> which <see cref="For"/> points to.
    /// </summary>
    protected TynePropertyKeyCache<TRequest, HashSet<TValue?>> ForCache { get; } = new();
    protected override TyneKey ForKey => ForCache.Update(For);

    /// <summary>
    ///     Sets the value of the attached filter value.
    /// </summary>
    /// <param name="newValue">The enumerable of new <typeparamref name="TValue"/>s.</param>
    /// <returns>A <see cref="Task"/> representing the values being set.</returns>
    /// <remarks>
    ///     <para>
    ///         This is converted to a <see cref="HashSet{T}"/>
    ///         and passed to <see cref="SetValueAsync(HashSet{TValue}?)"/>.
    ///         <see langword="null"/> values for <paramref name="newValue"/>
    ///         will use an empty <see cref="HashSet{T}"/>.
    ///     </para>
    ///     <para>
    ///         Alternatively, you can use <see cref="AddValueAsync(TValue)"/>
    ///         and <see cref="RemoveValueAsync(TValue)"/> to add and remove
    ///         individual values from the selection.
    ///     </para>
    /// </remarks>
    protected Task SetValueAsync(IEnumerable<TValue>? newValue)
    {
        if (newValue is not HashSet<TValue> hashSet)
        {
            if (newValue is null)
                hashSet = [];
            else
                hashSet = new(newValue);
        }

        return SetValueAsync(hashSet);
    }

    /// <summary>
    ///     Sets the value of the attached filter value.
    /// </summary>
    /// <param name="newValue">The <see cref="HashSet{T}"/> of the new <typeparamref name="TValue"/>s.</param>
    /// <returns>A <see cref="Task"/> representing the values being set.</returns>
    /// <remarks>
    ///     <para>
    ///         <see langword="null"/> values for <paramref name="newValue"/>
    ///         will use an empty <see cref="HashSet{T}"/>.
    ///     </para>
    ///     <para>
    ///         Alternatively, you can use <see cref="AddValueAsync(TValue)"/>
    ///         and <see cref="RemoveValueAsync(TValue)"/> to add and remove
    ///         individual values from the selection.
    ///     </para>
    /// </remarks>
    protected Task SetValueAsync(HashSet<TValue>? newValue) =>
        Handle.Filter.SetValueAsync(newValue ?? []);

    /// <summary>
    ///     Adds <paramref name="item"/> to the selected filter value <see cref="HashSet{T}"/>.
    /// </summary>
    /// <param name="item">The <typeparamref name="TValue"/> to add.</param>
    /// <returns>A <see cref="Task"/> representing the value being added.</returns>
    protected Task AddValueAsync(TValue item)
    {
        var currentValue = Handle.Filter.Value ?? [];
        if (item is not null && currentValue.Contains(item))
            return Task.CompletedTask;

        var newValue = new HashSet<TValue>(currentValue)
        {
            item
        };
        return SetValueAsync(newValue);
    }

    /// <summary>
    ///     Removes <paramref name="item"/> from the selected filter value <see cref="HashSet{T}"/>.
    /// </summary>
    /// <param name="item">The <typeparamref name="TValue"/> to remove.</param>
    /// <returns>
    ///     A <see cref="Task"/> representing the value being removed.
    ///     This may be <see cref="Task.CompletedTask"/> if the value was not selected.
    /// </returns>
    protected Task RemoveValueAsync(TValue item)
    {
        var currentValue = Handle.Filter.Value;
        if (currentValue is null || currentValue.Count == 0)
            // Can't remove a value from a null or empty set
            return Task.CompletedTask;

        var newValue = new HashSet<TValue>(currentValue);
        newValue.Remove(item);
        return SetValueAsync(newValue);
    }
}
