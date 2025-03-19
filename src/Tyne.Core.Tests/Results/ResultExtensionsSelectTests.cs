namespace Tyne;

public class ResultExtensionsSelectTests
{
    #region SyncResult, SyncSelector
    [Test]
    public async Task Select_SyncResult_SyncSelector_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;
        var selector = Substitute.For<Func<int, string>>();

        // Act
        void Act() => result.Select(selector);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_SyncResult_SyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("error");
        Func<int, string> selector = null!;

        // Act
        void ActOk() => okResult.Select(selector);
        void ActError() => errorResult.Select(selector);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_SyncResult_SyncSelector_OkResult_CallsSelectorOnce()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var selector = Substitute.For<Func<int, long>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(101);

        // Act
        var newResult = result.Select(selector);

        // Assert
        await Assert.That(newResult).IsOk(101);
        selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_SyncResult_SyncSelector_OkResult_SelectorReturnsNull_ThrowsArgumentException()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        object selector(int _) => null!;

        // Act
        object Act() => result.Select(selector);

        // Assert
        await Assert.That(Act).Throws<ArgumentException>();
    }

    [Test]
    public async Task Select_SyncResult_SyncSelector_ErrorResult_DoesNotCallSelector()
    {
        // Arrange
        var result = Result.Error<int, string>("error");
        var selector = Substitute.For<Func<int, long>>();

        // Act
        var newResult = result.Select(selector);

        // Assert
        await Assert.That(newResult).IsError("error");
        selector.DidNotReceive().Invoke(Arg.Any<int>());
    }
    #endregion

    #region SyncResult, AsyncSelector
    [Test]
    public async Task Select_SyncResult_AsyncSelector_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;
        var selector = Substitute.For<Func<int, Task<string>>>();

        // Act
        void Act() => _ = result.Select(selector);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_SyncResult_AsyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<int, string>(42);
        var errorResult = Result.Error<int, string>("error");
        Func<int, Task<string>> selector = null!;

        // Act
        void ActOk() => _ = okResult.Select(selector);
        void ActError() => _ = errorResult.Select(selector);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_SyncResult_AsyncSelector_OkResult_CallsSelectorOnce()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var selector = Substitute.For<Func<int, Task<long>>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(Task.FromResult(101L));

        // Act
        var newResult = await result.Select(selector);

        // Assert
        await Assert.That(newResult).IsOk(101);
        _ = selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_SyncResult_AsyncSelector_ErrorResult_DoesNotCallSelector()
    {
        // Arrange
        var result = Result.Error<int, string>("error");
        var selector = Substitute.For<Func<int, Task<long>>>();

        // Act
        var newResult = result.Select(selector);

        // Assert
        await Assert.That(newResult).IsError("error");
    }
    #endregion

    #region AsyncResult, SyncSelector
    [Test]
    public async Task Select_AsyncResult_SyncSelector_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, int>> resultTask = null!;
        var selector = Substitute.For<Func<int, string>>();

        // Act
        void Act() => _ = resultTask.Select(selector);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_AsyncResult_SyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResultTask = Result.Error<int, string>("error").ToTask();
        Func<int, string> selector = null!;

        // Act
        void ActOk() => _ = okResultTask.Select(selector);
        void ActError() => _ = errorResultTask.Select(selector);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_AsyncResult_SyncSelector_OkResult_CallsSelectorOnce()
    {
        // Arrange
        var resultTask = Result.Ok<int, string>(42).ToTask();
        var selector = Substitute.For<Func<int, long>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(101);

        // Act
        var newResult = await resultTask.Select(selector);

        // Assert
        await Assert.That(newResult).IsOk(101);
        selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_AsyncResult_SyncSelector_ErrorResult_DoesNotCallSelector()
    {
        // Arrange
        var resultTask = Result.Error<int, string>("error").ToTask();
        var selector = Substitute.For<Func<int, long>>();

        // Act
        var newResult = await resultTask.Select(selector);

        // Assert
        await Assert.That(newResult).IsError("error");
    }
    #endregion

    #region AsyncResult, AsyncSelector
    [Test]
    public async Task Select_AsyncResult_AsyncSelector_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Result<int, int>> resultTask = null!;
        var selector = Substitute.For<Func<int, Task<string>>>();

        // Act
        void Act() => _ = resultTask.Select(selector);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_AsyncResult_AsyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var okResultTask = Result.Ok<int, string>(42).ToTask();
        var errorResultTask = Result.Error<int, string>("error").ToTask();
        Func<int, Task<string>> selector = null!;

        // Act
        void ActOk() => _ = okResultTask.Select(selector);
        void ActError() => _ = errorResultTask.Select(selector);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_AsyncResult_AsyncSelector_OkResult_CallsSelectorOnce()
    {
        // Arrange
        var resultTask = Result.Ok<int, string>(42).ToTask();
        var selector = Substitute.For<Func<int, Task<long>>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(Task.FromResult(101L));

        // Act
        var newResult = resultTask.Select(selector);

        // Assert
        await Assert.That(newResult).IsOk(101);
        _ = selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_AsyncResult_AsyncSelector_ErrorResult_DoesNotCallSelector()
    {
        // Arrange
        var resultTask = Result.Error<int, string>("error").ToTask();
        var selector = Substitute.For<Func<int, Task<long>>>();

        // Act
        var newResult = await resultTask.Select(selector);

        // Assert
        await Assert.That(newResult).IsError("error");
    }
    #endregion
}
