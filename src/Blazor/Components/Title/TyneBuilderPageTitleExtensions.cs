using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor;

/// <summary>
///     Page title extensions for <see cref="TyneBuilder"/>.
/// </summary>
public static class TyneBuilderPageTitleExtensions
{
    /// <summary>
    ///     Configures <see cref="TynePageTitleOptions"/> by binding it to the configuration section named <paramref name="configSectionPath"/>.
    /// </summary>
    /// <param name="tyneBuilder">The <see cref="TyneBuilder"/>.</param>
    /// <param name="configSectionPath">The name of the configuration section to configure the <see cref="TynePageTitleOptions"/> with.</param>
    /// <returns>The <paramref name="TyneBuilder"/> to allow for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBuilder"/> is null.</exception>
    public static TyneBuilder ConfigurePageTitle(this TyneBuilder tyneBuilder, string configSectionPath)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);
        ArgumentNullException.ThrowIfNull(configSectionPath);

        tyneBuilder
            .Services
            .AddOptions<TynePageTitleOptions>()
            .Configure<IConfiguration>((options, configuration) => configuration.Bind(configSectionPath, options));

        return tyneBuilder;
    }

    /// <summary>
    ///     Configures <see cref="TynePageTitleOptions"/> by binding it to the <paramref name="configSection"/>.
    /// </summary>
    /// <param name="tyneBuilder">The <see cref="TyneBuilder"/>.</param>
    /// <param name="configSection">The configuration section to configure the <see cref="TynePageTitleOptions"/> with.</param>
    /// <returns>The <paramref name="TyneBuilder"/> to allow for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBuilder"/> is null.</exception>
    public static TyneBuilder ConfigurePageTitle(this TyneBuilder tyneBuilder, IConfigurationSection configSection)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);
        ArgumentNullException.ThrowIfNull(configSection);

        tyneBuilder
            .Services
            .AddOptions<TynePageTitleOptions>()
            .Configure(configSection.Bind);

        return tyneBuilder;
    }

    /// <summary>
    ///     Configures <see cref="TynePageTitleOptions"/> using <paramref name="configure"/>.
    /// </summary>
    /// <param name="tyneBuilder">The <see cref="TyneBuilder"/>.</param>
    /// <param name="configure">An <see cref="Action{T}"/> which configures the <see cref="TynePageTitleOptions"/>.</param>
    /// <returns>The <paramref name="tyneBuilder"/> to allow for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBuilder"/> or <paramref name="configure"/> are null.</exception>
    public static TyneBuilder ConfigurePageTitle(this TyneBuilder tyneBuilder, Action<TynePageTitleOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);
        ArgumentNullException.ThrowIfNull(configure);

        tyneBuilder
            .Services
            .AddOptions<TynePageTitleOptions>()
            .Configure(configure);

        return tyneBuilder;
    }
}
