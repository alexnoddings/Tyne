using Tyne.Queries;
using Tyne.Results;

namespace Tyne.Actions;

/// <summary>
///		An <see cref="IAction{TModel, TResult}"/> which produces a <see cref="SearchResults{T}"/> of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TQuery">The type of query the action takes as an input. This must be of type <see cref="ISearchQuery"/>.</typeparam>
/// <typeparam name="TResult">The type of result the action produces as an output.</typeparam>
public interface ISearchAction<TQuery, TResult> : IAction<TQuery, SearchResults<TResult>> where TQuery : ISearchQuery
{
}
