using Microsoft.Extensions.DependencyInjection;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     A builder for <see cref="IHttpMediatorMiddleware"/>s.
/// </summary>
public sealed class HttpMediatorMiddlewareBuilder
{
    internal HttpMediatorMiddlewareCollection Middleware { get; }

    /// <summary>
    ///     The <see cref="IServiceCollection"/> which the HTTP mediator middlewares are being added to.
    /// </summary>
    public IServiceCollection Services { get; }

    internal HttpMediatorMiddlewareBuilder(IServiceCollection services)
    {
        Middleware = new();
        Services = services;
    }

    /// <summary>
    ///     Registers a <see cref="IHttpMediatorMiddleware"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IHttpMediatorMiddleware"/>.</typeparam>
    /// <returns>This <see cref="HttpMediatorMiddlewareBuilder"/> for chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         Middleware is executed in the order it is added.
    ///     </para>
    ///     <para>
    ///         Ensure that <typeparamref name="T"/> is added appropriately to the <see cref="IServiceCollection"/> (available through <see cref="Services"/>).
    ///     </para>
    /// </remarks>
    public HttpMediatorMiddlewareBuilder Use<T>() where T : IHttpMediatorMiddleware
    {
        Middleware.Add(typeof(T));
        return this;
    }
}
