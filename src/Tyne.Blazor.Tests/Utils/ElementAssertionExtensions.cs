using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;

namespace Tyne.Blazor;

public static class ElementAssertionExtensions
{
    public static InvokableValueAssertionBuilder<IElement> Matches(
        this IValueSource<IElement> valueSource,
        string matches,
        [CallerArgumentExpression(nameof(valueSource))] string valueSourceExpression = "",
        [CallerArgumentExpression(nameof(matches))] string matchesExpression = ""
    )
    {
        return valueSource.RegisterAssertion(
            assertCondition: new ElementMatchesExpectedValueAssertCondition(matches),
            argumentExpressions: [valueSourceExpression, matchesExpression]
        );
    }

    public static InvokableValueAssertionBuilder<IElement> DoesNotMatch(
        this IValueSource<IElement> valueSource,
        string matches,
        [CallerArgumentExpression(nameof(valueSource))] string valueSourceExpression = "",
        [CallerArgumentExpression(nameof(matches))] string matchesExpression = ""
    )
    {
        return valueSource.RegisterAssertion(
            assertCondition: new ElementDoesNotMatchExpectedValueAssertCondition(matches),
            argumentExpressions: [valueSourceExpression, matchesExpression]
        );
    }

    public static InvokableValueAssertionBuilder<IElement> IsEnabled(
        this IValueSource<IElement> valueSource,
        [CallerArgumentExpression(nameof(valueSource))]
        string valueSourceExpression = ""
    ) => valueSource.DoesNotMatch("[disabled=\"\"]");

    public static InvokableValueAssertionBuilder<IElement> IsDisabled(
        this IValueSource<IElement> valueSource,
        [CallerArgumentExpression(nameof(valueSource))]
        string valueSourceExpression = ""
    ) => valueSource.Matches("[disabled=\"\"]");
}
