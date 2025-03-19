using Tyne;
using Tyne.Blazor;

namespace Microsoft.Extensions.DependencyInjection;

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

        _ = tyneBuilder.Services.AddScoped<IEnvironment, TEnvironmentService>();

        return tyneBuilder;
    }

    public static TyneBuilder AddEnvironmentService(this TyneBuilder tyneBuilder, Func<IServiceProvider, string> getEnvironmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);
        ArgumentNullException.ThrowIfNull(getEnvironmentName);

        _ = tyneBuilder.Services.AddScoped<IEnvironment>(services =>
            new SimpleEnvironment { EnvironmentName = getEnvironmentName(services) }
        );

        return tyneBuilder;
    }

    public static TyneBuilder AddEnvironmentService(this TyneBuilder tyneBuilder, string environmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);
        ArgumentException.ThrowIfNullOrEmpty(environmentName);

        _ = tyneBuilder.Services.AddSingleton<IEnvironment>(new SimpleEnvironment { EnvironmentName = environmentName });

        return tyneBuilder;
    }
}
