namespace Tyne;

public class ResultExtensionsApplyTests
{
    [Test]
    public async Task Apply1_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, string> result = null!;
        Action<int> ok = _ => { };

        // Act
        void Act() => result.Apply(ok);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("some error");
        Action<int> ok = null!;

        // Act
        void ActOk() => okResult.Apply(ok);
        void ActError() => errorResult.Apply(ok);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public void Apply1_OkResult_ExecutesOk()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var ok = Substitute.For<Action<int>>();
        ok.Invoke(Arg.Is(42));

        // Act
        result.Apply(ok);

        // Assert
        ok.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public void Apply1_ErrorResult_DoesNotExecuteOk()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");
        var ok = Substitute.For<Action<int>>();

        // Act
        result.Apply(ok);

        // Assert
        ok.DidNotReceive().Invoke(Arg.Any<int>());
    }

    [Test]
    public async Task Apply2_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, string> result = null!;
        Action<int> ok = _ => { };
        Action<string> error = _ => { };

        // Act
        void Act() => result.Apply(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_NullOk_Throws()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("some error");
        Action<int> ok = null!;
        Action<string> error = _ => { };

        // Act
        void ActOk() => okResult.Apply(ok, error);
        void ActError() => errorResult.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_NullError_Throws()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("some error");
        Action<int> ok = _ => { };
        Action<string> error = null!;

        // Act
        void ActOk() => okResult.Apply(ok, error);
        void ActError() => errorResult.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public void Apply2_OkResult_ExecutesOk()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var ok = Substitute.For<Action<int>>();
        ok.Invoke(Arg.Is(42));
        var error = Substitute.For<Action<string>>();

        // Act
        result.Apply(ok, error);

        // Assert
        ok.Received(1).Invoke(Arg.Is(42));
        error.DidNotReceive().Invoke(Arg.Any<string>());
    }

    [Test]
    public void Apply2_ErrorResult_ExecutesError()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");
        var ok = Substitute.For<Action<int>>();
        var error = Substitute.For<Action<string>>();
        error.Invoke(Arg.Is("some error"));

        // Act
        result.Apply(ok, error);

        // Assert
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        error.Received(1).Invoke(Arg.Is("some error"));
    }
}
