using System.Net;
using MediatR;

namespace Tyne.MediatorEndpoints;

public class SimpleRequestHandler : IRequestHandler<SimpleRequest, SimpleResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SimpleRequestHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<SimpleResponse> Handle(SimpleRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Simulates server errors
        if (request.Count == SimpleRequest.CountToReturnNoResponse)
        {
            var httpContext = _httpContextAccessor.HttpContext!;
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            await httpContext.Response.StartAsync(cancellationToken);
            return new SimpleResponse();
        }

        if (request.Count == SimpleRequest.CountToReturnWrongResponse)
        {
            var httpContext = _httpContextAccessor.HttpContext!;
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            await httpContext.Response.WriteAsync(@"{ ""this"": ""is not a valid result"" }", cancellationToken);
            return new SimpleResponse();
        }

        var response = new SimpleResponse
        {
            NewCount = request.Count + 1
        };

        return response;
    }
}
