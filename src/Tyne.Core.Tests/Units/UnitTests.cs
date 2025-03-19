using System.Diagnostics.CodeAnalysis;
using Tyne.Preludes.Core;
using Assert = TUnit.Assertions.Assert;

namespace Tyne;

public class UnitTests
{
    [Test]
    public async Task AsTask_CompletesSuccessfully()
    {
        var task1 = Unit.AsTask;

        // Task should already be completed
        await Assert.That(task1.IsCompleted).IsTrue();
        _ = await task1;
    }

    [Test]
    public async Task AsTask_IsCached()
    {
#pragma warning disable AsyncFixer05
        var task1 = Unit.AsTask;
        var task2 = Unit.AsTask;
        await Assert.That<Task<Unit>>(task1).IsEqualTo(task2);
#pragma warning restore AsyncFixer05
    }

    [Test]
    public async Task AsValueTask_CompletesSuccessfully()
    {
        var task1 = Unit.AsValueTask;
        // ValueTask should already be completed
        await Assert.That(task1.IsCompleted).IsTrue();
        _ = await task1;
    }

    [Test]
    public async Task AsValueTask_IsNotCached()
    {
        // Multiple calls should return multiple ValueTasks
        // (a ValueTask can only be awaited once, so this would throw if it was the same one)
        _ = await Unit.AsValueTask;
        _ = await Unit.AsValueTask;
    }

    [Test]
    public async Task Equals_Unit_AlwaysTrue()
    {
        var unit1 = UnitPrelude.unit;
        var unit2 = Unit.Value;

        await Assert.That(unit1).IsEqualTo(unit2);
        await Assert.That(unit2).IsEqualTo(unit1);

        await Assert.That(unit1 == unit2).IsTrue();
        await Assert.That(unit2 == unit1).IsTrue();

        await Assert.That(unit1 != unit2).IsFalse();
        await Assert.That(unit2 != unit1).IsFalse();

        await Assert.That(unit1.Equals(unit2)).IsTrue();
        await Assert.That(unit2.Equals(unit1)).IsTrue();

        await Assert.That(unit1.Equals(unit2 as object)).IsTrue();
        await Assert.That(unit2.Equals(unit1 as object)).IsTrue();

        await Assert.That(unit1).IsEqualTo(default);
        await Assert.That(unit1).IsEqualTo(new Unit());
        await Assert.That(unit1).IsEqualTo(await Unit.AsTask);
        await Assert.That(unit1).IsEqualTo(await Unit.AsValueTask);
    }

    [ExcludeFromCodeCoverage]
    [SuppressMessage("Usage", "TUnit0046: Return a `Func<T>` rather than a `<T>`.", Justification = "False positive.")]
    public static IEnumerable<Func<object?>> NotEqualData()
    {
        yield return () => null;
        yield return () => string.Empty;
        yield return () => -1;
        yield return () => Unit.AsTask;
        yield return () => new object();
        yield return Array.Empty<object>;
    }

    [Test]
    [MethodDataSource(nameof(NotEqualData))]
    public async Task Equals_NotUnit_AlwaysFalse(object? value)
    {
        var unit1 = Unit.Value;

        // Assert.NotEqual(unit, value) uses CompareTo, which will always return 0
        await Assert.That(unit1.Equals(value)).IsFalse();
    }

    [Test]
    public async Task CompareTo_AlwaysZero()
    {
        var unit1 = Unit.Value;
        var unit2 = Unit.Value;

        await Assert.That(unit1.CompareTo(unit2)).IsEqualTo(0);
        await Assert.That(unit1.CompareTo(unit2 as object)).IsEqualTo(0);

        await Assert.That(unit1.CompareTo("")).IsEqualTo(0);
        await Assert.That(unit1.CompareTo(42)).IsEqualTo(0);

        await Assert.That(unit1 < unit2).IsFalse();
        await Assert.That(unit1 <= unit2).IsTrue();
        await Assert.That(unit1 > unit2).IsFalse();
        await Assert.That(unit1 >= unit2).IsTrue();
    }

    [Test]
    public async Task ToString_ReturnsBrackets() =>
        await Assert.That(Unit.Value.ToString()).IsEqualTo("()");

    [Test]
    public async Task GetHashCode_ReturnsZero()
    {
        // Act
        var hashCode = Unit.Value.GetHashCode();

        await Assert.That(hashCode).IsEqualTo(0);
    }
}
