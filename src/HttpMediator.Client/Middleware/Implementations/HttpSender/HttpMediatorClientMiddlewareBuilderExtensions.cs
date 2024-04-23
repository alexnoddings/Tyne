using Microsoft.Extensions.DependencyInjection.Extensions;
using Tyne.HttpMediator.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class HttpMediatorClientMiddlewareBuilderExtensions
{
    /// <summary>
    ///     Adds a middleware to the pipeline which sends the request to the server.
    /// </summary>
    /// <param name="builder">The <see cref="HttpMediatorMiddlewareBuilder"/>.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    /// <remarks>
    ///     This should be the last middleware registered as it will terminate the pipeline, ignoring any further middlewares.
    ///     As such, the <paramref name="builder"/> is not returned as no further middleware can be executed.
    /// </remarks>
    public static void Send(this HttpMediatorMiddlewareBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddScoped<HttpSenderMiddleware>();
        _ = builder.Use<HttpSenderMiddleware>();
    }
}
