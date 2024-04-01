using Tyne.HttpMediator.Server;

namespace Tyne.HttpMediator;

public class HttpPatchRequestHandler : IHttpRequestHandler<HttpPatchRequest, HttpPatchResponse>
{
    public Task<HttpResult<HttpPatchResponse>> Handle(HttpPatchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new HttpPatchResponse
        {
            NewCount = request.Count + 1
        };

        return HttpResult.Codes.OK(response).ToTask();
    }
}
