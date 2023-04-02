using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor;

/// <summary>
///     Extensions for adding <see cref="IEnvironment"/> to <see cref="TyneBuilder"/>.
/// </summary>
public static class TyneBuilderEnvironmentExtensions
{
    private sealed class SimpleEnvironment : IEnvironment
    {
        public required string EnvironmentName { get; init; }
    }

    public static TyneBuilder AddEnvironmentService<TEnvironmentService>(this TyneBuilder tyneBuilder) where TEnvironmentService : class, IEnvironment
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);

        tyneBuilder.Services.AddScoped<IEnvironment, TEnvironmentService>();

        return tyneBuilder;
    }

    public static TyneBuilder AddEnvironmentService(this TyneBuilder tyneBuilder, Func<IServiceProvider, string> getEnvironmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);
        ArgumentNullException.ThrowIfNull(getEnvironmentName);

        tyneBuilder.Services.AddScoped<IEnvironment>(services =>
        {
            var environmentName = getEnvironmentName(services);
            return new SimpleEnvironment { EnvironmentName = environmentName };
        });

        return tyneBuilder;
    }

    public static TyneBuilder AddEnvironmentService(this TyneBuilder tyneBuilder, string environmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);
        ArgumentException.ThrowIfNullOrEmpty(environmentName);

        tyneBuilder.Services.AddSingleton<IEnvironment>(new SimpleEnvironment { EnvironmentName = environmentName });

        return tyneBuilder;
    }
}
