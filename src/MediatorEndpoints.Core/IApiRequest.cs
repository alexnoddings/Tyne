using MediatR;

namespace Tyne.MediatorEndpoints;

public interface IApiRequest : IApiRequest<Unit>
{
}
