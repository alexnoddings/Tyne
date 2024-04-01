using System.Diagnostics.CodeAnalysis;

namespace Tyne.MediatorEndpoints;

[SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "It's not a utility class.")]
public sealed class TestWebAppHost
{
    public const string ApiBase = "/some-api/base/_path/";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMediatR(static options =>
            options.RegisterServicesFromAssemblyContaining<SimpleRequestHandler>()
        );

        builder.Services
            .AddTyne()
            .AddServerHttpMediator(static builder =>
                builder
                .Configure(static options => options.ApiBase = ApiBase)
                .WithMiddleware(static middleware =>
                    middleware.UseLegacyMediatR()
                )
            );

        builder.Services.AddHttpContextAccessor();

        using var app = builder.Build();

        app.UseRouting();
        app.MapHttpMediatorRequestsFromAssemblyContaining<SimpleRequest>();
        app.Run();
    }
}
