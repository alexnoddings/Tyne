using TUnit.Assertions.AssertConditions;

namespace Tyne.Assertions;

internal sealed class OptionIsSomeExpectedValueAssertCondition<T> : ExpectedValueAssertCondition<Option<T>, T>
{
    public OptionIsSomeExpectedValueAssertCondition(T? expected) : base(expected)
    {
    }

    protected override string GetExpectation() => $"to be Some({ExpectedValue})";

    protected override ValueTask<AssertionResult> GetResult(Option<T> actualValue, T? expectedValue)
    {
        if (!actualValue.TryUnwrap(out var innerValue))
            return AssertionResult.Fail($"it was {actualValue}");

        if (!EqualityComparer<T>.Default.Equals(innerValue, expectedValue))
            return AssertionResult.Fail($"it was {actualValue}");

        return AssertionResult.Passed;
    }
}
