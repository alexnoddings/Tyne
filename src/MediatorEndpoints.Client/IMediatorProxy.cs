using MediatR;

namespace Tyne.MediatorEndpoints;

/// <summary>
///		Acts as a proxy for a mediator to encapsulate request/response patterns.
/// </summary>
/// <remarks>
///		This interface replicates MediatR's IMediator.Send method, but takes <c>request</c> as an <see cref="IApiRequest{TResponse}"/>.
/// </remarks>
public interface IMediatorProxy
{
    public Task<TResponse> Send<TResponse>(IApiRequest<TResponse> request, CancellationToken cancellationToken = default);
    public Task Send(IApiRequest<Unit> request, CancellationToken cancellationToken = default);
}
