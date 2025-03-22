namespace Tyne;

public class ResultExtensionsApply2Tests
{
    #region SyncResult, SyncMethod
    [Test]
    public async Task Apply2_SyncResult_SyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, string> result = null!;
        var ok = Substitute.For<Action<int>>();
        var error = Substitute.For<Action<string>>();

        // Act
        Result<int, string> Act() => result.Apply(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_SyncResult_SyncMethod_NullOk_Throws()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("some error");
        Action<int> ok = null!;
        var error = Substitute.For<Action<string>>();

        // Act
        Result<int, string> ActOk() => okResult.Apply(ok, error);
        Result<int, string> ActError() => errorResult.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_SyncResult_SyncMethod_NullError_Throws()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("some error");
        var ok = Substitute.For<Action<int>>();
        Action<string> error = null!;

        // Act
        Result<int, string> ActOk() => okResult.Apply(ok, error);
        Result<int, string> ActError() => errorResult.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_SyncResult_SyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var ok = Substitute.For<Action<int>>();
        ok.Invoke(Arg.Is(42));
        var error = Substitute.For<Action<string>>();

        // Act
        var newResult = result.Apply(ok, error);

        // Assert
        ok.Received(1).Invoke(Arg.Is(42));
        error.DidNotReceive().Invoke(Arg.Any<string>());
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply2_SyncResult_SyncMethod_ErrorResult_ExecutesError()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");
        var ok = Substitute.For<Action<int>>();
        var error = Substitute.For<Action<string>>();
        error.Invoke(Arg.Is("some error"));

        // Act
        var newResult = result.Apply(ok, error);

        // Assert
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        error.Received(1).Invoke(Arg.Is("some error"));
        await Assert.That(newResult).IsError("some error");
    }
    #endregion

    #region SyncResult, AsyncMethod
    [Test]
    public async Task Apply2_SyncResult_AsyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, string> resultTask = null!;
        var ok = Substitute.For<Func<int, Task>>();
        var error = Substitute.For<Func<string, Task>>();

