using Microsoft.Extensions.Configuration;
using Tyne.Blazor;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Page title extensions for <see cref="TyneBlazorBuilder"/>.
/// </summary>
public static class TyneBlazorBuilderPageTitleExtensions
{
    /// <summary>
    ///     Configures <see cref="TynePageTitleOptions"/> by binding it to the configuration section named <paramref name="configSectionPath"/>.
    /// </summary>
    /// <param name="tyneBlazorBuilder">The <see cref="TyneBlazorBuilder"/>.</param>
    /// <param name="configSectionPath">The name of the configuration section to configure the <see cref="TynePageTitleOptions"/> with.</param>
    /// <returns>The <paramref name="tyneBlazorBuilder"/> to allow for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBlazorBuilder"/> is null.</exception>
    public static TyneBlazorBuilder ConfigurePageTitle(this TyneBlazorBuilder tyneBlazorBuilder, string configSectionPath)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);
        ArgumentNullException.ThrowIfNull(configSectionPath);

        tyneBlazorBuilder
            .Services
            .AddOptions<TynePageTitleOptions>()
            .Configure<IConfiguration>((options, configuration) => configuration.Bind(configSectionPath, options));

        return tyneBlazorBuilder;
    }

    /// <summary>
    ///     Configures <see cref="TynePageTitleOptions"/> by binding it to the <paramref name="configSection"/>.
    /// </summary>
    /// <param name="tyneBlazorBuilder">The <see cref="TyneBlazorBuilder"/>.</param>
    /// <param name="configSection">The configuration section to configure the <see cref="TynePageTitleOptions"/> with.</param>
    /// <returns>The <paramref name="tyneBlazorBuilder"/> to allow for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBlazorBuilder"/> is null.</exception>
    public static TyneBlazorBuilder ConfigurePageTitle(this TyneBlazorBuilder tyneBlazorBuilder, IConfigurationSection configSection)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);
        ArgumentNullException.ThrowIfNull(configSection);

        tyneBlazorBuilder
            .Services
            .AddOptions<TynePageTitleOptions>()
            .Configure(configSection.Bind);

        return tyneBlazorBuilder;
    }

    /// <summary>
    ///     Configures <see cref="TynePageTitleOptions"/> using <paramref name="configure"/>.
    /// </summary>
    /// <param name="tyneBlazorBuilder">The <see cref="TyneBlazorBuilder"/>.</param>
    /// <param name="configure">An <see cref="Action{T}"/> which configures the <see cref="TynePageTitleOptions"/>.</param>
    /// <returns>The <paramref name="tyneBlazorBuilder"/> to allow for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBlazorBuilder"/> or <paramref name="configure"/> are null.</exception>
    public static TyneBlazorBuilder ConfigurePageTitle(this TyneBlazorBuilder tyneBlazorBuilder, Action<TynePageTitleOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);
        ArgumentNullException.ThrowIfNull(configure);

        tyneBlazorBuilder
            .Services
            .AddOptions<TynePageTitleOptions>()
            .Configure(configure);

        return tyneBlazorBuilder;
    }
}
