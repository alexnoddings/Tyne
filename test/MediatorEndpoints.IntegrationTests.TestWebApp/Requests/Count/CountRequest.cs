namespace Tyne.MediatorEndpoints;

public class CountRequest : IApiRequest<CountResponse>
{
    public static string Uri => "testapp/count";

    public int Count { get; set; }
}
