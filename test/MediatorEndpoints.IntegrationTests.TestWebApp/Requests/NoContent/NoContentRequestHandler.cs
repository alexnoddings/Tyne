using System.Net;
using MediatR;

namespace Tyne.MediatorEndpoints;

public class NoContentRequestHandler : IRequestHandler<NoContentRequest, NoContentResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NoContentRequestHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<NoContentResponse> Handle(NoContentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var httpContext = _httpContextAccessor.HttpContext!;
        httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        // Response model won't be written if the HTTP response has already started
        await httpContext.Response.StartAsync(cancellationToken);
        return new NoContentResponse();
    }
}
