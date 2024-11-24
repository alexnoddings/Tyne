using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Tyne.Internal.HttpMediator;

namespace Tyne.HttpMediator.Server;

[SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes.", Justification = $"Instantiated by DI as the {nameof(IHttpResponseResultWriter)} implementation.")]
internal sealed class HttpResponseResultWriter : IHttpResponseResultWriter
{
    private readonly IOptions<JsonSerializerOptions> _jsonSerialiserOptions;

    public HttpResponseResultWriter(IOptions<JsonSerializerOptions> jsonSerialiserOptions)
    {
        _jsonSerialiserOptions = jsonSerialiserOptions ?? throw new ArgumentNullException(nameof(jsonSerialiserOptions));
    }

    public Task WriteAsync<T>(HttpContext httpContext, HttpResult<T> result, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(result);
        httpContext.Response.EnsureHasNotStarted();

        options ??= _jsonSerialiserOptions.Value;
        return WriteCoreAsync(httpContext, result, options, cancellationToken);
    }

    private static async Task WriteCoreAsync<T>(HttpContext httpContext, HttpResult<T> result, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        var response = httpContext.Response;
        response.StatusCode = (int)result.StatusCode;

        // Don't serialise content for 204 (no content) responses
        if (result.StatusCode is HttpStatusCode.NoContent)
            return;

        if (result.IsOk)
        {
            await response.WriteAsJsonAsync(result.Value, options, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            var error = result.Error;
            var problemDetails = new ProblemDetails
            {
                Status = (int)result.StatusCode,
                Detail = error.Message,
                Instance = httpContext.Request.Path
            };
            problemDetails.Extensions[nameof(Error.Code)] = error.Code;
            await response.WriteAsJsonAsync(problemDetails, options, cancellationToken).ConfigureAwait(false);
        }
    }
}
