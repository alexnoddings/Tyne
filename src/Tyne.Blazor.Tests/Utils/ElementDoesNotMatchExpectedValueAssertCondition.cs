using AngleSharp.Dom;
using TUnit.Assertions.AssertConditions;

namespace Tyne.Blazor;

internal sealed class ElementDoesNotMatchExpectedValueAssertCondition : ExpectedValueAssertCondition<IElement, string>
{
    public ElementDoesNotMatchExpectedValueAssertCondition(string? expected) : base(expected)
    {
    }

    protected override string GetExpectation() => $"to not match {ExpectedValue}";

    protected override ValueTask<AssertionResult> GetResult(IElement? actualValue, string? expectedValue)
    {
        if (actualValue is null)
            return AssertionResult.Fail("it was null");

        if (expectedValue is null)
            return AssertionResult.Passed;

        var matches = actualValue.Matches(expectedValue);
        if (matches)
            return AssertionResult.Fail($"it matched {expectedValue}");

        return AssertionResult.Passed;
    }
}
