namespace Tyne.Utilities;

public class DisposableActionTests
{
	[Fact]
	public void ActionShouldBeCalledOnce()
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

	public static object[] ActionShouldBeCalledMultipleTimesObjects() => new[]
	{
		new object[] { 1 },
		new object[] { 2 },
		new object[] { 10 },
		new object[] { 100 },
	};

	[Theory]
	[MemberData(nameof(ActionShouldBeCalledMultipleTimesObjects))]
	public void ActionShouldBeCalledMultipleTimes(int disposalCallCount)
	{
		// Arrange
		var mock = new Mock<Action>();
		mock.Setup(action => action());

		// Act
		var disposableAction = new DisposableAction(mock.Object, onlyCallOnce: false);
		for (var i = 0; i < disposalCallCount; i++)
			disposableAction.Dispose();

		// Assert
		Assert.Equal(disposalCallCount, mock.Invocations.Count);
	}
}
