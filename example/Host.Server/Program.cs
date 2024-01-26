using Tyne.Aerospace.Client;

var builder = WebApplication.CreateBuilder(args);
ExampleProgram.Configure(builder.Configuration);

#if DEBUG
ExampleProgram.ConfigureBuildTag(builder.Services, "Server");
#endif

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

ExampleProgram.ConfigureServices(builder.Services, builder.Environment.EnvironmentName);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
