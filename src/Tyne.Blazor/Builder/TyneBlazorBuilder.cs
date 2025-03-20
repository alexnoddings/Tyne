using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor;

/// <summary>
///		Adds Tyne Blazor features.
/// </summary>
public sealed class TyneBlazorBuilder
{
    /// <summary>
    ///		The <see cref="IServiceCollection"/> this represents.
    /// </summary>
    public IServiceCollection Services { get; }

    internal TyneBlazorBuilder(IServiceCollection services)
    {
        Services = services;
    }
}
