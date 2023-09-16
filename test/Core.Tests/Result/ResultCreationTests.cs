using Tyne.Preludes.Core;

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
        var result1 = Result.Ok(42);
        var result2 = ResultPrelude.Ok(42);

        AssertResult.IsOk(42, result1);
        AssertResult.IsOk(42, result2);
    }

    [Fact]
    public void Ok_NullableValueType_WithValue_IsOk()
    {
        var result1 = Result.Ok<int?>(42);
        var result2 = ResultPrelude.Ok<int?>(42);

        AssertResult.IsOk(42, result1);
        AssertResult.IsOk(42, result2);
    }

    [Fact]
    public void Ok_ReferenceType_WithValue_IsOk()
    {
        var obj = new object();

        var result1 = Result.Ok(obj);
        var result2 = ResultPrelude.Ok(obj);

        AssertResult.IsOk(obj, result1);
        AssertResult.IsOk(obj, result2);
    }

    [Fact]
    public void Ok_ValueType_DefaultValue_IsOk()
    {
        var value = default(int);

        var result1 = Result.Ok(value);
        var result2 = ResultPrelude.Ok(value);

        AssertResult.IsOk(value, result1);
        AssertResult.IsOk(value, result2);
    }

    [Fact]
    public void Ok_NullableValueType_NullValue_Throws()
    {
        var exception1 = Assert.Throws<BadResultException>(() => Result.Ok<int?>(null));
        var exception2 = Assert.Throws<BadResultException>(() => ResultPrelude.Ok<int?>(null));

        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception1.Message);
        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception2.Message);
    }

    [Fact]
    public void Ok_ReferenceType_NullValue_Throws()
    {
        var exception1 = Assert.Throws<BadResultException>(() => Result.Ok<object>(null!));
        var exception2 = Assert.Throws<BadResultException>(() => ResultPrelude.Ok<object>(null!));

        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception1.Message);
        Assert.Equal(ExceptionMessages.Result_OkMustHaveValue, exception2.Message);
    }

    [Fact]
    public void ErrorMethod_Message_ValueType_IsError()
    {
        var result1 = Result.Error<int>(TestError.Message);
        var result2 = ResultPrelude.Error<int>(TestError.Message);

        AssertResult.IsError(TestError.Message, result1);
        AssertResult.IsError(TestError.Message, result2);
    }

    [Fact]
    public void ErrorMethod_Message_NullableValueType_IsError()
    {
        var result1 = Result.Error<int?>(TestError.Message);
        var result2 = ResultPrelude.Error<int?>(TestError.Message);

        AssertResult.IsError(TestError.Message, result1);
        AssertResult.IsError(TestError.Message, result2);
    }

    [Fact]
    public void ErrorMethod_Message_ReferenceType_IsError()
    {
        var result1 = Result.Error<object>(TestError.Message);
        var result2 = ResultPrelude.Error<object>(TestError.Message);

        AssertResult.IsError(TestError.Message, result1);
        AssertResult.IsError(TestError.Message, result2);
    }

    [Fact]
    public void ErrorMethod_CodeAndMessage_ValueType_IsError()
    {
        var result1 = Result.Error<int>(TestError.Code, TestError.Message);
        var result2 = ResultPrelude.Error<int>(TestError.Code, TestError.Message);

        AssertResult.IsError(TestError.Code, TestError.Message, result1);
        AssertResult.IsError(TestError.Code, TestError.Message, result2);
    }

    [Fact]
    public void ErrorMethod_CodeAndMessage_NullableValueType_IsError()
    {
        var result1 = Result.Error<int?>(TestError.Code, TestError.Message);
        var result2 = ResultPrelude.Error<int?>(TestError.Code, TestError.Message);

        AssertResult.IsError(TestError.Code, TestError.Message, result1);
        AssertResult.IsError(TestError.Code, TestError.Message, result2);
    }

    [Fact]
    public void ErrorMethod_CodeAndMessage_ReferenceType_IsError()
    {
        var result1 = Result.Error<object>(TestError.Code, TestError.Message);
        var result2 = ResultPrelude.Error<object>(TestError.Code, TestError.Message);

        AssertResult.IsError(TestError.Code, TestError.Message, result1);
        AssertResult.IsError(TestError.Code, TestError.Message, result2);
    }

    [Fact]
    public void ErrorMethod_All_ValueType_IsError()
    {
        var exception = new InvalidOperationException("Some exception.");

        var result1 = Result.Error<int>(TestError.Code, TestError.Message, exception);
        var result2 = ResultPrelude.Error<int>(TestError.Code, TestError.Message, exception);

        AssertResult.IsError(TestError.Code, TestError.Message, exception, result1);
        AssertResult.IsError(TestError.Code, TestError.Message, exception, result2);
    }

    [Fact]
    public void ErrorMethod_All_NullableValueType_IsError()
    {
        var exception = new InvalidOperationException("Some exception.");

        var result1 = Result.Error<int?>(TestError.Code, TestError.Message, exception);
        var result2 = ResultPrelude.Error<int?>(TestError.Code, TestError.Message, exception);

        AssertResult.IsError(TestError.Code, TestError.Message, exception, result1);
        AssertResult.IsError(TestError.Code, TestError.Message, exception, result2);
    }

    [Fact]
    public void ErrorMethod_All_ReferenceType_IsError()
    {
        var exception = new InvalidOperationException("Some exception.");

        var result1 = Result.Error<object>(TestError.Code, TestError.Message, exception);
        var result2 = ResultPrelude.Error<object>(TestError.Code, TestError.Message, exception);

        AssertResult.IsError(TestError.Code, TestError.Message, exception, result1);
        AssertResult.IsError(TestError.Code, TestError.Message, exception, result2);
    }

    [Fact]
    public void ErrorMethod_Error_ValueType_IsError()
    {
        var error = TestError.Instance;

        var result1 = Result.Error<int>(error);
        var result2 = ResultPrelude.Error<int>(error);

        AssertResult.IsError(error, result1);
        AssertResult.IsError(error, result2);
    }

    [Fact]
    public void ErrorMethod_Error_NullableValueType_IsError()
    {
        var error = TestError.Instance;

        var result1 = Result.Error<int?>(error);
        var result2 = ResultPrelude.Error<int?>(error);

        AssertResult.IsError(error, result1);
        AssertResult.IsError(error, result2);
    }

    [Fact]
    public void ErrorMethod_Error_ReferenceType_IsError()
    {
        var error = TestError.Instance;

        var result1 = Result.Error<object>(error);
        var result2 = ResultPrelude.Error<object>(error);

        AssertResult.IsError(error, result1);
        AssertResult.IsError(error, result2);
    }
}
