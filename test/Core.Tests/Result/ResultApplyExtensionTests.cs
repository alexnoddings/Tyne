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
    public void Apply1_None_InvokesNone()
    {
        var option = Option.None<int>();

        var ok = Substitute.For<Action<int>>();

        option.Apply(ok);

        ok.DidNotReceive().Invoke(0);
    }

    [Fact]
    public void Apply1_Some_InvokesSome()
    {
        var option = Option.Some(42);

        var ok = Substitute.For<Action<int>>();

        option.Apply(ok);

        ok.Received(1).Invoke(42);
    }

    [Fact]
    public void Apply2_NullSome_Throws()
    {
        var okResult = Result.Ok(42);
        var errorResult = Result.Error<int>(TestError.Instance);

        Action<int> ok = null!;
        Action<Error> err = _ => { };

        AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok, err));
        AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok, err));
    }

    [Fact]
    public void Apply2_NullNone_Throws()
    {
        var okResult = Result.Ok(42);
        var errorResult = Result.Error<int>(TestError.Instance);

        Action<int> ok = _ => { };
        Action<Error> err = null!;

        AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok, err));
        AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok, err));
    }

    [Fact]
    public void Apply2_None_InvokesNone()
    {
        var option = Option.None<int>();

        var ok = Substitute.For<Action<int>>();
        var err = Substitute.For<Action>();

        option.Apply(ok, err);

        ok.DidNotReceive().Invoke(0);
        err.Received(1).Invoke();
    }

    [Fact]
    public void Apply2_Some_InvokesSome()
    {
        var option = Option.Some(42);

        var ok = Substitute.For<Action<int>>();
        var err = Substitute.For<Action>();

        option.Apply(ok, err);

        ok.Received(1).Invoke(42);
        err.DidNotReceive().Invoke();
    }
}
