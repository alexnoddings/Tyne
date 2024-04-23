using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tyne.HttpMediator.Server;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for adding MediatR middleware to <see cref="HttpMediatorMiddlewareBuilder"/>.
/// </summary>
public static class HttpMediatorServerMediatRMiddlewareBuilderExtensions
{
    /// <summary>
    ///     Adds a middleware to the pipeline which executes the request, sending it to MedaitR's <see cref="IMediator"/>.
    /// </summary>
    /// <param name="builder">The <see cref="HttpMediatorMiddlewareBuilder"/>.</param>
    /// <remarks>
    ///     This will act as a terminal middleware for any requests which implement <see cref="IRequest{TResponse}"/>.
    /// </remarks>
    public static HttpMediatorMiddlewareBuilder UseMediatR(this HttpMediatorMiddlewareBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddScoped<MediatRSenderMiddleware>();
        _ = builder.Use<MediatRSenderMiddleware>();

        return builder;
    }
}
