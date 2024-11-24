using Tyne.HttpMediator.Server;

namespace Tyne.HttpMediator;

public class HttpPutRequestHandler : IHttpRequestHandler<HttpPutRequest, HttpPutResponse>
{
    public Task<HttpResult<HttpPutResponse>> Handle(HttpPutRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new HttpPutResponse
        {
            NewCount = request.Count + 1
        };

        return HttpResult.Codes.OK(response).ToTask();
    }
}
