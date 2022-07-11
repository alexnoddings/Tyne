using Blazored.LocalStorage;
using ME.Web.Infrastructure.Theme;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Tyne.Docs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
// Need to native link in sqlite to use
//builder.Services.AddDbContextFactory<DocsDbContext>(options =>
//	options.UseSqlite("Filename=app.db")
//	.EnableDetailedErrors()
//	.EnableSensitiveDataLogging()
//);
builder.Services.AddScoped<ThemeService>();
builder.Services.AddTyneTitle("Tyne Docs", " - ");

await builder.Build().RunAsync();
