namespace Tyne;

public class OptionUnwrapExtensionTests
{
    private const string UnwrapExceptionMessage = "Test unwrapping exception message.";
    private static string UnwrapExceptionMessageFactory() => UnwrapExceptionMessage;
    private static InvalidOperationException UnwrapExceptionFactory() => new(UnwrapExceptionMessage);

    [Fact]
    public void Unwrap_None_Throws()
    {
        var option = Option.None<int>();

        var exception = Assert.Throws<UnwrapOptionException>(() => option.Unwrap());
        Assert.Equal(ExceptionMessages.Option_CannotUnwrapNone, exception.Message);
    }

    [Fact]
    public void Unwrap_Some_ReturnsSome()
    {
        var option = Option.Some(42);

        var value = option.Unwrap();

        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_Message_None_Throws()
    {
        var option = Option.None<int>();

        var exception = Assert.Throws<UnwrapOptionException>(() => option.Unwrap(UnwrapExceptionMessage));
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
    }

    [Fact]
    public void Unwrap_Message_Some_ReturnsSome()
    {
        var option = Option.Some(42);

        var value = option.Unwrap(UnwrapExceptionMessage);

        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_MessageFactory_NullMessageFactory_Throws()
    {
        var option = Option.Some(42);
        Func<string> messageFactory = null!;

        _ = AssertExt.ThrowsArgumentNullException(() => option.Unwrap(messageFactory));
    }

    [Fact]
    public void Unwrap_MessageFactory_None_Throws()
    {
        var option = Option.None<int>();

        var exception = Assert.Throws<UnwrapOptionException>(() => option.Unwrap(UnwrapExceptionMessageFactory));
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
    }

    [Fact]
    public void Unwrap_MessageFactory_Some_ReturnsSome()
    {
        var option = Option.Some(42);

        var value = option.Unwrap(UnwrapExceptionMessageFactory);

        Assert.Equal(42, value);
    }

    [Fact]
    public void Unwrap_ExceptionFactory_NullMessageFactory_Throws()
    {
        var option = Option.Some(42);
        Func<Exception> exceptionFactory = null!;

        _ = AssertExt.ThrowsArgumentNullException(() => option.Unwrap(exceptionFactory));
    }

    [Fact]
    public void Unwrap_ExceptionFactory_None_Throws()
    {
        var option = Option.None<int>();

        var exception = Assert.Throws<InvalidOperationException>(() => option.Unwrap(UnwrapExceptionFactory));
        Assert.Equal(UnwrapExceptionMessage, exception.Message);
    }

    [Fact]
    public void Unwrap_ExceptionFactory_Some_ReturnsSome()
    {
        var option = Option.Some(42);

        var value = option.Unwrap(UnwrapExceptionFactory);

        Assert.Equal(42, value);
    }
}
