namespace Tyne;

public class OptionOrExtensionTests
{
    [Fact]
    public void OrValue_None_Null_Throws()
    {
        var option = Option.None<int?>();
        int? value = null;

        AssertExt.ThrowsArgumentNullException(() => option.Or(value));
    }

    [Fact]
    public void OrValue_None_ReturnsValue()
    {
        var option = Option.None<int>();

        var or = option.Or(101);

        Assert.Equal(101, or);
    }

    [Fact]
    public void OrValue_Some_Null_Throws()
    {
        var option = Option.Some<int?>(42);
        int? value = null;

        AssertExt.ThrowsArgumentNullException(() => option.Or(value));
    }

    [Fact]
    public void OrValue_Some_ReturnsSome()
    {
        var option = Option.Some(42);

        var or = option.Or(101);

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrValueFactory_None_Null_Throws()
    {
        var option = Option.None<int>();
        Func<int> valueFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => option.Or(valueFactory));
    }

    [Fact]
    public void OrValueFactory_None_ReturnsValueFactory()
    {
        var option = Option.None<int>();

        var valueFactory = Substitute.For<Func<int>>();
        valueFactory.Invoke().Returns(101);

        var or = option.Or(valueFactory);

        Assert.Equal(101, or);
        valueFactory.Received(1).Invoke();
    }

    [Fact]
    public void OrValueFactory_Some_Null_Throws()
    {
        var option = Option.Some(42);
        Func<int> valueFactory = null!;

        AssertExt.ThrowsArgumentNullException(() => option.Or(valueFactory));
    }

    [Fact]
    public void OrValueFactory_Some_ReturnsSome()
    {
        var option = Option.Some(42);

        var or = option.Or(() => throw new InvalidOperationException("Value factory should not be invoked for Some option."));

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_None_ValueType_ReturnsZero()
    {
        var option = Option.None<int>();

        var or = option.OrDefault();

        Assert.Equal(0, or);
    }

    [Fact]
    public void OrDefault_None_NullableValueType_ReturnsNull()
    {
        var option = Option.None<int?>();

        var or = option.OrDefault();

        Assert.Null(or);
    }

    [Fact]
    public void OrDefault_None_ReferenceType_ReturnsNull()
    {
        var option = Option.None<string?>();

        var or = option.OrDefault();

        Assert.Null(or);
    }

    [Fact]
    public void OrDefault_Some_ValueType_ReturnsSome()
    {
        var option = Option.Some(42);

        var or = option.OrDefault();

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_Some_NullableValueType_ReturnsSome()
    {
        var option = Option.Some<int?>(42);

        var or = option.OrDefault();

        Assert.Equal(42, or);
    }

    [Fact]
    public void OrDefault_Some_ReferenceType_ReturnsSome()
    {
        var option = Option.Some<string?>("abc");

        var or = option.OrDefault();

        Assert.Equal("abc", or);
    }
}
