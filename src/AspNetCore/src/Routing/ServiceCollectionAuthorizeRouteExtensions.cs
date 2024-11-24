using Tyne;
using Tyne.AspNetCore.Routing;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionAuthorizeRouteExtensions
{
    public static TyneBuilder AddRouteAuthorisation(this TyneBuilder builder, Action<IRouteAuthorisationOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        var configuredOptions = new RouteAuthorisationOptions();
        configure(configuredOptions);

        _ = builder.Services.Configure<RouteAuthorisationOptions>(innerOptions =>
            innerOptions.RouteAuthorisations.AddRange(configuredOptions.RouteAuthorisations)
        );

        return builder;
    }
}
