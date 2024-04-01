using System.Net;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tyne.HttpMediator;
using Tyne.HttpMediator.Server;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class HttpMediatorServerMiddlewareBuilderExtensions
{
    /// <summary>
    ///     Adds a middleware which handles exceptions in the pipeline.
    ///     If an unhandled exception in the pipeline is caught by the middleware,
    ///     an <see cref="HttpStatusCode.InternalServerError"/> <see cref="HttpResult{T}"/> is returned.
    /// </summary>
    /// <param name="builder">The <see cref="HttpMediatorMiddlewareBuilder"/>.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    /// <remarks>
    ///     This is usually one of the first middlewares registered to wrap inner middlewares.
    /// </remarks>
    public static HttpMediatorMiddlewareBuilder UseExceptionHandler(this HttpMediatorMiddlewareBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddScoped<ExceptionHandlerMiddleware>();
        return builder.Use<ExceptionHandlerMiddleware>();
    }
}
