using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A controller which attaches to a min and a max <typeparamref name="TValue"/> on <typeparamref name="TRequest"/>.
///     The values to attach to are provided by <see cref="ForMin"/> and <see cref="ForMax"/>.
///     This controller uses a <see cref="MudDateRangePicker"/> to interact with the values.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type the filter values manage.</typeparam>
public abstract partial class TyneMinMaxFilterController<TRequest, TValue> : TyneMinMaxFilterControllerBase<TRequest, TValue>
{
    /// <summary>
    ///     An <see cref="Expression"/> for the <typeparamref name="TValue"/> minimum property to attach to.
    /// </summary>
    [Parameter]
    public Expression<Func<TRequest, TValue?>> ForMin { get; set; } = null!;
    /// <summary>
    ///     A <see cref="TynePropertyKeyCache{TSource, TProperty}"/> which caches the <see cref="TyneKey"/> which <see cref="ForMin"/> points to.
    /// </summary>
    protected TynePropertyKeyCache<TRequest, TValue?> ForMinCache { get; } = new();
    protected override TyneKey ForMinKey => ForMinCache.Update(ForMin);

    /// <summary>
    ///     An <see cref="Expression"/> for the <typeparamref name="TValue"/> maximum property to attach to.
    /// </summary>
    [Parameter]
    public Expression<Func<TRequest, TValue?>> ForMax { get; set; } = null!;
    /// <summary>
    ///     A <see cref="TynePropertyKeyCache{TSource, TProperty}"/> which caches the <see cref="TyneKey"/> which <see cref="ForMax"/> points to.
    /// </summary>
    protected TynePropertyKeyCache<TRequest, TValue?> ForMaxCache { get; } = new();
    protected override TyneKey ForMaxKey => ForMaxCache.Update(ForMax);
}
