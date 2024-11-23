namespace Tyne;

public class ResultSelectExtensionTests
{
    [Fact]
    public void Select_ConsumesResult_SyncSelector_NullResult_Throws()
    {
        // Arrange
        Result<int>? result = null;
        int Selector(int i) => i;

        // Act
        Result<int> Act() => result!.Select(Selector);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(Act);
    }

    [Fact]
    public void Select_ConsumesResult_SyncSelector_NullSelector_Throws()
    {
        // Arrange
        var result = Result.Ok(42);
        Func<int, int>? selector = null;

        // Act
        Result<int> Act() => result.Select(selector!);

        // Assert
        _ = AssertExt.ThrowsArgumentNullException(Act);
    }

    [Fact]
    public void Select_ConsumesResult_SyncSelector_Error_ReturnsError()
    {
        // Arrange
        var result = Result.Error<int>(TestError.Instance);

        var selector = Substitute.For<Func<int, int>>();

        // Act
        var selected = result.Select(selector);

        // Assert
        _ = AssertResult.IsError(TestError.Instance, selected);
    }

    [Fact]
    public void Select_ConsumesResult_SyncSelector_Ok_ReturnsOkValue()
    {
        // Arrange
        var result = Result.Ok(42);

        var selector = Substitute.For<Func<int, int>>();
        _ = selector.Invoke(42).Returns(101);

        // Act
        var selected = result.Select(selector);

        // Assert
        _ = AssertResult.IsOk(101, selected);
        _ = selector.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Select_ConsumesResult_AsyncSelector_NullResult_Throws()
    {
        // Arrange
        Result<int>? result = null;
        Task<int> Selector(int i) => Task.FromResult(i);

        // Act
        Task<Result<int>> Act() => result!.Select(Selector);

        // Assert
        _ = await AssertExt.ThrowsArgumentNullExceptionAsync(Act);
    }

    [Fact]
    public async Task Select_ConsumesResult_AsyncSelector_NullSelector_Throws()
    {
        // Arrange
        var result = Result.Ok(42);
        Func<int, Task<int>>? selector = null;

        // Act
        Task<Result<int>> Act() => result.Select(selector!);

        // Assert
        _ = await AssertExt.ThrowsArgumentNullExceptionAsync(Act);
    }

    [Fact]
    public async Task Select_ConsumesResult_AsyncSelector_Error_ReturnsError()
    {
        // Arrange
        var result = Result.Error<int>(TestError.Instance);

        var selector = Substitute.For<Func<int, Task<int>>>();

        // Act
        var selected = await result.Select(selector);

        // Assert
        _ = AssertResult.IsError(TestError.Instance, selected);
    }

    [Fact]
    public async Task Select_ConsumesResult_AsyncSelector_Ok_ReturnsOkValue()
    {
        // Arrange
        var result = Result.Ok(42);

        var selector = Substitute.For<Func<int, Task<int>>>();
        _ = selector.Invoke(42).Returns(101);

        // Act
        var selected = await result.Select(selector);

        // Assert
        _ = AssertResult.IsOk(101, selected);
        _ = selector.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Select_ConsumesTask_SyncSelector_NullResult_Throws()
    {
        // Arrange
        Task<Result<int>>? resultTask = null;
        int Selector(int i) => i;

        // Act
        Task<Result<int>> Act() => resultTask!.Select(Selector);

        // Assert
        _ = await AssertExt.ThrowsArgumentNullExceptionAsync(Act);
    }

    [Fact]
    public async Task Select_ConsumesTask_SyncSelector_NullSelector_Throws()
    {
        // Arrange
        var resultTask = Result.Ok(42).ToTask();
        Func<int, int>? selector = null;

        // Act
        Task<Result<int>> Act() => resultTask.Select(selector!);

        // Assert
        _ = await AssertExt.ThrowsArgumentNullExceptionAsync(Act);
    }

    [Fact]
    public async Task Select_ConsumesTask_SyncSelector_Error_ReturnsError()
    {
        // Arrange
        var resultTask = Result.Error<int>(TestError.Instance).ToTask();

        var selector = Substitute.For<Func<int, int>>();

        // Act
        var selected = await resultTask.Select(selector);

        // Assert
        _ = AssertResult.IsError(TestError.Instance, selected);
    }

    [Fact]
    public async Task Select_ConsumesTask_SyncSelector_Ok_ReturnsOkValue()
    {
        // Arrange
        var resultTask = Result.Ok(42).ToTask();

        var selector = Substitute.For<Func<int, int>>();
        _ = selector.Invoke(42).Returns(101);

        // Act
        var selected = await resultTask.Select(selector);

        // Assert
        _ = AssertResult.IsOk(101, selected);
        _ = selector.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Select_ConsumesTask_AsyncSelector_NullResult_Throws()
    {
        // Arrange
        Task<Result<int>>? resultTask = null;
        Task<int> Selector(int i) => Task.FromResult(i);

        // Act
        Task<Result<int>> Act() => resultTask!.Select(Selector);

        // Assert
        _ = await AssertExt.ThrowsArgumentNullExceptionAsync(Act);
    }

    [Fact]
    public async Task Select_ConsumesTask_AsyncSelector_NullSelector_Throws()
    {
        // Arrange
        var resultTask = Result.Ok(42).ToTask();
        Func<int, Task<int>>? selector = null;

        // Act
        Task<Result<int>> Act() => resultTask.Select(selector!);

        // Assert
        _ = await AssertExt.ThrowsArgumentNullExceptionAsync(Act);
    }

    [Fact]
    public async Task Select_ConsumesTask_AsyncSelector_Error_ReturnsError()
    {
        // Arrange
        var resultTask = Result.Error<int>(TestError.Instance).ToTask();

        var selector = Substitute.For<Func<int, Task<int>>>();

        // Act
        var selected = await resultTask.Select(selector);

        // Assert
        _ = AssertResult.IsError(TestError.Instance, selected);
    }

    [Fact]
    public async Task Select_ConsumesTask_AsyncSelector_Ok_ReturnsOkValue()
    {
        // Arrange
        var resultTask = Result.Ok(42).ToTask();

        var selector = Substitute.For<Func<int, Task<int>>>();
        _ = selector.Invoke(42).Returns(101);

        // Act
        var selected = await resultTask.Select(selector);

        // Assert
        _ = AssertResult.IsOk(101, selected);
        _ = selector.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Select_Chained_Error_ReturnsError()
    {
        // Arrange
        var resultTask = Result.Error<int>(TestError.Instance).ToTask();

        var selector = Substitute.For<Func<int, Task<int>>>();

        // Act
        var selected =
            await resultTask
                .Select(selector)
                .Select(selector)
                .Select(selector);

        // Assert
        _ = AssertResult.IsError(TestError.Instance, selected);
    }

    [Fact]
    public async Task Select_Chained_Ok_ReturnsOkValue()
    {
        // Arrange
        var resultTask = Result.Ok(1).ToTask();

        var selector = Substitute.For<Func<int, Task<int>>>();
        _ = selector.Invoke(1).Returns(2);
        _ = selector.Invoke(2).Returns(4);
        _ = selector.Invoke(4).Returns(8);

        // Act
        var selected = await
            resultTask
                .Select(selector)
                .Select(selector)
                .Select(selector);

        // Assert
        _ = AssertResult.IsOk(8, selected);
        _ = selector.Received(1).Invoke(1);
        _ = selector.Received(1).Invoke(2);
        _ = selector.Received(1).Invoke(4);
    }
}
