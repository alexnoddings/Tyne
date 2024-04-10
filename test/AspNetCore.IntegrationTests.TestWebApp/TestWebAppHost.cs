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

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        using var app = builder.Build();

        app.UseRouting();

        app.MapFallbackToNotFound("/test/notfound/default/{**_}");
        app.MapFallbackToNotFound("/test/notfound/sync-handler/{**_}", httpContext => httpContext.Response.Headers.Append(NotFoundSyncHandlerHeaderKey, "true"));
        app.MapFallbackToNotFound("/test/notfound/async-handler/{**_}", async httpContext => await httpContext.Response.WriteAsync(NotFoundAsyncHandlerBodyMessage));

        app.MapFallback("{**_}", async httpContext =>
        {
            httpContext.Response.StatusCode = InvalidTestRequestStatusCode;
            await httpContext.Response.WriteAsync(InvalidTestRequestBodyMessage);
        });

        app.Run();
    }
}
