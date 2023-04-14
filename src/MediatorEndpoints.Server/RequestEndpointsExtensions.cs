using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Tyne.MediatorEndpoints;

namespace Tyne;

public static partial class RequestEndpointsExtensions
{
    public static IEndpointRouteBuilder MapRequestsFromAssembly(this WebApplication app, Assembly assembly, string apiBase)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(apiBase);

        var logger = app.Services.GetService<ILoggerFactory>()?.CreateLogger("Tyne.MediatorEndpoints");

        foreach (var endpointInfo in EndpointInfos.GetFromAssembly(assembly))
            app.MapRequestGeneric(endpointInfo, apiBase, logger);

        return app;
    }

    public static IEndpointRouteBuilder MapRequestsFromAssemblyContaining<T>(this WebApplication app, string apiBase) =>
        MapRequestsFromAssembly(app, typeof(T).Assembly, apiBase);

    private static readonly MethodInfo MapRequestGenericMethodInfo =
        MethodHelper.Get(
              typeof(RequestEndpointsExtensions),
              nameof(MapRequest),
              BindingFlags.Static | BindingFlags.Public,
              typeof(IEndpointRouteBuilder), typeof(EndpointInfo), typeof(string), typeof(ILogger)
        );

    private static IEndpointRouteBuilder MapRequestGeneric(this IEndpointRouteBuilder endpoints, EndpointInfo endpointInfo, string apiBase, ILogger? logger = null)
    {
        MapRequestGenericMethodInfo
            .MakeGenericMethod(endpointInfo.RequestType, endpointInfo.ResponseType)
            .Invoke(null, new object?[] { endpoints, endpointInfo, apiBase, logger });

        return endpoints;
    }

    private static string FormatApiUri<TRequest>(string apiBase) where TRequest : IApiRequestMetadata =>
        Path.Combine(apiBase, TRequest.Uri.TrimStart('/'));

    public static IEndpointRouteBuilder MapRequest<TRequest, TResponse>(this IEndpointRouteBuilder endpoints, EndpointInfo endpointInfo, string apiBase, ILogger? logger = null) where TRequest : IApiRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(endpointInfo);

        var uri = FormatApiUri<TRequest>(apiBase);
        logger?.LogRequestMapped<TRequest, TResponse>(uri);

#if Tyne_MediatorEndpoints_GetSupport
		var routeBuilder = TRequest.Method switch
		{
			ApiRequestMethod.Get => endpoints.MapGet(uri, HandleGetAsync<TRequest, TResponse>),
            ApiRequestMethod.Post => endpoints.MapPost(uri, HandlePostAsync<TRequest, TResponse>),
			_ => throw new InvalidOperationException($"Unexpected {nameof(ApiRequestMethod)} for {typeof(TRequest).Name}: {TRequest.Method}.")
        };
#else
        var routeBuilder = endpoints.MapPost(uri, HandlePostAsync<TRequest, TResponse>);
#endif

        routeBuilder.Add(builder =>
        {
            foreach (var metadata in endpointInfo.Metadata)
                builder.Metadata.Add(metadata);
        });

        return endpoints;
    }

    // GET endpoints aren't supported currently
    // Minimal APIs don't support custom IBindingSourceMetadata or IModelBinder, so we can't implement a custom model binder
    // [FromQuery] currently only supports simple (i.e. not complex object) binding for GETs: https://github.com/dotnet/aspnetcore/issues/42438
#if Tyne_MediatorEndpoints_GetSupport
    private static Task<TResponse?> HandleGetAsync<TRequest, TResponse>(
		[FromQuery][Xxx] TRequest request,
		[FromServices] IEnumerable<IValidator<TRequest>> validators,
		[FromServices] IMediator mediator
	) where TRequest : IApiRequest<TResponse> =>
        HandleInnerAsync<TRequest, TResponse>(request, validators, mediator);
#endif

    private static Task<TResponse?> HandlePostAsync<TRequest, TResponse>(
        [FromBody] TRequest request,
        [FromServices] IEnumerable<IValidator<TRequest>> validators,
        [FromServices] IMediator mediator
    ) where TRequest : IApiRequest<TResponse> =>
        HandleInnerAsync<TRequest, TResponse>(request, validators, mediator);

    private static async Task<TResponse?> HandleInnerAsync<TRequest, TResponse>(TRequest request, IEnumerable<IValidator<TRequest>> validators, IMediator mediator) where TRequest : IApiRequest<TResponse>
    {
        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(request).ConfigureAwait(false);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }

        return await mediator.Send(request).ConfigureAwait(false);
    }
}
