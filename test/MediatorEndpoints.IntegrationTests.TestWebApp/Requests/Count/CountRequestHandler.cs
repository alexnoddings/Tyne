using MediatR;

namespace Tyne.MediatorEndpoints;

public class CountRequestHandler : IRequestHandler<CountRequest, CountResponse>
{
    public Task<CountResponse> Handle(CountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = new CountResponse
        {
            NewCount = request.Count + 1
        };

        return Task.FromResult(response);
    }
}
