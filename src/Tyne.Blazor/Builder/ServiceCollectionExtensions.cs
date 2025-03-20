using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor;

/// <summary>
///		Extensions for adding Tyne to an <see cref="IServiceCollection"/>.
/// </summary>
public static class TyneBlazorBuilderServiceCollectionExtensions
{
    /// <summary>
    ///		Adds Tyne Blazor services to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add Tyne Blazor services to.</param>
    /// <returns>A <see cref="TyneBlazorBuilder"/> to add/configure specific Tyne features with.</returns>
    public static TyneBlazorBuilder AddTyneBlazor(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        return new(services);
    }
}
