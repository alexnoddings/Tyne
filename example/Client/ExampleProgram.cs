using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Tyne.Aerospace.Client.Infrastructure;
using Tyne.Aerospace.Client.Infrastructure.Layouts;

namespace Tyne.Aerospace.Client;

public static class ExampleProgram
{
    public static void Configure(IConfigurationBuilder configuration)
    {
        var initialData = GetConfigData().Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value));
        _ = configuration.AddInMemoryCollection(initialData);

        static IEnumerable<(string Key, string? Value)> GetConfigData()
        {
            yield return ("Tyne:Title:Empty", "Tyne Aerospace 🚀");
            yield return ("Tyne:Title:Format", "{0} 🚀 Tyne Aerospace");
        }
    }

    public static void ConfigureBuildTag(IServiceCollection services, string tag) =>
        services.Configure<BuildTag>(buildTag => buildTag.Tag = tag);

    public static void ConfigureServices(IServiceCollection services, string environmentName)
    {
        _ = services
            .AddMudServices()
            .AddScoped<ThemeService>();

        _ = services
            .AddMediatR(options => options.RegisterServicesFromAssemblyContaining<ExampleApp>())
            .AddValidatorsFromAssemblyContaining<ExampleApp>();

        _ = services
            .AddTyne()
            .ConfigurePageTitle("Tyne:Title")
            .AddEnvironmentService(environmentName)
            .AddUserTimeZoneFromJavascript()
            .AddUrlQueryStringFormatter()
            .AddUrlPersistenceService();
    }
}
