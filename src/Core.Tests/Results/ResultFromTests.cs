using Tyne.TestUtilities;

namespace Tyne;

public class ResultFromTests
{
	[Fact]
	public void Ok_From_Works()
	{
		// Arrange
		var result1 = Result.Ok<TestInheritedType, TestHumanError>(TestValues.TestInherited);

		var result2 = Result.From(result1);
		var result3 = Result.From(result1);

		// Assert
		Assert.True(result1.IsOk);
		Assert.True(result2.IsOk);
		Assert.True(result3.IsOk);
	}

	[Fact]
	public void Err_From_Works()
	{
		// Arrange
		var result1 = Result.Err<TestInheritedType, TestHumanError>(TestValues.TestError);

		var result2 = Result.From(result1);
		var result3 = Result.From(result1);

		// Assert
		Assert.True(result1.IsError);

		Assert.True(result2.IsError);
		Assert.Equal(result1.Error, result2.Error);

		Assert.True(result3.IsError);
		Assert.Equal(result1.Error, result3.Error);
	}

	[Fact]
	public void Ok_From_TE_Works()
	{
		// Arrange
		var result1 = Result.Ok<TestInheritedType, TestInheritedError>(TestValues.TestInherited);

		var result2 = Result<TestInheritedType, TestInheritedError>.From(result1);
		var result3 = Result<TestType, TestHumanError>.From(result1);

		// Assert
		Assert.True(result1.IsOk);

		Assert.True(result2.IsOk);
		Assert.Equal(result1.Value, result2.Value);

		Assert.True(result3.IsOk);
		Assert.Equal(result1.Value, result3.Value);
	}

	[Fact]
	public void Err_From_TE_Works()
	{
		// Arrange
		var result1 = Result.Err<TestInheritedType, TestInheritedError>(TestValues.TestInheritedError);

		var result2 = Result<TestInheritedType, TestInheritedError>.From(result1);
		var result3 = Result<TestType, TestHumanError>.From(result1);

		// Assert
		Assert.True(result1.IsError);

		Assert.True(result2.IsError);
		Assert.Equal(result1.Error, result2.Error);

		Assert.True(result3.IsError);
		Assert.Equal(result1.Error, result3.Error);
	}
}
