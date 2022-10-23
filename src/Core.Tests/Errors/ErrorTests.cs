namespace Tyne;

public class ErrorTests
{
	[Fact]
	public void Constructor_ShouldThrowForInvalidMessage()
	{
		// Assert
		Assert.Throws<ArgumentNullException>(() => new HumanError(null!));
		Assert.Throws<ArgumentException>(() => new HumanError(string.Empty));
		Assert.Throws<ArgumentException>(() => new HumanError("          "));
	}

	[Fact]
	public void Constructor_Works()
	{
		// Arrange
		var errorMessage = "sample error message";

		// Assert
		Assert.Equal(errorMessage, new HumanError(errorMessage).HumanErrorMessage);
	}

	[Fact]
	public void Errors_AreEqual()
	{
		// Arrange
		var errorMessage = "sample error message";
		var error1 = new HumanError(errorMessage);
		var error2 = new HumanError(errorMessage);

		// Assert
		Assert.Equal(error1, error2);
		Assert.True(error1 == error2);
		Assert.False(error1 != error2);
		Assert.True(error1.Equals(error2));
		Assert.True(error1.Equals(error2 as object));

		Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
	}

	[Fact]
	public void Errors_AreNotEqual()
	{
		// Arrange
		var error1 = new HumanError("sample error message");
		var error2 = new HumanError("different error message");

		// Assert
		Assert.NotEqual(error1, error2);
		Assert.False(error1 == error2);
		Assert.True(error1 != error2);
		Assert.False(error1.Equals(error2));
		Assert.False(error1.Equals(error2 as object));

		Assert.NotEqual(error1.GetHashCode(), error2.GetHashCode());
	}
}
