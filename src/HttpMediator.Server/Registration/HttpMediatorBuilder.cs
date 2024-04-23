using Microsoft.Extensions.DependencyInjection;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     A builder for HTTP Mediator.
/// </summary>
public sealed class HttpMediatorBuilder
{
    /// <summary>
    ///     The <see cref="IServiceCollection"/> which the HTTP mediator is being added to.
    /// </summary>
    public IServiceCollection Services { get; }

    internal HttpMediatorMiddlewareBuilder MiddlewareBuilder { get; }

    internal HttpMediatorBuilder(IServiceCollection services)
    {
        Services = services;
        MiddlewareBuilder = new(services);
    }

    /// <summary>
    ///     Registers an action used to configure <see cref="HttpMediatorOptions"/>.
    /// </summary>
    /// <param name="configure">The action used to configure the <see cref="HttpMediatorOptions"/>.</param>
    /// <returns>This <see cref="HttpMediatorBuilder"/> for chaining.</returns>
    public HttpMediatorBuilder Configure(Action<HttpMediatorOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        _ = Services.Configure(configure);
        return this;
    }

    /// <summary>
    ///     Registers middlewares to be used in the HTTP mediator pipeline.
    /// </summary>
    /// <param name="configure">An action used to configure the middlewares to use.</param>
    /// <returns>This <see cref="HttpMediatorBuilder"/> for chaining.</returns>
    public HttpMediatorBuilder WithMiddleware(Action<HttpMediatorMiddlewareBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        configure(MiddlewareBuilder);
        return this;
    }
}
