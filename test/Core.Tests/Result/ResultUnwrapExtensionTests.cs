namespace Tyne;

public class ResultUnwrapExtensionTests
{
    private const string UnwrapExceptionMessage = "Test unwrapping exception message.";
    private static string UnwrapExceptionMessageFactory() => UnwrapExceptionMessage;
    private static InvalidOperationException UnwrapExceptionFactory() => new(UnwrapExceptionMessage);

    [Fact]
    public void Unwrap_Error_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);

        var exception = Assert.Throws<UnwrapResultValueException>(() => result.Unwrap());
        Assert.Equal(ExceptionMessages.Result_CannotUnwrapValueFromError, exception.Message);
    }

    [Fact]
    public void Unwrap_Ok_ReturnsOk()
    {
        var result = Result.Ok(42);

        var value = result.Unwrap();

        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_Message_Error_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);

        var exception = Assert.Throws<UnwrapResultValueException>(() => result.Unwrap(UnwrapExceptionMessage));
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
    }

    [Fact]
    public void Unwrap_Message_Ok_ReturnsOk()
    {
        var result = Result.Ok(42);

        var value = result.Unwrap(UnwrapExceptionMessage);

        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_MessageFactory_NullMessageFactory_Throws()
    {
        var result = Result.Ok(42);
        Func<string> messageFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => result.Unwrap(messageFactory));
    }

    [Fact]
    public void Unwrap_MessageFactory_Error_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);

        var exception = Assert.Throws<UnwrapResultValueException>(() => result.Unwrap(UnwrapExceptionMessageFactory));
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
    }

    [Fact]
    public void Unwrap_MessageFactory_Ok_ReturnsOk()
    {
        var result = Result.Ok(42);

        var value = result.Unwrap(UnwrapExceptionMessageFactory);

        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_ExceptionFactory_NullMessageFactory_Throws()
    {
        var result = Result.Ok(42);
        Func<Exception> exceptionFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => result.Unwrap(exceptionFactory));
    }

    [Fact]
    public void Unwrap_ExceptionFactory_Error_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);

        var exception = Assert.Throws<InvalidOperationException>(() => result.Unwrap(UnwrapExceptionFactory));
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
    }

    [Fact]
    public void Unwrap_ExceptionFactory_Ok_ReturnsOk()
    {
        var result = Result.Ok(42);

        var value = result.Unwrap(UnwrapExceptionFactory);

        Assert.Equal(42, value);
    }
}
