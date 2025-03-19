using System.Diagnostics.CodeAnalysis;

namespace Tyne;

public class DisposableActionTests
{
    [Test]
    public async Task NullAction_Throws()
    {
        // Arrange
        Action? action = null;

        // Act
        DisposableAction ActDefault() => new(action!);
        DisposableAction ActTrue() => new(action!, true);
        DisposableAction ActFalse() => new(action!, false);

        // Assert
        await Assert.That(ActDefault).Throws<ArgumentNullException>();
        await Assert.That(ActTrue).Throws<ArgumentNullException>();
        await Assert.That(ActFalse).Throws<ArgumentNullException>();
    }

    [Test]
    [SuppressMessage("Reliability", "CA2000: Dispose objects before losing scope", Justification = "It is disposed.")]
    [SuppressMessage("Major Code Smell", "S3966: Objects should not be disposed more than once", Justification = "Done for testing.")]
    public void OnlyCallOnce_Default_OnlyCallsOnce()
    {
        // Arrange
        var action = Substitute.For<Action>();
        var disposableAction = new DisposableAction(action);

        // Act
        for (var i = 0; i < 10; i++)
            disposableAction.Dispose();

        // Assert
        action.Received(1).Invoke();
    }

    [Test]
    [SuppressMessage("Reliability", "CA2000: Dispose objects before losing scope", Justification = "It is disposed.")]
    [SuppressMessage("Major Code Smell", "S3966: Objects should not be disposed more than once", Justification = "Done for testing.")]
    public void OnlyCallOnce_True_OnlyCallsOnce()
    {
        // Arrange
        var action = Substitute.For<Action>();
        var disposableAction = new DisposableAction(action, onlyCallOnce: true);

        // Act
        for (var i = 0; i < 10; i++)
            disposableAction.Dispose();

        // Assert
        action.Received(1).Invoke();
    }

    [Test]
    [SuppressMessage("Reliability", "CA2000: Dispose objects before losing scope", Justification = "It is disposed.")]
    [SuppressMessage("Major Code Smell", "S3966: Objects should not be disposed more than once", Justification = "Done for testing.")]
    public void OnlyCallOnce_False_CallsEachTime()
    {
        // Arrange
        var action = Substitute.For<Action>();
        var disposableAction = new DisposableAction(action, onlyCallOnce: false);

        // Act
        for (var i = 0; i < 10; i++)
            disposableAction.Dispose();

        // Assert
        action.Received(10).Invoke();
    }
}
