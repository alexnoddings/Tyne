using Tyne.Blazor.Filtering.Context;
using Tyne.Blazor.Filtering.Values;

namespace Tyne.Blazor.Filtering.Controllers;

/// <summary>
///     A handle given to a <see cref="IFilterController{TValue}"/> when it attaches to an <see cref="IFilterContext{TRequest}"/>.
/// </summary>
/// <typeparam name="TValue">The type the filter value manages.</typeparam>
/// <remarks>
///     This handle should be disposed once the <see cref="IFilterController{TValue}"/>
///     goes out of scope to ensure it is detached from the <see cref="IFilterContext{TRequest}"/>.
/// </remarks>
public interface IFilterControllerHandle<TValue> : IDisposable
{
    /// <summary>
    ///     The <see cref="IFilterValue{TValue}"/> which the controller has attached to.
    /// </summary>
    public IFilterValue<TValue> FilterValue { get; }
}
