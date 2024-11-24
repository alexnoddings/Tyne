namespace Tyne.HttpMediator;

public class HttpPatchRequest : TestHttpRequestBase, IHttpRequest<HttpPatchResponse>
{
    public static string Uri => "testapp/http/Patch";
    public static HttpMethod Method { get; } = HttpMethod.Patch;
}
