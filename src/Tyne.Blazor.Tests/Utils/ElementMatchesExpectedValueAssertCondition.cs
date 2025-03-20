using AngleSharp.Dom;
using TUnit.Assertions.AssertConditions;

namespace Tyne.Blazor;

internal sealed class ElementMatchesExpectedValueAssertCondition : ExpectedValueAssertCondition<IElement, string>
{
    public ElementMatchesExpectedValueAssertCondition(string? expected) : base(expected)
    {
    }

    protected override string GetExpectation() => $"to match {ExpectedValue}";

    protected override ValueTask<AssertionResult> GetResult(IElement? actualValue, string? expectedValue)
    {
        if (actualValue is null)
            return AssertionResult.Fail("it was null");

        if (expectedValue is null)
            return AssertionResult.Passed;

        var matches = actualValue.Matches(expectedValue);
        if (!matches)
            return AssertionResult.Fail($"it did not match {expectedValue}");

        return AssertionResult.Passed;
    }
}
