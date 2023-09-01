namespace Tyne;

internal static class TestError
{
    public const int Code = 101;
    public const string Message = "Test result error message.";

    private static readonly Error _instance = Error.From(Code, Message);
    public static ref readonly Error Instance => ref _instance;

    static TestError()
    {
        // Test code and message should not equal defaults
        Assert.NotEqual(Code, Error.DefaultCode);
        Assert.NotEqual(Message, Error.DefaultErrorMessage);
    }
}
