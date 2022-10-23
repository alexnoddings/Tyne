namespace Tyne;

public class ResultTests
{
	public static Result<int, HumanError>[][] EqualResultObjects() => new[]
	{
		new Result<int, HumanError>[] { Result.Ok<int, HumanError>(TestValues.Ok), Result.Ok<int, HumanError>(TestValues.Ok) },
		new Result<int, HumanError>[] { Result.Err<int, HumanError>(TestValues.Error), Result.Err<int, HumanError>(TestValues.Error) },
	};

	[Theory]
	[MemberData(nameof(EqualResultObjects))]
	public void Results_ShouldBeEqual(Result<int, HumanError> result1, Result<int, HumanError> result2)
	{
		// Assert
		Assert.Equal(result1, result2);
		Assert.True(result1 == result2);
		Assert.False(result1 != result2);
		Assert.True(result1.Equals(result2));
		Assert.True(result1.Equals(result2 as object));
		Assert.Equal(result1.GetHashCode(), result2.GetHashCode());
	}

	[Fact]
	public void ResultValue_ShouldNotBeEqual()
	{
		// Arrange
		var result1 = Result.Ok<int, HumanError>(TestValues.Ok);
		var result2 = Result.Err<int, HumanError>(TestValues.Error);

		// Assert
		Assert.NotEqual((object)TestValues.Ok, result1);
		Assert.False(result1.Equals(TestValues.Ok));

		Assert.NotEqual((object)TestValues.Error, result2);
		Assert.False(result2.Equals(TestValues.Error));

		Assert.NotEqual((object)TestValues.ErrorMessage, result2);
		Assert.False(result2.Equals(TestValues.ErrorMessage));
	}

	[Fact]
	public void IsOk_Works()
	{
		// Arrange
		var result = Result.Ok<int, HumanError>(TestValues.Ok);

		// Assert
		Assert.True(result.IsOk);
		Assert.False(result.IsError);
		Assert.Equal(TestValues.Ok, result.Value);
		Assert.Throws<InvalidOperationException>(() => result.Error);
	}

	[Fact]
	public void IsError_Works()
	{
		// Arrange
		var result = Result.Err<int, HumanError>(TestValues.Error);

		// Assert
		Assert.True(result.IsError);
		Assert.False(result.IsOk);
		Assert.Equal(TestValues.Error, result.Error);
		Assert.Throws<InvalidOperationException>(() => result.Value);
	}
}
