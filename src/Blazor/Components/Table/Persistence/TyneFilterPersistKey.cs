using System.Diagnostics;
using System.Reflection;

namespace Tyne.Blazor;

/// <summary>
///     Represents a key for <see cref="ITyneTablePersistedFilter{TValue}"/>s to have their values saved as.
/// </summary>
/// <remarks>
///     <para>
///         A key is either <see cref="string.Empty"/> (if no key is known or desired),
///         or a valid series of characters. These may not start, end with, or be exclusively, whitespace.
///     </para>
///     <para>
///         Any keys created are trimmed for whitespace, and invalid keys (e.g. <see langword="null"/> or <c>"      "</c>)
///         are replaced with <see cref="string.Empty"/>.
///     </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
public readonly struct TyneFilterPersistKey : IEquatable<TyneFilterPersistKey>
{
    /// <summary>
    ///     The value of the key.
    /// </summary>
    /// <remarks>
    ///     See <see cref="TyneFilterPersistKey"/> for more details on what a valid key may be.
    /// </remarks>
    public string Key { get; } = string.Empty;

    /// <summary>
    ///     <see langword="true"/> when <see cref="Key"/> is <see cref="string.Empty"/>, otherwise <see langword="false"/>.
    /// </summary>
    public bool IsEmpty => Key.Length is 0;

    /// <summary>
    ///     A static instance of <see cref="TyneFilterPersistKey"/> whose <see cref="Key"/> is <see cref="string.Empty"/>.
    /// </summary>
    public static TyneFilterPersistKey Empty { get; } = new(string.Empty);

    private TyneFilterPersistKey(string key)
    {
        Key = key;
    }

    /// <summary>
    ///     Determines whether <paramref name="obj"/> is a <see cref="TyneFilterPersistKey"/>,
    ///     and if so whether it equals this object.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/>? to compare with this instance.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="obj"/> is a <see cref="TyneFilterPersistKey"/>
    ///     and is equal to this instance.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(TyneFilterPersistKey)"/> for how equality is determined.
    /// </remarks>
    public override bool Equals(object? obj) =>
        obj is TyneFilterPersistKey key
        && Equals(key);

    /// <summary>
    ///     Determines whether <paramref name="other"/> represents the same key as this instance.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="TyneFilterPersistKey"/> to compare with this instance.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="other"/>'s <see cref="Key"/> is equal to this instance's.
    ///     Equality is compared with <see cref="StringComparison.Ordinal"/>.
    /// </returns>
    public bool Equals(TyneFilterPersistKey other) =>
        Key.Equals(other.Key, StringComparison.Ordinal);

    /// <inheritdoc />
    public override int GetHashCode() =>
        Key.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    ///     Returns a <see cref="string"/> representation of this instance.
    /// </summary>
    /// <returns>
    ///     <para>
    ///         The <see cref="string"/> returned by this is NOT suitable for round-tripping with <see cref="From(string?)"/>.
    ///     </para>
    ///     <para>
    ///         Non-<see cref="Empty"/> keys are represented by their <see cref="Key"/> value.
    ///         <see cref="Empty"/> keys are represented by the literal <c>"(empty)"</c>.
    ///     </para>
    /// </returns>
    public override string? ToString() =>
        IsEmpty ?
        "(empty)"
        : Key;

    /// <summary>
    ///     Determines whether <paramref name="left"/> is equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">
    ///     The left <see cref="TyneFilterPersistKey"/>.
    /// </param>
    /// <param name="right">
    ///     The right <see cref="TyneFilterPersistKey"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/>
    ///     are equal, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(TyneFilterPersistKey)"/> for how equality is determined.
    /// </remarks>
    public static bool operator ==(TyneFilterPersistKey left, TyneFilterPersistKey right) =>
        left.Equals(right);

    /// <summary>
    ///     Determines whether <paramref name="left"/> is not equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">
    ///     The left <see cref="TyneFilterPersistKey"/>.
    /// </param>
    /// <param name="right">
    ///     The right <see cref="TyneFilterPersistKey"/>.
    /// </param>
    /// <returns>
    ///     <see langword="false"/> if <paramref name="left"/> and <paramref name="right"/>
    ///     are equal, otherwise <see langword="true"/>.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(TyneFilterPersistKey)"/> for how equality is determined.
    /// </remarks>
    public static bool operator !=(TyneFilterPersistKey left, TyneFilterPersistKey right) =>
        !(left == right);

    /// <summary>
    ///     Implicitly converts this instance to a <see cref="string"/>.
    /// </summary>
    /// <param name="key">
    ///     The <see cref="TyneFilterPersistKey"/> to convert.
    /// </param>
    /// <remarks>
    ///     This simply returns <paramref name="key"/>'s <see cref="Key"/>.
    /// </remarks>
    public static implicit operator string(TyneFilterPersistKey key) =>
        key.Key;

    /// <summary>
    ///     Creates a <see cref="TyneFilterPersistKey"/> derived from <paramref name="persistAs"/>.
    /// </summary>
    /// <param name="persistAs">
    ///     A <see cref="string"/> to use as the persistence key.
    /// </param>
    /// <returns>
    ///     A <see cref="TyneFilterPersistKey"/>.
    /// </returns>
    /// <remarks>
    ///     This is equivalent to calling <see cref="From(string?, PropertyInfo?)"/> with <c>(<paramref name="persistAs"/>, <see langword="null"/>)</c>.
    /// </remarks>
    public static TyneFilterPersistKey From(string? persistAs) =>
        From(persistAs, null);

    /// <summary>
    ///     Creates a <see cref="TyneFilterPersistKey"/> derived from
    ///     <paramref name="persistAs"/> and <paramref name="propertyInfo"/>.
    /// </summary>
    /// <param name="persistAs">
    ///     A <see cref="string"/> to use as the persistence key.
    /// </param>
    /// <param name="propertyInfo">
    ///     Optionally, a <see cref="PropertyInfo"/> to use as the persistence key.
    ///     This is only used in the special case that <paramref name="persistAs"/> is <c>"*"</c>.
    ///     See remarks for more info about this.
    /// </param>
    /// <returns>
    ///     A <see cref="TyneFilterPersistKey"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="persistAs"/> is null, empty, or exclusively whitespace (i.e. <see cref="string.IsNullOrWhiteSpace(string?)"/>),
    ///         then <see cref="Empty"/> will be returned.
    ///     </para>
    ///     <para>
    ///         If <paramref name="persistAs"/> is the literal <c>"*"</c>, then it will be substituted
    ///         with <paramref name="propertyInfo"/>'s <see cref="MemberInfo.Name"/>.
    ///         If <paramref name="propertyInfo"/> is <see langword="null"/>, then this will fall into the above case.
    ///     </para>
    ///     <para>
    ///         In any other case, a new <see cref="TyneFilterPersistKey" /> will be created.
    ///         It's <see cref="Key"/> will be equivalent to <paramref name="persistAs"/> after it has been trimmed.
    ///     </para>
    /// </remarks>
    public static TyneFilterPersistKey From(string? persistAs, PropertyInfo? propertyInfo)
    {
        var key =
            persistAs == "*"
            ? propertyInfo?.Name
            : persistAs;

        if (string.IsNullOrWhiteSpace(key) && key?.Length != 0)
        {
            // Set key to "" if it is null or whitespace (but skip re-assigning if already "")
            key = string.Empty;
        }
        else
        {
            // If key is not empty, trim the whitespace
            key = key.Trim();
        }

        // If the key is "", return the static Empty instance
        if (key.Length == 0)
            return Empty;

        // Otherwise create a new instance
        return new(key);
    }
}
