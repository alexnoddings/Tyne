using Tyne.HttpMediator.Client;

namespace Tyne.MediatorEndpoints;

internal class MediatorProxy : IMediatorProxy
{
    private readonly IHttpMediator _mediator;

    public MediatorProxy(IHttpMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<TResponse> Send<TResponse>(IApiRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.SendAsync(request, cancellationToken).ConfigureAwait(false);
        // This is what the old MediatorEndpoints HttpMediatorProxy would throw if the Response was null
        return result.Unwrap(() => new InvalidOperationException($"Could not read response. De-serialising returned null, not {nameof(TResponse)}."));
    }
}
