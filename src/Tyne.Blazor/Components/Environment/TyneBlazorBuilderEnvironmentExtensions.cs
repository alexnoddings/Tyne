using Tyne.Blazor;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for adding <see cref="IEnvironment"/> to <see cref="TyneBlazorBuilder"/>.
/// </summary>
public static class TyneBlazorBuilderEnvironmentExtensions
{
    private sealed class DefaultEnvironmentImpl : IEnvironment
    {
        public required string EnvironmentName { get; init; }
    }

    public static TyneBlazorBuilder AddEnvironment<TEnvironment>(this TyneBlazorBuilder tyneBlazorBuilder) where TEnvironment : class, IEnvironment
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);

        tyneBlazorBuilder.Services.AddScoped<IEnvironment, TEnvironment>();

        return tyneBlazorBuilder;
    }

    public static TyneBlazorBuilder AddEnvironment(this TyneBlazorBuilder tyneBlazorBuilder, Func<IServiceProvider, string> getEnvironmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);
        ArgumentNullException.ThrowIfNull(getEnvironmentName);

        tyneBlazorBuilder.Services.AddScoped<IEnvironment>(services =>
        {
            var environmentName = getEnvironmentName(services);
            return new DefaultEnvironmentImpl { EnvironmentName = environmentName };
        });

        return tyneBlazorBuilder;
    }

    public static TyneBlazorBuilder AddEnvironment(this TyneBlazorBuilder tyneBlazorBuilder, string environmentName)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);
        ArgumentException.ThrowIfNullOrEmpty(environmentName);

        var environmentInstance = new DefaultEnvironmentImpl { EnvironmentName = environmentName };
        tyneBlazorBuilder.Services.AddScoped<IEnvironment>(_ => environmentInstance);

        return tyneBlazorBuilder;
    }
}
