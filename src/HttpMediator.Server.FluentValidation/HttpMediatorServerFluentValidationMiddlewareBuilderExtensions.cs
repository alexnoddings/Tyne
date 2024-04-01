using System.Net;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tyne.HttpMediator;
using Tyne.HttpMediator.Server;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for adding Fluent Validation middleware to <see cref="HttpMediatorMiddlewareBuilder"/>.
/// </summary>
public static class HttpMediatorServerFluentValidationMiddlewareBuilderExtensions
{
    /// <summary>
    ///     Adds a middleware which ensures the request is valid using <see cref="FluentValidation.AbstractValidator{T}"/>s.
    ///     If validation fails, the pipeline is short-circuited and an <see cref="HttpStatusCode.BadRequest"/> <see cref="HttpResult{T}"/> is returned.
    /// </summary>
    /// <param name="builder">The <see cref="HttpMediatorMiddlewareBuilder"/>.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    public static HttpMediatorMiddlewareBuilder UseFluentValidation(this HttpMediatorMiddlewareBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddScoped<FluentValidationMiddleware>();
        return builder.Use<FluentValidationMiddleware>();
    }
}
