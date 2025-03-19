namespace Tyne;

public class ResultMiscellaneousTests
{
    [Test]
    public async Task ImplicitCast_ToUnitResult_Ok_ReturnsOkUnitResult()
    {
        // Arrange
        var result1 = Result.Ok<int, string>(42);
        var result2 = Result.Ok<int?, string>(42);
        var result3 = Result.Ok<string, string>("abc");

        // Act
        Result<Unit, string> result1Unit = result1;
        Result<Unit, string> result2Unit = result2;
        Result<Unit, string> result3Unit = result3;

        // Assert
        await Assert.That(result1Unit).IsOk(Unit.Value);
        await Assert.That(result2Unit).IsOk(Unit.Value);
        await Assert.That(result3Unit).IsOk(Unit.Value);
    }

    [Test]
    public async Task ImplicitCast_ToUnitResult_Error_ReturnsErrorUnitResult()
    {
        // Arrange
        var result1 = Result.Error<int, string>("some error");
        var result2 = Result.Error<int?, string>("some error");
        var result3 = Result.Error<string, string>("some error");

        // Act
        Result<Unit, string> result1Unit = result1;
        Result<Unit, string> result2Unit = result2;
        Result<Unit, string> result3Unit = result3;

        // Assert
        await Assert.That(result1Unit).IsError("some error");
        await Assert.That(result2Unit).IsError("some error");
        await Assert.That(result3Unit).IsError("some error");
    }

    [Test]
    public async Task ImplicitCast_ToUnitResult_Null_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, string>? result1 = null;
        Result<int?, string>? result2 = null;
        Result<string, string>? result3 = null;

        // Act
        var act1 = () =>
        {
            Result<Unit, string> resultUnit = result1!;
            _ = resultUnit;
        };
        var act2 = () =>
        {
            Result<Unit, string> resultUnit = result2!;
            _ = resultUnit;
        };
        var act3 = () =>
        {
            Result<Unit, string> resultUnit = result3!;
            _ = resultUnit;
        };

        // Assert
        await Assert.That(act1).Throws<ArgumentNullException>();
        await Assert.That(act2).Throws<ArgumentNullException>();
        await Assert.That(act3).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ImplicitCast_ToOption_Ok_ReturnsOkUnitResult()
    {
        // Arrange
        var result1 = Result.Ok<int, string>(42);
        var result2 = Result.Ok<int?, string>(42);
        var result3 = Result.Ok<string, string>("abc");

        // Act
        Option<int> result1Unit = result1;
        Option<int?> result2Unit = result2;
        Option<string> result3Unit = result3;

        // Assert
        await Assert.That(result1Unit).IsSome(42);
        await Assert.That(result2Unit).IsSome(42);
        await Assert.That(result3Unit).IsSome("abc");
    }

    [Test]
    public async Task ImplicitCast_ToOption_Error_ReturnsErrorUnitResult()
    {
        // Arrange
        var result1 = Result.Error<int, string>("some error");
        var result2 = Result.Error<int?, string>("some error");
        var result3 = Result.Error<string, string>("some error");

        // Act
        Option<int> result1Unit = result1;
        Option<int?> result2Unit = result2;
        Option<string> result3Unit = result3;

        // Assert
        await Assert.That(result1Unit).IsNone();
        await Assert.That(result2Unit).IsNone();
        await Assert.That(result3Unit).IsNone();
    }

    [Test]
    public async Task HashCode_Ok_EqualsValueHashCode()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);

        // Act
        var hashCode = result.GetHashCode();

        // Assert
        await Assert.That(hashCode).IsEqualTo(42.GetHashCode());
    }

    [Test]
    public async Task HashCode_Ok_CallsHashCodeOnce()
    {
        // Arrange
        var obj = new MockObject();
        var result = Result.Ok<MockObject, string>(obj);

        // Act
        var hashCode = result.GetHashCode();

        // Assert
        await Assert.That(hashCode).IsEqualTo(MockObject.HashCode);
        await Assert.That(obj.GetHashCodeInvocationCount).IsEqualTo(1);
    }

    [Test]
    public async Task HashCode_Error_EqualsValueHashCode()
    {
        // Arrange
        var result = Result.Error<int, string>("error code");

        // Act
        var hashCode = result.GetHashCode();

        // Assert
#pragma warning disable CA1307
        // CA1307: Specify StringComparison for clarity.
        // REASON: We want to mirror the default behaviour without StringComparison.
        await Assert.That(hashCode).IsEqualTo("error code".GetHashCode());
#pragma warning restore CA1307
    }

    [Test]
    public async Task HashCode_Error_CallsHashCodeOnce()
    {
        // Arrange
        var obj = new MockObject();
        var result = Result.Error<int, MockObject>(obj);

        // Act
        var hashCode = result.GetHashCode();

        // Assert
        await Assert.That(hashCode).IsEqualTo(MockObject.HashCode);
        await Assert.That(obj.GetHashCodeInvocationCount).IsEqualTo(1);
    }

    [Test]
    public async Task ToString_Ok()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);

        // Act
        var str = result.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("Ok(42)");
    }

    [Test]
    public async Task ToString_Ok_CallsValueToStringOnce()
    {
        // Arrange
        var obj = new MockObject();
        var result = Result.Ok<MockObject, string>(obj);

        // Act
        var str = result.ToString();

        // Assert
        await Assert.That(str).IsEqualTo($"Ok({MockObject.AsString})");
        await Assert.That(obj.ToStringInvocationCount).IsEqualTo(1);
    }

    [Test]
    public async Task ToString_Error()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");

        // Act
        var str = result.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("Error(some error)");
    }

    [Test]
    public async Task ToString_Error_CallsValueToStringOnce()
    {
        // Arrange
        var obj = new MockObject();
        var result = Result.Error<int, MockObject>(obj);

        // Act
        var str = result.ToString();

        // Assert
        await Assert.That(str).IsEqualTo($"Error({MockObject.AsString})");
        await Assert.That(obj.ToStringInvocationCount).IsEqualTo(1);
    }
}
