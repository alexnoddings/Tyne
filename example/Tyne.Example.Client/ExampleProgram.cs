using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Tyne.Blazor.Persistence;
using Tyne.Example.Client.Infrastructure;
using Tyne.Example.Client.Infrastructure.Layouts;

namespace Tyne.Example.Client;

public static class ExampleProgram
{
    public static void Configure(IConfigurationBuilder configuration)
    {
        var initialData = GetConfigData().Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value));
        _ = configuration.AddInMemoryCollection(initialData);

        static IEnumerable<(string Key, string? Value)> GetConfigData()
        {
            yield return ("Tyne:Title:Empty", "Tyne Blazor Docs 🚀");
            yield return ("Tyne:Title:Format", "{0} 🚀 Tyne Blazor Docs");
        }
    }

    public static void ConfigureBuildTag(IServiceCollection services, string tag) =>
        services.Configure<BuildTag>(buildTag => buildTag.Tag = tag);

    public static void ConfigureServices(IServiceCollection services, string environmentName)
    {
        _ = services
            .AddMudServices()
            .AddScoped<ThemeService>();

        _ = services.AddValidatorsFromAssemblyContaining<ExampleApp>();

        _ = services
            .AddTyneBlazor()
            .ConfigurePageTitle("Tyne:Title")
            .AddEnvironment(environmentName)
            .AddUserTimeZoneFromJavascript()
            .AddUrlQueryStringFormatter()
            .AddUrlPersistenceService();
    }
}
