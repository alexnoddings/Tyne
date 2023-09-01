namespace Tyne;

public class ResultOrExtensionTests
{
    [Fact]
    public void OrValue_Ok_Null_Throws()
    {
        var result = Result.Ok<int?>(42);
        int? value = null;

        AssertExt.ThrowsArgumentNullException(() => result.Or(value));
    }

    [Fact]
    public void OrValue_Ok_ReturnsOk()
    {
        var result = Result.Ok(42);

        var or = result.Or(101);

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrValue_Error_Null_Throws()
    {
        var result = Result.Error<int?>(TestError.Instance);
        int? value = null;

        AssertExt.ThrowsArgumentNullException(() => result.Or(value));
    }

    [Fact]
    public void OrValue_Error_ReturnsValue()
    {
        var result = Result.Error<int>(TestError.Instance);

        var or = result.Or(101);

        Assert.Equal(101, or);
    }

    [Fact]
    public void OrValueFactory_Error_Null_Throws()
    {
        var result = Result.Error<int>(TestError.Instance);
        Func<int> valueFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => result.Or(valueFactory));
    }

    [Fact]
    public void OrValueFactory_Error_ReturnsValueFactory()
    {
        var result = Result.Error<int>(TestError.Instance);

        var valueFactory = Substitute.For<Func<int>>();
        valueFactory.Invoke().Returns(101);

        var or = result.Or(valueFactory);

        Assert.Equal(101, or);
        valueFactory.Received(1).Invoke();
    }

    [Fact]
    public void OrValueFactory_Ok_Null_Throws()
    {
        var result = Result.Ok(42);
        Func<int> valueFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => result.Or(valueFactory));
    }

    [Fact]
    public void OrValueFactory_Ok_ReturnsOk()
    {
        var result = Result.Ok(42);

        var or = result.Or(() => throw new InvalidOperationException("Value factory should not be invoked for Ok result."));

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_Error_ValueType_ReturnsZero()
    {
        var result = Result.Error<int>(TestError.Instance);

        var or = result.OrDefault();

        Assert.Equal(0, or);
    }

    [Fact]
    public void OrDefault_Error_NullableValueType_ReturnsNull()
    {
        var result = Result.Error<int?>(TestError.Instance);

        var or = result.OrDefault();

        Assert.Null(or);
    }

    [Fact]
    public void OrDefault_Error_ReferenceType_ReturnsNull()
    {
        var result = Result.Error<string?>(TestError.Instance);

        var or = result.OrDefault();

        Assert.Null(or);
    }

    [Fact]
    public void OrDefault_Ok_ValueType_ReturnsOk()
    {
        var result = Result.Ok(42);

        var or = result.OrDefault();

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_Ok_NullableValueType_ReturnsOk()
    {
        var result = Result.Ok<int?>(42);

        var or = result.OrDefault();

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_Ok_ReferenceType_ReturnsOk()
    {
        var result = Result.Ok<string?>("abc");

        var or = result.OrDefault();

        Assert.Equal("abc", or);
    }
}
