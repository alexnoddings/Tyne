using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

[SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes.", Justification = "Instantiated by DI part of the HTTP Mediator pipeline.")]
internal sealed class FluentValidationMiddleware : IHttpMediatorMiddleware
{
    internal const string ErrorCode = "HttpMediatorClient.ValidationError";

    private readonly ILogger? _logger;
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationMiddleware(ILogger<FluentValidationMiddleware>? logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>
    {
        using (var loggerValidationScope = _logger?.BeginValidationScope())
        {
            var validators = _serviceProvider.GetService<IEnumerable<IValidator<TRequest>>>();
            if (validators is null)
            {
                _logger?.LogNoValidators();
            }
            else
            {
                foreach (var validator in validators)
                {
                    _logger?.LogExecutingValidator(validator);
                    var validationResult = await validator.ValidateAsync(request).ConfigureAwait(false);
                    if (validationResult.IsValid)
                        continue;

                    _logger?.LogValidationFailure(validationResult);
                    // Terminate pipeline
                    return HttpResult.Codes.BadRequest<TResponse>(ErrorCode, validationResult.Errors[0].ErrorMessage);
                }
            }
        }

        return await next(request).ConfigureAwait(false);
    }
}
