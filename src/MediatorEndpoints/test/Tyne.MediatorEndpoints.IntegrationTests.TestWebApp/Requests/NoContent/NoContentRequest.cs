namespace Tyne.MediatorEndpoints;

public class NoContentRequest : IApiRequest<NoContentResponse>
{
    public static string Uri => "testapp/no-content";
}
