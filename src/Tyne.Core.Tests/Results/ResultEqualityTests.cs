namespace Tyne;

public class ResultEqualityTests
{
    #region Both Ok
    [Test]
    public async Task BothOk_ValueType_AreEqual()
    {
        // Arrange
        var result1 = Result.Ok<int, string>(42);
        var result2 = Result.Ok<int, string>(42);

        // Act/assert
        await AreEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothOk_ValueType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Ok<int, string>(42);
        var result2 = Result.Ok<int, string>(101);

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothOk_NullableValueType_AreEqual()
    {
        // Arrange
        var result1 = Result.Ok<int?, string>(42);
        var result2 = Result.Ok<int?, string>(42);

        // Act/assert
        await AreEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothOk_NullableValueType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Ok<int?, string>(42);
        var result2 = Result.Ok<int?, string>(101);

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothOk_ReferenceType_AreEqual()
    {
        // Arrange
        var str = "abc";
        var result1 = Result.Ok<string, int>(str);
        var result2 = Result.Ok<string, int>(str);

        // Act/assert
        await AreEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothOk_ReferenceType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Ok<string, int>("abc");
        var result2 = Result.Ok<string, int>("xyz");

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothOk_DifferentTypes_AreNotEqual()
    {
        // Arrange
        object result1 = Result.Ok<int, string>(42);
        object result2 = Result.Ok<string, int>("42");

        // Act/assert
        await Assert.That(result1).IsNotEqualTo(result2);
        await Assert.That(result2).IsNotEqualTo(result1);
    }
    #endregion

    #region Split
    [Test]
    public async Task Split_ValueType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Ok<int, string>(42);
        var result2 = Result.Error<int, string>("some error");

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task Split_NullableValueType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Ok<int?, string>(42);
        var result2 = Result.Error<int?, string>("some error");

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task Split_ReferenceType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Ok<string, int>("some string");
        var result2 = Result.Ok<string, int>("other string");

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task Split_DifferentTypes_AreNotEqual()
    {
        // Arrange
        object result1 = Result.Ok<int, string>(42);
        object result2 = Result.Error<int, string>("some error");

        // Act/assert
        await Assert.That(result1).IsNotEqualTo(result2);
        await Assert.That(result2).IsNotEqualTo(result1);
    }

    [Test]
    public async Task Split_OneOk_OneNull_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Ok<int, string>(42);
        Result<int, string>? result2 = null;

        // Act/assert
        await AreNotEqual(result1, result2);
    }

    [Test]
    public async Task Split_OneError_OneNull_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Error<int, string>("42");
        Result<int, string>? result2 = null;

        // Act/assert
        await AreNotEqual(result1, result2);
    }

    private static Result<int, string>? GetNullResult()
    {
        // Unreachable condition, but avoids the compiler inlining the return value as null,
        // which can also inline the (left == right)s below to true.
        if (Guid.NewGuid() == Guid.Empty)
            return Result.Error<int, string>("some error");

        return null;
    }

    [Test]
    public async Task BothNull_AreEqual()
    {
        // Arrange
        var left = GetNullResult();
        var right = GetNullResult();

        // Act/assert
        await Assert.That(right).IsEqualTo(left);
        await Assert.That(left).IsEqualTo(right);

        await Assert.That(left == right).IsTrue();
        await Assert.That(right == left).IsTrue();

        await Assert.That(left != right).IsFalse();
        await Assert.That(right != left).IsFalse();
    }
    #endregion

    #region BothError
    [Test]
    public async Task BothError_ValueType_AreEqual()
    {
        // Arrange
        var result1 = Result.Error<string, int>(42);
        var result2 = Result.Error<string, int>(42);

        // Act/assert
        await AreEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothError_ValueType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Error<string, int>(42);
        var result2 = Result.Error<string, int>(101);

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothError_NullableValueType_AreEqual()
    {
        // Arrange
        var result1 = Result.Error<string, int?>(42);
        var result2 = Result.Error<string, int?>(42);

        // Act/assert
        await AreEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothError_NullableValueType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Error<string, int?>(42);
        var result2 = Result.Error<string, int?>(101);

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothError_ReferenceType_AreEqual()
    {
        // Arrange
        var str = "abc";
        var result1 = Result.Error<int, string>(str);
        var result2 = Result.Error<int, string>(str);

        // Act/assert
        await AreEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothError_ReferenceType_AreNotEqual()
    {
        // Arrange
        var result1 = Result.Error<int, string>("abc");
        var result2 = Result.Error<int, string>("xyz");

        // Act/assert
        await AreNotEqualExhaustive(result1, result2);
    }

    [Test]
    public async Task BothError_DifferentTypes_AreNotEqual()
    {
        // Arrange
        object result1 = Result.Error<string, int>(42);
        object result2 = Result.Error<int, string>("42");

        // Act/assert
        await Assert.That(result1).IsNotEqualTo(result2);
        await Assert.That(result2).IsNotEqualTo(result1);
    }
    #endregion

    // Exhaustively checks every equality comparison method rather than just relying on the default equality checks.
    private static async Task AreEqualExhaustive<T, E>(Result<T, E> left, Result<T, E> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        await Assert.That(right).IsEqualTo(left);
        await Assert.That(left).IsEqualTo(right);

        await Assert.That(left == right).IsTrue().Because("Equality operator should return true.");
        await Assert.That(right == left).IsTrue().Because("Equality operator should return true.");

        await Assert.That(left != right).IsFalse().Because("Inequality operator should return false.");
        await Assert.That(right != left).IsFalse().Because("Inequality operator should return false.");

        await Assert.That(left.Equals(right)).IsTrue().Because("Equals method should return true.");
        await Assert.That(right.Equals(left)).IsTrue().Because("Equals method  should return true.");

        await Assert.That(left.Equals(right as object)).IsTrue().Because("Equals (as object) method should return true.");
        await Assert.That(right.Equals(left as object)).IsTrue().Because("Equals (as object) method should return true.");

        await Assert.That(left.GetHashCode()).IsEqualTo(right.GetHashCode()).Because("HashCodes should be the same.");
        await Assert.That(left.ToString()).IsEqualTo(right.ToString()).Because("ToString representations should be the same.");
    }

    private static async Task AreNotEqual<T, E>(Result<T, E>? left, Result<T, E>? right)
    {
        await Assert.That(right).IsNotEqualTo(left);
        await Assert.That(left).IsNotEqualTo(right);

        await Assert.That(left == right).IsFalse().Because("Equality operator should return false.");
        await Assert.That(right == left).IsFalse().Because("Equality operator should return false.");

        await Assert.That(left != right).IsTrue().Because("Inequality operator should return true.");
        await Assert.That(right != left).IsTrue().Because("Inequality operator should return true.");

        await Assert.That(left?.Equals(right)).IsNotTrue().Because("Equals method should return false.");
        await Assert.That(right?.Equals(left)).IsNotTrue().Because("Equals method should return false.");

        await Assert.That(left?.Equals(right as object)).IsNotTrue().Because("Equals (as object) method should return false.");
        await Assert.That(right?.Equals(left as object)).IsNotTrue().Because("Equals (as object) method should return false.");
    }

    private static async Task AreNotEqualExhaustive<T, E>(Result<T, E> left, Result<T, E> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        await AreNotEqual(left, right);

        await Assert.That(left.GetHashCode()).IsNotEqualTo(right.GetHashCode()).Because("HashCodes should be different.");
        await Assert.That(left.ToString()).IsNotEqualTo(right.ToString()).Because("ToString representations should be different.");
    }
}
