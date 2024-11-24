using Tyne.HttpMediator.Server;

namespace Tyne.HttpMediator;

public class SimpleRequestHandler : IHttpRequestHandler<SimpleRequest, SimpleResponse>
{
    public Task<HttpResult<SimpleResponse>> Handle(SimpleRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new SimpleResponse
        {
            NewCount = request.Count + 1
        };

        return HttpResult.Codes.OK(response).ToTask();
    }
}
