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
        return result.Unwrap(() => $"Error sending request {request?.GetType().Name}.");
    }
}
