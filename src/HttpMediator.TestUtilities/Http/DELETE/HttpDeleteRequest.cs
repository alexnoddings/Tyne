namespace Tyne.HttpMediator;

public class HttpDeleteRequest : TestHttpRequestBase, IHttpRequest<HttpDeleteResponse>
{
    public static string Uri => "testapp/http/Delete";
    public static HttpMethod Method { get; } = HttpMethod.Delete;
}
