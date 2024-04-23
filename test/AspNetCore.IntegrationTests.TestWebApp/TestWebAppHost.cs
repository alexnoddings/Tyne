using System.Diagnostics.CodeAnalysis;

namespace Tyne.AspNetCore;

[SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "It's not a utility class.")]
public sealed class TestWebAppHost
{
    public const string NotFoundSyncHandlerHeaderKey = "NotFoundSyncHeader";
    public const string NotFoundAsyncHandlerBodyMessage = "Not found async handler content";

    public const int InvalidTestRequestStatusCode = 418;
    public const string InvalidTestRequestBodyMessage = "invalid test request";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.Services.AddControllersWithViews();
        _ = builder.Services.AddRazorPages();

        using var app = builder.Build();

        _ = app.UseRouting();

        _ = app.MapFallbackToNotFound("/test/notfound/default/{**_}");
        _ = app.MapFallbackToNotFound("/test/notfound/sync-handler/{**_}", httpContext => httpContext.Response.Headers.Append(NotFoundSyncHandlerHeaderKey, "true"));
        _ = app.MapFallbackToNotFound("/test/notfound/async-handler/{**_}", async httpContext => await httpContext.Response.WriteAsync(NotFoundAsyncHandlerBodyMessage));
        _ = app.MapFallback("{**_}", async httpContext =>
            {
                httpContext.Response.StatusCode = InvalidTestRequestStatusCode;
                await httpContext.Response.WriteAsync(InvalidTestRequestBodyMessage);
            });

        app.Run();
    }
}
