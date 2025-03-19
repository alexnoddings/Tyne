namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A combined <see cref="IFilter{TRequest}"/> and <see cref="IFilterValue{TValue}"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <typeparam name="TValue">The type of value being managed.</typeparam>
public interface IFilterValue<in TRequest, TValue> : IFilter<TRequest>, IFilterValue<TValue>
{
}
