using Microsoft.Extensions.DependencyInjection;
using Tyne.MediatorEndpoints;

namespace Tyne;

public static class ServiceCollectionMediatorExtensions
{
    public static TyneBuilder AddHttpMediatorProxy(this TyneBuilder builder, ServiceLifetime serviceLifetime)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.Add(new ServiceDescriptor(typeof(IMediatorProxy), typeof(HttpMediatorProxy), serviceLifetime));

        return builder;
    }

    public static TyneBuilder AddHttpMediatorProxy(this TyneBuilder builder) =>
        builder.AddHttpMediatorProxy(ServiceLifetime.Scoped);
}
