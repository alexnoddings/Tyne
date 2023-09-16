namespace Tyne;

public class ResultOtherTests
{
    [Fact]
    public void Ok_ExplicitCast_ReturnsValue()
    {
        var result1 = Result.Ok(42);
        var result2 = Result.Ok((int?)42);
        var result3 = Result.Ok("abc");

        Assert.Equal(42, (int)result1);
        Assert.Equal(42, (int?)result2);
        Assert.Equal("abc", (string)result3);
    }

    [Fact]
    public void Error_ExplicitCast_Throws()
    {
        var result1 = Result.Error<int>(TestError.Instance);
        var result2 = Result.Error<int?>(TestError.Instance);
        var result3 = Result.Error<string>(TestError.Instance);

        static void AssertThrowsNoValue(Func<object?> func)
        {
            var exception = Assert.Throws<BadResultException>(func);
            Assert.Equal(ExceptionMessages.Result_ErrorHasNoValue, exception.Message);
        }

        AssertThrowsNoValue(() => (int)result1);
        AssertThrowsNoValue(() => (int?)result2);
        AssertThrowsNoValue(() => (string)result3);
    }

    [Fact]
    public void Ok_ToErrorOption_ReturnsNoneOption()
    {
        var result1 = Result.Ok(42);
        var result2 = Result.Ok((int?)42);
        var result3 = Result.Ok("abc");

        var option1 = result1.ToErrorOption();
        var option2 = result2.ToErrorOption();
        var option3 = result3.ToErrorOption();

        AssertOption.IsNone(option1);
        AssertOption.IsNone(option2);
        AssertOption.IsNone(option3);
    }

    [Fact]
    public void Error_ToErrorOption_ReturnsSomeOption()
    {
        var result1 = Result.Error<int>(TestError.Instance);
        var result2 = Result.Error<int?>(TestError.Instance);
        var result3 = Result.Error<string>(TestError.Instance);

        var option1 = result1.ToErrorOption();
        var option2 = result2.ToErrorOption();
        var option3 = result3.ToErrorOption();

        AssertOption.IsSome(TestError.Instance, option1);
        AssertOption.IsSome(TestError.Instance, option2);
        AssertOption.IsSome(TestError.Instance, option3);
    }

    [Fact]
    public void Ok_ToOption_ReturnsSomeOption()
    {
        var result1 = Result.Ok(42);
        var result2 = Result.Ok((int?)42);
        var result3 = Result.Ok("abc");

        var option1 = result1.ToOption();
        var option2 = result2.ToOption();
        var option3 = result3.ToOption();

        AssertOption.IsSome(42, option1);
        AssertOption.IsSome(42, option2);
        AssertOption.IsSome("abc", option3);
    }

    [Fact]
    public void Error_ToOption_ReturnsNoneOption()
    {
        var result1 = Result.Error<int>(TestError.Instance);
        var result2 = Result.Error<int?>(TestError.Instance);
        var result3 = Result.Error<string>(TestError.Instance);

        var option1 = result1.ToOption();
        var option2 = result2.ToOption();
        var option3 = result3.ToOption();

        AssertOption.IsNone(option1);
        AssertOption.IsNone(option2);
        AssertOption.IsNone(option3);
    }

    [Fact]
    public void Ok_ImplicitCastToOption_ReturnsSomeOption()
    {
        var result1 = Result.Ok(42);
        var result2 = Result.Ok((int?)42);
        var result3 = Result.Ok("abc");

        Option<int> option1 = result1;
        Option<int?> option2 = result2;
        Option<string> option3 = result3;

        AssertOption.IsSome(42, option1);
        AssertOption.IsSome(42, option2);
        AssertOption.IsSome("abc", option3);
    }

    [Fact]
    public void Error_ImplicitCastToOption_ReturnsNoneOption()
    {
        var result1 = Result.Error<int>(TestError.Instance);
        var result2 = Result.Error<int?>(TestError.Instance);
        var result3 = Result.Error<string>(TestError.Instance);

        Option<int> option1 = result1;
        Option<int?> option2 = result2;
        Option<string> option3 = result3;

        AssertOption.IsNone(option1);
        AssertOption.IsNone(option2);
        AssertOption.IsNone(option3);
    }

