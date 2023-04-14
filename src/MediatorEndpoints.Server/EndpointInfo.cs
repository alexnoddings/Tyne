namespace Tyne.MediatorEndpoints;

public record EndpointInfo(Type RequestType, Type ResponseType, ICollection<object> Metadata);
