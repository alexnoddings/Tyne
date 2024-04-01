namespace Tyne.HttpMediator;

public class HttpGetRequest : TestHttpRequestBase, IHttpRequest<HttpGetResponse>
{
    public static string Uri => "testapp/http/Get";
    public static HttpMethod Method { get; } = HttpMethod.Get;
}
