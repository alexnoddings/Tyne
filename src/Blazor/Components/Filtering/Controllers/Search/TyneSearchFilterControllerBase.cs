using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A base implementation of a search controller.
/// </summary>
/// <inheritdoc/>
public abstract class TyneSearchFilterControllerBase<TRequest, TSearchValue, TFilterValue> : TyneFilterControllerBase<TRequest, TSearchValue>
{
    /// <summary>
    ///     An <see cref="Expression"/> for the <typeparamref name="TSearchValue"/> property to attach to.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TRequest, TFilterValue>> For { get; set; } = null!;
    /// <summary>
    ///     A <see cref="TynePropertyKeyCache{TSource, TProperty}"/> which caches the <see cref="TyneKey"/> which <see cref="For"/> points to.
    /// </summary>
    protected TynePropertyKeyCache<TRequest, TFilterValue> ForCache { get; } = new();
    protected override TyneKey ForKey => ForCache.Update(For);

    private IFilterSearchValue<TSearchValue>? _filterSearchValue;
    private IFilterSearchValue<TSearchValue> EnsureFilterSupportsSearching()
    {
        if (_filterSearchValue is not null)
            return _filterSearchValue;

        if (Handle.FilterValue is not IFilterSearchValue<TSearchValue> filterSearchValue)
            throw new InvalidOperationException($"{nameof(TyneSearchFilterControllerBase<TRequest, TSearchValue, TFilterValue>)} is not compatible with filter value for '{ForKey}'; filter value does not support {nameof(IFilterSearchValue<TSearchValue>)}<{typeof(TSearchValue).Name}>.");

        _filterSearchValue = filterSearchValue;
        return filterSearchValue;
    }

    /// <summary>
    ///     Initialises the controller and ensures that the attached filter value supports searching (i.e. implements <see cref="IFilterSearchValue{TSearchValue}"/>).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the initialisation.</returns>
    protected override Task OnInitializedAsync()
    {
        // Check the handle filter is valid.
        _ = EnsureFilterSupportsSearching();
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Searches a data source for <typeparamref name="TSearchValue"/>s which match <paramref name="search"/>.
    /// </summary>
    /// <param name="search">Optionally, a parameter used to search <typeparamref name="TSearchValue"/>s.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> whose result is a list of <typeparamref name="TSearchValue"/>s found.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This is a convenient shorthand to access <see cref="IFilterSearchValue{TSearchValue}.SearchAsync(string, CancellationToken)"/> on the handle.
    ///         See that method for more information on how it works.
    ///     </para>
    ///     <para>
    ///         This allows components to use:
    ///         <code lang="razor">SearchFunc="SearchItemsAsync"</code>
    ///         for <see cref="MudBlazor.MudAutocomplete{T}"/>s, as normally the type signatures don't match.
    ///         MudBlazor expects a task of <see cref="IEnumerable{T}"/>, whereas Tyne uses tasks of <see cref="List{T}"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When the attached filter does not support <see cref="IFilterSearchValue{TSearchValue}"/>.</exception>
    protected async Task<IEnumerable<TSearchValue>> SearchItemsAsync(string search, CancellationToken cancellationToken = default) =>
        await EnsureFilterSupportsSearching()
                .SearchAsync(search, cancellationToken)
                .ConfigureAwait(true);
}
