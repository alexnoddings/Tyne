using Tyne.Searching;

namespace Tyne.HttpMediator;

/// <summary>
///     An <see cref="IHttpRequestBase{TResponse}"/> which produces a <see cref="SearchResults{T}"/> of <typeparamref name="TResponse"/>.
/// </summary>
/// <inheritdoc/>
public interface IHttpSearchRequestBase<TResponse> : IHttpRequestBase<SearchResults<TResponse>>
{
}
