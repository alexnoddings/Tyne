using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tyne.Internal.HttpMediator;

namespace Tyne.HttpMediator.Client;

internal sealed class HttpResponseResultReader : IHttpResponseResultReader
{
    private readonly IOptions<JsonSerializerOptions> _jsonSerialiserOptions;
    private readonly ILogger _logger;

    public HttpResponseResultReader(IOptions<JsonSerializerOptions> jsonSerialiserOptions, ILogger<HttpResponseResultReader> logger)
    {
        _jsonSerialiserOptions = jsonSerialiserOptions ?? throw new ArgumentNullException(nameof(jsonSerialiserOptions));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<HttpResult<T>> ReadAsync<T>(HttpResponseMessage response, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(response);

        options ??= _jsonSerialiserOptions.Value;
        return ReadCoreAsync<T>(response, options, cancellationToken);
    }

    private async Task<HttpResult<T>> ReadCoreAsync<T>(HttpResponseMessage response, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        var statusCode = response.StatusCode;
        if (response.IsSuccessStatusCode)
        {
            T? result;
            try
            {
                result = await ReadResponseContentAsAsync<T>().ConfigureAwait(false);
            }
            catch (JsonException jsonException) when (jsonException.Message.StartsWith("The input does not contain any JSON tokens.", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogJsonValueNoInput(jsonException);
                // The server should never return an empty response
                return HttpResult.Codes.InternalServerError<T>("No response content received from server.");
            }
            catch (JsonException jsonException) when (jsonException.Message.StartsWith("The JSON value could not be converted to", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogJsonValueCouldNotBeConverted(jsonException);
                // The server returned a response which we weren't expecting
                // This could be a server error, or it could be the client asking to deserialise the wrong response type
                // We err on the side of a client error
                return HttpResult.Codes.BadRequest<T>("Could not convert response from server.");
            }
            catch (JsonException jsonException)
            {
                _logger.LogJsonValueUnknownError(jsonException);
                // Some other JSON exception
                // We don't know what the cause is, err on the side of a client error
                return HttpResult.Codes.BadRequest<T>("Error converting response from server.");
            }

            if (result is null)
            {
                _logger.LogJsonValueNull();
                return HttpResult.Codes.BadRequest<T>("Invalid response value.");
            }

            return HttpResult.Ok(result, statusCode);
        }

        var problemDetails = await ReadResponseContentAsAsync<ProblemDetails>().ConfigureAwait(false);
        if (problemDetails is null)
        {
            _logger.LogJsonProblemDetailsNull();
            return HttpResult.Error<T>("Invalid response error.", statusCode);
        }

        var message = Internal.Error.MessageOrDefault(problemDetails.Detail ?? problemDetails.Title);
        var code = GetErrorCodeOrDefault(problemDetails);

        // We don't log this error as it has been handled successfully,
        // it's an error in the user's domain, not in our processing
        return HttpResult.Error<T>(code, message, statusCode);

        Task<TContent?> ReadResponseContentAsAsync<TContent>() =>
            response.Content.ReadFromJsonAsync<TContent>(options, cancellationToken);
    }

    private static string GetErrorCodeOrDefault(ProblemDetails problemDetails)
    {
        if (!problemDetails.Extensions.TryGetValue(nameof(Error.Code), out var codeElement))
            return Error.Default.Code;

        if (codeElement is not JsonElement { ValueKind: JsonValueKind.String } jsonString)
            return Error.Default.Code;

        return Internal.Error.CodeOrDefault(jsonString.GetString());
    }
}
