using Tyne.Preludes.Core;

namespace Tyne;

public class ErrorCreationTests
{
    [Fact]
    public void FromMessage_NullMessage_UsesDefault()
    {
        var error1 = Error.From(null!);
        var error2 = ErrorPrelude.Error(null!);

        static void assert(Error error)
        {
            Assert.Equal(Error.DefaultCode, error.Code);
            Assert.Equal(Error.Default.Message, error.Message);
            AssertOption.IsNone(error.CausedBy);
        }

        assert(error1);
        assert(error2);
    }

    [Fact]
    public void FromMessage_UsesMessage()
    {
        var error1 = Error.From(TestError.Message);
        var error2 = Error.From(TestError.Message);

        static void assert(Error error)
        {
            Assert.Equal(Error.DefaultCode, error.Code);
            Assert.Equal(TestError.Message, error.Message);
            AssertOption.IsNone(error.CausedBy);
        }

        assert(error1);
        assert(error2);
    }

    [Fact]
    public void FromCodeAndMessage_NullMessage_UsesDefault()
    {
        var error1 = Error.From(TestError.Code, null!);
        var error2 = Error.From(TestError.Code, null!);

        static void assert(Error error)
        {
            Assert.Equal(TestError.Code, error.Code);
            Assert.Equal(Error.Default.Message, error.Message);
            AssertOption.IsNone(error.CausedBy);
        }

        assert(error1);
        assert(error2);
    }

    [Fact]
    public void FromCodeAndMessage_UsesCodeAndMessage()
    {
        var error1 = Error.From(TestError.Code, TestError.Message);
        var error2 = Error.From(TestError.Code, TestError.Message);

        static void assert(Error error)
        {
            Assert.Equal(TestError.Code, error.Code);
            Assert.Equal(TestError.Message, error.Message);
            AssertOption.IsNone(error.CausedBy);
        }

        assert(error1);
        assert(error2);
    }

    [Fact]
    public void FromAll_NullMessage_UsesDefault()
    {
        var causedBy = new InvalidOperationException("Some exception.");

        var error1 = Error.From(TestError.Code, null!, causedBy);
        var error2 = Error.From(TestError.Code, null!, causedBy);

        void assert(Error error)
        {
            Assert.Equal(TestError.Code, error.Code);
            Assert.Equal(Error.Default.Message, error.Message);
            Assert.Equal(causedBy, error.CausedBy);
        }

        assert(error1);
        assert(error2);
    }

    [Fact]
    public void FromAll_UsesAll()
    {
        var causedBy = new InvalidOperationException("Some exception.");

        var error1 = Error.From(TestError.Code, TestError.Message, causedBy);
        var error2 = Error.From(TestError.Code, TestError.Message, causedBy);

        void assert(Error error)
        {
            Assert.Equal(TestError.Code, error.Code);
            Assert.Equal(TestError.Message, error.Message);
            Assert.Equal(causedBy, error.CausedBy);
        }

        assert(error1);
        assert(error2);
    }
}
