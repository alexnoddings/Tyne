namespace Tyne;

public class DisposableActionTests
{
	[Fact]
	public void OnlyCallOnce_Action_ShouldBeCalledOnce()
	{
		// Arrange
		var mock = new Mock<Action>();
		mock.Setup(action => action());

		// Act
		var disposableAction = new DisposableAction(mock.Object);
		disposableAction.Dispose();
		disposableAction.Dispose();

		// Assert
		Assert.Equal(1, mock.Invocations.Count);
	}

	[Fact]
	public void CallMoreThanOnce_Action_ShouldBeCalledMultipleTimes()
	{
		// Arrange
		var mock = new Mock<Action>();
		mock.Setup(action => action());

		// Act
		var disposableAction = new DisposableAction(mock.Object, onlyCallOnce: false);
		for (var i = 0; i < 5; i++)
			disposableAction.Dispose();

		// Assert
		Assert.Equal(5, mock.Invocations.Count);
	}

	private class TestException : Exception
	{
	}

	[Fact]
	public void Action_ShouldThrowOriginalException()
	{
		// Arrange
		var exception = new TestException();
		Action action = () => throw exception;

		// Act
		var disposableAction = new DisposableAction(action);

		// Assert
		Assert.Throws<TestException>(disposableAction.Dispose);
	}
}
