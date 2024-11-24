using MediatR;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     An <see cref="IRequestHandler{TRequest}"/> which handles <typeparamref name="TRequest"/>, producing a <see cref="HttpResult{T}"/> of <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The <see cref="IHttpRequestBase{TResponse}"/> type.</typeparam>
/// <typeparam name="TResponse">The type of response produced by <typeparamref name="TRequest"/>.</typeparam>
public interface IHttpRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, HttpResult<TResponse>>
    where TRequest : IHttpRequestBase<TResponse>, IRequest<HttpResult<TResponse>>
{
}
