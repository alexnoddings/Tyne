using System.Diagnostics.CodeAnalysis;

namespace Tyne.HttpMediator.Client;

/// <summary>
///     A delegate which executes the next middleware in the HTTP mediator pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of request.</typeparam>
/// <typeparam name="TResponse">The type of response.</typeparam>
/// <param name="request">The <typeparamref name="TRequest"/>.</param>
/// <returns>A <see cref="Task{TResult}"/> whose result is a <see cref="HttpResult{T}"/> of <typeparamref name="TResponse"/>.</returns>
[SuppressMessage("Naming", "CA1711: Identifiers should not have incorrect suffix.", Justification = "It matches ASP.NET Core's middleware.")]
public delegate Task<HttpResult<TResponse>> HttpMediatorDelegate<in TRequest, TResponse>(TRequest request) where TRequest : IHttpRequestBase<TResponse>;
