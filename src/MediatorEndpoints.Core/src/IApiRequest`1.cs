using MediatR;
using Tyne.HttpMediator;

namespace Tyne.MediatorEndpoints;

public interface IApiRequest<out TResponse>
    : IRequest<TResponse>,
    IHttpRequestBase<TResponse>
{
}
