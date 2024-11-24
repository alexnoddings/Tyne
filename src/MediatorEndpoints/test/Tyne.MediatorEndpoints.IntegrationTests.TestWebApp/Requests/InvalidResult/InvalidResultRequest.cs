namespace Tyne.MediatorEndpoints;

public class InvalidResultRequest : IApiRequest<InvalidResultResponse>
{
    public static string Uri => "testapp/invalid-result";
}
