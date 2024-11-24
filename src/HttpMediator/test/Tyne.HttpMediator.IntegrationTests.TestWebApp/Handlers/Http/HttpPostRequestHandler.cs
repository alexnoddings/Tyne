using Tyne.HttpMediator.Server;

namespace Tyne.HttpMediator;

public class HttpPostRequestHandler : IHttpRequestHandler<HttpPostRequest, HttpPostResponse>
{
    public Task<HttpResult<HttpPostResponse>> Handle(HttpPostRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new HttpPostResponse
        {
            NewCount = request.Count + 1
        };

        return HttpResult.Codes.OK(response).ToTask();
    }
}
