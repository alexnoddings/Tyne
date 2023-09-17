namespace Tyne;

// Checks for equality between Option<T> and T
public class OptionEqualityTTests
{
    [Fact]
    public void None_ValueType_IsNotEqual()
    {
        // Option<T>.Equals(T) checks if T is null,
        // which isn't possible for ValueTypes,
        // so there is no ValueType_ValueIsEqual
        var option = Option.None<int>();

        // Don't include hash code here - Option<T>.None and 0 both return hash codes of 0
        AssertOption.AreNotEqual(option, 0, includeHashCode: false);
        AssertOption.AreNotEqual(option, 101);
    }

    [Fact]
    public void None_NullableType_IsEqual()
    {
        var option = Option.None<int?>();

        AssertOption.AreEqual(option, null);
    }

    [Fact]
    public void None_NullableType_IsNotEqual()
    {
        var option = Option.None<int?>();

        // Don't include hash code here - Option<T>.None and (int?)null both return hash codes of 0
        AssertOption.AreNotEqual(option, 0, includeHashCode: false);
        AssertOption.AreNotEqual(option, 101);
    }

    [Fact]
    public void None_ReferenceType_IsEqual()
    {
        var option = Option.None<string>();

        AssertOption.AreEqual(option, null);
    }

    [Fact]
    public void None_ReferenceType_IsNotEqual()
    {
        var option = Option.None<string>();

        AssertOption.AreNotEqual(option, "xyz");
    }

    [Fact]
    public void Some_ValueType_IsEqual()
    {
        var option = Option.Some(42);

        AssertOption.AreEqual(option, 42);
    }

    [Fact]
    public void Some_ValueType_IsNotEqual()
    {
        var option = Option.Some(42);

        AssertOption.AreNotEqual(option, 0);
        AssertOption.AreNotEqual(option, 101);
    }

    [Fact]
    public void Some_NullableType_IsEqual()
    {
        var option = Option.Some<int?>(42);

        AssertOption.AreEqual(option, 42);
    }

    [Fact]
    public void Some_NullableType_IsNotEqual()
    {
        var option = Option.Some<int?>(42);

        AssertOption.AreNotEqual(option, null);
        AssertOption.AreNotEqual(option, 0);
        AssertOption.AreNotEqual(option, 101);
    }

    [Fact]
    public void Some_ReferenceType_IsEqual()
    {
        var option = Option.Some("abc");

        AssertOption.AreEqual(option, "abc");
    }

    [Fact]
    public void Some_ReferenceType_AreNotEqual()
    {
        var option = Option.Some("abc");

        AssertOption.AreNotEqual(option, null);
        AssertOption.AreNotEqual(option, "xyz");
    }
}
