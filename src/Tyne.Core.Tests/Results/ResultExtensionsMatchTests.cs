namespace Tyne;

public class ResultExtensionsMatchTests
{
    #region SyncResult, SyncMethod
    [Test]
    public async Task Match_SyncResult_SyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;
        var ok = Substitute.For<Func<int, string>>();
        var error = Substitute.For<Func<int, string>>();

        // Act
        string Act() => result.Match(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_SyncResult_SyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, int>(42);
        var errorResult = Result.Error<int, int>(42);
        Func<int, string> ok = null!;
        var error = Substitute.For<Func<int, string>>();

        // Act
        string ActOk() => okResult.Match(ok, error);
        string ActError() => errorResult.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_SyncResult_SyncMethod_NullError_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, int>(42);
        var errorResult = Result.Error<int, int>(42);
        var ok = Substitute.For<Func<int, string>>();
        Func<int, string> error = null!;

        // Act
        string ActOk() => okResult.Match(ok, error);
        string ActError() => errorResult.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_SyncResult_SyncMethod_OkResult_ExecutesOk()
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
    public async Task Match_SyncResult_SyncMethod_ErrorResult_ExecutesError()
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
    #endregion

    #region SyncResult, AsyncMethod
    [Test]
    public async Task Match_SyncResult_AsyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;
        var ok = Substitute.For<Func<int, Task<string>>>();
        var error = Substitute.For<Func<int, Task<string>>>();

        // Act
        Task<string> Act() => result.Match(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_SyncResult_AsyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, int>(42);
        var errorResult = Result.Error<int, int>(42);
        Func<int, Task<string>> ok = null!;
        var error = Substitute.For<Func<int, Task<string>>>();

        // Act
        Task<string> ActOk() => okResult.Match(ok, error);
        Task<string> ActError() => errorResult.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_SyncResult_AsyncMethod_NullError_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, int>(42);
        var errorResult = Result.Error<int, int>(42);
        var ok = Substitute.For<Func<int, Task<string>>>();
        Func<int, Task<string>> error = null!;

        // Act
        Task<string> ActOk() => okResult.Match(ok, error);
        Task<string> ActError() => errorResult.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_SyncResult_AsyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        const int okValue = 42;
        var result = Result.Ok<int, string>(okValue);
        var ok = Substitute.For<Func<int, Task<int>>>();
        ok.Invoke(okValue).Returns(101);
        var error = Substitute.For<Func<string, Task<int>>>();

        // Act
        var value = await result.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        _ = ok.Received(1).Invoke(okValue);
        _ = error.DidNotReceive().Invoke(Arg.Any<string>());
    }

    [Test]
    public async Task Match_SyncResult_AsyncMethod_ErrorResult_ExecutesError()
    {
        // Arrange
        const string errorValue = "error";
        var result = Result.Error<int, string>(errorValue);
        var ok = Substitute.For<Func<int, Task<int>>>();
        var error = Substitute.For<Func<string, Task<int>>>();
        _ = error.Invoke(errorValue).Returns(101);

        // Act
        var value = await result.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        _ = ok.DidNotReceive().Invoke(Arg.Any<int>());
        _ = error.Received(1).Invoke(errorValue);
    }
    #endregion

    #region AsyncResult, SyncMethod
    [Test]
    public async Task Match_AsyncResult_SyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, int>> result = null!;
        var ok = Substitute.For<Func<int, string>>();
        var error = Substitute.For<Func<int, string>>();

        // Act
        Task<string> Act() => result.Match(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_AsyncResult_SyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, int>(42).ToTask();
        var errorResultTask = Result.Error<int, int>(42).ToTask();
        Func<int, string> ok = null!;
        var error = Substitute.For<Func<int, string>>();

        // Act
        Task<string> ActOk() => okResultTask.Match(ok, error);
        Task<string> ActError() => errorResultTask.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_AsyncResult_SyncMethod_NullError_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, int>(42).ToTask();
        var errorResultTask = Result.Error<int, int>(42).ToTask();
        var ok = Substitute.For<Func<int, string>>();
        Func<int, string> error = null!;

        // Act
        Task<string> ActOk() => okResultTask.Match(ok, error);
        Task<string> ActError() => errorResultTask.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_AsyncResult_SyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        const int okValue = 42;
        var resultTask = Result.Ok<int, string>(okValue).ToTask();
        var ok = Substitute.For<Func<int, int>>();
        ok.Invoke(okValue).Returns(101);
        var error = Substitute.For<Func<string, int>>();

        // Act
        var value = await resultTask.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        ok.Received(1).Invoke(okValue);
        error.DidNotReceive().Invoke(Arg.Any<string>());
    }

    [Test]
    public async Task Match_AsyncResult_SyncMethod_ErrorResult_ExecutesError()
    {
        // Arrange
        const string errorValue = "error";
        var resultTask = Result.Error<int, string>(errorValue).ToTask();
        var ok = Substitute.For<Func<int, int>>();
        var error = Substitute.For<Func<string, int>>();
        _ = error.Invoke(errorValue).Returns(101);

        // Act
        var value = await resultTask.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        ok.DidNotReceive().Invoke(Arg.Any<int>());
        error.Received(1).Invoke(errorValue);
    }
    #endregion

    #region AsyncResult, AsyncMethod
    [Test]
    public async Task Match_AsyncResult_AsyncMethod_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, int>> resultTask = null!;
        var ok = Substitute.For<Func<int, Task<string>>>();
        var error = Substitute.For<Func<int, Task<string>>>();

        // Act
        Task<string> Act() => resultTask.Match(ok, error);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_AsyncResult_AsyncMethod_NullOk_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, int>(42).ToTask();
        var errorResultTask = Result.Error<int, int>(42).ToTask();
        Func<int, Task<string>> ok = null!;
        var error = Substitute.For<Func<int, Task<string>>>();

        // Act
        Task<string> ActOk() => okResultTask.Match(ok, error);
        Task<string> ActError() => errorResultTask.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_AsyncResult_AsyncMethod_NullError_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, int>(42).ToTask();
        var errorResultTask = Result.Error<int, int>(42).ToTask();
        var ok = Substitute.For<Func<int, Task<string>>>();
        Func<int, Task<string>> error = null!;

        // Act
        Task<string> ActOk() => okResultTask.Match(ok, error);
        Task<string> ActError() => errorResultTask.Match(ok, error);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Match_AsyncResult_AsyncMethod_OkResult_ExecutesOk()
    {
        // Arrange
        const int okValue = 42;
        var resultTask = Result.Ok<int, string>(okValue).ToTask();
        var ok = Substitute.For<Func<int, Task<int>>>();
        ok.Invoke(okValue).Returns(101);
        var error = Substitute.For<Func<string, Task<int>>>();

        // Act
        var value = await resultTask.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        _ = ok.Received(1).Invoke(okValue);
        _ = error.DidNotReceive().Invoke(Arg.Any<string>());
    }

    [Test]
    public async Task Match_AsyncResult_AsyncMethod_ErrorResult_ExecutesError()
    {
        // Arrange
        const string errorValue = "error";
        var resultTask = Result.Error<int, string>(errorValue).ToTask();
        var ok = Substitute.For<Func<int, Task<int>>>();
        var error = Substitute.For<Func<string, Task<int>>>();
        _ = error.Invoke(errorValue).Returns(101);

        // Act
        var value = await resultTask.Match(ok.Invoke, error.Invoke);

        // Assert
        await Assert.That(value).IsEqualTo(101);
        _ = ok.DidNotReceive().Invoke(Arg.Any<int>());
        _ = error.Received(1).Invoke(errorValue);
    }
    #endregion
}
