using System.Net;

namespace Tyne.HttpMediator;

internal static class TestError
{
    public const string Code = "Tyne.HttpMediator.Tests.ErrorCode";
    public const string Message = "Test result error message.";
    public const string Json = @$"{{""Code"":""{Code}"",""Message"":""{Message}""}}";
    public const HttpStatusCode StatusCode = (HttpStatusCode)418;

    private static readonly Error _instance = Error.From(Code, Message);
    public static ref readonly Error Instance => ref _instance;

    static TestError()
    {
        // Test code and message should not equal defaults
        Assert.NotEqual(Code, Error.Default.Code);
        Assert.NotEqual(Message, Error.Default.Message);
    }
}
