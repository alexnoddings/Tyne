namespace Tyne;

public class ResultExtensionsMatchTests
{
    [Test]
    public async Task Match_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;
        var ok = Substitute.For<Func<int, string>>();
        var error = Substitute.For<Func<int, string>>();

        // Act
        void Act() => _ = result.Match(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, int>(42);
        var errorResult = Result.Error<int, int>(42);
        Func<int, string> ok = null!;
        var error = Substitute.For<Func<int, string>>();

        // Act
        void ActOk() => _ = okResult.Match(ok, error);
        void ActError() => _ = errorResult.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_NullError_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, int>(42);
        var errorResult = Result.Error<int, int>(42);
        var ok = Substitute.For<Func<int, string>>();
        Func<int, string> error = null!;

        // Act
        void ActOk() => _ = okResult.Match(ok, error);
        void ActError() => _ = errorResult.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_OkResult_ExecutesOk()
    {
        // Arrange
        const int okValue = 42;
        var result = Result.Ok<int, string>(okValue);
        var ok = Substitute.For<Func<int, int>>();
        ok.Invoke(okValue).Returns(101);
        var error = Substitute.For<Func<string, int>>();

        // Act
        var value = result.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        ok.Received(1).Invoke(okValue);
        error.DidNotReceive().Invoke(Arg.Any<string>());
    }

    [Test]
    public async Task Match_ErrorResult_ExecutesError()
    {
        // Arrange
        const string errorValue = "error";
        var result = Result.Error<int, string>(errorValue);
        var ok = Substitute.For<Func<int, int>>();
        var error = Substitute.For<Func<string, int>>();
        _ = error.Invoke(errorValue).Returns(101);

        // Act
        var value = result.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        error.Received(1).Invoke(errorValue);
    }
}
