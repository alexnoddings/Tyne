using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Tyne;

/// <summary>
///     Extensions for ordering <see cref="IQueryable{T}"/>s.
/// </summary>
public static class QueryableOrderingExtensions
{
    private static MethodInfo OrderByMethod { get; }
        = MethodHelper.Get(typeof(Queryable), nameof(Queryable.OrderBy), typeof(IQueryable<>), typeof(Expression<>));
    private static MethodInfo OrderByDescendingMethod { get; }
        = MethodHelper.Get(typeof(Queryable), nameof(Queryable.OrderByDescending), typeof(IQueryable<>), typeof(Expression<>));

    /// <summary>
    ///     Sorts the elements of <paramref name="source"/> according to <paramref name="keySelector"/> in the direction controlled by <paramref name="isDescending"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the property represented by <paramref name="keySelector"/>.</typeparam>
    /// <param name="source">A sequence of <typeparamref name="TSource"/>s to order.</param>
    /// <param name="keySelector">A function to extract a property from a <typeparamref name="TSource"/>.</param>
    /// <param name="isDescending">A <see cref="bool"/> indicating if the ordering should be ascending (<see langword="false"/>) or descdending (<see langword="true"/>).</param>
    /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to <paramref name="keySelector"/> and <paramref name="isDescending"/>.</returns>
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool isDescending) =>
        isDescending
        ? source.OrderByDescending(keySelector)
        : source.OrderBy(keySelector);

    /// <summary>
    ///     Sorts the elements of <paramref name="source"/> according to the property <paramref name="propertyName"/> in the direction controlled by <paramref name="isDescending"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of <typeparamref name="TSource"/>s to order.</param>
    /// <param name="propertyName">The name of a property on <typeparamref name="TSource"/> to order by.</param>
    /// <param name="isDescending">A <see cref="bool"/> indicating if the ordering should be ascending (<see langword="false"/>) or descdending (<see langword="true"/>).</param>
    /// <returns>An <see cref="IQueryable{T}"/> whose elements are sorted according to <paramref name="propertyName"/> and <paramref name="isDescending"/>.</returns>
    /// <remarks>
    ///     If <typeparamref name="TSource"/> does not have a <paramref name="propertyName"/>,
    ///     this will return <paramref name="source"/> unordered.
    ///     As such, only an <see cref="IQueryable{T}"/> is returned.
    /// </remarks>
    public static IQueryable<TSource> OrderByProperty<TSource>(this IQueryable<TSource> source, string propertyName, bool isDescending) =>
        isDescending
        ? OrderByPropertyDescending(source, propertyName)
        : OrderByPropertyAscending(source, propertyName);

    /// <summary>
    ///     Sorts the elements of <paramref name="source"/> ascending according to the property <paramref name="propertyName"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of <typeparamref name="TSource"/>s to order.</param>
    /// <param name="propertyName">The name of a property on <typeparamref name="TSource"/> to order by.</param>
    /// <returns>An <see cref="IQueryable{T}"/> whose elements are sorted ascending according to <paramref name="propertyName"/>.</returns>
    /// <remarks>
    ///     If <typeparamref name="TSource"/> does not have a <paramref name="propertyName"/>,
    ///     this will return <paramref name="source"/> unordered.
    ///     As such, only an <see cref="IQueryable{T}"/> is returned.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="propertyName"/> are <see langword="null"/>.</exception>
    public static IQueryable<TSource> OrderByPropertyAscending<TSource>(this IQueryable<TSource> source, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(source);

        ArgumentNullException.ThrowIfNull(propertyName);

        if (!HasProperty<TSource>(propertyName, out var propertyInfo))
            return source;

        return OrderByCore(source, propertyInfo, false);
    }

    /// <summary>
    ///     Sorts the elements of <paramref name="source"/> descending according to the property <paramref name="propertyName"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of <typeparamref name="TSource"/>s to order.</param>
    /// <param name="propertyName">The name of a property on <typeparamref name="TSource"/> to order by.</param>
    /// <returns>An <see cref="IQueryable{T}"/> whose elements are sorted descending according to <paramref name="propertyName"/>.</returns>
    /// <remarks>
    ///     If <typeparamref name="TSource"/> does not have a <paramref name="propertyName"/>,
    ///     this will return <paramref name="source"/> unordered.
    ///     As such, only an <see cref="IQueryable{T}"/> is returned.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="propertyName"/> are <see langword="null"/>.</exception>
    public static IQueryable<TSource> OrderByPropertyDescending<TSource>(this IQueryable<TSource> source, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(source);

        ArgumentNullException.ThrowIfNull(propertyName);

        if (!HasProperty<TSource>(propertyName, out var propertyInfo))
            return source;

        return OrderByCore(source, propertyInfo, true);
    }

    /// <summary>
    ///     Sorts the elements of <paramref name="source"/> according to the property <paramref name="propertyName"/> and <paramref name="isDescending"/>.
    ///     If <paramref name="propertyName"/> is not found on <typeparamref name="TSource"/>, falls back to <paramref name="defaultKeySelector"/> and <paramref name="defaultIsDescending"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the property represented by <paramref name="defaultKeySelector"/>.</typeparam>
    /// <param name="source">A sequence of <typeparamref name="TSource"/>s to order.</param>
    /// <param name="propertyName">The name of a property on <typeparamref name="TSource"/> to order by.</param>
    /// <param name="isDescending">A <see cref="bool"/> indicating if the ordering should be ascending (<see langword="false"/>) or descdending (<see langword="true"/>).</param>
    /// <param name="defaultKeySelector">A function to extract a property from a <typeparamref name="TSource"/> as a fall back if <paramref name="propertyName"/> cannot be used.</param>
    /// <param name="defaultIsDescending">A <see cref="bool"/> indicating if the fall back ordering should be ascending (<see langword="false"/>) or descdending (<see langword="true"/>).</param>
    /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to <paramref name="propertyName"/> and <paramref name="isDescending"/>, or <paramref name="defaultKeySelector"/> and <paramref name="defaultIsDescending"/>.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="propertyName"/> are <see langword="null"/>.</exception>
    public static IOrderedQueryable<TSource> OrderByPropertyOrDefault<TSource, TKey>(this IQueryable<TSource> source, string propertyName, bool isDescending, Expression<Func<TSource, TKey>> defaultKeySelector, bool defaultIsDescending) =>
        isDescending
        ? OrderByPropertyOrDefaultDescending(source, propertyName, defaultKeySelector, defaultIsDescending)
        : OrderByPropertyOrDefaultAscending(source, propertyName, defaultKeySelector, defaultIsDescending);

    /// <summary>
    ///     Sorts the elements of <paramref name="source"/> ascending according to the property <paramref name="propertyName"/>.
    ///     If <paramref name="propertyName"/> is not found on <typeparamref name="TSource"/>, falls back to <paramref name="defaultKeySelector"/> and <paramref name="defaultIsDescending"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the property represented by <paramref name="defaultKeySelector"/>.</typeparam>
    /// <param name="source">A sequence of <typeparamref name="TSource"/>s to order.</param>
    /// <param name="propertyName">The name of a property on <typeparamref name="TSource"/> to order by.</param>
    /// <param name="defaultKeySelector">A function to extract a property from a <typeparamref name="TSource"/> as a fall back if <paramref name="propertyName"/> cannot be used.</param>
    /// <param name="defaultIsDescending">A <see cref="bool"/> indicating if the fall back ordering should be ascending (<see langword="false"/>) or descdending (<see langword="true"/>).</param>
    /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted ascending according to <paramref name="propertyName"/>, or <paramref name="defaultKeySelector"/> and <paramref name="defaultIsDescending"/>.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="source"/>, <paramref name="propertyName"/>, or <paramref name="defaultKeySelector"/> are <see langword="null"/>.</exception>
    public static IOrderedQueryable<TSource> OrderByPropertyOrDefaultAscending<TSource, TKey>(this IQueryable<TSource> source, string propertyName, Expression<Func<TSource, TKey>> defaultKeySelector, bool defaultIsDescending)
    {
        ArgumentNullException.ThrowIfNull(source);

        ArgumentNullException.ThrowIfNull(propertyName);

        ArgumentNullException.ThrowIfNull(defaultKeySelector);

        if (HasProperty<TSource>(propertyName, out var propertyInfo))
            return OrderByCore(source, propertyInfo, false);

        return source.OrderBy(defaultKeySelector, defaultIsDescending);
    }

    /// <summary>
    ///     Sorts the elements of <paramref name="source"/> descending according to the property <paramref name="propertyName"/>.
    ///     If <paramref name="propertyName"/> is not found on <typeparamref name="TSource"/>, falls back to <paramref name="defaultKeySelector"/> and <paramref name="defaultIsDescending"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the property represented by <paramref name="defaultKeySelector"/>.</typeparam>
    /// <param name="source">A sequence of <typeparamref name="TSource"/>s to order.</param>
    /// <param name="propertyName">The name of a property on <typeparamref name="TSource"/> to order by.</param>
    /// <param name="defaultKeySelector">A function to extract a property from a <typeparamref name="TSource"/> as a fall back if <paramref name="propertyName"/> cannot be used.</param>
    /// <param name="defaultIsDescending">A <see cref="bool"/> indicating if the fall back ordering should be ascending (<see langword="false"/>) or descdending (<see langword="true"/>).</param>
    /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted descending according to <paramref name="propertyName"/>, or <paramref name="defaultKeySelector"/> and <paramref name="defaultIsDescending"/>.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="source"/>, <paramref name="propertyName"/>, or <paramref name="defaultKeySelector"/> are <see langword="null"/>.</exception>
    public static IOrderedQueryable<TSource> OrderByPropertyOrDefaultDescending<TSource, TKey>(this IQueryable<TSource> source, string propertyName, Expression<Func<TSource, TKey>> defaultKeySelector, bool defaultIsDescending)
    {
        ArgumentNullException.ThrowIfNull(source);

        ArgumentNullException.ThrowIfNull(propertyName);

        ArgumentNullException.ThrowIfNull(defaultKeySelector);

        if (HasProperty<TSource>(propertyName, out var propertyInfo))
            return OrderByCore(source, propertyInfo, true);

        return source.OrderBy(defaultKeySelector, defaultIsDescending);
    }

    /// <summary>
    ///		Sorts the elements of <paramref name="source"/> according to the <paramref name="propertyInfo"/>.
    ///		Can be sorted in descending order using <paramref name="isDescending"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="propertyInfo">The property on <typeparamref name="TSource"/> to sort by.</param>
    /// <param name="isDescending">Whether to sort in ascending order (when <see langword="false"/>) or in descending order (when <see langword="true"/>).</param>
    /// <returns>An <see cref="IOrderedQueryable{TSource}"/> whose elements are sorted according to the <paramref name="propertyInfo"/>.</returns>
    private static IOrderedQueryable<TSource> OrderByCore<TSource>(IQueryable<TSource> source, PropertyInfo propertyInfo, bool isDescending)
    {
        Type sourceType = typeof(TSource);
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
        var newSourceObj = orderByMethod.Invoke(null, parameters);

        if (newSourceObj is IOrderedQueryable<TSource> newSource)
            return newSource;

        // OrderBy should always return an IQueryable
        throw new InvalidOperationException($"{orderByMethod.Name} returned a {newSourceObj?.GetType().Name} which is not assignable to {nameof(IQueryable<TSource>)}.");
    }

    /// <summary>
    ///     Checks if <typeparamref name="T"/> has a property <paramref name="propertyName"/>.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="propertyInfo">
    ///     Contains a <see cref="PropertyInfo"/> when the method returns <see langword="true"/>,
    ///     otherwise <see langword="null"/> when the method returns <see langword="false"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <typeparamref name="T"/> has a property <paramref name="propertyName"/>, otherwise <see langword="false"/>.
    /// </returns>
    private static bool HasProperty<T>(string propertyName, [NotNullWhen(true)] out PropertyInfo? propertyInfo)
    {
        propertyInfo = typeof(T).GetProperty(propertyName);
        return propertyInfo is not null;
    }
}
