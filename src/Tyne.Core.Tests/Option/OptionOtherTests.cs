namespace Tyne;

public class OptionOtherTests
{
    [Fact]
    public void None_IsNone()
    {
        AssertOption.IsNone(Option<int>.None);
        AssertOption.IsNone(Option.None<int>());
        AssertOption.IsNone(Option.None<int?>());
        AssertOption.IsNone(Option.None<string>());
    }

    [Fact]
    public void Some_ExplicitCast_ReturnsValue()
    {
        var option1 = Option.Some(42);
        var option2 = Option.Some((int?)42);
        var option3 = Option.Some("abc");

        Assert.Equal(42, (int)option1);
        Assert.Equal(42, (int?)option2);
        Assert.Equal("abc", (string)option3);
    }

    [Fact]
    public void None_ExplicitCast_Throws()
    {
        var option1 = Option.None<int>();
        var option2 = Option.None<int?>();
        var option3 = Option.None<string>();

        static void AssertThrowsNoValue(Func<object?> func)
        {
            var exception = Assert.Throws<BadOptionException>(func);
            Assert.Equal(ExceptionMessages.Option_NoneHasNoValue, exception.Message);
        }

        AssertThrowsNoValue(() => (int)option1);
        AssertThrowsNoValue(() => (int?)option2);
        AssertThrowsNoValue(() => (string)option3);
    }

    [Fact]
    public async Task ToTask_ReturnsOptionTask()
    {
        // Arrange
        var some = Option.Some(42);
        var none = Option.None<int>();

        // Act
        var someTask = some.ToTask();
        var noneTask = none.ToTask();

        // Assert
        AssertOption.AreEqual(some, await someTask);
        AssertOption.AreEqual(none, await noneTask);
    }

    [Fact]
    public async Task ToValueTask_ReturnsOptionValueTask()
    {
        // Arrange
        var some = Option.Some(42);
        var none = Option.None<int>();

        // Act
        var someTask = some.ToValueTask();
        var noneTask = none.ToValueTask();

        // Assert
        AssertOption.AreEqual(some, await someTask);
        AssertOption.AreEqual(none, await noneTask);
    }

    [Fact]
    public void HashCode_None_EqualsZero()
    {
        static void Assert_HashCode_IsZero<T>(Option<T> option) =>
            Assert.Equal(0, option.GetHashCode());

        Assert_HashCode_IsZero(Option.None<int>());
        Assert_HashCode_IsZero(Option.None<int?>());
        Assert_HashCode_IsZero(Option.None<string>());
        Assert_HashCode_IsZero(Option.None<object>());
    }

    [Fact]
    public void HashCode_Some_EqualsValueHashCode()
    {
        static void Assert_HashCode_EqualsValueHashCode<T>(T value)
        {
            var option = Option.Some(value);
            Assert.Equal(value!.GetHashCode(), option.GetHashCode());
        }

        Assert_HashCode_EqualsValueHashCode(42);
        Assert_HashCode_EqualsValueHashCode((int?)42);
        Assert_HashCode_EqualsValueHashCode("abc");
        Assert_HashCode_EqualsValueHashCode(new object());

        var obj = new MockObject();
        var option = Option.Some(obj);
        var optionHashCode = option.GetHashCode();

        Assert.Equal(MockObject.HashCode, optionHashCode);
        Assert.Equal(1, obj.GetHashCodeInvocationCount);
    }

    [Fact]
    public void ToString_None()
    {
        var option1 = Option.None<int>();
        var option2 = Option.None<int?>();
        var option3 = Option.None<string>();

        Assert.Equal("None", option1.ToString());
        Assert.Equal("None", option2.ToString());
        Assert.Equal("None", option3.ToString());
    }

    [Fact]
    public void ToString_Some()
    {
        static void Assert_ToString_ContainsValueToString<T>(T value)
        {
            var option = Option.Some(value);
            Assert.Equal($"Some({value})", option.ToString());
        }

        Assert_ToString_ContainsValueToString(42);
        Assert_ToString_ContainsValueToString((int?)42);
        Assert_ToString_ContainsValueToString("abc");
        Assert_ToString_ContainsValueToString(new object());

        var obj = new MockObject();
        var option = Option.Some(obj);
        var optionToString = option.ToString();

        // Ensure ToString was only called once
        Assert.Equal($"Some({MockObject.AsString})", optionToString);
        Assert.Equal(1, obj.ToStringInvocationCount);
    }
}
