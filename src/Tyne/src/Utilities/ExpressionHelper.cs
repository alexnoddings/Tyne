using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Utilities;

/// <summary>
///		Helpers for working with <see cref="Expression"/>s.
/// </summary>
public static class ExpressionHelper
{
	/// <summary>
	///		Gets a <see cref="MemberExpression"/> for the member accessed by <paramref name="expression"/>.
	/// </summary>
	/// <typeparam name="TSource">The type containing the member.</typeparam>
	/// <typeparam name="TValue">The type of the member being accessed.</typeparam>
	/// <param name="expression">An <see cref="Expression"/> accessing a member of type <typeparamref name="TSource"/> on <typeparamref name="TValue"/>.</param>
	/// <returns>A <see cref="MemberExpression"/> for the member accessed by <paramref name="expression"/>.</returns>
	/// <exception cref="ArgumentException">When <paramref name="expression"/> is not of the expected form <c>$Foo => $Foo.Bar</c>.</exception>
	/// <remarks>
	///		This expects <paramref name="expression"/> to be of the form <c>$Foo => $Foo.Bar</c>.
	/// </remarks>
	public static MemberExpression GetMemberExpression<TSource, TValue>(Expression<Func<TSource, TValue>> expression)
	{
		if (expression.Body is MemberExpression memberExpression)
			return memberExpression;

		if (expression.Body is UnaryExpression unaryExpression
			&& unaryExpression.NodeType == ExpressionType.Convert
			&& unaryExpression.Operand is MemberExpression unaryMemberExpression)
			return unaryMemberExpression;

		throw new ArgumentException("Expression was not of the expected form `$Foo => $Foo.Bar`.", nameof(expression));
	}

	/// <summary>
	///		Gets a <see cref="PropertyInfo"/> for the property accessed by <paramref name="expression"/>.
	/// </summary>
	/// <typeparam name="TSource">The type containing the property.</typeparam>
	/// <typeparam name="TValue">The type of the property being accessed.</typeparam>
	/// <param name="expression">An <see cref="Expression"/> accessing a property of type <typeparamref name="TSource"/> on <typeparamref name="TValue"/>.</param>
	/// <returns>A <see cref="PropertyInfo"/> for the property accessed by <paramref name="expression"/>.</returns>
	/// <exception cref="ArgumentException">When <paramref name="expression"/> is not of the expected form <c>$Foo => $Foo.Bar</c>, or the member accessed is not a property.</exception>
	public static PropertyInfo GetPropertyInfo<TSource, TValue>(Expression<Func<TSource, TValue>> expression)
	{
		MemberInfo memberInfo = GetMemberExpression(expression).Member;
		if (memberInfo is PropertyInfo propertyInfo)
			return propertyInfo;

		throw new ArgumentException($"Member {memberInfo.Name} is not a property.", nameof(expression));
	}

	/// <summary>
	///		Gets the name of the property accessed by <paramref name="expression"/>.
	/// </summary>
	/// <typeparam name="TSource">The type containing the property.</typeparam>
	/// <typeparam name="TValue">The type of the property being accessed.</typeparam>
	/// <param name="expression">An <see cref="Expression"/> accessing a property of type <typeparamref name="TSource"/> on <typeparamref name="TValue"/>.</param>
	/// <returns>The name of the property accessed by <paramref name="expression"/>.</returns>
	/// <exception cref="ArgumentException">When <paramref name="expression"/> is not of the expected form <c>$Foo => $Foo.Bar</c>, or the member accessed is not a property.</exception>
	public static string GetPropertyName<TSource, TValue>(Expression<Func<TSource, TValue>> expression) =>
		GetPropertyInfo(expression).Name;
}
