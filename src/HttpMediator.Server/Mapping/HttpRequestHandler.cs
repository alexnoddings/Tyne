using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tyne.HttpMediator.Server;

internal static class HttpRequestHandler
{
    internal static Task HandleRequestFromQueryAsync<TRequest, TResponse>(HttpContext httpContext) where TRequest : IHttpRequestBase<TResponse>
    {
        if (!httpContext.Request.Query.TryGetValue("request", out var requestStrValues))
            return WriteBadRequestResult(ExceptionMessages.HandleRequest_NoQueryParameter);

        if (requestStrValues.OfType<string>().FirstOrDefault() is not string requestStr)
            return WriteBadRequestResult(ExceptionMessages.HandleRequest_NoQueryParameter);

        var jsonOptions = httpContext.RequestServices.GetService<IOptions<JsonSerializerOptions>>();
        var request = JsonSerializer.Deserialize<TRequest>(requestStr, jsonOptions?.Value);

        // Explicitly treat result as not-null, the middleware is expected to handle null values
        // (this standardises both the query/body approaches)
        return HandleRequestCoreAsync<TRequest, TResponse>(httpContext, request!);

        Task WriteBadRequestResult(string message)
        {
            var result = HttpResult.Error<TResponse>(message, HttpStatusCode.BadRequest);
            var resultWrite = httpContext.RequestServices.GetRequiredService<IHttpResponseResultWriter>();
            return resultWrite.WriteAsync(httpContext, result);
        }
    }

    internal static Task HandleRequestFromBodyAsync<TRequest, TResponse>(HttpContext httpContext, [FromBody] TRequest request) where TRequest : IHttpRequestBase<TResponse> =>
        HandleRequestCoreAsync<TRequest, TResponse>(httpContext, request);

    internal static async Task HandleRequestCoreAsync<TRequest, TResponse>(HttpContext httpContext, TRequest request) where TRequest : IHttpRequestBase<TResponse>
    {
        var resultWriter = httpContext.RequestServices.GetRequiredService<IHttpResponseResultWriter>();
        var loggerFactory = httpContext.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(HttpRequestHandler));
        using var _ = logger.BeginScope("HTTP Mediator request");

        HttpResult<TResponse> result;
        if (request is null)
        {
            logger.LogNullRequest();
            result = HttpResult.Codes.BadRequest<TResponse>("Request was empty or could not be parsed.");
        }
        else
        {
            logger.LogHandlingRequest<TRequest>();
            var pipeline = httpContext.RequestServices.GetRequiredService<HttpMediatorMiddlewarePipeline>();
            result = await pipeline.ExecuteAsync<TRequest, TResponse>(httpContext, request).ConfigureAwait(false);
        }

        // If the response has already started, then we assume the pipeline has short-circuited and written to it directly
        if (!httpContext.Response.HasStarted)
            await resultWriter.WriteAsync(httpContext, result).ConfigureAwait(false);
    }
}
