namespace Tyne.HttpMediator.Client;

/// <summary>
///     A mediator which sends requests across HTTP.
/// </summary>
public interface IHttpMediator
{
    /// <summary>
    ///     Sends the <paramref name="request"/>, returning a <see cref="HttpResult{T}"/> of <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of response.</typeparam>
    /// <param name="request">The <see cref="IHttpRequestBase{TResponse}"/> to send.</param>
    /// <param name="cancellationToken">Optionally, a <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="Task"/> whose result is the <typeparamref name="TResponse"/>.</returns>
    public Task<HttpResult<TResponse>> SendAsync<TResponse>(IHttpRequestBase<TResponse> request, CancellationToken cancellationToken = default);
}
