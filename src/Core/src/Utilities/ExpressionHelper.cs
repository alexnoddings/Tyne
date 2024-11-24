using System.Linq.Expressions;
using System.Reflection;

namespace Tyne;

/// <summary>
///		Helper methods for working with <see cref="Expression"/>s.
/// </summary>
public static class ExpressionHelper
{
    /// <summary>
    ///		Gets a <see cref="MemberExpression"/> for the member accessed by <paramref name="expression"/>.
    ///		Returns <see langword="null"/> if the <paramref name="expression"/> is not of the form <c>foo => foo.Bar</c>.
    /// </summary>
    /// <typeparam name="TSource">The type containing the member.</typeparam>
    /// <typeparam name="TValue">The type of the member being accessed.</typeparam>
    /// <param name="expression">An <see cref="Expression"/> accessing a member of type <typeparamref name="TSource"/> on <typeparamref name="TValue"/>.</param>
    /// <returns>
    ///     A <see cref="MemberExpression"/> for the member accessed by <paramref name="expression"/>,
    ///     or <see langword="null"/> if the <paramref name="expression"/> is not of the form <c>foo => foo.Bar</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">When <paramref name="expression"/> is null.</exception>
    /// <remarks>
    ///		This expects <paramref name="expression"/> to be of the form <c>foo => foo.Bar</c>.
    /// </remarks>
    public static MemberExpression? TryGetAccessedMemberExpression<TSource, TValue>(Expression<Func<TSource, TValue>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        // A simple expression for foo => foo.Bar
        if (expression.Body is MemberExpression memberExpression)
            return memberExpression;

        // A more complex expression involving a conversion, such as a cast (e.g. an int to a nullable int)
        if (expression.Body is UnaryExpression unaryExpression
            && unaryExpression.NodeType == ExpressionType.Convert
            && unaryExpression.Operand is MemberExpression unaryMemberExpression)
        {
            return unaryMemberExpression;
        }

        return null;
    }

    /// <summary>
    ///		Gets a <see cref="MemberExpression"/> for the member accessed by <paramref name="expression"/>.
    /// </summary>
    /// <typeparam name="TSource">The type containing the member.</typeparam>
    /// <typeparam name="TValue">The type of the member being accessed.</typeparam>
    /// <param name="expression">An <see cref="Expression"/> accessing a member of type <typeparamref name="TSource"/> on <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="MemberExpression"/> for the member accessed by <paramref name="expression"/>.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="expression"/> is null.</exception>
    /// <exception cref="ArgumentException">When <paramref name="expression"/> is not of the expected form <c>foo => foo.Bar</c>.</exception>
    /// <remarks>
    ///		This expects <paramref name="expression"/> to be of the form <c>foo => foo.Bar</c>.
    /// </remarks>
    public static MemberExpression GetAccessedMemberExpression<TSource, TValue>(Expression<Func<TSource, TValue>> expression) =>
        TryGetAccessedMemberExpression(expression)
        ?? throw new ArgumentException("Expression was not of the expected form `$Foo => $Foo.Bar`.", nameof(expression));

    /// <summary>
    ///		Gets a <see cref="PropertyInfo"/> for the property accessed by <paramref name="expression"/>.
    ///		Returns <see langword="null"/> if the <paramref name="expression"/> is not of the form <c>foo => foo.Bar</c>.
    /// </summary>
    /// <typeparam name="TSource">The type containing the property.</typeparam>
    /// <typeparam name="TValue">The type of the property being accessed.</typeparam>
    /// <param name="expression">An <see cref="Expression"/> accessing a property of type <typeparamref name="TSource"/> on <typeparamref name="TValue"/>.</param>
    /// <returns>
    ///     A <see cref="PropertyInfo"/> for the property accessed by <paramref name="expression"/>.,
    ///     or <see langword="null"/> if the <paramref name="expression"/> is not of the form <c>foo => foo.Bar</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">When <paramref name="expression"/> is null.</exception>
    /// <remarks>
    ///		This expects <paramref name="expression"/> to be of the form <c>foo => foo.Bar</c>, where <c>Bar</c> is a property on the <c>Foo</c> type.
    /// </remarks>
    public static PropertyInfo? TryGetAccessedPropertyInfo<TSource, TValue>(Expression<Func<TSource, TValue>> expression)
    {
        var memberInfo = TryGetAccessedMemberExpression(expression)?.Member;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo;

        return null;
    }

    /// <summary>
    ///		Gets a <see cref="PropertyInfo"/> for the property accessed by <paramref name="expression"/>.
    /// </summary>
    /// <typeparam name="TSource">The type containing the property.</typeparam>
    /// <typeparam name="TValue">The type of the property being accessed.</typeparam>
    /// <param name="expression">An <see cref="Expression"/> accessing a property of type <typeparamref name="TSource"/> on <typeparamref name="TValue"/>.</param>
    /// <returns>A <see cref="PropertyInfo"/> for the property accessed by <paramref name="expression"/>.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="expression"/> is null.</exception>
    /// <exception cref="ArgumentException">When <paramref name="expression"/> is not of the expected form <c>foo => foo.Bar</c>, or the member accessed is not a property.</exception>
    /// <remarks>
    ///		This expects <paramref name="expression"/> to be of the form <c>foo => foo.Bar</c>, where <c>Bar</c> is a property on the <c>Foo</c> type.
    /// </remarks>
    public static PropertyInfo GetAccessedPropertyInfo<TSource, TValue>(Expression<Func<TSource, TValue>> expression)
    {
        var memberInfo = GetAccessedMemberExpression(expression).Member;
        if (memberInfo is PropertyInfo propertyInfo)
            return propertyInfo;

        throw new ArgumentException($"Member {memberInfo.Name} is not a property.", nameof(expression));
    }
}
