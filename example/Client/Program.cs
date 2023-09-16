using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SqliteWasmHelper;
using Tyne;
using Tyne.Aerospace.Client;
using Tyne.Aerospace.Client.Infrastructure;
using Tyne.Aerospace.Client.Infrastructure.Data;
using Tyne.Aerospace.Client.Infrastructure.Users;
using Tyne.Aerospace.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddScoped<ThemeService>();

builder.Services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<ITyneUserService, UserService>();

builder.Services.AddTyne()
    .ConfigurePageTitle("Tyne:Title")
    .AddEnvironmentService(builder.HostEnvironment.Environment)
    .AddDbContextModificationTracker(ServiceLifetime.Singleton)
    .AddUserTimeZoneFromJavascript();

builder.Services.AddSqliteWasmDbContextFactory<AppDbContext>(options => {
    options.UseSqlite("Data Source=tyne-aerospace.sqlite3");
    // This makes errors more useful
    // Normally it would only be enabled in dev, but there's nothing sensitive in this demo
    options.EnableDetailedErrors().EnableSensitiveDataLogging();
});
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<DataSeeder>().EnsureSeededAsync();
}

await app.RunAsync();

[SuppressMessage("Performance", "CA1852: Seal internal types", Justification = "Irrelevant for the Program.")]
[SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Irrelevant for the Program.")]
partial class Program { }
