using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tyne.HttpMediator.Client;

public sealed class HttpMediatorClientTestScope : IDisposable
{
    public const string BaseAddress = "http://127.0.0.101";
    public const string ApiBase = "/unit-tests/";

    public static readonly JsonSerializerOptions JsonSerializerOptions = CreateJsonSerialiserOptions();

    public IHttpMediator Mediator { get; }
    public MockHttpMessageHandler Http { get; }
    public IServiceProvider RootServices { get; }
    private readonly IServiceScope _servicesScope;
    public IServiceProvider Services => _servicesScope.ServiceProvider;

    private HttpMediatorClientTestScope(IHttpMediator mediator, MockHttpMessageHandler http, IServiceProvider rootServices, IServiceScope servicesScope)
    {
        Mediator = mediator;
        Http = http;
        RootServices = rootServices;
        _servicesScope = servicesScope;
    }

    public static HttpMediatorClientTestScope Create(
        Action<MockHttpMessageHandler>? http = null,
        Action<HttpMediatorOptions>? options = null,
        Action<HttpMediatorMiddlewareBuilder>? middleware = null
    )
    {
        var mockHttpHandler = new MockHttpMessageHandler();
        http?.Invoke(mockHttpHandler);

        options ??= ConfigureDefaultOptions;
        middleware ??= ConfigureDefaultMiddleware;

        var services = new ServiceCollection();

        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        services.Add(new ServiceDescriptor(typeof(ILogger), typeof(NullLogger), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ILogger<>), typeof(NullLogger<>), ServiceLifetime.Transient));

        services.AddOptions<JsonSerializerOptions>().Configure(ConfigureJsonSerialiserOptions);
        services.AddValidatorsFromAssemblyContaining<ValidatedRequestValidator>();

        var httpClient = mockHttpHandler.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseAddress);
        services.AddSingleton(httpClient);

        services
            .AddTyne()
            .AddClientHttpMediator(builder =>
                builder
                .Configure(options)
                .WithMiddleware(middleware)
            );

        var rootServiceProvider = services.BuildServiceProvider(validateScopes: true);
        var serviceScope = rootServiceProvider.CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IHttpMediator>();

        return new(mediator, mockHttpHandler, rootServiceProvider, serviceScope);
    }

    private static void ConfigureDefaultOptions(HttpMediatorOptions options) =>
        options.ApiBase = ApiBase;

    private static void ConfigureDefaultMiddleware(HttpMediatorMiddlewareBuilder builder) =>
        builder.Send();

    private static JsonSerializerOptions CreateJsonSerialiserOptions()
    {
        var options = new JsonSerializerOptions();
        ConfigureJsonSerialiserOptions(options);
        return options;
    }

    private static void ConfigureJsonSerialiserOptions(JsonSerializerOptions options)
    {
        // Uses JsonSerializerDefaults.Web
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    }

    public void Dispose()
    {
        Http.Dispose();
        _servicesScope.Dispose();

        if (RootServices is IDisposable disposableRootServices)
            disposableRootServices.Dispose();
    }
}
