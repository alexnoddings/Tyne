namespace Tyne.MediatorEndpoints.Http;

/// <summary>
///     How an <see cref="IHttpMediatorProxy"/> should handle the <see cref="HttpResponseMessage"/>.
/// </summary>
public enum HttpMediatorResponseBehaviour
{
    /// <summary>
    ///     The <see cref="HttpResponseMessage.Content"/> should be automatically read into <see cref="HttpMediatorResult{TResponse}.Response"/>.
    /// </summary>
    Automatic,
    /// <summary>
    ///     The <see cref="HttpResponseMessage.Content"/> should not be read by the <see cref="IHttpMediatorProxy"/>.
    /// </summary>
    /// <remarks>
    ///     This means that <see cref="HttpMediatorResult{TResponse}.Response"/> will always be null. It is up to the caller to read a <c>TResponse</c> from the <see cref="HttpMediatorResult{TResponse}.HttpResponse"/>.
    /// </remarks>
    Manual
}
