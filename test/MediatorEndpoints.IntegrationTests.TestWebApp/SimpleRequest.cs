namespace Tyne.MediatorEndpoints;

public class SimpleRequest : IApiRequest<SimpleResponse>
{
    public static string Uri => "testapp/simple_request";

    public const int CountToReturnNoResponse = -1;
    public const int CountToReturnWrongResponse = -2;

    public int Count { get; set; }
}
