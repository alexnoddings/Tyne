using System.Diagnostics.CodeAnalysis;

namespace Tyne.AspNetCore.TestApp;

[SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "It's not a utility class.")]
public sealed class TestWebAppHost
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddProblemDetails();

        using var app = builder.Build();

        app.UseRouting();

        app.Run();
    }
}
