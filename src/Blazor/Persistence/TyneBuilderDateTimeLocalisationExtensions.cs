using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Persistence;

/// <summary>
///     Extensions for registering <see cref="IUrlPersistenceService"/>s with <see cref="TyneBuilder"/>s.
/// </summary>
public static class TyneBuilderPersistenceExtensions
{
    /// <summary>
    ///     Adds an <see cref="IUrlPersistenceService"/> implementation.
    /// </summary>
    /// <param name="tyneBuilder">The <see cref="TyneBuilder"/>.</param>
    /// <returns><paramref name="tyneBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBuilder"/> is <see langword="null"/>.</exception>
    public static TyneBuilder AddUrlPersistenceService(this TyneBuilder tyneBuilder)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);

        _ = tyneBuilder.Services.AddScoped<IUrlPersistenceService, UrlPersistenceService>();

        return tyneBuilder;
    }

    /// <summary>
    ///     Adds an <see cref="IUrlQueryStringFormatter"/> implementation.
    /// </summary>
    /// <param name="tyneBuilder">The <see cref="TyneBuilder"/>.</param>
    /// <returns><paramref name="tyneBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBuilder"/> is <see langword="null"/>.</exception>
    public static TyneBuilder AddUrlQueryStringFormatter(this TyneBuilder tyneBuilder)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);

        _ = tyneBuilder.Services.AddScoped<IUrlQueryStringFormatter, UrlQueryStringFormatter>();

        return tyneBuilder;
    }
}
