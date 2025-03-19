namespace Tyne;

internal static class TestError
{
    public const string Code = "Tyne.Core.Tests.ErrorCode";
    public const string Message = "Test result error message.";
    public const string Json = @$"{{""Code"":""{Code}"",""Message"":""{Message}""}}";

    public static Error Instance { get; } = Error.From(Code, Message);

    static TestError()
    {
        // Test code and message should not equal defaults
        Assert.NotEqual(Code, Error.DefaultCode);
        Assert.NotEqual(Message, Error.DefaultMessage);
    }
}
