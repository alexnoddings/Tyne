namespace Tyne;

public class ResultCreationTests
{
    [Test]
    public async Task Ok_Null_Throws_ArgumentNullException()
    {
        // Arrange
        int? value = null;

        // Act
        Action act = () => _ = Result.Ok<int?, string>(value!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Ok_Cached_Unit_ReturnsOk()
    {
        // Arrange
        var value = Unit.Value;

        // Act
        var result = Result.Ok<Unit, string>(value);

        // Assert
        await Assert.That(result).IsOk(value);
        await Assert.That(result).IsSameReferenceAs(Result.Cache<string>.OkUnit);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Ok_Cached_bool_ReturnsOk(bool b)
    {
        // Arrange
        var expectedResult = b
            ? Result.Cache<string>.OkTrue
            : Result.Cache<string>.OkFalse;

        // Act
        var result = Result.Ok<bool, string>(b);

        // Assert
        await Assert.That(result).IsOk(b);
        await Assert.That(result).IsSameReferenceAs(expectedResult);
    }

    [Test]
    public async Task Ok_Cached_intZero_ReturnsOk()
    {
        // Arrange
        var value = 0;

        // Act
        var result = Result.Ok<int, string>(value);

        // Assert
        await Assert.That(result).IsOk(value);
        await Assert.That(result).IsSameReferenceAs(Result.Cache<string>.OkIntZero);
    }

    [Test]
    public async Task Ok_Cached_GuidEmpty_ReturnsOk()
    {
        // Arrange
        var value = Guid.Empty;

        // Act
        var result = Result.Ok<Guid, string>(value);

        // Assert
        await Assert.That(result).IsOk(value);
        await Assert.That(result).IsSameReferenceAs(Result.Cache<string>.OkGuidEmpty);
    }

    [Test]
    public async Task Ok_ValueType_ReturnsOk()
    {
        // Arrange
        var value = 42;

        // Act
        var result = Result.Ok<int, string>(value);

        // Assert
        await Assert.That(result).IsOk(value);
    }

    [Test]
    public async Task Ok_NullableValueType_ReturnsOk()
    {
        // Arrange
        int? value = 42;

        // Act
        var result = Result.Ok<int?, string>(value);

        // Assert
        await Assert.That(result).IsOk(value);
    }

    [Test]
    public async Task Ok_ReferenceType_ReturnsOk()
    {
        // Arrange
        var value = "some value";

        // Act
        var result = Result.Ok<string, string>(value);

        // Assert
        await Assert.That(result).IsOk(expectedValue: value);
    }

    [Test]
    public async Task Error_Null_Throws_BadResultException()
    {
        // Arrange
        string error = null!;

        // Act
        Action act = () => _ = Result.Error<int, string>(error);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Error_ValueType_ReturnsError()
    {
        // Arrange
        var error = 42;

        // Act
        var result = Result.Error<int, int>(error);

        // Assert
        await Assert.That(result).IsError(error);
    }

    [Test]
    public async Task Ok_NullableValueType_ReturnsError()
    {
        // Arrange
        int? error = 42;

        // Act
        var result = Result.Error<int, int?>(error);

        // Assert
        await Assert.That(result).IsError(error);
    }

    [Test]
    public async Task Ok_ReferenceType_ReturnsError()
    {
        // Arrange
        var error = "some error";

        // Act
        var result = Result.Error<int, string>(error);

        // Assert
        await Assert.That(result).IsError(error);
    }
}
