using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tyne;
using Tyne.HttpMediator;
using Tyne.HttpMediator.Server;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
///     Extensions for mapping endpoints.
/// </summary>
public static class WebApplicationEndpointMappingExtensions
{
    /// <summary>
    ///     Maps <see cref="IHttpRequestBase{TResponse}"/>s from <paramref name="assembly"/> with Tyne's HTTP Mediator.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/>.</param>
    /// <param name="assembly">The <see cref="Assembly"/>.</param>
    /// <returns>The <paramref name="app"/> instance.</returns>
    public static IEndpointRouteBuilder MapHttpMediatorRequestsFromAssembly(this WebApplication app, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(assembly);

        var options = app.Services.GetRequiredService<IOptions<HttpMediatorOptions>>().Value;
        var logger = app.Services.GetService<ILoggerFactory>()?.CreateLogger(typeof(WebApplicationEndpointMapping));

        var requestDescriptors = AssemblyHttpRequestDescriptorScanner.GetDescriptorsFromAssembly(assembly);
        foreach (var requestDescriptor in requestDescriptors)
            app.MapRequestCoreGeneric(requestDescriptor, options, logger);

        logger?.LogRequestsMapped(assembly, requestDescriptors.Length);

        return app;
    }

    /// <summary>
    ///     Maps <see cref="IHttpRequestBase{TResponse}"/>s from the <see cref="Assembly"/> containing <typeparamref name="T"/> with Tyne's HTTP Mediator.
    /// </summary>
    /// <typeparam name="T">A <see cref="Type"/> in the target <see cref="Assembly"/>.</typeparam>
    /// <param name="app">The <see cref="WebApplication"/>.</param>
    /// <returns>The <paramref name="app"/> instance.</returns>
    public static IEndpointRouteBuilder MapHttpMediatorRequestsFromAssemblyContaining<T>(this WebApplication app) =>
        MapHttpMediatorRequestsFromAssembly(app, typeof(T).Assembly);

    [SuppressMessage("Major Code Smell", "S3011: Reflection should not be used to increase accessibility of classes, methods, or fields.", Justification = "We are reflecting on a method private to this class.")]
    private static readonly MethodInfo MapRequestCoreGenericMethodInfo =
        MethodHelper.Get(
              typeof(WebApplicationEndpointMappingExtensions),
              nameof(MapHttpMediatorRequestCore),
              BindingFlags.Static | BindingFlags.NonPublic,
              typeof(IEndpointRouteBuilder), typeof(ICollection<object>), typeof(HttpMediatorOptions), typeof(ILogger)
        );

    private static WebApplication MapRequestCoreGeneric(this WebApplication app, HttpRequestDescriptor requestDescriptor, HttpMediatorOptions options, ILogger? logger)
    {
        try
        {
            MapRequestCoreGenericMethodInfo
                .MakeGenericMethod(requestDescriptor.RequestType, requestDescriptor.ResponseType)
                .Invoke(null, [app, requestDescriptor.Metadata, options, logger]);
        }
        catch (TargetInvocationException invocationException)
        {
            // Throw the base (inner) exception instead
            // This can happen for invalid URIs, the Argument/InvalidOperationException is
            // much more developer-friendly than it being wrapped in an TargetInvocationException.
            throw invocationException.GetBaseException();
        }

        return app;
    }

    /// <summary>
    ///     Maps <typeparamref name="TRequest"/> to produce a <typeparamref name="TResponse"/> with Tyne's HTTP Mediator.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/>.</param>
    /// <param name="metadata">A collection of metadata to associate with the created endpoint.</param>
    /// <returns>The <paramref name="app"/> instance.</returns>
    public static IEndpointRouteBuilder MapHttpMediatorRequest<TRequest, TResponse>(this WebApplication app, ICollection<object> metadata) where TRequest : IHttpRequestBase<TResponse>
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(metadata);

        var options = app.Services.GetRequiredService<IOptions<HttpMediatorOptions>>().Value;
        var logger = app.Services.GetService<ILoggerFactory>()?.CreateLogger(typeof(WebApplicationEndpointMapping));

        return MapHttpMediatorRequestCore<TRequest, TResponse>(app, metadata, options, logger);
    }

    private static IEndpointRouteBuilder MapHttpMediatorRequestCore<TRequest, TResponse>(this IEndpointRouteBuilder endpoints, ICollection<object> metadata, HttpMediatorOptions options, ILogger? logger = null) where TRequest : IHttpRequestBase<TResponse>
    {
        var uri = options.GetFullApiUri<TRequest>().ToString();

        var methodName = TRequest.Method.Method.ToUpperInvariant();
        var routeBuilder = methodName switch
        {
            "GET" => endpoints.MapGet(uri, HttpRequestHandler.HandleRequestFromQueryAsync<TRequest, TResponse>),
            "DELETE" => endpoints.MapDelete(uri, HttpRequestHandler.HandleRequestFromQueryAsync<TRequest, TResponse>),
            "POST" => endpoints.MapPost(uri, HttpRequestHandler.HandleRequestFromBodyAsync<TRequest, TResponse>),
            "PUT" => endpoints.MapPut(uri, HttpRequestHandler.HandleRequestFromBodyAsync<TRequest, TResponse>),
            "PATCH" => endpoints.MapPatch(uri, HttpRequestHandler.HandleRequestFromBodyAsync<TRequest, TResponse>),
            var method => throw new NotSupportedException(CoreExceptionMessages.ApiRequest_HttpMethodNotSupported(typeof(TRequest), method))
        };

        logger?.LogRequestMapped<TRequest, TResponse>(uri, metadata);

        routeBuilder.Add(builder =>
        {
            foreach (var metadatum in metadata)
                builder.Metadata.Add(metadatum);
        });

        return endpoints;
    }
}
