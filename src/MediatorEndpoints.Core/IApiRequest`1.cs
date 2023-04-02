using MediatR;

namespace Tyne.MediatorEndpoints;

public interface IApiRequest<out TResponse> : IRequest<TResponse>, IApiRequestMetadata
{
}
