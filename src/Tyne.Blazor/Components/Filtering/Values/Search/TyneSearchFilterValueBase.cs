namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A <see cref="TyneFilterValueCore{TRequest, TValue}"/> which also implements <see cref="IFilterSearchValue{TSearchValue}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TSearchValue"></typeparam>
/// <typeparam name="TFilterValue">The type which the value manages.</typeparam>
public abstract class TyneSearchFilterValueBase<TRequest, TSearchValue, TFilterValue>
    : TyneFilterValueCore<TRequest, TSearchValue>, IFilterSearchValue<TSearchValue>
{
    private FilterValueSetter<TRequest, TFilterValue>? _forSetter;

    protected sealed override void OnInitialized()
    {
        base.OnInitialized();
        _forSetter = FilterValueSetter.CreateForSetter<TRequest, TFilterValue>(ForKey.Key);
    }

    public override ValueTask ConfigureRequestAsync(TRequest request)
    {
        if (Value is null)
            return ValueTask.CompletedTask;

        if (_forSetter is not null)
        {
            var filterValue = GetFilterValueFrom(Value);
            if (filterValue is not null)
                _forSetter(request, filterValue);
        }

        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     Gets the <typeparamref name="TFilterValue"/> from a <paramref name="searchValue"/>.
    /// </summary>
    /// <param name="searchValue">The selected <typeparamref name="TSearchValue"/>.</param>
    /// <returns>A <typeparamref name="TFilterValue"/> which is used to configure the request.</returns>
    protected abstract TFilterValue? GetFilterValueFrom(TSearchValue searchValue);

    /// <inheritdoc/>
    public abstract Task<List<TSearchValue>> SearchAsync(string? search, CancellationToken cancellationToken = default);
}
