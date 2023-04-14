using Microsoft.Extensions.Logging;
using Tyne.MediatorEndpoints;

namespace Tyne;

public static partial class RequestEndpointsExtensions
{
    [LoggerMessage(EventId = 101_001_001, Level = LogLevel.Debug, Message = "Mapping {RequestMethod} '{RequestUri}': {metadata}.")]
    private static partial void LogRequestMapped(this ILogger logger, string requestMethod, string requestUri, object metadata);

    private static void LogRequestMapped<TRequest, TResponse>(this ILogger logger, string apiUri) where TRequest : IApiRequest<TResponse> =>
        logger.LogRequestMapped(
#if Tyne_MediatorEndpoints_GetSupport
			 TRequest.Method.ToString().ToUpperInvariant(),
#else
             "POST",
#endif
             apiUri,
             new
             {
                 RequestType = typeof(TRequest),
                 ResponseType = typeof(TResponse),
             }
        );
}
