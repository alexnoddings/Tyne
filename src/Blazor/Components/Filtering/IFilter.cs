namespace Tyne.Blazor.Filtering;

/// <summary>
///     A filter which can configure <typeparamref name="TRequest"/>s in a filter context.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
public interface IFilter<in TRequest>
{
    /// <summary>
    ///     Ensures the filter is initialised.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the value initialising.</returns>
    public Task EnsureInitialisedAsync();

    /// <summary>
    ///     Configures <paramref name="request"/> according to this filter.
    /// </summary>
    /// <param name="request">The <typeparamref name="TRequest"/> to configure.</param>
    /// <returns>A <see cref="ValueTask"/> representing the <paramref name="request"/> being configured.</returns>
    public ValueTask ConfigureRequestAsync(TRequest request);
}