    [Fact]
    public async Task ToValueTask_Null_Throws()
    {
        Result<int> result = null!;

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await result.ToValueTask());
        Assert.False(string.IsNullOrWhiteSpace(exception.ParamName));
    }

    [Fact]
    public async Task ToValueTask_ReturnsValueTask()
    {
        var ok = Result.Ok(42);
        var err = Result.Error<int>(TestError.Message);

        var okTask = ok.ToValueTask();
        var errTask = err.ToValueTask();

        Assert.True(okTask is ValueTask<Result<int>> _);
        Assert.True(errTask is ValueTask<Result<int>> _);

        AssertResult.AreEqual(ok, await okTask);
        AssertResult.AreEqual(err, await errTask);
    }

    [Fact]
    public async Task ToTask_Null_Throws()
    {
        Result<int> result = null!;

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await result.ToTask());
        Assert.False(string.IsNullOrWhiteSpace(exception.ParamName));
    }

    [Fact]
    public async Task ToTask_ReturnsTask()
    {
        var ok = Result.Ok(42);
        var err = Result.Error<int>(TestError.Message);

        var okTask = ok.ToTask();
        var errTask = err.ToTask();

        Assert.True(okTask is Task<Result<int>> _);
        Assert.True(errTask is Task<Result<int>> _);

        AssertResult.AreEqual(ok, await okTask);
        AssertResult.AreEqual(err, await errTask);
    }

    [Fact]
    public void HashCode_Ok_EqualsValueHashCode()
    {
        static void Assert_HashCode_EqualsValueHashCode<T>(T value)
        {
            var result = Result.Ok(value);
            Assert.Equal(value!.GetHashCode(), result.GetHashCode());
        }

        Assert_HashCode_EqualsValueHashCode(42);
        Assert_HashCode_EqualsValueHashCode((int?)42);
        Assert_HashCode_EqualsValueHashCode("abc");
        Assert_HashCode_EqualsValueHashCode(new object());

        var obj = new MockObject();
        var result = Result.Ok(obj);
        var resultHashCode = result.GetHashCode();

        Assert.Equal(MockObject.HashCode, resultHashCode);
        Assert.Equal(1, obj.GetHashCodeInvocationCount);
    }

    [Fact]
    public void HashCode_Error_EqualsErrorHashCode()
    {
        static void Assert_HashCode_EqualsErrorHashCode<T>(Result<T> result) =>
            Assert.Equal(result.Error.GetHashCode(), result.GetHashCode());

        Assert_HashCode_EqualsErrorHashCode(Result.Error<int>(TestError.Instance));
        Assert_HashCode_EqualsErrorHashCode(Result.Error<int?>(TestError.Instance));
        Assert_HashCode_EqualsErrorHashCode(Result.Error<string>(TestError.Instance));
        Assert_HashCode_EqualsErrorHashCode(Result.Error<object>(TestError.Instance));
    }

    [Fact]
    public void ToString_Ok()
    {
        static void Assert_ToString_ContainsValueToString<T>(T value)
        {
            var result = Result.Ok(value);
            Assert.Equal($"Ok({value})", result.ToString());
        }

        Assert_ToString_ContainsValueToString(42);
        Assert_ToString_ContainsValueToString((int?)42);
        Assert_ToString_ContainsValueToString("abc");
        Assert_ToString_ContainsValueToString(new object());

        var obj = new MockObject();
        var result = Result.Ok(obj);
        var resultToString = result.ToString();

        // Ensure ToString was only called once
        Assert.Equal($"Ok({MockObject.AsString})", resultToString);
        Assert.Equal(1, obj.ToStringInvocationCount);
    }

    [Fact]
    public void ToString_Error()
    {
        var error = TestError.Instance;
        var result1 = Result.Error<int>(error);
        var result2 = Result.Error<int?>(error);
        var result3 = Result.Error<string>(error);

        var errorToString = error.ToString();
        Assert.Equal(errorToString, result1.ToString());
        Assert.Equal(errorToString, result2.ToString());
        Assert.Equal(errorToString, result3.ToString());
    }
}
