using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tyne.HttpMediator;
using Tyne.HttpMediator.Server;
using Tyne.MediatorEndpoints;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for adding MediatR middleware to <see cref="HttpMediatorMiddlewareBuilder"/>.
/// </summary>
public static class HttpMediatorServerLegacyMediatRMiddlewareBuilderExtensions
{
    /// <summary>
    ///     Adds a middleware to the pipeline which executes the request, sending it to MedaitR's <see cref="IMediator"/>.
    /// </summary>
    /// <param name="builder">The <see cref="HttpMediatorMiddlewareBuilder"/>.</param>
    /// <remarks>
    ///     This will act as a terminal middleware for any requests which implement <see cref="IRequest{TResponse}"/> where <c>TResponse</c> is <see cref="HttpResult{T}"/>.
    /// </remarks>
    public static HttpMediatorMiddlewareBuilder UseLegacyMediatR(this HttpMediatorMiddlewareBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddScoped<LegacyMediatRSenderMiddleware>();
        builder.Use<LegacyMediatRSenderMiddleware>();

        return builder;
    }
}
