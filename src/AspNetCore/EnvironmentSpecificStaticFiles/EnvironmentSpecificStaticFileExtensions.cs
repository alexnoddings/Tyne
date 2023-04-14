using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tyne.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

public static class EnvironmentSpecificStaticFileExtensions
{
    public static IEndpointConventionBuilder MapEnvironmentSpecificStaticFile(this WebApplication app, string requestPattern, string newPathTemplate)
    {
        ArgumentException.ThrowIfNullOrEmpty(requestPattern);
        ArgumentException.ThrowIfNullOrEmpty(newPathTemplate);

        void Configure(EnvironmentSpecificStaticFileOptions options)
        {
            options.RequestPattern = requestPattern;
            options.NewPathTemplate = newPathTemplate;
        }

        return MapEnvironmentSpecificStaticFile(app, Configure);
    }

    public static IEndpointConventionBuilder MapEnvironmentSpecificStaticFile(this WebApplication app, Action<EnvironmentSpecificStaticFileOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(configure);

        // Get the current environment name
        var environmentName = app.Environment.EnvironmentName;

        // Strip any invalid characters
        var invalidFileNameChars = Path.GetInvalidFileNameChars();
        foreach (var invalidFileNameChar in invalidFileNameChars)
            environmentName = environmentName.Replace(invalidFileNameChar, '_');

        var options = new EnvironmentSpecificStaticFileOptions();
        configure(options);
        options.Validate();

        var requestPattern = options.RequestPattern;
        var newPath = string.Format(CultureInfo.InvariantCulture, options.NewPathTemplate, environmentName);
        var requestDelegate = CreateRequestDelegate(app, newPath);

        return app.Map(requestPattern, requestDelegate);
    }

    private static RequestDelegate CreateRequestDelegate(IEndpointRouteBuilder endpoints, string newRequestPath)
    {
        var app = endpoints.CreateApplicationBuilder();
        app.Use(next => context =>
        {
            // Change the request path
            context.Request.Path = newRequestPath;

            // Set endpoint to null so the static files middleware will handle the request.
            context.SetEndpoint(null);

            // Let the static files middleware handle it
            return next(context);
        });

        app.UseStaticFiles();

        return app.Build();
    }
}
