using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A helper class for creating <see cref="FilterValueSetter{TRequest, TValue}"/>s.
/// </summary>
public static class FilterValueSetter
{
    /// <summary>
    ///     Creates a new <see cref="FilterValueSetter{TRequest, TValue}"/> using <paramref name="forPropertyName"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of request.</typeparam>
    /// <typeparam name="TValue">The type of value on <typeparamref name="TRequest"/>.</typeparam>
    /// <param name="forPropertyName">The name of the <typeparamref name="TValue"/> property on <typeparamref name="TRequest"/>.</param>
    /// <returns>
    ///     <para>
    ///         A <see cref="FilterValueSetter{TRequest, TValue}"/> if <paramref name="forPropertyName"/>
    ///         is a valid property on <typeparamref name="TRequest"/>, which <typeparamref name="TValue"/> is assignable to.
    ///         Otherwise, returns <see langword="null"/>.
    ///     </para>
    ///     <para>
    ///         This is an overload of <see cref="CreateForSetter{TRequest, TValue}(string, ILogger?)"/> which doesn't enable logging.
    ///     </para>
    /// </returns>
    public static FilterValueSetter<TRequest, TValue>? CreateForSetter<TRequest, TValue>(string forPropertyName) =>
        CreateForSetter<TRequest, TValue>(forPropertyName, null);

    /// <summary>
    ///     Creates a new <see cref="FilterValueSetter{TRequest, TValue}"/> using <paramref name="forPropertyName"/>,
    ///     and optionally logging errors to <paramref name="logger"/> (if not <see langword="null"/>).
    /// </summary>
    /// <typeparam name="TRequest">The type of request.</typeparam>
    /// <typeparam name="TValue">The type of value on <typeparamref name="TRequest"/>.</typeparam>
    /// <param name="forPropertyName">The name of the <typeparamref name="TValue"/> property on <typeparamref name="TRequest"/>.</param>
    /// <param name="logger">Optionally, an <see cref="ILogger"/> used to log creation errors to.</param>
    /// <returns>
    ///     A <see cref="FilterValueSetter{TRequest, TValue}"/> if <paramref name="forPropertyName"/>
    ///     is a valid property on <typeparamref name="TRequest"/>, which <typeparamref name="TValue"/> is assignable to.
    ///     Otherwise, returns <see langword="null"/> and logs an error to <paramref name="logger"/> (if not <see langword="null"/>).
    /// </returns>
    public static FilterValueSetter<TRequest, TValue>? CreateForSetter<TRequest, TValue>(string forPropertyName, ILogger? logger)
    {
        // forProperty must be a public, non-static property on TRequest.
        // We don't currently have any reason to support non-public properties.
        var forProperty = typeof(TRequest).GetProperty(forPropertyName, BindingFlags.Instance | BindingFlags.Public);
        if (forProperty is null)
        {
            logger?.LogFilterValueCreateSetterPropertyNotFound(forPropertyName, typeof(TRequest));
            return null;
        }

        // TValue must be assignable to the property, otherwise we can't set it.
        // For example, a TValue of string is assignable to a string property,
        // or an object property, but not to an int property
        if (!typeof(TValue).IsAssignableTo(forProperty.PropertyType))
        {
            logger?.LogFilterValueCreateSetterIncompatibleType(forPropertyName, typeof(TRequest), typeof(TValue), forProperty.PropertyType);
            return null;
        }

        // The property must also have a setter method available.
        // A missing setter indicates a non-auto property implementation that only implements a getter.
        var forSetMethodInfo = forProperty.SetMethod;
        if (forSetMethodInfo is null)
        {
            logger?.LogFilterValueCreateSetterNoPropertySetter(forPropertyName, typeof(TRequest));
            return null;
        }

        // Turn the method info into a delegate
        return forSetMethodInfo.CreateDelegate<FilterValueSetter<TRequest, TValue>>();
    }
}
