namespace Tyne;

public class ResultExtensionsApply1Tests
{
    #region SyncResult, SyncMethod
    [Test]
    public async Task Apply1_SyncResult_SyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, string> result = null!;
        var ok = Substitute.For<Action<int>>();

        // Act
        Result<int, string> Act() => result.Apply(ok);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_SyncResult_SyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("some error");
        Action<int> ok = null!;

        // Act
        Result<int, string>  ActOk() => okResult.Apply(ok);
        Result<int, string>  ActError() => errorResult.Apply(ok);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_SyncResult_SyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var ok = Substitute.For<Action<int>>();
        ok.Invoke(Arg.Is(42));

        // Act
        var newResult = result.Apply(ok);

        // Assert
        ok.Received(1).Invoke(Arg.Is(42));
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply1_SyncResult_SyncMethod_ErrorResult_DoesNotExecuteOk()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");
        var ok = Substitute.For<Action<int>>();

        // Act
        var newResult = result.Apply(ok);

        // Assert
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        await Assert.That(newResult).IsError("some error");
    }
    #endregion

    #region SyncResult, AsyncMethod
    [Test]
    public async Task Apply1_SyncResult_AsyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, string> result = null!;
        var ok = Substitute.For<Func<int, Task>>();

        // Act
        Task<Result<int, string>> Act() => result.Apply(ok);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_SyncResult_AsyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("some error");
        Func<int, Task> ok = null!;

        // Act
        Task<Result<int, string>> ActOk() => okResult.Apply(ok);
        Task<Result<int, string>> ActError() => errorResult.Apply(ok);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_SyncResult_AsyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var ok = Substitute.For<Func<int, Task>>();
        _ = ok.Invoke(Arg.Is(42)).Returns(Task.CompletedTask);

        // Act
        var newResult = await result.Apply(ok);

        // Assert
        _ = ok.Received(1).Invoke(Arg.Is(42));
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply1_SyncResult_AsyncMethod_ErrorResult_DoesNotExecuteOk()
    {
        // Arrange
        var result = Result.Error<int, string>("some error");
        var ok = Substitute.For<Func<int, Task>>();

        // Act
        var newResult = await result.Apply(ok);

        // Assert
        _ = ok.DidNotReceive().Invoke(Arg.Any<int>());
        await Assert.That(newResult).IsError("some error");
    }
    #endregion

    #region AyncResult, SyncMethod
    [Test]
    public async Task Apply1_AsyncResult_SyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, string>> resultTask = null!;
        var ok = Substitute.For<Action<int>>();

        // Act
        Task<Result<int, string>> Act() => resultTask.Apply(ok);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_AsyncResult_SyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResultTask = Result.Error<int, string>("some error").ToTask();
        Action<int> ok = null!;

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok);
        Task<Result<int, string>> ActError() => errorResultTask.Apply(ok);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_AsyncResult_SyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var resultTask = Result.Ok<int, string>(42).ToTask();
        var ok = Substitute.For<Action<int>>();
        ok.Invoke(Arg.Is(42));

        // Act
        var newResult = await resultTask.Apply(ok);

        // Assert
        ok.Received(1).Invoke(Arg.Is(42));
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply1_AsyncResult_SyncMethod_ErrorResult_DoesNotExecuteOk()
    {
        // Arrange
        var resultTask = Result.Error<int, string>("some error").ToTask();
        var ok = Substitute.For<Action<int>>();

        // Act
        var newResult = await resultTask.Apply(ok);

        // Assert
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        await Assert.That(newResult).IsError("some error");
    }
    #endregion

    #region AsyncResult, AsyncMethod
    [Test]
    public async Task Apply1_AsyncResult_AsyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, string>> resultTask = null!;
        var ok = Substitute.For<Func<int, Task>>();

        // Act
        Task<Result<int, string>> Act() => resultTask.Apply(ok);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_AsyncResult_AsyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResultTask = Result.Error<int, string>("some error").ToTask();
        Func<int, Task> ok = null!;

        // Act
        Task<Result<int, string>> ActOk() => okResultTask.Apply(ok);
        Task<Result<int, string>> ActError() => errorResultTask.Apply(ok);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Apply1_AsyncResult_AsyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        var resultTask = Result.Ok<int, string>(42).ToTask();
        var ok = Substitute.For<Func<int, Task>>();
        _ = ok.Invoke(Arg.Is(42)).Returns(Task.CompletedTask);

        // Act
        var newResult = await resultTask.Apply(ok);

        // Assert
        _ = ok.Received(1).Invoke(Arg.Is(42));
        await Assert.That(newResult).IsOk(42);
    }

    [Test]
    public async Task Apply1_AsyncResult_AsyncMethod_ErrorResult_DoesNotExecuteOk()
    {
        // Arrange
        var resultTask = Result.Error<int, string>("some error").ToTask();
        var ok = Substitute.For<Func<int, Task>>();

        // Act
        var newResult = await resultTask.Apply(ok);

        // Assert
        _ = ok.DidNotReceive().Invoke(Arg.Any<int>());
        await Assert.That(newResult).IsError("some error");
    }
    #endregion
}
