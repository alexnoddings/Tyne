using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.HttpMediator.Client;

[SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes.", Justification = "Instantiated by DI part of the HTTP Mediator pipeline.")]
internal sealed class FluentValidationMiddleware : IHttpMediatorMiddleware
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationMiddleware(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>
    {
        var validators = _serviceProvider.GetService<IEnumerable<IValidator<TRequest>>>();
        if (validators is not null)
        {
            foreach (var validator in validators)
            {
                var validationResult = await validator.ValidateAsync(request).ConfigureAwait(false);
                if (validationResult.IsValid)
                    continue;

                // Terminate pipeline
                return HttpResult.Codes.BadRequest<TResponse>(validationResult.Errors[0].ErrorMessage);
            }
        }

        return await next(request).ConfigureAwait(false);
    }
}
