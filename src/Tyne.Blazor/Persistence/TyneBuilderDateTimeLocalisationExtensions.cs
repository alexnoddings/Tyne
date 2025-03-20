using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

/// <summary>
///     Extensions for registering <see cref="IUrlPersistenceService"/>s with <see cref="TyneBlazorBuilder"/>s.
/// </summary>
public static class TyneBlazorBuilderPersistenceExtensions
{
    /// <summary>
    ///     Adds an <see cref="IUrlPersistenceService"/> implementation.
    /// </summary>
    /// <param name="tyneBlazorBuilder">The <see cref="TyneBlazorBuilder"/>.</param>
    /// <returns><paramref name="tyneBlazorBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBlazorBuilder"/> is <see langword="null"/>.</exception>
    public static TyneBlazorBuilder AddUrlPersistenceService(this TyneBlazorBuilder tyneBlazorBuilder)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);

        _ = tyneBlazorBuilder.Services.AddScoped<IUrlPersistenceService, UrlPersistenceService>();

        return tyneBlazorBuilder;
    }

    /// <summary>
    ///     Adds an <see cref="IUrlQueryStringFormatter"/> implementation.
    /// </summary>
    /// <param name="tyneBlazorBuilder">The <see cref="TyneBlazorBuilder"/>.</param>
    /// <returns><paramref name="tyneBlazorBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBlazorBuilder"/> is <see langword="null"/>.</exception>
    public static TyneBlazorBuilder AddUrlQueryStringFormatter(this TyneBlazorBuilder tyneBlazorBuilder)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);

        _ = tyneBlazorBuilder.Services.AddScoped<IUrlQueryStringFormatter, UrlQueryStringFormatter>();

        return tyneBlazorBuilder;
    }
}