        // Act
        Task<Result<int, string>> Act() => resultTask.Apply(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_SyncResult_AsyncMethod_NullOk_Throws()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42);
        var errorResultTask = Result.Error<int, string>("some error");
        Func<int, Task> ok = null!;
        var error = Substitute.For<Func<string, Task>>();

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok, error);
        Task<Result<int, string>> ActError() => errorResultTask.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_SyncResult_AsyncMethod_NullError_Throws()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42);
        var errorResultTask = Result.Error<int, string>("some error");
        var ok = Substitute.For<Func<int, Task>>();
        Func<string, Task> error = null!;

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok, error);
        Task<Result<int, string>> ActError() => errorResultTask.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_SyncResult_AsyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var resultTask = Result.Ok<int, string>(42);
        var ok = Substitute.For<Func<int, Task>>();
        _ = ok.Invoke(Arg.Is(42)).Returns(Task.CompletedTask);
        var error = Substitute.For<Func<string, Task>>();

        // Act
        var newResult = await resultTask.Apply(ok, error);

        // Assert
        _ = ok.Received(1).Invoke(Arg.Is(42));
        _ = error.DidNotReceive().Invoke(Arg.Any<string>());
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply2_SyncResult_AsyncMethod_ErrorResult_ExecutesError()
    {
        // Arrange
        var resultTask = Result.Error<int, string>("some error");
        var ok = Substitute.For<Func<int, Task>>();
        var error = Substitute.For<Func<string, Task>>();
        _ = error.Invoke(Arg.Is("some error")).Returns(Task.CompletedTask);

        // Act
        var newResult = await resultTask.Apply(ok, error);

        // Assert
        _ = ok.DidNotReceive().Invoke(Arg.Any<int>());
        _ = error.Received(1).Invoke(Arg.Is("some error"));
        await Assert.That(newResult).IsError("some error");
    }
    #endregion

    #region AsyncResult, SyncMethod
    [Test]
    public async Task Apply2_AsyncResult_SyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, string>> resultTask = null!;
        var ok = Substitute.For<Action<int>>();
        var error = Substitute.For<Action<string>>();

        // Act
        Task<Result<int, string>> Act() => resultTask.Apply(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_AsyncResult_SyncMethod_NullOk_Throws()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResultTask = Result.Error<int, string>("some error").ToTask();
        Action<int> ok = null!;
        var error = Substitute.For<Action<string>>();

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok, error);
        Task<Result<int, string>> ActError() => errorResultTask.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_AsyncResult_SyncMethod_NullError_Throws()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResultTask = Result.Error<int, string>("some error").ToTask();
        var ok = Substitute.For<Action<int>>();
        Action<string> error = null!;

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok, error);
        Task<Result<int, string>> ActError() => errorResultTask.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_AsyncResult_SyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var resultTask = Result.Ok<int, string>(42).ToTask();
        var ok = Substitute.For<Action<int>>();
        ok.Invoke(Arg.Is(42));
        var error = Substitute.For<Action<string>>();

        // Act
        var newResult = await resultTask.Apply(ok, error);

        // Assert
        ok.Received(1).Invoke(Arg.Is(42));
        error.DidNotReceive().Invoke(Arg.Any<string>());
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply2_AsyncResult_SyncMethod_ErrorResult_ExecutesError()
    {
        // Arrange
        var resultTask = Result.Error<int, string>("some error").ToTask();
        var ok = Substitute.For<Action<int>>();
        var error = Substitute.For<Action<string>>();
        error.Invoke(Arg.Is("some error"));

        // Act
        var newResult = await resultTask.Apply(ok, error);

        // Assert
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        error.Received(1).Invoke(Arg.Is("some error"));
        await Assert.That(newResult).IsError("some error");
    }
    #endregion

    #region AsyncResult, AsyncMethod
    [Test]
    public async Task Apply2_AsyncResult_AsyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, string>> resultTask = null!;
        var ok = Substitute.For<Func<int, Task>>();
        var error = Substitute.For<Func<string, Task>>();

        // Act
        Task<Result<int, string>> Act() => resultTask.Apply(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_AsyncResult_AsyncMethod_NullOk_Throws()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResultTask = Result.Error<int, string>("some error").ToTask();
        Func<int, Task> ok = null!;
        var error = Substitute.For<Func<string, Task>>();

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok, error);
        Task<Result<int, string>> ActError() => errorResultTask.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_AsyncResult_AsyncMethod_NullError_Throws()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResult = Result.Error<int, string>("some error").ToTask();
        var ok = Substitute.For<Func<int, Task>>();
        Func<string, Task> error = null!;

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok, error);
        Task<Result<int, string>> ActError() => errorResult.Apply(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply2_AsyncResult_AsyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var resultTask = Result.Ok<int, string>(42).ToTask();
        var ok = Substitute.For<Func<int, Task>>();
        _ = ok.Invoke(Arg.Is(42)).Returns(Task.CompletedTask);
        var error = Substitute.For<Func<string, Task>>();

        // Act
        var newResult = await resultTask.Apply(ok, error);

        // Assert
        _ = ok.Received(1).Invoke(Arg.Is(42));
        _ = error.DidNotReceive().Invoke(Arg.Any<string>());
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply2_AsyncResult_AsyncMethod_ErrorResult_ExecutesError()
    {
        // Arrange
        var resultTask = Result.Error<int, string>("some error").ToTask();
        var ok = Substitute.For<Func<int, Task>>();
        var error = Substitute.For<Func<string, Task>>();
        _ = error.Invoke(Arg.Is("some error")).Returns(Task.CompletedTask);

        // Act
        var newResult = await resultTask.Apply(ok, error);

        // Assert
        _ = ok.DidNotReceive().Invoke(Arg.Any<int>());
        _ = error.Received(1).Invoke(Arg.Is("some error"));
        await Assert.That(newResult).IsError("some error");
    }
    #endregion
}
