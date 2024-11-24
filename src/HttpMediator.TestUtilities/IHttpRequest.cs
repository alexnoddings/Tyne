using MediatR;

namespace Tyne.HttpMediator;

public interface IHttpRequest<TResponse> : IHttpRequestBase<TResponse>, IRequest<HttpResult<TResponse>>
{
}
