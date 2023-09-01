namespace Tyne;

public class ResultCreationTests
{
    [Fact]
    public void NullValueCtor_NullableValueType_Throws()
    {
        var exception = Assert.Throws<BadResultException>(() => new Result<int?>((int?)null));
        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception.Message);
    }

    [Fact]
    public void NullValueCtor_ReferenceType_Throws()
    {
        var exception = Assert.Throws<BadResultException>(() => new Result<object>((object)null!));
        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception.Message);
    }

    [Fact]
    public void ValueCtor_ValueType_IsOk()
    {
        var result = new Result<int>(42);
        AssertResult.IsOk(42, result);
    }

    [Fact]
    public void ValueCtor_NullableValueType_IsOk()
    {
        var result = new Result<int?>(42);
        AssertResult.IsOk(42, result);
    }

    [Fact]
    public void ValueCtor_ReferenceType_IsOk()
    {
        var obj = new object();
        var result = new Result<object>(obj);
        AssertResult.IsOk(obj, result);
    }

    [Fact]
    public void NullErrorCtor_NullableValueType_Throws()
    {
        var exception = Assert.Throws<BadResultException>(() => new Result<int?>((Error)null!));
        Assert.Equal(ExceptionMessages.Result_ErrorMustHaveError, exception.Message);
    }

    [Fact]
    public void NullErrorCtor_ReferenceType_Throws()
    {
        var exception = Assert.Throws<BadResultException>(() => new Result<object>(null!));
        Assert.Equal(ExceptionMessages.Result_ErrorMustHaveError, exception.Message);
    }

    [Fact]
    public void ErrorCtor_ValueType_IsOk()
    {
        var result = new Result<int>(TestError.Instance);
        AssertResult.IsError(TestError.Instance, result);
    }

    [Fact]
    public void ErrorCtor_NullableValueType_IsOk()
    {
        var result = new Result<int?>(TestError.Instance);
        AssertResult.IsError(TestError.Instance, result);
    }

    [Fact]
    public void ErrorCtor_ReferenceType_IsOk()
    {
        var result = new Result<object>(TestError.Instance);
        AssertResult.IsError(TestError.Instance, result);
    }

    [Fact]
    public void Ok_ValueType_WithValue_IsOk()
    {
        var result = Result.Ok(42);
        AssertResult.IsOk(42, result);
    }

    [Fact]
    public void Ok_NullableValueType_WithValue_IsOk()
    {
        var result = Result.Ok<int?>(42);
        AssertResult.IsOk(42, result);
    }

    [Fact]
    public void Ok_ReferenceType_WithValue_IsOk()
    {
        var obj = new object();
        var result = Result.Ok(obj);
        AssertResult.IsOk(obj, result);
    }

    [Fact]
    public void Ok_ValueType_DefaultValue_IsOk()
    {
        var value = default(int);
        var result = Result.Ok<int>(value);
        AssertResult.IsOk(value, result);
    }

    [Fact]
    public void Ok_NullableValueType_NullValue_Throws()
    {
        var exception = Assert.Throws<BadResultException>(() => Result.Ok<int?>(null));
        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception.Message);
    }

    [Fact]
    public void Ok_ReferenceType_NullValue_Throws()
    {
        var exception = Assert.Throws<BadResultException>(() => Result.Ok<object>(null!));
        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception.Message);
    }

    [Fact]
    public void ErrorMethod_Message_ValueType_IsError()
    {
        var result = Result.Error<int>(TestError.Message);
        AssertResult.IsError(TestError.Message, result);
    }

    [Fact]
    public void ErrorMethod_Message_NullableValueType_IsError()
    {
        var result = Result.Error<int?>(TestError.Message);
        AssertResult.IsError(TestError.Message, result);
    }

    [Fact]
    public void ErrorMethod_Message_ReferenceType_IsError()
    {
        var result = Result.Error<object>(TestError.Message);
        AssertResult.IsError(TestError.Message, result);
    }

    [Fact]
    public void ErrorMethod_CodeAndMessage_ValueType_IsError()
    {
        var result = Result.Error<int>(TestError.Code, TestError.Message);
        AssertResult.IsError(TestError.Code, TestError.Message, result);
    }

    [Fact]
    public void ErrorMethod_CodeAndMessage_NullableValueType_IsError()
    {
        var result = Result.Error<int?>(TestError.Code, TestError.Message);
        AssertResult.IsError(TestError.Code, TestError.Message, result);
    }

    [Fact]
    public void ErrorMethod_CodeAndMessage_ReferenceType_IsError()
    {
        var result = Result.Error<object>(TestError.Code, TestError.Message);
        AssertResult.IsError(TestError.Code, TestError.Message, result);
    }

    [Fact]
    public void ErrorMethod_All_ValueType_IsError()
    {
        var exception = new InvalidOperationException("Some exception.");
        var result = Result.Error<int>(TestError.Code, TestError.Message, exception);
        AssertResult.IsError(TestError.Code, TestError.Message, exception, result);
    }

    [Fact]
    public void ErrorMethod_All_NullableValueType_IsError()
    {
        var exception = new InvalidOperationException("Some exception.");
        var result = Result.Error<int?>(TestError.Code, TestError.Message, exception);
        AssertResult.IsError(TestError.Code, TestError.Message, exception, result);
    }

    [Fact]
    public void ErrorMethod_All_ReferenceType_IsError()
    {
        var exception = new InvalidOperationException("Some exception.");
        var result = Result.Error<object>(TestError.Code, TestError.Message, exception);
        AssertResult.IsError(TestError.Code, TestError.Message, exception, result);
    }

    [Fact]
    public void ErrorMethod_Error_ValueType_IsError()
    {
        var error = TestError.Instance;
        var result = Result.Error<int>(error);
        AssertResult.IsError(error, result);
    }

    [Fact]
    public void ErrorMethod_Error_NullableValueType_IsError()
    {
        var error = TestError.Instance;
        var result = Result.Error<int?>(error);
        AssertResult.IsError(error, result);
    }

    [Fact]
    public void ErrorMethod_Error_ReferenceType_IsError()
    {
        var error = TestError.Instance;
        var result = Result.Error<object>(error);
        AssertResult.IsError(error, result);
    }
}
