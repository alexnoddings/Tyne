using Tyne.HttpMediator.Server;

namespace Tyne.HttpMediator;

public class ValidatedRequestHandler : IHttpRequestHandler<ValidatedRequest, ValidatedResponse>
{
    public Task<HttpResult<ValidatedResponse>> Handle(ValidatedRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new ValidatedResponse
        {
            Message = request.Message,
        };

        return HttpResult.Codes.OK(response).ToTask();
    }
}
