using Tyne;
using Tyne.Blazor;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for adding <see cref="IEnvironment"/> to <see cref="TyneBlazorBuilder"/>.
/// </summary>
public static class TyneBlazorBuilderEnvironmentExtensions
{
    private sealed class SimpleEnvironment : IEnvironment
    {
        public required string EnvironmentName { get; init; }
    }

    public static TyneBlazorBuilder AddEnvironmentService<TEnvironmentService>(this TyneBlazorBuilder tyneBlazorBuilder) where TEnvironmentService : class, IEnvironment
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);

        _ = tyneBlazorBuilder.Services.AddScoped<IEnvironment, TEnvironmentService>();

        return tyneBlazorBuilder;
    }

    public static TyneBlazorBuilder AddEnvironmentService(this TyneBlazorBuilder tyneBlazorBuilder, Func<IServiceProvider, string> getEnvironmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);
        ArgumentNullException.ThrowIfNull(getEnvironmentName);

        _ = tyneBlazorBuilder.Services.AddScoped<IEnvironment>(services =>
            new SimpleEnvironment { EnvironmentName = getEnvironmentName(services) }
        );

        return tyneBlazorBuilder;
    }

    public static TyneBlazorBuilder AddEnvironmentService(this TyneBlazorBuilder tyneBlazorBuilder, string environmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);
        ArgumentException.ThrowIfNullOrEmpty(environmentName);

        _ = tyneBlazorBuilder.Services.AddSingleton<IEnvironment>(new SimpleEnvironment { EnvironmentName = environmentName });

        return tyneBlazorBuilder;
    }
}
