namespace Tyne;

public class ErrorCreationTests
{
    [Fact]
    public void FromMessage_NullMessage_UsesDefault()
    {
        var error = Error.From(null!);

        Assert.Equal(Error.DefaultCode, error.Code);
        Assert.Equal(Error.Default.Message, error.Message);
        AssertOption.IsNone(error.CausedBy);
    }

    [Fact]
    public void FromMessage_UsesMessage()
    {
        var error = Error.From(TestError.Message);

        Assert.Equal(Error.DefaultCode, error.Code);
        Assert.Equal(TestError.Message, error.Message);
        AssertOption.IsNone(error.CausedBy);
    }

    [Fact]
    public void FromCodeAndMessage_NullMessage_UsesDefault()
    {
        var error = Error.From(TestError.Code, null!);

        Assert.Equal(TestError.Code, error.Code);
        Assert.Equal(Error.Default.Message, error.Message);
        AssertOption.IsNone(error.CausedBy);
    }

    [Fact]
    public void FromCodeAndMessage_UsesCodeAndMessage()
    {
        var error = Error.From(TestError.Code, TestError.Message);

        Assert.Equal(TestError.Code, error.Code);
        Assert.Equal(TestError.Message, error.Message);
        AssertOption.IsNone(error.CausedBy);
    }

    [Fact]
    public void FromAll_NullMessage_UsesDefault()
    {
        var causedBy = new InvalidOperationException("Some exception.");

        var error = Error.From(TestError.Code, null!, causedBy);

        Assert.Equal(TestError.Code, error.Code);
        Assert.Equal(Error.Default.Message, error.Message);
        Assert.Equal(causedBy, error.CausedBy);
    }

    [Fact]
    public void FromAll_UsesAll()
    {
        var causedBy = new InvalidOperationException("Some exception.");

        var error = Error.From(TestError.Code, TestError.Message, causedBy);

        Assert.Equal(TestError.Code, error.Code);
        Assert.Equal(TestError.Message, error.Message);
        Assert.Equal(causedBy, error.CausedBy);
    }
}
