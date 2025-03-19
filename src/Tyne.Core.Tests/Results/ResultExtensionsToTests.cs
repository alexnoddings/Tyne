using System.Diagnostics.CodeAnalysis;

namespace Tyne;

public class ResultExtensionsToTests
{
    [Test]
    public async Task ToTask_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;

        // Act
        void Act() => result.ToTask();

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ToTask_Result_ReturnsTask()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("error");

        // Act
        var okResultTask = okResult.ToTask();
        var errorResultTask = errorResult.ToTask();

        // Assert
        await Assert.That(okResultTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(errorResultTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(await okResultTask).IsEqualTo(okResult);
        await Assert.That(await errorResultTask).IsEqualTo(errorResult);
    }

    [Test]
    [SuppressMessage("Reliability",
        "CA2012: Use ValueTasks correctly.",
        Justification = "Method invocation should throw, not return a ValueTask."
    )]
    public async Task ToValueTask_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;

        // Act
        void Act() => _ = result.ToValueTask();

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ToValueTask_Result_ReturnsTask()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("error");

        // Act
        var okResultTask = okResult.ToValueTask();
        var errorResultTask = errorResult.ToValueTask();

        // Assert
        await Assert.That(okResultTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(errorResultTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(await okResultTask).IsEqualTo(okResult);
        await Assert.That(await errorResultTask).IsEqualTo(errorResult);
    }

    [Test]
    public async Task ToOption_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;

        // Act
        void Act() => _ = result.ToOption();

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ToOption_OkResult_ReturnsSome()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);

        // Act
        var option = result.ToOption();

        // Assert
        await Assert.That(option).IsSome(42);
    }

    [Test]
    public async Task ToOption_ErrorResult_ReturnsNone()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");

        // Act
        var option = result.ToOption();

        // Assert
        await Assert.That(option).IsNone();
    }
}
