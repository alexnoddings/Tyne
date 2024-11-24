using System.Diagnostics.CodeAnalysis;

namespace Tyne.HttpMediator.Client;

/// <summary>
///     A middleware in the HTTP mediator pipeline.
/// </summary>
/// <remarks>
///     Middleware may choose to modify a request prior to execution,
///     modify the response after execution, or short-circuit the execution.
/// </remarks>
[SuppressMessage("Naming", "CA1716: Identifiers should not match keywords.", Justification = "It matches ASP.NET Core's middleware.")]
public interface IHttpMediatorMiddleware
{
    /// <summary>
    ///     Invoked as part of the pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The type of request.</typeparam>
    /// <typeparam name="TResponse">The type of response.</typeparam>
    /// <param name="request">The <typeparamref name="TRequest"/>.</param>
    /// <param name="next">The next step in the pipeline.</param>
    /// <returns>A <see cref="Task{TResult}"/> whose result is a <see cref="HttpResult{T}"/> of <typeparamref name="TResponse"/>.</returns>
    public Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>;
}
