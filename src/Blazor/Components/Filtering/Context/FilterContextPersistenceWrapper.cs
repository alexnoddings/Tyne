using Tyne.Blazor.Persistence;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     Wraps <see cref="IFilterContext{TRequest}.Persistence"/> to defer calls to a
///     <see cref="FilterContextBatchUpdateQueue{TRequest}"/> during a batch update.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
internal sealed class FilterContextPersistenceWrapper<TRequest> : IUrlPersistenceService
{
    private readonly TyneFilterContext<TRequest> _filterContext;
    private readonly IUrlPersistenceService _inner;

    public FilterContextPersistenceWrapper(TyneFilterContext<TRequest> filterContext, IUrlPersistenceService inner)
    {
        _filterContext = filterContext ?? throw new ArgumentNullException(nameof(filterContext));
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public Option<T> GetValue<T>(string key) =>
        // Getting isn't queued as it doesn't modify state.
        // This *could* cause inconsistency, e.g.:
        //  .SetValue(key, 42)
        //  .GetValue(key) != 42
        // However there isn't currently a use case for setting then
        // immediately getting a value, and having get pull from
        // the queued values wouldn't be trivial.
        _inner.GetValue<T>(key);

    public void SetValue<T>(string key, T? value)
    {
        if (_filterContext.CurrentBatchUpdate is null)
        {
            _inner.SetValue(key, value);
            return;
        }

        // If key has already been queued, it will get overwritten
        var parameterQueue = _filterContext.CurrentBatchUpdate.PersistenceParameterQueue;
        parameterQueue[key] = value;
    }

    public void BulkSetValues(IReadOnlyDictionary<string, object?> parameters)
    {
        if (_filterContext.CurrentBatchUpdate is null)
        {
            _inner.BulkSetValues(parameters);
            return;
        }

        var parameterQueue = _filterContext.CurrentBatchUpdate.PersistenceParameterQueue;
        foreach (var (key, value) in parameters)
            parameterQueue[key] = value;
    }
}
