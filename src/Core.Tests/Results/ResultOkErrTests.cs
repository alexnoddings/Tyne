namespace Tyne;

public class ResultOkErrTests
{
	[Fact]
	public void Ok_Returns_Ok_Unit_HumanError()
	{
		// Arrange
		var result = Result.Ok();

		// Assert
		Assert.True(result.IsOk);
		Assert.Equal(unit, result.Value);
	}

	[Fact]
	public void Ok_T_Returns_Ok_HumanError()
	{
		// Arrange
		var result = Result.Ok<int>(TestValues.Ok);

		// Assert
		Assert.True(result.IsOk);
		Assert.Equal(TestValues.Ok, result.Value);
	}

	[Fact]
	public void Ok_TE_Returns_Ok_TE()
	{
		// Arrange
		var result = Result.Ok<int, TestHumanError>(TestValues.Ok);

		// Assert
		Assert.True(result.IsOk);
		Assert.Equal(TestValues.Ok, result.Value);
	}

	[Fact]
	public void Ok_TE_Returns_Ok_TE_Enum()
	{
		// Arrange
		var result = Result.Ok<int, TestErrorKind>(TestValues.Ok);

		// Assert
		Assert.True(result.IsOk);
		Assert.Equal(TestValues.Ok, result.Value);
	}

	[Fact]
	public void Err_Returns_Err_Unit_HumanError()
	{
		// Arrange
		var result1 = Result.Err(TestValues.Error);
		var result2 = Result.Err(TestValues.ErrorMessage);

		// Assert
		Assert.True(result1.IsError);
		Assert.Equal(TestValues.Error, result1.Error);

		Assert.True(result2.IsError);
		Assert.Equal(TestValues.Error, result2.Error);
	}

	[Fact]
	public void Err_T_Returns_Err_HumanError()
	{
		// Arrange
		var result1 = Result.Err<int>(TestValues.Error);
		var result2 = Result.Err<int>(TestValues.ErrorMessage);

		// Assert
		Assert.True(result1.IsError);
		Assert.Equal(TestValues.Error, result1.Error);

		Assert.True(result2.IsError);
		Assert.Equal(TestValues.Error, result2.Error);
	}

	[Fact]
	public void Err_TE_Returns_Err_TE()
	{
		// Arrange
		var error = new TestHumanError(TestValues.ErrorMessage);
		var result = Result.Err<int, TestHumanError>(error);

		// Assert
		Assert.True(result.IsError);
		Assert.Equal(error, result.Error);
	}

	[Fact]
	public void Err_TE_Returns_Err_TE_Enum()
	{
		// Arrange
		var result = Result.Err<int, TestErrorKind>(TestValues.ErrorKind);

		// Assert
		Assert.True(result.IsError);
		Assert.Equal(TestValues.ErrorKind, result.Error);
	}
}
