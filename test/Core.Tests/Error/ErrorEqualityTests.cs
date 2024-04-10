namespace Tyne;

public class ErrorEqualityTests
{
    [Fact]
    public void Defaults_AreEqual()
    {
        AssertError.AreEqual(Error.Default, Error.Default);
    }

    [Fact]
    public void From_Message_AreEqual()
    {
        var error1 = Error.From(TestError.Message);
        var error2 = Error.From(TestError.Message);

        AssertError.AreEqual(error1, error2);
    }

    [Fact]
    public void From_CodeAndMessage_AreEqual()
    {
        var error1 = Error.From(TestError.Code, TestError.Message);
        var error2 = Error.From(TestError.Code, TestError.Message);

        AssertError.AreEqual(error1, error2);
    }

    private const string ErrorMessage1 = "An error message.";
    private const string ErrorMessage2 = "Some other error message.";

    [Fact]
    public void From_DifferentMessage_AreNotEqual()
    {
        var error1 = Error.From(ErrorMessage1);
        var error2 = Error.From(ErrorMessage2);

        AssertError.AreNotEqual(error1, error2);
    }

    [Fact]
    public void From_DifferentCodeAndSameMessage_AreNotEqual()
    {
        const string code1 = "test-code-1";
        const string code2 = "test-code-2";

        var error1 = Error.From(code1, TestError.Message);
        var error2 = Error.From(code2, TestError.Message);

        AssertError.AreNotEqual(error1, error2);
    }

    [Fact]
    public void From_SameCodeAndDifferentMessage_AreNotEqual()
    {
        var error1 = Error.From(TestError.Code, ErrorMessage1);
        var error2 = Error.From(TestError.Code, ErrorMessage2);

        AssertError.AreNotEqual(error1, error2);
    }
}
