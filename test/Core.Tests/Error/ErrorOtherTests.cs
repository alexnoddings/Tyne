namespace Tyne;

public class ErrorOtherTests
{
    [Fact]
    public void Default_IsValid()
    {
        AssertError.IsDefault(Error.Default);

        // The default error message shouldn't be null or empty
        Assert.False(string.IsNullOrWhiteSpace(Error.DefaultMessage));

        // And it should be valid
        Assert.True(Internal.Error.IsValidMessage(Error.DefaultMessage));

        // The default error should use the default code
        Assert.Equal(Error.DefaultCode, Error.Default.Code);

        // And use the default error message
        Assert.Equal(Error.DefaultMessage, Error.Default.Message);
    }

    [Fact]
    public void IsValidMessage_Works()
    {
        Assert.False(Internal.Error.IsValidMessage(null));
        Assert.False(Internal.Error.IsValidMessage(""));
        Assert.False(Internal.Error.IsValidMessage("\t "));

        Assert.True(Internal.Error.IsValidMessage("An error message"));
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
    public void ToString_IncludeCode_WithCode_ContainsCodeAndMessage()
    {
        const string code = "ErrorOtherTestsErrorCode";
        const string message = "Some error message.";

        var error = Error.From(code, message);

        Assert.Equal($"Error({code}: {message})", error.ToString(includeCode: true));
    }

    [Fact]
    public void ToString_IncludeCode_WithDefaultCode_ContainsOnlyMessage()
    {
        const string message = "Some error message.";

        var error = Error.From(message);

        Assert.Equal($"Error({message})", error.ToString(includeCode: true));
    }

    [Fact]
    public void ToString_DoNotIncludeCode_WithCode_OnlyMessage()
    {
        const string code = "ErrorOtherTestsErrorCode";
        const string message = "Some error message.";

        var error = Error.From(code, message);

        Assert.Equal($"Error({message})", error.ToString(includeCode: false));
    }

    [Fact]
    public void ToString_DoNotIncludeCode_WithDefaultCode_ContainsOnlyMessage()
    {
        const string message = "Some error message.";

        var error = Error.From(message);

        Assert.Equal($"Error({message})", error.ToString(includeCode: false));
    }
}
