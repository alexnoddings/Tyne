namespace Tyne;

public class ResultLinqAsyncExtensionTests
{
    private sealed class AsyncEvenValueDoubler
    {
        public int ExecutionCount { get; private set; }

        public Task<Result<int>> ExecuteAsync(int value)
        {
            ExecutionCount++;
            var doubledValue =
                value % 2 == 0
                    ? Result.Ok(value * 2)
                    : Result.Error<int>("Not supported.");

            return Task.FromResult(doubledValue);
        }
    }

    [Fact]
    public async Task FromSelect_ChainSingle_Ok_ReturnsOk()
    {
        // Arrange
        var doubler = new AsyncEvenValueDoubler();

        // Act
        var result =
            await (
                from value in doubler.ExecuteAsync(42)
                select value
            );

        // Assert
        _ = AssertResult.IsOk(84, result);
        Assert.Equal(1, doubler.ExecutionCount);
    }

    [Fact]
    public async Task FromSelect_ChainMulti_Ok_ReturnsOk()
    {
        // Arrange
        var doubler = new AsyncEvenValueDoubler();

        // Act
        var result =
            await (
                from value1 in doubler.ExecuteAsync(2)
                from value2 in doubler.ExecuteAsync(value1)
                from value3 in doubler.ExecuteAsync(value2)
                from value4 in doubler.ExecuteAsync(value3)
                select value4
            );

        // Assert
        _ = AssertResult.IsOk(32, result);
        Assert.Equal(4, doubler.ExecutionCount);
    }

    [Fact]
    public async Task FromSelect_ChainSingle_Error_ReturnsError()
    {
        // Arrange
        var doubler = new AsyncEvenValueDoubler();

        // Act
        var result =
            await (
                from value in doubler.ExecuteAsync(101)
                select value
            );

        // Assert
        _ = AssertResult.IsError(result);
        Assert.Equal(1, doubler.ExecutionCount);
    }

    [Fact]
    public async Task FromSelect_ChainMulti_Error_ReturnsError()
    {
        // Arrange
        var doubler = new AsyncEvenValueDoubler();

        // Act
        var result =
            await (
                from value1 in doubler.ExecuteAsync(101)
                from value2 in doubler.ExecuteAsync(value1)
                from value3 in doubler.ExecuteAsync(value2)
                from value4 in doubler.ExecuteAsync(value3)
                select value4
            );

        // Assert
        _ = AssertResult.IsError(result);
        Assert.Equal(1, doubler.ExecutionCount);
    }

    [Fact]
    public async Task FromSelect_Chain_OkThenError_ReturnsError()
    {
        // Arrange
        var doubler = new AsyncEvenValueDoubler();

        // Act
        var result =
            await (
                from value1 in doubler.ExecuteAsync(42)
                from value2 in doubler.ExecuteAsync(value1)
                from value3 in doubler.ExecuteAsync(value2 + 1)
                from value4 in doubler.ExecuteAsync(value3)
                select value4
            );

        // Assert
        _ = AssertResult.IsError(result);
        Assert.Equal(3, doubler.ExecutionCount);
    }

    [Fact]
    public async Task FromSelect_WithLet_ReturnsOk()
    {
        // Arrange
        var doubler = new AsyncEvenValueDoubler();
        static int Trebler(int i) => i * 3;

        // Act
        var result =
            await (
                from value1 in doubler.ExecuteAsync(2)
                let value2 = Trebler(value1)
                from value3 in doubler.ExecuteAsync(value2)
                let value4 = Trebler(value3)
                select value4
            );

        // Assert
        _ = AssertResult.IsOk(72, result);
        Assert.Equal(2, doubler.ExecutionCount);
    }
}
