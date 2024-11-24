using System.Reflection;
using Microsoft.Extensions.Logging;
using Tyne.HttpMediator;

namespace Microsoft.AspNetCore.Builder;

internal static partial class WebApplicationEndpointMappingExtensionsLogging
{
    [LoggerMessage(EventId = 101_003_000, Level = LogLevel.Debug, Message = "Mapping {RequestMethod} '{RequestUri}' to '{TRequest}' -> '{TResponse}' with {MetadataCount} metadata.")]
    private static partial void LogRequestMapped(this ILogger logger, string requestMethod, string requestUri, Type tRequest, Type tResponse, int metadataCount);

    public static void LogRequestMapped<TRequest, TResponse>(this ILogger logger, string apiUri, ICollection<object> metadata) where TRequest : IHttpRequestBase<TResponse> =>
        LogRequestMapped(
            logger,
             TRequest.Method.ToString().ToUpperInvariant(),
             apiUri,
             typeof(TRequest),
             typeof(TResponse),
             metadata.Count
        );

    [LoggerMessage(EventId = 101_003_001, Level = LogLevel.Debug, Message = "Mapped {RequestCount} requests from assembly {AssemblyName}.")]
    private static partial void LogRequestsMapped(this ILogger logger, string assemblyName, int requestCount);

    public static void LogRequestsMapped(this ILogger logger, Assembly assembly, int requestCount) =>
        LogRequestsMapped(
            logger,
            assembly.GetName().Name ?? assembly.GetName().FullName,
            requestCount
        );
}
