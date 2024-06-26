using Tyne;
using Tyne.HttpMediator.Server;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="TyneBuilder"/>.
/// </summary>
public static class HttpMediatorServerTyneBuilderExtensions
{
    /// <summary>
    ///     Adds server HTTP mediator services to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="TyneBuilder"/>.</param>
    /// <param name="configure">An <see cref="Action{T}"/> which configures HTTP Mediator.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="builder"/> or <paramref name="configure"/> are <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">When HTTP mediator services have already been added.</exception>
    public static TyneBuilder AddServerHttpMediator(this TyneBuilder builder, Action<HttpMediatorBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);
        EnsureNotAlreadyAdded(builder);

        var httpMediatorBuilder = new HttpMediatorBuilder(builder.Services);
        configure(httpMediatorBuilder);

        _ = builder.Services
            .AddSingleton(httpMediatorBuilder.MiddlewareBuilder.Middleware)
            .AddScoped<IHttpResponseResultWriter, HttpResponseResultWriter>()
            .AddScoped<HttpMediatorMiddlewarePipeline>();

        return builder;
    }

    private static void EnsureNotAlreadyAdded(TyneBuilder builder)
    {
        if (builder.Services.Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(HttpMediatorMiddlewareCollection)))
            throw new InvalidOperationException("HTTP mediator already added.");
    }
}
