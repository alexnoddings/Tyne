namespace Tyne;

// Checks for equality between Result<T>s
public class ResultEqualityTests
{
    [Fact]
    public void Ok_ValueType_AreEqual()
    {
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(42);

        AssertResult.AreEqual(result1, result2);
    }

    [Fact]
    public void Ok_ValueType_AreNotEqual()
    {
        var result1 = Result.Ok(42);
        var result2 = Result.Ok(101);

        AssertResult.AreNotEqual(result1, result2);
    }

    [Fact]
    public void Ok_NullableType_AreEqual()
    {
        var result1 = Result.Ok<int?>(42);
        var result2 = Result.Ok<int?>(42);

        AssertResult.AreEqual(result1, result2);
    }

    [Fact]
    public void Ok_NullableType_AreNotEqual()
    {
        var result1 = Result.Ok<int?>(42);
        var result2 = Result.Ok<int?>(101);

        AssertResult.AreNotEqual(result1, result2);
    }

    [Fact]
    public void Ok_ReferenceType_AreEqual()
    {
        var str = "abc";
        var result1 = Result.Ok(str);
        var result2 = Result.Ok(str);

        AssertResult.AreEqual(result1, result2);
    }

    [Fact]
    public void Ok_ReferenceType_AreNotEqual()
    {
        var result1 = Result.Ok("abc");
        var result2 = Result.Ok("xyz");

        AssertResult.AreNotEqual(result1, result2);
    }

    [Fact]
    public void Ok_DifferentTypes_AreNotEqual()
    {
        var result1 = Result.Ok(42);
        var result2 = Result.Ok("abc");

        Assert.False(result1.Equals(result2));
        Assert.False(result2.Equals(result1));
    }

    [Fact]
    public void Error_ValueType_AreEqual()
    {
        var result1 = Result.Error<int>(TestError.Instance);
        var result2 = Result.Error<int>(TestError.Instance);

        AssertResult.AreEqual(result1, result2);
    }

    [Fact]
    public void Error_NullableType_AreEqual()
    {
        var result1 = Result.Error<int?>(TestError.Instance);
        var result2 = Result.Error<int?>(TestError.Instance);

        AssertResult.AreEqual(result1, result2);
    }

    [Fact]
    public void Error_ReferenceType_AreEqual()
    {
        var result1 = Result.Error<string>(TestError.Instance);
        var result2 = Result.Error<string>(TestError.Instance);

        AssertResult.AreEqual(result1, result2);
    }

    [Fact]
    public void Error_DifferentTypes_AreNotEqual()
    {
        var result1 = Result.Error<int>(TestError.Instance);
        var result2 = Result.Error<string>(TestError.Instance);

        Assert.NotEqual<object>(result1, result2);
        Assert.NotEqual<object>(result2, result1);

        Assert.False(result1.Equals(result2));
        Assert.False(result2.Equals(result1));
    }
}
