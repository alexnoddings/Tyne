using Tyne.Preludes.Core;

namespace Tyne;

public class OptionCreationTests
{
    [Fact]
    public void Default_ValueType_IsNone()
    {
        Option<int> option = default;
        AssertOption.IsNone(option);
    }

    [Fact]
    public void Default_NullableValueType_IsNone()
    {
        Option<int?> option = default;
        AssertOption.IsNone(option);
    }

    [Fact]
    public void Default_ReferenceType_IsNone()
    {
        Option<object> option = default;
        AssertOption.IsNone(option);
    }

    // TYN0001: Use Option.Some or Option.None to create options.
    // Ignored for testing
#pragma warning disable TYN0001
    [Fact]
    public void EmptyCtor_ValueType_IsNone()
    {
        var option = new Option<int>();
        AssertOption.IsNone(option);
    }

    [Fact]
    public void EmptyCtor_NullableValueType_IsNone()
    {
        var option = new Option<int?>();
        AssertOption.IsNone(option);
    }

    [Fact]
    public void EmptyCtor_ReferenceType_IsNone()
    {
        var option = new Option<object>();
        AssertOption.IsNone(option);
    }
#pragma warning restore TYN0001

    [Fact]
    public void NullCtor_NullableValueType_Throws()
    {
        var exception = Assert.Throws<BadOptionException>(() => new Option<int?>(null));
        Assert.Equal(ExceptionMessages.Option_SomeMustHaveValue, exception.Message);
    }

    [Fact]
    public void NullCtor_ReferenceType_Throws()
    {
        var exception = Assert.Throws<BadOptionException>(() => new Option<object>(null!));
        Assert.Equal(ExceptionMessages.Option_SomeMustHaveValue, exception.Message);
    }

    [Fact]
    public void ValueCtor_ValueType_IsSome()
    {
        var option = new Option<int>(42);
        AssertOption.IsSome(42, option);
    }

    [Fact]
    public void ValueCtor_NullableValueType_IsSome()
    {
        var option = new Option<int?>(42);
        AssertOption.IsSome(42, option);
    }

    [Fact]
    public void ValueCtor_ReferenceType_IsSome()
    {
        var obj = new object();
        var option = new Option<object>(obj);
        AssertOption.IsSome(obj, option);
    }

    [Fact]
    public void NoneField_ValueType_IsNone()
    {
        var option = Option<int>.None;
        AssertOption.IsNone(option);
    }

    [Fact]
    public void NoneField_NullableValueType_IsNone()
    {
        var option = Option<int?>.None;
        AssertOption.IsNone(option);
    }

    [Fact]
    public void NoneField_ReferenceType_IsNone()
    {
        var option = Option<object>.None;
        AssertOption.IsNone(option);
    }

    [Fact]
    public void NoneMethod_ValueType_IsNone()
    {
        var option1 = Option.None<int>();
        var option2 = OptionPrelude.None<int>();

        AssertOption.IsNone(option1);
        AssertOption.IsNone(option2);
    }

    [Fact]
    public void NoneMethod_NullableValueType_IsNone()
    {
        var option1 = Option.None<int?>();
        var option2 = OptionPrelude.None<int?>();

        AssertOption.IsNone(option1);
        AssertOption.IsNone(option2);
    }

    [Fact]
    public void NoneMethod_ReferenceType_IsNone()
    {
        var option1 = Option.None<object>();
        var option2 = OptionPrelude.None<object>();

        AssertOption.IsNone(option1);
        AssertOption.IsNone(option2);
    }

    [Fact]
    public void Some_ValueType_WithValue_IsSome()
    {
        var option1 = Option.Some(42);
        var option2 = OptionPrelude.Some(42);

        AssertOption.IsSome(42, option1);
        AssertOption.IsSome(42, option2);
    }

    [Fact]
    public void Some_NullableValueType_WithValue_IsSome()
    {
        var option1 = Option.Some<int?>(42);
        var option2 = OptionPrelude.Some<int?>(42);

        AssertOption.IsSome(42, option1);
        AssertOption.IsSome(42, option2);
    }

    [Fact]
    public void Some_ReferenceType_WithValue_IsSome()
    {
        var obj = new object();

        var option1 = Option.Some(obj);
        var option2 = OptionPrelude.Some(obj);

        AssertOption.IsSome(obj, option1);
        AssertOption.IsSome(obj, option2);
    }

    [Fact]
    public void Some_ValueType_DefaultValue_IsSome()
    {
        var value = default(int);

        var option1 = Option.Some(value);
        var option2 = OptionPrelude.Some(value);

        AssertOption.IsSome(value, option1);
        AssertOption.IsSome(value, option2);
    }

    [Fact]
    public void Some_NullableValueType_NullValue_Throws()
    {
        var exception1 = Assert.Throws<BadOptionException>(() => Option.Some<int?>(null));
        var exception2 = Assert.Throws<BadOptionException>(() => OptionPrelude.Some<int?>(null));

        Assert.Equal(ExceptionMessages.Option_SomeMustHaveValue, exception1.Message);
        Assert.Equal(ExceptionMessages.Option_SomeMustHaveValue, exception2.Message);
    }

    [Fact]
    public void Some_ReferenceType_NullValue_Throws()
    {
        var exception1 = Assert.Throws<BadOptionException>(() => Option.Some<object>(null!));
        var exception2 = Assert.Throws<BadOptionException>(() => OptionPrelude.Some<object>(null!));

        Assert.Equal(ExceptionMessages.Option_SomeMustHaveValue, exception1.Message);
        Assert.Equal(ExceptionMessages.Option_SomeMustHaveValue, exception2.Message);
    }

    [Fact]
    public void From_ValueType_WithValue_IsSome()
    {
        var option = Option.From(42);
        AssertOption.IsSome(42, option);
    }

    [Fact]
    public void From_NullableValueType_WithValue_IsSome()
    {
        var option = Option.From<int?>(42);
        AssertOption.IsSome(42, option);
    }

    [Fact]
    public void From_ReferenceType_WithValue_IsSome()
    {
        var obj = new object();
        var option = Option.From(obj);
        AssertOption.IsSome(obj, option);
    }

    [Fact]
    public void From_NullableValueType_NullValue_IsNone()
    {
        var option = Option.From<int?>(null);
        AssertOption.IsNone(option);
    }

    [Fact]
    public void From_ReferenceType_NullValue_IsNone()
    {
        var option = Option.From<object>(null);
        AssertOption.IsNone(option);
    }

    [Fact]
    public void ImplicitFrom_ValueType_WithValue_IsSome()
    {
        Option<int> option = 42;
        AssertOption.IsSome(42, option);
    }

    [Fact]
    public void ImplicitFrom_NullableValueType_WithValue_IsSome()
    {
        Option<int?> option = 42;
        AssertOption.IsSome(42, option);
    }

    [Fact]
    public void ImplicitFrom_ReferenceType_WithValue_IsSome()
    {
        var obj = new object();
        Option<object> option = obj;
        AssertOption.IsSome(obj, option);
    }

    [Fact]
    public void ImplicitFrom_NullableValueType_NullValue_IsNone()
    {
        Option<int?> option = null;
        AssertOption.IsNone(option);
    }

    [Fact]
    public void ImplicitFrom_ReferenceType_NullValue_IsNone()
    {
        Option<object> option = null;
        AssertOption.IsNone(option);
    }
}
