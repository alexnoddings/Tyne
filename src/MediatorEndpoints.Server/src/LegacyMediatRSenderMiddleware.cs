using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Http;
using Tyne.HttpMediator;
using Tyne.HttpMediator.Server;
using Tyne.Utilities;

namespace Tyne.MediatorEndpoints;

// Middleware to provide legacy compatability with Tyne's MediatorEndpoints which executed IRequest<T> rather than IRequest<HttpResult<T>>
internal sealed class LegacyMediatRSenderMiddleware : IHttpMediatorMiddleware
{
    private readonly IMediator _mediator;

    public LegacyMediatRSenderMiddleware(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(HttpContext context, TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>
    {
        if (IsUnwrappedMediatRRequest<TRequest, TResponse>())
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
        typeof(LegacyMediatRSenderMiddleware)
        .GetMethod(nameof(SendGenericAsync), BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"No \"{nameof(SendGenericAsync)}\" method found on \"{nameof(LegacyMediatRSenderMiddleware)}\".");

    private async Task<HttpResult<TResponse>> SendGenericAsync<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
    {
        var response = await _mediator.Send(request).ConfigureAwait(false);
        if (response is null)
            return HttpResult.Codes.InternalServerError<TResponse>("No value.");
        return HttpResult.Codes.OK(response);
    }

    private static bool IsUnwrappedMediatRRequest<TRequest, TResponse>() =>
        Array.Exists(
            typeof(TRequest).GetInterfaces(),
            interfaceType => interfaceType == typeof(IRequest<TResponse>)
        );
}
