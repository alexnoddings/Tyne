namespace Tyne;

public class ResultLinqExtensionTests
{
    private sealed class EvenValueDoubler
    {
        public int ExecutionCount { get; private set; }

        public Result<int> Execute(int value)
        {
            ExecutionCount++;
            return value % 2 == 0
                ? Result.Ok(value * 2)
                : Result.Error<int>("Not supported.");
        }
    }

    [Fact]
    public void FromSelect_ChainSingle_Ok_ReturnsOk()
    {
        // Arrange
        var doubler = new EvenValueDoubler();

        // Act
        var result =
            from value in doubler.Execute(42)
            select value;

        // Assert
        _ = AssertResult.IsOk(84, result);
        Assert.Equal(1, doubler.ExecutionCount);
    }

    [Fact]
    public void FromSelect_ChainMulti_Ok_ReturnsOk()
    {
        // Arrange
        var doubler = new EvenValueDoubler();

        // Act
        var result =
            from value1 in doubler.Execute(2)
            from value2 in doubler.Execute(value1)
            from value3 in doubler.Execute(value2)
            from value4 in doubler.Execute(value3)
            select value4;

        // Assert
        _ = AssertResult.IsOk(32, result);
        Assert.Equal(4, doubler.ExecutionCount);
    }

    [Fact]
    public void FromSelect_ChainSingle_Error_ReturnsError()
    {
        // Arrange
        var doubler = new EvenValueDoubler();

        // Act
        var result =
            from value in doubler.Execute(101)
            select value;

        // Assert
        _ = AssertResult.IsError(result);
        Assert.Equal(1, doubler.ExecutionCount);
    }

    [Fact]
    public void FromSelect_ChainMulti_Error_ReturnsError()
    {
        // Arrange
        var doubler = new EvenValueDoubler();

        // Act
        var result =
            from value1 in doubler.Execute(101)
            from value2 in doubler.Execute(value1)
            from value3 in doubler.Execute(value2)
            from value4 in doubler.Execute(value3)
            select value4;

        // Assert
        _ = AssertResult.IsError(result);
        Assert.Equal(1, doubler.ExecutionCount);
    }

    [Fact]
    public void FromSelect_Chain_OkThenError_ReturnsError()
    {
        // Arrange
        var doubler = new EvenValueDoubler();

        // Act
        var result =
            from value1 in doubler.Execute(42)
            from value2 in doubler.Execute(value1)
            from value3 in doubler.Execute(value2 + 1)
            from value4 in doubler.Execute(value3)
            select value4;

        // Assert
        _ = AssertResult.IsError(result);
        Assert.Equal(3, doubler.ExecutionCount);
    }

    [Fact]
    public void FromSelect_WithLet_ReturnsOk()
    {
        // Arrange
        var doubler = new EvenValueDoubler();
        static int Trebler(int i) => i * 3;

        // Act
        var result =
            from value1 in doubler.Execute(2)
            let value2 = Trebler(value1)
            from value3 in doubler.Execute(value2)
            let value4 = Trebler(value3)
            select value4;

        // Assert
        _ = AssertResult.IsOk(72, result);
        Assert.Equal(2, doubler.ExecutionCount);
    }
}
