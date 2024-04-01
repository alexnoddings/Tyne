using Microsoft.Extensions.DependencyInjection;
using Tyne.MediatorEndpoints;

namespace Tyne;

public static class ServiceCollectionMediatorExtensions
{
    public static TyneBuilder AddLegacyClientMediatorEndpointsCompatibility(this TyneBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddScoped<IMediatorProxy, MediatorProxy>();

        return builder;
    }
}
