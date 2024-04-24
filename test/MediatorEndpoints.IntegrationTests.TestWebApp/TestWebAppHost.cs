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
            options.RegisterServicesFromAssemblyContaining<SimpleRequestHandler>()
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

        _ = app.UseRouting();
        _ = app.MapHttpMediatorRequestsFromAssemblyContaining<SimpleRequest>();
        app.Run();
    }
}
