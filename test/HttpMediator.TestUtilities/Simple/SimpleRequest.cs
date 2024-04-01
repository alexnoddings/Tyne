namespace Tyne.HttpMediator;

public class SimpleRequest : IHttpRequest<SimpleResponse>
{
    public static string Uri => "testapp/simple_request";
    public static HttpMethod Method { get; } = HttpMethod.Get;

    public int Count { get; set; }
}
