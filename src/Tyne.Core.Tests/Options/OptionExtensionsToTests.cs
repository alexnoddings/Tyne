namespace Tyne;

public class OptionExtensionsToTests
{
    [Test]
    public async Task ToTask_ReturnsTask()
    {
        // Arrange
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();

        // Act
        var someOptionTask = someOption.ToTask();
        var noneOptionTask = noneOption.ToTask();

        // Assert
        await Assert.That(someOptionTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(noneOptionTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(await someOptionTask).IsEqualTo(someOption);
        await Assert.That(await noneOptionTask).IsEqualTo(noneOption);
    }

    [Test]
    public async Task ToValueTask_ReturnsTask()
    {
        // Arrange
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();

        // Act
        var someOptionTask = someOption.ToValueTask();
        var noneOptionTask = noneOption.ToValueTask();

        // Assert
        await Assert.That(someOptionTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(noneOptionTask.IsCompletedSuccessfully).IsTrue();
        await Assert.That(await someOptionTask).IsEqualTo(someOption);
        await Assert.That(await noneOptionTask).IsEqualTo(noneOption);
    }

    [Test]
    public async Task ToResult_ErrorNull_Throws_ArgumentNullException()
    {
        // Arrange
        string? error = null;
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();

        // Act
        Result<int, string> ActSome() => someOption.ToResult(error!);
        Result<int, string> ActNone() => noneOption.ToResult(error!);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ToResult_Some_Error_ReturnsOk()
    {
        // Arrange
        var error = "some error";
        var option = Option.Some(42);

        // Act
        var result = option.ToResult(error);

        // Assert
        await Assert.That(result).IsOk(42);
    }

    [Test]
    public async Task ToResult_None_Error_ReturnsError()
    {
        // Arrange
        var error = "some error";
        var option = Option.None<int>();

        // Act
        var result = option.ToResult(error);

        // Assert
        await Assert.That(result).IsError(error);
    }

    [Test]
    public async Task ToResult_ErrorFactory_Null_Throws_ArgumentNullException()
    {
        // Arrange
        Func<string>? errorFacotory = null;
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();

        // Act
        Result<int, string> ActSome() => someOption.ToResult(errorFacotory!);
        Result<int, string> ActNone() => noneOption.ToResult(errorFacotory!);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ToResult_ErrorFactory_ReturnsNull_Throws_ArgumentException()
    {
        // Arrange
        Func<string> errorFacotory = () => null!;
        var option = Option.None<int>();

        // Act
        Result<int, string> ActNone() => option.ToResult(errorFacotory);

        // Assert
        await Assert.That(ActNone).Throws<ArgumentException>();
    }

    [Test]
    public async Task ToResult_Some_ErrorFactory_ReturnsOk()
    {
        // Arrange
        var errorFactory = () => "some error";
        var option = Option.Some(42);

        // Act
        var result = option.ToResult(errorFactory);

        // Assert
        await Assert.That(result).IsOk(42);
    }

    [Test]
    public async Task ToResult_None_ErrorFactory_ReturnsError()
    {
        // Arrange
        var error = "some error";
        var errorFactory = () => error;
        var option = Option.None<int>();

        // Act
        var result = option.ToResult(errorFactory);

        // Assert
        await Assert.That(result).IsError(error);
    }
}
