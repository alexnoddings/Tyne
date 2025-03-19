using TUnit.Assertions.AssertConditions;

namespace Tyne.Assertions;

internal sealed class ResultIsOkExpectedValueAssertCondition<T, E> : ExpectedValueAssertCondition<Result<T, E>, T>
{
    public ResultIsOkExpectedValueAssertCondition(T? expected) : base(expected)
    {
    }

    protected override string GetExpectation() => $"to be Ok({ExpectedValue})";

    protected override ValueTask<AssertionResult> GetResult(Result<T, E>? actualValue, T? expectedValue)
    {
        if (actualValue is null)
            return AssertionResult.Fail("it was null");

        if (!actualValue.TryUnwrap(out var innerValue, out _))
            return AssertionResult.Fail($"it was {actualValue}");

        if (!EqualityComparer<T>.Default.Equals(innerValue, expectedValue))
            return AssertionResult.Fail($"it was {actualValue}");

        return AssertionResult.Passed;
    }
}
