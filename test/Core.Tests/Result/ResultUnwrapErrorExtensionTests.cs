namespace Tyne;

public class ResultUnwrapErrorExtensionTests
{
    private const string UnwrapErrorExceptionMessage = "Test UnwrapErrorping exception message.";
    private static string UnwrapErrorExceptionMessageFactory() => UnwrapErrorExceptionMessage;
    private static InvalidOperationException UnwrapErrorExceptionFactory() => new(UnwrapErrorExceptionMessage);

    [Fact]
    public void UnwrapError_Ok_Throws()
    {
        var result = Result.Ok(42);

        var exception = Assert.Throws<UnwrapResultErrorException>(() => result.UnwrapError());
        Assert.Equal(ExceptionMessages.Result_CannotUnwrapErrorFromOk, exception.Message);
    }

    [Fact]
    public void UnwrapError_Error_ReturnsError()
    {
        var result = Result.Error<int>(TestError.Instance);

        var error = result.UnwrapError();

        Assert.Equal(TestError.Instance, error);
    }

    [Fact]
    public void UnwrapError_Message_Ok_Throws()
    {
        var result = Result.Ok(42);

        var exception = Assert.Throws<UnwrapResultErrorException>(() => result.UnwrapError(UnwrapErrorExceptionMessage));
        Assert.Equal(UnwrapErrorExceptionMessage, exception.Message);
    }

    [Fact]
    public void UnwrapError_Message_Error_ReturnsError()
    {
        var result = Result.Error<int>(TestError.Instance);

        var error = result.UnwrapError(UnwrapErrorExceptionMessage);

        Assert.Equal(TestError.Instance, error);
    }

    [Fact]
    public void UnwrapError_MessageFactory_NullMessageFactory_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);
        Func<string> messageFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => result.UnwrapError(messageFactory));
    }

    [Fact]
    public void UnwrapError_MessageFactory_Ok_Throws()
    {
        var result = Result.Ok(42);

        var exception = Assert.Throws<UnwrapResultErrorException>(() => result.UnwrapError(UnwrapErrorExceptionMessageFactory));
        Assert.Equal(UnwrapErrorExceptionMessage, exception.Message);
    }

    [Fact]
    public void UnwrapError_MessageFactory_Error_ReturnsError()
    {
        var result = Result.Error<int>(TestError.Instance);

        var error = result.UnwrapError(UnwrapErrorExceptionMessageFactory);

        Assert.Equal(TestError.Instance, error);
    }

    [Fact]
    public void UnwrapError_ExceptionFactory_NullMessageFactory_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);
        Func<Exception> exceptionFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => result.UnwrapError(exceptionFactory));
    }

    [Fact]
    public void UnwrapError_ExceptionFactory_Ok_Throws()
    {
        var result = Result.Ok(42);

        var exception = Assert.Throws<InvalidOperationException>(() => result.UnwrapError(UnwrapErrorExceptionFactory));
        Assert.Equal(UnwrapErrorExceptionMessage, exception.Message);
    }

    [Fact]
    public void UnwrapError_ExceptionFactory_Error_ReturnsError()
    {
        var result = Result.Error<int>(TestError.Instance);

        var error = result.UnwrapError(UnwrapErrorExceptionFactory);

        Assert.Equal(TestError.Instance, error);
    }
}
