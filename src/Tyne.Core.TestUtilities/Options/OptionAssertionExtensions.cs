using System.Runtime.CompilerServices;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;
using Tyne;
using Tyne.Assertions;

namespace TUnit.Assertions.Extensions;

public static class OptionAssertionExtensions
{
    public static InvokableValueAssertionBuilder<Option<T>> IsSome<T>(
        this IValueSource<Option<T>> valueSource,
        T expectedValue,
        [CallerArgumentExpression(nameof(valueSource))] string valueSourceExpression = "",
        [CallerArgumentExpression(nameof(expectedValue))] string expectedValueExpression = ""
    )
    {
        return valueSource.RegisterAssertion(
            assertCondition: new OptionIsSomeExpectedValueAssertCondition<T>(expectedValue),
            argumentExpressions: [valueSourceExpression, expectedValueExpression]
        );
    }

    public static InvokableValueAssertionBuilder<Option<T>> IsNone<T>(
        this IValueSource<Option<T>> valueSource,
        [CallerArgumentExpression(nameof(valueSource))] string valueSourceExpression = ""
    )
    {
        return valueSource.RegisterAssertion(
            assertCondition: new OptionIsNoneExpectedValueAssertCondition<T>(),
            argumentExpressions: [valueSourceExpression]
        );
    }
}
