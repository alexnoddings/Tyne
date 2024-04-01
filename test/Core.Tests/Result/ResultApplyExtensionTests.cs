namespace Tyne;

public class ResultApplyExtensionTests
{
    [Fact]
    public void Apply1_NullSome_Throws()
    {
        var okResult = Result.Ok(42);
        var errorResult = Result.Error<int>(TestError.Instance);

        Action<int> ok = null!;

        AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok));
        AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok));
    }

    [Fact]
    public void Apply2_NullSome_Throws()
    {
        var okResult = Result.Ok(42);
        var errorResult = Result.Error<int>(TestError.Instance);

        Action<int> ok = null!;
        static void err(Error _)
        {
            // Just a filler method
        }

        AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok, err));
        AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok, err));
    }

    [Fact]
    public void Apply2_NullNone_Throws()
    {
        var okResult = Result.Ok(42);
        var errorResult = Result.Error<int>(TestError.Instance);

        static void ok(int _)
        {
            // Just a filler method
        }
        Action<Error> err = null!;

        AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok, err));
        AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok, err));
    }
}
