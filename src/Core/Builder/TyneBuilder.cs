using Microsoft.Extensions.DependencyInjection;

namespace Tyne;

/// <summary>
///		Used to build Tyne-specific functionality.
/// </summary>
public sealed class TyneBuilder
{
    /// <summary>
    ///		The <see cref="IServiceCollection"/> this represents.
    /// </summary>
    public IServiceCollection Services { get; }

    internal TyneBuilder(IServiceCollection services)
    {
        Services = services;
    }
}
