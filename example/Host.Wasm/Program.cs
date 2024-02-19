using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tyne.Aerospace.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
ExampleProgram.Configure(builder.Configuration);

#if DEBUG
ExampleProgram.ConfigureBuildTag(builder.Services, "WASM");
#endif

builder.RootComponents.Add<ExampleApp>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

ExampleProgram.ConfigureServices(builder.Services, builder.HostEnvironment.Environment);

var app = builder.Build();
await app.RunAsync();
