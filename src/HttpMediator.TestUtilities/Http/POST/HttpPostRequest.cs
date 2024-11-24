namespace Tyne.HttpMediator;

public class HttpPostRequest : TestHttpRequestBase, IHttpRequest<HttpPostResponse>
{
    public static string Uri => "testapp/http/Post";
    public static HttpMethod Method { get; } = HttpMethod.Post;
}
