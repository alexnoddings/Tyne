using System.Net;

namespace Tyne.HttpMediator;

public class HttpResultOtherTests
{
    [Fact]
    public async Task ToValueTask_Null_Throws()
    {
        // Arrange
        HttpResult<int> result = null!;

        // Act and assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await result.ToValueTask());
        Assert.False(string.IsNullOrWhiteSpace(exception.ParamName));
    }

    [Fact]
    public async Task ToValueTask_ReturnsValueTask()
    {
        // Arrange
        var ok = HttpResult.Ok(42, HttpStatusCode.OK);
        var err = HttpResult.Error<int>(TestError.Message, TestError.StatusCode);

        // Act
        var okTask = ok.ToValueTask();
        var errTask = err.ToValueTask();

        // Assert
        AssertResult.AreEqual(ok, await okTask);
        AssertResult.AreEqual(err, await errTask);
    }

    [Fact]
    public async Task ToTask_Null_Throws()
    {
        // Arrange
        HttpResult<int> result = null!;

        // Act and assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await result.ToTask());
        Assert.False(string.IsNullOrWhiteSpace(exception.ParamName));
    }

    [Fact]
    public async Task ToTask_ReturnsTask()
    {
        // Arrange
        var ok = HttpResult.Ok(42, HttpStatusCode.OK);
        var err = HttpResult.Error<int>(TestError.Message, TestError.StatusCode);

        // Act
        var okTask = ok.ToTask();
        var errTask = err.ToTask();

        // Assert
        AssertResult.AreEqual(ok, await okTask);
        AssertResult.AreEqual(err, await errTask);
    }

    [Fact]
    public void HashCode_Ok_EqualsStatusCodeAndValueHashCode()
    {
        static void Assert_HashCode_EqualsStatusCodeAndValueHashCode<T>(T value)
        {
            const HttpStatusCode statusCode = HttpStatusCode.OK;
            var expected = HashCode.Combine(value!.GetHashCode(), statusCode);
            var actual = HttpResult.Ok(value, statusCode).GetHashCode();
            Assert.Equal(expected, actual);
        }

        Assert_HashCode_EqualsStatusCodeAndValueHashCode(42);
        Assert_HashCode_EqualsStatusCodeAndValueHashCode((int?)42);
        Assert_HashCode_EqualsStatusCodeAndValueHashCode("abc");
        Assert_HashCode_EqualsStatusCodeAndValueHashCode(new object());

        var obj = new MockObject();
        var result = HttpResult.Ok(obj, HttpStatusCode.OK);
        var expectedHashCode = HashCode.Combine(MockObject.HashCode, HttpStatusCode.OK);
        var actualHashCode = result.GetHashCode();

        Assert.Equal(expectedHashCode, actualHashCode);
        Assert.Equal(1, obj.GetHashCodeInvocationCount);
    }

    [Fact]
    public void HashCode_Error_EqualsStatusCodeAndErrorHashCode()
    {
        static void Assert_HashCode_EqualsStatusCodeAndErrorHashCode<T>()
        {
            const HttpStatusCode statusCode = TestError.StatusCode;
            var expected = HashCode.Combine(TestError.Instance, statusCode);
            var actual = HttpResult.Error<T>(TestError.Instance, statusCode).GetHashCode();
            Assert.Equal(expected, actual);
        }

        Assert_HashCode_EqualsStatusCodeAndErrorHashCode<int>();
        Assert_HashCode_EqualsStatusCodeAndErrorHashCode<int?>();
        Assert_HashCode_EqualsStatusCodeAndErrorHashCode<string>();
        Assert_HashCode_EqualsStatusCodeAndErrorHashCode<object>();
    }

    [Fact]
    public void ToString_Ok()
    {
        static void Assert_ToString_ContainsStatusCodeAndValueToString<T>(T value)
        {
            var result = HttpResult.Ok(value, HttpStatusCode.OK);
            var expected = $"200: Ok({value})";
            var actual = result.ToString();
            Assert.Equal(expected, actual);
        }

        Assert_ToString_ContainsStatusCodeAndValueToString(42);
        Assert_ToString_ContainsStatusCodeAndValueToString((int?)42);
        Assert_ToString_ContainsStatusCodeAndValueToString("abc");
        Assert_ToString_ContainsStatusCodeAndValueToString(new object());

        var obj = new MockObject();
        var result = HttpResult.Ok(obj, HttpStatusCode.OK);
        var resultToString = result.ToString();

        // Ensure ToString was only called once
        Assert.Equal($"200: Ok({MockObject.AsString})", resultToString);
        Assert.Equal(1, obj.ToStringInvocationCount);
    }

    [Fact]
    public void ToString_Error()
    {
        static void Assert_ToString_ContainsStatusCodeAndErrorToString<T>()
        {
            var result = HttpResult.Error<T>(TestError.Instance, TestError.StatusCode);
            var expected = $"{(int)TestError.StatusCode}: {TestError.Instance}";
            var actual = result.ToString();
            Assert.Equal(expected, actual);
        }

        Assert_ToString_ContainsStatusCodeAndErrorToString<int>();
        Assert_ToString_ContainsStatusCodeAndErrorToString<int?>();
        Assert_ToString_ContainsStatusCodeAndErrorToString<string>();
        Assert_ToString_ContainsStatusCodeAndErrorToString<object>();
    }
}
