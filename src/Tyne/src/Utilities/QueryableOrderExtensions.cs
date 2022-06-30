using System.Linq.Expressions;
using System.Reflection;
using Tyne.Utilities;

namespace System.Linq;

/// <summary>
///     Helpers for ordering a <see cref="IQueryable{T}"/> by the name of a property.
/// </summary>
public static class QueryableOrderExtensions
{
	private static MethodInfo OrderByMethod { get; }
		= MethodHelper.Get(typeof(Queryable), nameof(Queryable.OrderBy), typeof(IQueryable<>), typeof(Expression<>));
	private static MethodInfo OrderByDescendingMethod { get; }
		= MethodHelper.Get(typeof(Queryable), nameof(Queryable.OrderByDescending), typeof(IQueryable<>), typeof(Expression<>));

	public static IQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool isDescending)
	{
		if (source is null)
			throw new ArgumentNullException(nameof(source));

		return isDescending
			? source.OrderByDescending(keySelector)
			: source.OrderBy(keySelector);
	}

	/// <summary>
	///		Sorts the elements of <paramref name="source"/> according to the <paramref name="propertyName"/>.
	///		If this is not set, <paramref name="source"/> is returned unmodified.
	///		Can be sorted in descending order using <paramref name="isDescending"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">A sequence of values to order.</param>
	/// <param name="propertyName"></param>
	/// <param name="isDescending">Whether to sort in ascending order (when <see langword="false"/>) or in descending order (when <see langword="true"/>).</param>
	/// <param name="throwForInvalidProperties">Whether to throw an <see cref="ArgumentException"/> when <typeparamref name="TSource"/> does not have a <paramref name="propertyName"/> property. If <see langword="true"/>, this will only throw when <paramref name="propertyName"/> is not null or whitespace.</param>
	/// <returns>An <see cref="IQueryable{TSource}"/> whose elements are sorted according to the <paramref name="propertyName"/>.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
	public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string? propertyName, bool isDescending = false, bool throwForInvalidProperties = false)
	{
		if (source is null)
			throw new ArgumentNullException(nameof(source));

		if (!string.IsNullOrWhiteSpace(propertyName))
			return OrderByCore(source, propertyName, isDescending, throwForInvalidProperties);

		return source;
	}


	/// <summary>
	///		Sorts the elements of <paramref name="source"/> according to the <paramref name="propertyName"/>.
	///		If one is not set, <paramref name="fallbackPropertyName"/> is used instead.
	///		If this is not set either, <paramref name="source"/> is returned unmodified.
	///		Can be sorted in descending order using <paramref name="isDescending"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">A sequence of values to order.</param>
	/// <param name="propertyName"></param>
	/// <param name="isDescending">Whether to sort in ascending order (when <see langword="false"/>) or in descending order (when <see langword="true"/>).</param>
	/// <returns>An <see cref="IQueryable{TSource}"/> whose elements are sorted according to the <paramref name="propertyName"/>.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
	/// <remarks>
	///		If <paramref name="propertyName"/> is null or whitespace, the <paramref name="source"/> is returned unmodified.
	/// </remarks>
	public static IQueryable<TSource> OrderByOrDefault<TSource>(this IQueryable<TSource> source, string? propertyName, string? fallbackPropertyName, bool isDescending = false, bool throwForInvalidProperties = false)
	{
		if (source is null)
			throw new ArgumentNullException(nameof(source));

		if (!string.IsNullOrWhiteSpace(propertyName))
			return OrderByCore(source, propertyName, isDescending, throwForInvalidProperties);

		if (!string.IsNullOrWhiteSpace(fallbackPropertyName))
			return OrderByCore(source, fallbackPropertyName, isDescending, throwForInvalidProperties);

		return source;
	}

	/// <summary>
	///		Sorts the elements of <paramref name="source"/> according to the <paramref name="propertyName"/>.
	///		If this is not set, <paramref name="source"/> is returned unmodified.
	///		Can be sorted in descending order using <paramref name="isDescending"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">A sequence of values to order.</param>
	/// <param name="propertyName">The property on <typeparamref name="TSource"/> to sort by.</param>
	/// <param name="isDescending">Whether to sort in ascending order (when <see langword="false"/>) or in descending order (when <see langword="true"/>).</param>
	/// <param name="throwForInvalidProperties">
	///		Whether to throw an <see cref="ArgumentException"/> when <typeparamref name="TSource"/> does not have a <paramref name="propertyName"/> property.
	///		If <see langword="true"/>, this will only throw when <paramref name="propertyName"/> is not null or whitespace.
	///		If <see langword="false"/>
	///	</param>
	/// <returns>An <see cref="IQueryable{TSource}"/> whose elements are sorted according to the <paramref name="propertyName"/>.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
	private static IQueryable<TSource> OrderByCore<TSource>(IQueryable<TSource> source, string propertyName, bool isDescending = false, bool throwForInvalidProperties = false)
	{
		Type sourceType = typeof(TSource);
		PropertyInfo? propertyInfo = sourceType.GetProperty(propertyName);
		if (propertyInfo is null)
		{
			if (throwForInvalidProperties)
				throw new ArgumentException($"{sourceType.FullName} does not have a property named {propertyName}.", nameof(propertyName));

			return source;
		}

		// An expression for the parameter (e.g. `$Foo =>`)
		ParameterExpression parameter = Expression.Parameter(sourceType, sourceType.Name);
		// An expression for the property (e.g. `$Foo => $Foo.Bar`)
		MemberExpression property = Expression.Property(parameter, propertyInfo);
		// Create a lambda for the property expression
		LambdaExpression lambda = Expression.Lambda(property, parameter);

		MethodInfo orderByMethod = isDescending ? OrderByDescendingMethod : OrderByMethod;
		// Creates a generic version of the OrderBy method (e.g. turns `.OrderBy<,>` into `.OrderBy<Foo, Bar>`)
		orderByMethod = orderByMethod.MakeGenericMethod(sourceType, propertyInfo.PropertyType);

		// The parameters to invoke OrderBy with
		var parameters = new object[] { source, lambda };

		// Pass null for the object instance as OrderBy is an extension method 
		var newSource = orderByMethod.Invoke(null, parameters);

		return newSource as IQueryable<TSource>
			// This should never happen as there is no reason for the OrderBy method to return null or something other than IQueryable
			?? throw new InvalidOperationException($"{orderByMethod.Name} provided a {newSource?.GetType().Name} which is not assignable to {nameof(IQueryable<TSource>)}.");
	}
}
