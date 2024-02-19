namespace Tyne;

public class ResultMatchExtensionTests
{
    [Fact]
    public void MatchT_NullOk_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);

        Func<int, int> ok = null!;
        static int err(Error _) => 0;

        AssertExt.ThrowsArgumentNullException(() => result.Match(ok, err));
    }

    [Fact]
    public void MatchT_NullError_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);

        static int ok(int _) => 0;
        Func<Error, int> err = null!;

        AssertExt.ThrowsArgumentNullException(() => result.Match(ok, err));
    }

    [Fact]
    public void MatchT_Ok_ReturnsOk()
    {
        var result = Result.Ok(42);

        var ok = Substitute.For<Func<int, int>>();
        ok.Invoke(42).Returns(101);

        var err = Substitute.For<Func<Error, int>>();

        var match = result.Match(ok.Invoke, err.Invoke);

        Assert.Equal(101, match);
        ok.Received(1).Invoke(42);
        err.DidNotReceive().Invoke(Arg.Any<Error>());
    }

    [Fact]
    public void MatchT_Error_ReturnsError()
    {
        var error = TestError.Instance;
        var result = Result.Error<int>(error);

        var ok = Substitute.For<Func<int, int>>();

        var err = Substitute.For<Func<Error, int>>();
        err.Invoke(error).Returns(101);

        var match = result.Match(ok.Invoke, err.Invoke);

        Assert.Equal(101, match);
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        err.Received(1).Invoke(error);
    }
}
