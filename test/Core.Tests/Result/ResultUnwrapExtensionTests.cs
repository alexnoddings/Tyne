namespace Tyne;

public class ResultUnwrapExtensionTests
{
    private const string UnwrapExceptionMessage = "Test unwrapping exception message.";
    private static string UnwrapExceptionMessageFactory() => UnwrapExceptionMessage;
    private static InvalidOperationException UnwrapExceptionFactory() => new(UnwrapExceptionMessage);

    [Fact]
    public void Unwrap_Error_ThrowsUnwrapException()
    {
        // Arrange
        var result = Result.Error<int>(TestError.Instance);

        // Act
        void act() =>
            _ = result.Unwrap();

        // Assert
        var exception = Assert.Throws<UnwrapResultValueException>(act);
        Assert.Equal(ExceptionMessages.Result_CannotUnwrapValueFromError, exception.Message);
        AssertHasInnerException(exception);
    }

    [Fact]
    public void Unwrap_Ok_ReturnsOk()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var value = result.Unwrap();

        // Assert
        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_Message_Error_ThrowsUnwrapException()
    {
        // Arrange
        var result = Result.Error<int>(TestError.Instance);

        // Act
        void act() =>
            _ = result.Unwrap(UnwrapExceptionMessage);

        // Assert
        var exception = Assert.Throws<UnwrapResultValueException>(act);
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
        AssertHasInnerException(exception);
    }

    [Fact]
    public void Unwrap_Message_Ok_ReturnsOk()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var value = result.Unwrap(UnwrapExceptionMessage);

        // Assert
        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_MessageFactory_NullMessageFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var result = Result.Ok(42);
        Func<string> messageFactory = null!;

        // Act
        void act() =>
            _ = result.Unwrap(messageFactory);

        // Assert
        AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void Unwrap_MessageFactory_Error_Throws()
    {
        // Arrange
        var result = Result.Error<int>(TestError.Instance);

        // Act
        void act() =>
            _ = result.Unwrap(UnwrapExceptionMessageFactory);

        // Assert
        var exception = Assert.Throws<UnwrapResultValueException>(act);
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
        AssertHasInnerException(exception);
    }

    [Fact]
    public void Unwrap_MessageFactory_Ok_ReturnsOk()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var value = result.Unwrap(UnwrapExceptionMessageFactory);

        // Assert
        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_ExceptionFactory_NullMessageFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var result = Result.Ok(42);
        Func<Exception> exceptionFactory = null!;

        // Act
        void act() =>
            _ = result.Unwrap(exceptionFactory);

        // Assert
        AssertExt.ThrowsArgumentNullException(act);
    }

    [Fact]
    public void Unwrap_ExceptionFactory_Error_ThrowsSpecifiedException()
    {
        // Arrange
        var result = Result.Error<int>(TestError.Instance);

        // Act
        void act() =>
            result.Unwrap(UnwrapExceptionFactory);

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
    }

    [Fact]
    public void Unwrap_ExceptionFactory_Ok_ReturnsOk()
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var value = result.Unwrap(UnwrapExceptionFactory);

        // Assert
        Assert.Equal(42, value);
    }

    private static void AssertHasInnerException(Exception outerException)
    {
        var error = TestError.Instance;
        var innerException = outerException.InnerException;
        Assert.NotNull(innerException);
        Assert.IsType<InvalidOperationException>(innerException);
        Assert.Equal(error.Message, innerException.Message);
    }
}
