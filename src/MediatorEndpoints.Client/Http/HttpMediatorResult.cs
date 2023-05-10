namespace Tyne.MediatorEndpoints.Http;

public record class HttpMediatorResult<TResponse>(TResponse? Response, HttpResponseMessage HttpResponse);
