using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Tyne.HttpMediator.Server;

[SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes.", Justification = "Instantiated by DI part of the HTTP Mediator pipeline.")]
internal sealed class FluentValidationMiddleware : IHttpMediatorMiddleware
{
    public async Task<HttpResult<TResponse>> InvokeAsync<TRequest, TResponse>(HttpContext context, TRequest request, HttpMediatorDelegate<TRequest, TResponse> next) where TRequest : IHttpRequestBase<TResponse>
    {
        var validators = context.RequestServices.GetService<IEnumerable<IValidator<TRequest>>>();
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

        return await next(context, request).ConfigureAwait(false);
    }
}
