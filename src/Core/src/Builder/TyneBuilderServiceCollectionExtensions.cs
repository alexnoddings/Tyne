using Tyne;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///		Extensions for adding Tyne to an <see cref="IServiceCollection"/>.
/// </summary>
public static class TyneBuilderServiceCollectionExtensions
{
    /// <summary>
    ///		Adds Tyne to the <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add Tyne to.</param>
    /// <returns>A <see cref="TyneBuilder"/> to add more Tyne functionality to.</returns>
    public static TyneBuilder AddTyne(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return new(services);
    }
}
