using Tyne.HttpMediator.Server;

namespace Tyne.HttpMediator;

public class HttpGetRequestHandler : IHttpRequestHandler<HttpGetRequest, HttpGetResponse>
{
    public Task<HttpResult<HttpGetResponse>> Handle(HttpGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new HttpGetResponse
        {
            NewCount = request.Count + 1
        };

        return HttpResult.Codes.OK(response).ToTask();
    }
}
