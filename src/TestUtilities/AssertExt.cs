namespace Tyne;

public static class AssertExt
{
    // Shorthands to check a valid arg null exception is thrown
    // (we don't bother checking message/param are anything other than not-empty)
    public static ArgumentNullException ThrowsArgumentNullException(Action testCode)
    {
        var exception = Assert.Throws<ArgumentNullException>(testCode);
        Assert.False(string.IsNullOrEmpty(exception.Message));
        Assert.False(string.IsNullOrEmpty(exception.ParamName));
        return exception;
    }

    public static ArgumentNullException ThrowsArgumentNullException(Func<object?> testCode)
    {
        var exception = Assert.Throws<ArgumentNullException>(testCode);
        Assert.False(string.IsNullOrEmpty(exception.Message));
        Assert.False(string.IsNullOrEmpty(exception.ParamName));
        return exception;
    }

    public static async Task<ArgumentNullException> ThrowsArgumentNullExceptionAsync(Func<Task> testCode)
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(testCode);
        Assert.False(string.IsNullOrEmpty(exception.Message));
        Assert.False(string.IsNullOrEmpty(exception.ParamName));
        return exception;
    }

    public static ArgumentException ThrowsArgumentException(Func<object?> testCode)
    {
        var exception = Assert.Throws<ArgumentException>(testCode);
        Assert.False(string.IsNullOrEmpty(exception.Message));
        Assert.False(string.IsNullOrEmpty(exception.ParamName));
        return exception;
    }
}
