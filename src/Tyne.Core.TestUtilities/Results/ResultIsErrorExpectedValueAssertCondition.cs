using TUnit.Assertions.AssertConditions;

namespace Tyne.Assertions;

internal sealed class ResultIsErrorExpectedValueAssertCondition<T, E> : ExpectedValueAssertCondition<Result<T, E>, E>
{
    public ResultIsErrorExpectedValueAssertCondition(E? expected) : base(expected)
    {
    }

    protected override string GetExpectation() => $"to be Error({ExpectedValue})";

    protected override ValueTask<AssertionResult> GetResult(Result<T, E>? actualValue, E? expectedValue)
    {
        if (actualValue is null)
            return AssertionResult.Fail("it was null");

        if (actualValue.TryUnwrap(out _, out var error))
            return AssertionResult.Fail($"it was {actualValue}");

        if (!EqualityComparer<E>.Default.Equals(error, expectedValue))
            return AssertionResult.Fail($"it was {actualValue}");

        return AssertionResult.Passed;
    }
}
