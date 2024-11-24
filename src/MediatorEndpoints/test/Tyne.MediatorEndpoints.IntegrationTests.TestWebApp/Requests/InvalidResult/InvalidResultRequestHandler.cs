using System.Net;
using MediatR;

namespace Tyne.MediatorEndpoints;

public class InvalidResultRequestHandler : IRequestHandler<InvalidResultRequest, InvalidResultResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InvalidResultRequestHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<InvalidResultResponse> Handle(InvalidResultRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var httpContext = _httpContextAccessor.HttpContext!;
        httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        await httpContext.Response.WriteAsync(@"{ ""this"": ""is not a valid result"" }", cancellationToken);
        return new InvalidResultResponse();
    }
}
