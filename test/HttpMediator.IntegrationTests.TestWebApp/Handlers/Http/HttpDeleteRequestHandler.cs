using Tyne.HttpMediator.Server;

namespace Tyne.HttpMediator;

public class HttpDeleteRequestHandler : IHttpRequestHandler<HttpDeleteRequest, HttpDeleteResponse>
{
    public Task<HttpResult<HttpDeleteResponse>> Handle(HttpDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new HttpDeleteResponse
        {
            NewCount = request.Count + 1
        };

        return HttpResult.Codes.OK(response).ToTask();
    }
}
