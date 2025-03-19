using System.Diagnostics.CodeAnalysis;

namespace Tyne;

public class DisposableActionTests
{
    [Fact]
    public void NullAction_Throws()
    {
        _ = AssertExt.ThrowsArgumentNullException(() => new DisposableAction(null!));
        _ = AssertExt.ThrowsArgumentNullException(() => new DisposableAction(null!, true));
        _ = AssertExt.ThrowsArgumentNullException(() => new DisposableAction(null!, false));
    }

    [Fact]
    [SuppressMessage("Reliability", "CA2000: Dispose objects before losing scope", Justification = "It is disposed.")]
    [SuppressMessage("Major Code Smell", "S3966: Objects should not be disposed more than once", Justification = "Done for testing.")]
    public void OnlyCallOnce_True_OnlyCallsOnce()
    {
        var action = Substitute.For<Action>();

        var disposableAction = new DisposableAction(action, onlyCallOnce: true);

        for (var i = 0; i < 10; i++)
            disposableAction.Dispose();

        action.Received(1).Invoke();
    }

    [Fact]
    [SuppressMessage("Reliability", "CA2000: Dispose objects before losing scope", Justification = "It is disposed.")]
    [SuppressMessage("Major Code Smell", "S3966: Objects should not be disposed more than once", Justification = "Done for testing.")]
    public void OnlyCallOnce_False_CallsEachTime()
    {
        var action = Substitute.For<Action>();

        var disposableAction = new DisposableAction(action, onlyCallOnce: false);

        for (var i = 0; i < 10; i++)
            disposableAction.Dispose();

        action.Received(10).Invoke();
    }

    [Fact]
    [SuppressMessage("Reliability", "CA2000: Dispose objects before losing scope", Justification = "It is disposed.")]
    [SuppressMessage("Major Code Smell", "S3966: Objects should not be disposed more than once", Justification = "Done for testing.")]
    public void OnlyCallOnce_Default_OnlyCallsOnce()
    {
        var action = Substitute.For<Action>();

        var disposableAction = new DisposableAction(action);

        for (var i = 0; i < 10; i++)
            disposableAction.Dispose();

        action.Received(1).Invoke();
    }
}
