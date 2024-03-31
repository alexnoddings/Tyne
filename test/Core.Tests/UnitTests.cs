namespace Tyne;

public class UnitTests
{
    [Fact]
    public async Task AsTask_CompletesSuccessfully()
    {
        var task1 = Unit.AsTask;
        // Task should already be completed
        Assert.True(task1.IsCompleted);
        await task1;
    }

    [Fact]
    public void AsTask_IsCached()
    {
        var task1 = Unit.AsTask;
        var task2 = Unit.AsTask;
        Assert.Same(task1, task2);
    }

    [Fact]
    public async Task AsValueTask_CompletesSuccessfully()
    {
        var task1 = Unit.AsValueTask;
        // ValueTask should already be completed
        Assert.True(task1.IsCompleted);
        await task1;
    }

    [Fact]
    public async Task AsValueTask_IsNotCached()
    {
        // Multiple calls should return multiple ValueTasks
        await Unit.AsValueTask;
        await Unit.AsValueTask;
        Assert.True(true);
    }

    [Fact]
    public async Task Equals_Unit_AlwaysTrue()
    {
        var unit1 = Unit.Value;
        var unit2 = Unit.Value;

        Assert.Equal(unit1, unit2);
        Assert.True(unit1 == unit2);
        Assert.True(unit1.Equals(unit2));
        Assert.True(unit1.Equals(unit2 as object));
        Assert.False(unit1 != unit2);

        Assert.Equal(unit1, default);
        Assert.Equal(unit1, new Unit());
        Assert.Equal(unit1, await Unit.AsTask);
        Assert.Equal(unit1, await Unit.AsValueTask);
    }

    public static object?[][] NotEqualObjects() =>
    [
        [null],
        [string.Empty],
        [-1],
        [Unit.AsTask],
        [new object()],
        [Array.Empty<object>()]
    ];

    [Theory]
    [MemberData(nameof(NotEqualObjects))]
    public void Equals_NotUnit_AlwaysFalse(object? value)
    {
        var unit1 = Unit.Value;

        // Assert.NotEqual(unit, value) uses CompareTo, which will always return 0
        Assert.False(unit1.Equals(value));
    }

    [Fact]
    public void CompareTo_AlwaysZero()
    {
        var unit1 = Unit.Value;
        var unit2 = Unit.Value;

        Assert.Equal(0, unit1.CompareTo(unit2));
        Assert.Equal(0, unit1.CompareTo(unit2 as object));

        Assert.Equal(0, unit1.CompareTo(""));
        Assert.Equal(0, unit1.CompareTo(42));

        Assert.False(unit1 < unit2);
        Assert.True(unit1 <= unit2);
        Assert.False(unit1 > unit2);
        Assert.True(unit1 >= unit2);
    }

    [Fact]
    public void ToStringWorks() =>
        Assert.Equal("()", Unit.Value.ToString());
}
