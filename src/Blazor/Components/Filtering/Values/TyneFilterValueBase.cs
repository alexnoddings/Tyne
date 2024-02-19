namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     Base <see cref="IFilterValue{TRequest, TValue}"/> implementation.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type which the value manages.</typeparam>
public abstract class TyneFilterValueBase<TRequest, TValue> : TyneFilterValueCore<TRequest, TValue>
{
    private FilterValueSetter<TRequest, TValue>? _forSetter;

    protected sealed override void OnInitialized()
    {
        base.OnInitialized();

        // Generating and caching a Delegate during init is a nice middle ground between:
        // - Running reflection every time we configure the request
        //   o no initialisation cost
        //   o ~10x higher cost per invocation
        // - Creating a delegate at initialisation
        //   o small initialisation cost to create and cache
        //   o much cheaper cost per invocation
        // - Compiling an expression at initialisation
        //   o very large initialisation cost to create and cache
        //   o only marginally cheaper cost per invocation than a delegate
        _forSetter = FilterValueSetter.CreateForSetter<TRequest, TValue>(ForKey.Key);
    }

    /// <summary>
    ///     Configures <paramref name="request"/> by setting the property
    ///     matching <see cref="ForKey"/> to <see cref="Value"/>.
    /// </summary>
    /// <param name="request">The <typeparamref name="TRequest"/> to configure.</param>
    /// <returns>A <see cref="ValueTask"/> representing the <paramref name="request"/> being configured.</returns>
    public override ValueTask ConfigureRequestAsync(TRequest request)
    {
        if (Value is null)
            return ValueTask.CompletedTask;

        if (_forSetter is not null)
            _forSetter(request, Value);

        return ValueTask.CompletedTask;
    }
}
