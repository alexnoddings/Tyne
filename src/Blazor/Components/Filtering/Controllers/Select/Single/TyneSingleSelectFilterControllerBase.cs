using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A base implementation of a single-selection controller.
/// </summary>
/// <inheritdoc/>
public abstract partial class TyneSingleSelectFilterControllerBase<TRequest, TValue> : TyneSelectFilterControllerBase<TRequest, TValue, TValue>
{
    /// <summary>
    ///     An <see cref="Expression"/> for the <typeparamref name="TValue"/> property to attach to.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TRequest, TValue>> For { get; set; } = null!;
    /// <summary>
    ///     A <see cref="TynePropertyKeyCache{TSource, TProperty}"/> which caches the <see cref="TyneKey"/> which <see cref="For"/> points to.
    /// </summary>
    protected TynePropertyKeyCache<TRequest, TValue> ForCache { get; } = new();
    protected override TyneKey ForKey => ForCache.Update(For);

    /// <summary>
    ///     Sets the value of the attached filter's <typeparamref name="TValue"/> to <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue">The new <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="Task"/> representing the value being set.</returns>
    /// <remarks>
    ///     This is a convenient shorthand to access the handle.
    /// </remarks>
    protected Task SetValueAsync(TValue? newValue) =>
        Handle.Filter.SetValueAsync(newValue);
}
