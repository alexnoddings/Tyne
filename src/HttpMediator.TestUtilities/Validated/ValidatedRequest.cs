namespace Tyne.HttpMediator;

public class ValidatedRequest : IHttpRequest<ValidatedResponse>
{
    public static string Uri => "testapp/validated_request";
    public static HttpMethod Method { get; } = HttpMethod.Post;

    public const string ValidMessage = "valid_test_message";
    public const string NotValidMessage = "not_valid_test_message";

    public string Message { get; set; } = string.Empty;
}
