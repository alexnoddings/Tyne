using TUnit.Assertions.AssertConditions;

namespace Tyne.Assertions;

internal sealed class OptionIsNoneAssertCondition<T> : BaseAssertCondition<Option<T>>
{
    protected override string GetExpectation() => "to be None";

    protected override ValueTask<AssertionResult> GetResult(
        Option<T> actualValue,
        Exception? exception,
        AssertionMetadata assertionMetadata
    )
    {
        if (actualValue.TryUnwrap(out _))
            return AssertionResult.Fail($"it was {actualValue}");

        return AssertionResult.Passed;
    }
}
