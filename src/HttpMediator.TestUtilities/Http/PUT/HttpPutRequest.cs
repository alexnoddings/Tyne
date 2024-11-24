namespace Tyne.HttpMediator;

public class HttpPutRequest : TestHttpRequestBase, IHttpRequest<HttpPutResponse>
{
    public static string Uri => "testapp/http/Put";
    public static HttpMethod Method { get; } = HttpMethod.Put;
}
