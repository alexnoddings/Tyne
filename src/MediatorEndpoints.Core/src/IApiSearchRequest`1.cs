using Tyne.Searching;

namespace Tyne.MediatorEndpoints;

/// <summary>
///		Interface to represent a search request with a search results response and API metadata.
/// </summary>
/// <typeparam name="TResponse">Response type</typeparam>
public interface IApiSearchRequest<TResponse> : IApiRequest<SearchResults<TResponse>>
{
}
