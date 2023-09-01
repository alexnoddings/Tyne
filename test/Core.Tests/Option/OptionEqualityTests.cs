namespace Tyne;

// Checks for equality between Option<T>s
public class OptionEqualityTests
{
    [Fact]
    public void None_ValueType_AreEqual()
    {
        var option1 = Option.None<int>();
        var option2 = Option.None<int>();

        AssertOption.AreEqual(option1, option2);
    }

    [Fact]
    public void None_NullableType_AreEqual()
    {
        var option1 = Option.None<int?>();
        var option2 = Option.None<int?>();

        AssertOption.AreEqual(option1, option2);
    }

    [Fact]
    public void None_ReferenceType_AreEqual()
    {
        var option1 = Option.None<string>();
        var option2 = Option.None<string>();

        AssertOption.AreEqual(option1, option2);
    }

    [Fact]
    public void None_DifferentTypes_AreNotEqual()
    {
        var option1 = Option.None<int>();
        var option2 = Option.None<string>();

        Assert.False(option1.Equals(option2));
        Assert.False(option2.Equals(option1));
    }

    [Fact]
    public void Some_ValueType_AreEqual()
    {
        var option1 = Option.Some(42);
        var option2 = Option.Some(42);

        AssertOption.AreEqual(option1, option2);
    }

    [Fact]
    public void Some_ValueType_AreNotEqual()
    {
        var option1 = Option.Some(42);
        var option2 = Option.Some(101);

        AssertOption.AreNotEqual(option1, option2);
    }

    [Fact]
    public void Some_NullableType_AreEqual()
    {
        var option1 = Option.Some<int?>(42);
        var option2 = Option.Some<int?>(42);

        AssertOption.AreEqual(option1, option2);
    }

    [Fact]
    public void Some_NullableType_AreNotEqual()
    {
        var option1 = Option.Some<int?>(42);
        var option2 = Option.Some<int?>(101);

        AssertOption.AreNotEqual(option1, option2);
    }

    [Fact]
    public void Some_ReferenceType_AreEqual()
    {
        var str = "abc";
        var option1 = Option.Some(str);
        var option2 = Option.Some(str);

        AssertOption.AreEqual(option1, option2);
    }

    [Fact]
    public void Some_ReferenceType_AreNotEqual()
    {
        var option1 = Option.Some("abc");
        var option2 = Option.Some("xyz");

        AssertOption.AreNotEqual(option1, option2);
    }

    [Fact]
    public void Some_DifferentTypes_AreNotEqual()
    {
        var option1 = Option.Some(42);
        var option2 = Option.Some("abc");

        Assert.False(option1.Equals(option2));
        Assert.False(option2.Equals(option1));
    }
}
