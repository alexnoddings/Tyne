using System.Runtime.CompilerServices;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;
using Tyne;
using Tyne.Assertions;

namespace TUnit.Assertions.Extensions;

public static class ResultAssertionExtensions
{
    public static InvokableValueAssertionBuilder<Result<T, E>> IsOk<T, E>(
        this IValueSource<Result<T, E>> valueSource,
        T expectedValue,
        [CallerArgumentExpression(nameof(valueSource))] string valueSourceExpression = "",
        [CallerArgumentExpression(nameof(expectedValue))] string expectedValueExpression = ""
    )
    {
        return valueSource.RegisterAssertion(
            assertCondition: new ResultIsOkExpectedValueAssertCondition<T, E>(expectedValue),
            argumentExpressions: [valueSourceExpression, expectedValueExpression]
        );
    }

    public static InvokableValueAssertionBuilder<Result<T, E>> IsError<T, E>(
        this IValueSource<Result<T, E>> valueSource,
        E expectedError,
        [CallerArgumentExpression(nameof(valueSource))] string valueSourceExpression = "",
        [CallerArgumentExpression(nameof(expectedError))] string expectedErrorExpression = ""
    )
    {
        return valueSource.RegisterAssertion(
            assertCondition: new ResultIsErrorExpectedValueAssertCondition<T, E>(expectedError),
            argumentExpressions: [valueSourceExpression, expectedErrorExpression]
        );
    }
}
