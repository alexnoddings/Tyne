using Microsoft.Extensions.DependencyInjection;
using Tyne.MediatorEndpoints;
using Tyne.MediatorEndpoints.Http;

namespace Tyne;

public static class ServiceCollectionMediatorExtensions
{
    public static TyneBuilder AddHttpMediatorProxy(this TyneBuilder builder, ServiceLifetime serviceLifetime)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Adds HttpMediatorProxy as a service
        builder.Services.Add(new ServiceDescriptor(typeof(HttpMediatorProxy), typeof(HttpMediatorProxy), serviceLifetime));

        // And IMediatorProxy and IHttpMediatorProxy resolve to the HttpMediatorProxy instance
        builder.Services.Add(new ServiceDescriptor(typeof(IMediatorProxy), static serviceProvider => serviceProvider.GetRequiredService(typeof(HttpMediatorProxy)), serviceLifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(IHttpMediatorProxy), static serviceProvider => serviceProvider.GetRequiredService(typeof(HttpMediatorProxy)), serviceLifetime));

        return builder;
    }

    public static TyneBuilder AddHttpMediatorProxy(this TyneBuilder builder) =>
        builder.AddHttpMediatorProxy(ServiceLifetime.Scoped);
}
