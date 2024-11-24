namespace Tyne;

public class OptionMatchExtensionTests
{
    [Fact]
    public void Apply1_NullSome_Throws()
    {
        var noneOption = Option.None<int>();
        var someOption = Option.Some(42);

        Action<int> some = null!;

        _ = AssertExt.ThrowsArgumentNullException(() => noneOption.Apply(some));
        _ = AssertExt.ThrowsArgumentNullException(() => someOption.Apply(some));
    }

    [Fact]
    public void Apply1_None_DoesNotInvoke()
    {
        var option = Option.None<int>();

        var some = Substitute.For<Action<int>>();

        _ = option.Apply(some.Invoke);

        some.DidNotReceive().Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Apply1_Some_InvokesSome()
    {
        var option = Option.Some(42);

        var some = Substitute.For<Action<int>>();

        _ = option.Apply(some.Invoke);

        some.Received(1).Invoke(42);
    }

    [Fact]
    public void Apply2_NullSome_Throws()
    {
        var noneOption = Option.None<int>();
        var someOption = Option.Some(42);

        Action<int> some = null!;
        static void none()
        {
            // Just a filler method
        }

        _ = AssertExt.ThrowsArgumentNullException(() => noneOption.Apply(some, none));
        _ = AssertExt.ThrowsArgumentNullException(() => someOption.Apply(some, none));
    }

    [Fact]
    public void Apply2_NullNone_Throws()
    {
        var noneOption = Option.None<int>();
        var someOption = Option.Some(42);

        static void some(int _)
        {
            // Just a filler method
        }
        Action none = null!;

        _ = AssertExt.ThrowsArgumentNullException(() => noneOption.Apply(some, none));
        _ = AssertExt.ThrowsArgumentNullException(() => someOption.Apply(some, none));
    }

    [Fact]
    public void Apply2_None_InvokesNone()
    {
        var option = Option.None<int>();

        var some = Substitute.For<Action<int>>();
        var none = Substitute.For<Action>();

        _ = option.Apply(some.Invoke, none.Invoke);

        some.DidNotReceive().Invoke(Arg.Any<int>());
        none.Received(1).Invoke();
    }

    [Fact]
    public void Apply2_Some_InvokesSome()
    {
        var option = Option.Some(42);

        var some = Substitute.For<Action<int>>();
        var none = Substitute.For<Action>();

        _ = option.Apply(some.Invoke, none.Invoke);

        some.Received(1).Invoke(42);
        none.DidNotReceive().Invoke();
    }
}
