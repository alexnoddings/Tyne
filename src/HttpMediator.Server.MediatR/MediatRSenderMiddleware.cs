using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Http;
using Tyne.Utilities;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     Terminal middleware which sends <see cref="IRequest{TResponse}"/>s to <see cref="IMediator"/> and returns the <c>TResponse</c>.
/// </summary>
[SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes.", Justification = "Instantiated by DI part of the HTTP Mediator pipeline.")]
internal sealed class MediatRSenderMiddleware : IHttpMediatorMiddleware
{
    private readonly IMediator _mediator;

    public MediatRSenderMiddleware(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(HttpContext context, TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>
    {
        if (IsMediatRRequest<TRequest, TResponse>())
            return SendAsync<TRequest, TResponse>(request);

        return next(context, request);
    }

    private Task<HttpResult<TResponse>> SendAsync<TRequest, TResponse>(TRequest request) where TRequest : IHttpRequestBase<TResponse>
    {
        ArgumentNullException.ThrowIfNull(request);

        return _sendGenericAsyncMethodInfo
            .MakeGenericMethod(request.GetType(), typeof(TResponse))
            .InvokeAsync<HttpResult<TResponse>>(this, request)!;
    }

    [SuppressMessage("Major Code Smell", "S3011: Reflection should not be used to increase accessibility of classes, methods, or fields.", Justification = "We are reflecting on a method private to this class.")]
    private static readonly MethodInfo _sendGenericAsyncMethodInfo =
        typeof(MediatRSenderMiddleware)
        .GetMethod(nameof(SendGenericAsync), BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"No \"{nameof(SendGenericAsync)}\" method found on \"{nameof(MediatRSenderMiddleware)}\".");

    // Using our own generic method is easier than reflecting on IMediator.Send as it has 3 overloads
    private Task<HttpResult<TResponse>> SendGenericAsync<TRequest, TResponse>(TRequest request) where TRequest : IRequest<HttpResult<TResponse>> =>
        _mediator.Send(request);

    private static bool IsMediatRRequest<TRequest, TResponse>() =>
        Array.Exists(
            typeof(TRequest).GetInterfaces(),
            interfaceType => interfaceType == typeof(IRequest<HttpResult<TResponse>>)
        );
}
