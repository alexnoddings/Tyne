namespace Tyne;

public class OptionEqualityTests
{
    #region Both Some
    [Test]
    public async Task BothSome_ValueType_AreEqual()
    {
        // Arrange
        var option1 = Option.Some(42);
        var option2 = Option.Some(42);

        // Act/assert
        await AreEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task BothSome_ValueType_AreNotEqual()
    {
        // Arrange
        var option1 = Option.Some(42);
        var option2 = Option.Some(101);

        // Act/assert
        await AreNotEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task BothSome_NullableValueType_AreEqual()
    {
        // Arrange
        var option1 = Option.Some<int?>(42);
        var option2 = Option.Some<int?>(42);

        // Act/assert
        await AreEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task BothSome_NullableValueType_AreNotEqual()
    {
        // Arrange
        var option1 = Option.Some<int?>(42);
        var option2 = Option.Some<int?>(101);

        // Act/assert
        await AreNotEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task BothSome_ReferenceType_AreEqual()
    {
        // Arrange
        var str = "abc";
        var option1 = Option.Some(str);
        var option2 = Option.Some(str);

        // Act/assert
        await AreEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task BothSome_ReferenceType_AreNotEqual()
    {
        // Arrange
        var option1 = Option.Some("abc");
        var option2 = Option.Some("xyz");

        // Act/assert
        await AreNotEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task BothSome_DifferentTypes_AreNotEqual()
    {
        // Arrange
        object option1 = Option.Some(42);
        object option2 = Option.Some("42");

        // Act/assert
        await Assert.That(option1).IsNotEqualTo(option2);
        await Assert.That(option2).IsNotEqualTo(option1);
    }
    #endregion

    #region Split
    [Test]
    public async Task Split_ValueType_AreNotEqual()
    {
        // Arrange
        var option1 = Option.Some(42);
        var option2 = Option.None<int>();

        // Act/assert
        await AreNotEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task Split_NullableValueType_AreNotEqual()
    {
        // Arrange
        var option1 = Option.Some<int?>(42);
        var option2 = Option.None<int?>();

        // Act/assert
        await AreNotEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task Split_ReferenceType_AreNotEqual()
    {
        // Arrange
        var option1 = Option.Some("some string");
        var option2 = Option.Some("other string");

        // Act/assert
        await AreNotEqualExhaustive(option1, option2);
    }

    [Test]
    public async Task Split_DifferentTypes_AreNotEqual()
    {
        // Arrange
        object option1 = Option.Some(42);
        object option2 = Option.None<int>();

        // Act/assert
        await Assert.That(option1).IsNotEqualTo(option2);
        await Assert.That(option2).IsNotEqualTo(option1);
    }

    [Test]
    public async Task Split_OneSome_OneNull_AreNotEqual()
    {
        // Arrange
        var option1 = Option.Some(42);
        Option<int>? option2 = null;

        // Act/assert
        await AreNotEqual(option1, option2);
    }

    [Test]
    public async Task Split_OneNone_OneNull_AreNotEqual()
    {
        // Arrange
        var option1 = Option.None<int>();
        Option<int>? option2 = null;

        await AreNotEqual(option1, option2);
    }

    private static Option<int>? GetNullOption()
    {
        // Unreachable condition, but avoids the compiler inlining the return value as null,
        // which can also inline the (left == right)s below to true.
        if (Guid.NewGuid() == Guid.Empty)
            return Option.None<int>();

        return null;
    }

    [Test]
    public async Task BothNull_AreEqual()
    {
        // Arrange
        var left = GetNullOption();
        var right = GetNullOption();

        // Act/assert
        await Assert.That(right).IsEqualTo(left);
        await Assert.That(left).IsEqualTo(right);

        await Assert.That(left == right).IsTrue();
        await Assert.That(right == left).IsTrue();

        await Assert.That(left != right).IsFalse();
        await Assert.That(right != left).IsFalse();
    }
    #endregion

    #region BothNone
    [Test]
    public async Task BothNone_ValueType_AreEqual()
    {
        // Arrange
        var option1 = Option.None<int>();
        var option2 = Option.None<int>();

        // Act/assert
        await AreEqualExhaustive(option1, option2);
        await AreNotEqual(option1, 0);
    }

    [Test]
    public async Task BothNone_NullableValueType_AreEqual()
    {
        // Arrange
        var option1 = Option.None<int?>();
        var option2 = Option.None<int?>();

        // Act/assert
        await AreEqualExhaustive(option1, option2);
        await AreEqual(option1, null);
        await AreNotEqual(option1, 0);
    }

    [Test]
    public async Task BothNone_ReferenceType_AreEqual()
    {
        // Arrange
        var option1 = Option.None<string>();
        var option2 = Option.None<string>();

        // Act/assert
        await AreEqualExhaustive(option1, option2);
        await AreEqual(option1, null);
        await AreNotEqual(option1, "");
    }

    [Test]
    public async Task BothNone_DifferentTypes_AreNotEqual()
    {
        // Arrange
        object option1 = Option.None<string>();
        object option2 = Option.None<int>();

        // Act/assert
        await Assert.That(option1).IsNotEqualTo(option2);
        await Assert.That(option2).IsNotEqualTo(option1);
    }
    #endregion

    #region Equals T
    [Test]
    public async Task EqualsT_Some_ValueType_EqualsT()
    {
        // Arrange
        var value = 42;
        var option = Option.Some(value);

        // Act/assert
        await AreEqualExhaustive(option, value);
    }

    [Test]
    public async Task EqualsT_Some_NullableValueType_EqualsT()
    {
        // Arrange
        var value = 42;
        var option = Option.Some<int?>(value);

        // Act/assert
        await AreEqualExhaustive(option, value);
        await AreNotEqual(option, 0);
        await AreNotEqual(option, null);
    }

    [Test]
    public async Task EqualsT_Some_ReferenceType_EqualsT()
    {
        // Arrange
        var value = "some value";
        var option = Option.Some(value);

        // Act/assert
        await AreEqualExhaustive(option, value);
        await AreNotEqual(option, null);
    }

    [Test]
    public async Task EqualsT_None_ValueType_DoesNotEqualDefault()
    {
        // Arrange
        var option = Option.None<int>();

        // Act/assert
        await AreNotEqual(option, 0);
    }

    [Test]
    public async Task EqualsT_None_NullableValueType_EqualsNull()
    {
        // Arrange
        var option = Option.None<int?>();

        // Act/assert
        await AreEqual(option, null);
        await AreNotEqual(option, 0);
    }

    [Test]
    public async Task EqualsT_None_ReferenceType_EqualsNull()
    {
        // Arrange
        var option = Option.None<string>();

        // Act/assert
        await AreEqual(option, null);
        await AreNotEqual(option, "");
    }
    #endregion

    private static async Task AreEqual<T>(Option<T> left, T? right)
    {
        await Assert.That(left).IsEqualTo(right);

        await Assert.That(left == right).IsTrue().Because("Equality operator should return true.");
        await Assert.That(right == left).IsTrue().Because("Equality operator should return true.");

        await Assert.That(left != right).IsFalse().Because("Inequality operator should return false.");
        await Assert.That(right != left).IsFalse().Because("Inequality operator should return false.");

        await Assert.That(left.Equals(right)).IsTrue().Because("Equals method should return true.");
    }

    private static async Task AreEqual<T>(Option<T>? left, Option<T>? right)
    {
        await Assert.That(left).IsEqualTo(right);
        await Assert.That(right).IsEqualTo(left);

        await Assert.That(left == right).IsTrue().Because("Equality operator should return true.");
        await Assert.That(right == left).IsTrue().Because("Equality operator should return true.");

        await Assert.That(left != right).IsFalse().Because("Inequality operator should return false.");
        await Assert.That(right != left).IsFalse().Because("Inequality operator should return false.");

        await Assert.That(left.Equals(right)).IsTrue().Because("Equals method should return true.");
        await Assert.That(right.Equals(left)).IsTrue().Because("Equals method  should return true.");

        await Assert.That(left.Equals(right as object)).IsTrue().Because("Equals (as object) method should return true.");
        await Assert.That(right.Equals(left as object)).IsTrue().Because("Equals (as object) method should return true.");
    }

    private static async Task AreEqualExhaustive<T>(Option<T> left, T right)
    {
        await AreEqual(left, right);

        await Assert.That<int?>(left.GetHashCode()).IsEqualTo(right?.GetHashCode()).Because("HashCodes should be the same.");
        // ToString should be Some({value}) or None, not {value}
        await Assert.That(left.ToString()).IsNotEqualTo(right?.ToString()).Because("String representations should not be the same.");
    }

    // Exhaustively checks every equality comparison method rather than just relying on the default equality checks.
    private static async Task AreEqualExhaustive<T>(Option<T> left, Option<T> right)
    {
        await AreEqual<T>(left, right);

        await Assert.That(right.GetHashCode()).IsEqualTo(left.GetHashCode()).Because("HashCodes should be the same.");
        await Assert.That(right.ToString()).IsEqualTo(left.ToString()).Because("ToString representations should be the same.");
    }

    private static async Task AreNotEqual<T>(Option<T> left, T? right)
    {
        await Assert.That(left).IsNotEqualTo(right);

        await Assert.That(left == right).IsFalse().Because("Equality operator should return false.");
        await Assert.That(right == left).IsFalse().Because("Equality operator should return false.");

        await Assert.That(left != right).IsTrue().Because("Inequality operator should return true.");
        await Assert.That(right != left).IsTrue().Because("Inequality operator should return true.");

        await Assert.That(left.Equals(right)).IsFalse().Because("Equals method should return false.");

        await Assert.That(left.Equals(right as object)).IsFalse().Because("Equals (as object) method should return false.");
    }

    private static async Task AreNotEqual<T>(Option<T>? left, Option<T>? right)
    {
        await Assert.That(left).IsNotEqualTo(right);
        await Assert.That(right).IsNotEqualTo(left);

        await Assert.That(left == right).IsFalse().Because("Equality operator should return false.");
        await Assert.That(right == left).IsFalse().Because("Equality operator should return false.");

        await Assert.That(left != right).IsTrue().Because("Inequality operator should return true.");
        await Assert.That(right != left).IsTrue().Because("Inequality operator should return true.");

        var zz = left?.Equals(right);
        _ = zz;
        await Assert.That(left?.Equals(right)).IsNotTrue().Because("Equals method should return false.");
        await Assert.That(right?.Equals(left)).IsNotTrue().Because("Equals method should return false.");

        await Assert.That(left?.Equals(right as object)).IsNotTrue().Because("Equals (as object) method should return false.");
        await Assert.That(right?.Equals(left as object)).IsNotTrue().Because("Equals (as object) method should return false.");
    }

    private static async Task AreNotEqualExhaustive<T>(Option<T> left, Option<T> right)
    {
        await AreNotEqual<T>(left, right);

        await Assert.That(right.GetHashCode()).IsNotEqualTo(left.GetHashCode()).Because("HashCodes should be different.");
        await Assert.That(right.ToString()).IsNotEqualTo(left.ToString()).Because("ToString representations should be different.");
    }
}
