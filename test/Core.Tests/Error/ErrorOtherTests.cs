namespace Tyne;

public class ErrorOtherTests
{
    [Fact]
    public void Default_IsValid()
    {
        AssertError.IsDefault(Error.Default);

        // The default error message shouldn't be null or empty
        Assert.False(string.IsNullOrWhiteSpace(Error.DefaultErrorMessage));

        // And it should be valid
        Assert.True(Error.IsValidMessage(Error.DefaultErrorMessage));

        // The default error should use the default code
        Assert.Equal(Error.DefaultCode, Error.Default.Code);

        // And use the default error message
        Assert.Equal(Error.DefaultErrorMessage, Error.Default.Message);
    }

    [Fact]
    public void IsValidMessage_Works()
    {
        Assert.False(Error.IsValidMessage(null));
        Assert.False(Error.IsValidMessage(""));
        Assert.False(Error.IsValidMessage("\t "));

        Assert.True(Error.IsValidMessage("An error message"));
    }

    [Fact]
    public void ToResult_Null_Throws()
    {
        Error error = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => error.ToResult<int>());
        Assert.False(string.IsNullOrWhiteSpace(exception.ParamName));
    }

    [Fact]
    public void ToResult_ReturnsErrorResult()
    {
        var error = TestError.Instance;

        var result = error.ToResult<int>();

        AssertResult.IsError(error, result);
    }

    [Fact]
    public async Task ToValueTask_Null_Throws()
    {
        Error error = null!;

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await error.ToValueTask());
        Assert.False(string.IsNullOrWhiteSpace(exception.ParamName));
    }

    [Fact]
    public async Task ToValueTask_ReturnsValueTask()
    {
        var error = TestError.Instance;

        var errorTask = error.ToValueTask();

        Assert.True(errorTask is ValueTask<Error> _);

        AssertError.AreEqual(error, await errorTask);
    }

    [Fact]
    public async Task ToTask_Null_Throws()
    {
        Error error = null!;

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await error.ToTask());
        Assert.False(string.IsNullOrWhiteSpace(exception.ParamName));
    }

    [Fact]
    public async Task ToTask_ReturnsTask()
    {
        var error = TestError.Instance;

        var errorTask = error.ToTask();

        Assert.True(errorTask is Task<Error> _);

        AssertError.AreEqual(error, await errorTask);
    }

    [Fact]
    public void ToString_Works()
    {
        var error = Error.From(42, "Some error");

        Assert.Equal("Error(42: Some error)", error.ToString());
    }
}
