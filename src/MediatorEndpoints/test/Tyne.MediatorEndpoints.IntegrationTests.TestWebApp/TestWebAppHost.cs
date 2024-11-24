using System.Diagnostics.CodeAnalysis;

namespace Tyne.MediatorEndpoints;

[SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "It's not a utility class.")]
public sealed class TestWebAppHost
{
    public const string ApiBase = "/some-api/base/_path/";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.Services.AddMediatR(static options =>
            options.RegisterServicesFromAssemblyContaining<CountRequestHandler>()
        );

        _ = builder.Services
            .AddTyne()
            .AddServerHttpMediator(static builder =>
                builder
                .Configure(static options => options.ApiBase = ApiBase)
                .WithMiddleware(static middleware =>
                    middleware.UseLegacyMediatR()
                )
            );

        _ = builder.Services.AddHttpContextAccessor();

        using var app = builder.Build();

        _ = app.Use(TryBufferResponseAsync);

        _ = app.UseRouting();
        _ = app.MapHttpMediatorRequestsFromAssemblyContaining<CountRequest>();
        app.Run();
    }

    // Changes in .NET 9 to System.Text.Json async stream flushing
    //  caused an issue with TestHost's ResponseBodyWriterStream
    // Buffering the response first, then flushing the buffer
    //  once the response has been written fixes the issue
    // See #164
    private static Task TryBufferResponseAsync(HttpContext httpContext, RequestDelegate next)
    {
        if (httpContext.Request.Path.Value?.Contains(CountRequest.Uri, StringComparison.OrdinalIgnoreCase) == true)
            return BufferResponseAsync(httpContext, next);

        return next(httpContext);
    }

    private static async Task BufferResponseAsync(HttpContext httpContext, RequestDelegate next)
    {
        var response = httpContext.Response;

        var originalBody = response.Body;
        await using var bufferedBody = new MemoryStream();
        response.Body = bufferedBody;

        await next(httpContext);

        _ = bufferedBody.Seek(0, SeekOrigin.Begin);
        response.Body = originalBody;
        await bufferedBody.CopyToAsync(originalBody);
    }
}
