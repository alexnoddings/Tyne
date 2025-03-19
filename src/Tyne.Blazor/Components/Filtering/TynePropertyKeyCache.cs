using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Blazor.Filtering;

/// <summary>
///     Caches <see cref="TyneKey"/>s and <see cref="System.Reflection.PropertyInfo"/>s from <see cref="Expression"/>s.
/// </summary>
/// <typeparam name="TSource">The source type of the <see cref="Expression"/>.</typeparam>
/// <typeparam name="TProperty">The property type on the <typeparamref name="TSource"/>.</typeparam>
/// <remarks>
///     This is useful to cut down on the cost of reflecting across an expression every time,
///     and saves complexity on manually checking for changed expressions when parameters are set.
/// </remarks>
public sealed class TynePropertyKeyCache<TSource, TProperty>
{
    // This could be made more efficient by caching expressions at the application level,
    // but it's not currently enough of a perf impact in any systems to warrant the effort.

    private readonly bool _onlyUpdateOnce;
    private string? _cachedPropertyAccessorString;

    private TyneKey _key;
    /// <summary>
    ///     The cached <see cref="TyneKey"/>.
    /// </summary>
    /// <remarks>
    ///     This will always be <see cref="TyneKey.Empty"/> before
    ///     <see cref="Update(Expression{Func{TSource, TProperty}}?)"/>
    ///     has been called to update the cache.
    /// </remarks>
    public ref readonly TyneKey Key => ref _key;

    /// <summary>
    ///     The cached <see cref="System.Reflection.PropertyInfo"/>.
    /// </summary>
    /// <remarks>
    ///     This will always be <see langword="null"/> before
    ///     <see cref="Update(Expression{Func{TSource, TProperty}}?)"/>
    ///     has been called to update the cache.
    /// </remarks>
    public PropertyInfo? PropertyInfo { get; private set; }

    /// <summary>
    ///     Constructs a new <see cref="TynePropertyKeyCache{TSource, TProperty}"/>.
    /// </summary>
    /// <remarks>
    ///     This constructor allows for the cache to be updated more than once.
    ///     See <see cref="TynePropertyKeyCache{TSource, TProperty}.TynePropertyKeyCache(bool)"/>.
    /// </remarks>
    public TynePropertyKeyCache() : this(false)
    {
    }

    /// <summary>
    ///     Constructs a new <see cref="TynePropertyKeyCache{TSource, TProperty}"/>.
    /// </summary>
    /// <param name="onlyUpdateOnce"><see langword="true"/> if the cache can only be set once; otherwise, <see langword="false"/>.</param>
    /// <remarks>
    ///     <paramref name="onlyUpdateOnce"/> can be used to ensure that only the first valid <see cref="TyneKey"/>
    ///     set by <see cref="Update(Expression{Func{TSource, TProperty}}?)"/> will be cached. Subsequent calls to
    ///     update the cache will be ignored. This is useful if you don't want the key to change after creation,
    ///     such as a key used to attach to a context.
    /// </remarks>
    public TynePropertyKeyCache(bool onlyUpdateOnce)
    {
        _onlyUpdateOnce = onlyUpdateOnce;
    }

    /// <summary>
    ///     Updates the cache with the new <paramref name="propertyAccessor"/>.
    /// </summary>
    /// <param name="propertyAccessor">
    ///     The new <see cref="Expression"/> accessing a <typeparamref name="TProperty"/> property on <typeparamref name="TSource"/>.
    /// </param>
    /// <returns>A <see langword="ref"/> to <see cref="Key"/> after it has been updated.</returns>
    /// <remarks>
    ///     <para>
    ///         This will set <see cref="Key"/> and <see cref="PropertyInfo"/>
    ///         based on the property which <paramref name="propertyAccessor"/> points too.
    ///         If <paramref name="propertyAccessor"/> is <see langword="null"/>,
    ///         then <see cref="Key"/> will be <see cref="TyneKey.Empty"/>,
    ///         and <see cref="PropertyInfo"/> will be be <see langword="null"/>.
    ///     </para>
    ///     <para>
    ///         If this cache is only set to update once
    ///         (see <see cref="TynePropertyKeyCache{TSource, TProperty}.TynePropertyKeyCache(bool)"/>),
    ///         then this call will be ignored if <see cref="Key"/> has already been set.
    ///     </para>
    /// </remarks>
    public ref TyneKey Update(Expression<Func<TSource, TProperty>>? propertyAccessor)
    {
        if (_onlyUpdateOnce && !_key.IsEmpty)
            return ref _key;

        // In theory, any expressions with the same source/property types
        // and string-ified representation should point to the same PropertyInfo.
        // Recursively checking the expression, while more accurate,
        // would be a lot more costly and defeat the point of caching.
        var propertyAccessorString = propertyAccessor?.ToString();
        if (_cachedPropertyAccessorString != propertyAccessorString)
        {
            // Update our cached version
            _cachedPropertyAccessorString = propertyAccessorString;
            if (propertyAccessor is null)
            {
                // If the accessor is null, we don't have a PropertyInfo and have an empty key
                PropertyInfo = null;
                _key = TyneKey.Empty;
            }
            else
            {
                // If the accessor is not null, try to load the PropertyInfo and key
                PropertyInfo = ExpressionHelper.TryGetAccessedPropertyInfo(propertyAccessor);
                _key = TyneKey.From(PropertyInfo?.Name);
            }
        }

        return ref _key;
    }
}
