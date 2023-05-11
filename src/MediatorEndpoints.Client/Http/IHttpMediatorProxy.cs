namespace Tyne.MediatorEndpoints.Http;

/// <summary>
///     Exposes the underlying <see cref="HttpResponseMessage"/> used by Mediator Proxies which use HTTP.
/// </summary>
/// <remarks>
///     <para>
///         This interface should only be used if you NEED access to the underlying <see cref="HttpResponseMessage"/>.
///         Otherwise, prefer using the more abstracted <see cref="IMediatorProxy"/>.
///     </para>
///     <para>
///         An implementation will only be available if <see cref="ServiceCollectionMediatorExtensions.AddHttpMediatorProxy(Tyne.TyneBuilder)"/> is used.
///         If the <see cref="IMediatorProxy"/> is not HTTP-based, no service implementing this will be available.
///     </para>
/// </remarks>
public interface IHttpMediatorProxy : IMediatorProxy
{
    public Task<HttpMediatorResult<TResponse>> SendHttp<TResponse>(IApiRequest<TResponse> request, HttpMediatorResponseBehaviour behaviour = HttpMediatorResponseBehaviour.Automatic, CancellationToken cancellationToken = default);
}
