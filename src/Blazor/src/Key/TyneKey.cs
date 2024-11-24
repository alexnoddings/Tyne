using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tyne.Blazor;

/// <summary>
///     Represents a key for use in Tyne.
/// </summary>
/// <remarks>
///     <para>
///         A key is either <see cref="string.Empty"/>, or a valid series of characters.
///         This may not start, end with, or be exclusively, whitespace.
///         A key is never <see langword="null"/>.
///     </para>
///     <para>
///         Any keys created are trimmed for whitespace,
///         and invalid keys (e.g. <see langword="null"/> or <c>"   "</c>)
///         instead use <see cref="Empty"/>.
///     </para>
///     <para>
///         Keys should be constructed using <see cref="From(string?)"/> or <see cref="From(string?, PropertyInfo?)"/>.
///     </para>
///     <para>
///         Keys are case-sensitive. See <see cref="Equals(in TyneKey)"/> for equality rules.
///     </para>
/// </remarks>
[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
public readonly struct TyneKey : IEquatable<TyneKey>
{
    // When Key is used as an auto-implemented getter, it can end up as
    // null when default(TyneTableKey) is used as no initialisation code is executed.
    // This backing field allows the Key property to never return null, even if un-initialised.

    /// <summary>
    ///     The value of the key.
    /// </summary>
    /// <remarks>
    ///     See <see cref="TyneKey"/> for more details on what a valid key may be.
    /// </remarks>
    [field: MaybeNull]
    public string Key => field ?? string.Empty;

    /// <summary>
    ///     <see langword="true"/> when <see cref="Key"/> is <see cref="string.Empty"/>, otherwise <see langword="false"/>.
    /// </summary>
    public bool IsEmpty => Key.Length is 0;

    // IDE0032: Use auto property
    // REASON:  Auto-properties can't return by reference.
#pragma warning disable IDE0032
    private static readonly TyneKey _empty = new(string.Empty);

    /// <summary>
    ///     A static instance of <see cref="TyneKey"/> whose <see cref="Key"/> is <see cref="string.Empty"/>.
    /// </summary>
    public static ref readonly TyneKey Empty => ref _empty;
#pragma warning restore IDE0032

    private TyneKey(string key)
    {
        Key = key;
    }

    /// <summary>
    ///     Initialises a new <see cref="TyneKey"/> with an empty <see cref="Key"/>.
    /// </summary>
    /// <remarks>
    ///     You should prefer using <see cref="Empty"/> to reference an empty key as it avoids an unnecessary allocation.
    /// </remarks>
    public TyneKey()
    {
    }

    /// <summary>
    ///     Determines whether <paramref name="obj"/> is a <see cref="TyneKey"/>,
    ///     and if so whether it equals this object.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/>? to compare with this instance.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="obj"/> is a <see cref="TyneKey"/>
    ///     and is equal to this instance.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(in TyneKey)"/> for how equality is determined.
    /// </remarks>
    [Pure]
    public override bool Equals(object? obj) =>
        obj is TyneKey key
        && Equals(key);

    /// <summary>
    ///     Determines whether <paramref name="other"/> represents the same key as this instance.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="TyneKey"/> to compare with this instance.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="other"/>'s <see cref="Key"/> is equal to this instance's,
    ///     otherwise <see langword="false" />.
    ///     Equality is compared with <see cref="StringComparison.Ordinal"/>.
    /// </returns>
    [Pure]
    public bool Equals(in TyneKey other) =>
        Key.Equals(other.Key, StringComparison.Ordinal);

    /// <remarks>
    ///     You should use <see cref="Equals(in TyneKey)"/> instead.
    /// </remarks>
    /// <inheritdoc cref="Equals(in TyneKey)"/>
    // This is implemented explicitly to guide callers to using the more efficient Equals() overload instead
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IEquatable<TyneKey>.Equals(TyneKey other) =>
        Key.Equals(other.Key, StringComparison.Ordinal);

    /// <summary>
    ///     Returns the hash code for <see cref="Key"/>.
    /// </summary>
    /// <returns>
    ///     A 32-bit signed integer hash code.
    /// </returns>
    [Pure]
    public override int GetHashCode() =>
        Key.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    ///     Returns a <see cref="string"/> representation of this instance.
    /// </summary>
    /// <returns>
    ///     <para>
    ///         The <see cref="string"/> returned by this is NOT suitable for round-tripping with <see cref="From(string?)"/>.
    ///     </para>
    ///     <list type="bullet">
    ///         <item>Non-<see cref="Empty"/> keys return their <see cref="Key"/> value.</item>
    ///         <item><see cref="Empty"/> keys return the literal <c>"(empty)"</c>.</item>
    ///     </list>
    /// </returns>
    [Pure]
    public override string? ToString() =>
        IsEmpty
        ? "(empty)"
        : Key;

    /// <summary>
    ///     Determines whether <paramref name="left"/> is equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">
    ///     The left <see cref="TyneKey"/>.
    /// </param>
    /// <param name="right">
    ///     The right <see cref="TyneKey"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/>
    ///     are equal, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(in TyneKey)"/> for how equality is determined.
    /// </remarks>
    [Pure]
    public static bool operator ==(in TyneKey left, in TyneKey right) =>
        left.Equals(right);

    /// <summary>
    ///     Determines whether <paramref name="left"/> is not equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">
    ///     The left <see cref="TyneKey"/>.
    /// </param>
    /// <param name="right">
    ///     The right <see cref="TyneKey"/>.
    /// </param>
    /// <returns>
    ///     <see langword="false"/> if <paramref name="left"/> and <paramref name="right"/>
    ///     are equal, otherwise <see langword="true"/>.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(in TyneKey)"/> for how equality is determined.
    /// </remarks>
    [Pure]
    public static bool operator !=(in TyneKey left, in TyneKey right) =>
        !(left == right);

    /// <summary>
    ///     Creates a <see cref="TyneKey"/> derived from <paramref name="key"/>.
    /// </summary>
    /// <param name="key">
    ///     A <see cref="string"/> to use as the key.
    /// </param>
    /// <returns>
    ///     A <see cref="TyneKey"/>.
    /// </returns>
    /// <remarks>
    ///     This is equivalent to calling <see cref="From(string?, PropertyInfo?)"/> with <c>(<paramref name="key"/>, <see langword="null"/>)</c>.
    /// </remarks>
    [Pure]
    public static TyneKey From(string? key) =>
        From(key, null);

    /// <summary>
    ///     Creates a <see cref="TyneKey"/> derived from
    ///     <paramref name="key"/> and <paramref name="propertyInfo"/>.
    /// </summary>
    /// <param name="key">
    ///     A <see cref="string"/> to use as the key.
    /// </param>
    /// <param name="propertyInfo">
    ///     Optionally, a <see cref="PropertyInfo"/> to use as the key.
    ///     This is only used in the special case that <paramref name="key"/> is <c>"*"</c>.
    ///     See remarks for more info about this.
    /// </param>
    /// <returns>
    ///     A <see cref="TyneKey"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="key"/> is null, empty, or exclusively whitespace (i.e. <see cref="string.IsNullOrWhiteSpace(string?)"/>),
    ///         then <see cref="Empty"/> will be returned.
    ///     </para>
    ///     <para>
    ///         If <paramref name="key"/> is the literal <c>"*"</c>, then it will be substituted
    ///         with <paramref name="propertyInfo"/>'s <see cref="MemberInfo.Name"/>.
    ///         If <paramref name="propertyInfo"/> is <see langword="null"/>, then this will fall into the above case.
    ///     </para>
    ///     <para>
    ///         In any other case, a new <see cref="TyneKey" /> will be created.
    ///         It's <see cref="Key"/> will be equivalent to <paramref name="key"/> after it has been trimmed.
    ///     </para>
    /// </remarks>
    [Pure]
    public static TyneKey From(string? key, PropertyInfo? propertyInfo)
    {
        // If the key is "*" but there's no property info,
        // then we'll want to return Empty
        if (key == "*")
            key = propertyInfo?.Name;

        // If the key is "", return the static Empty instance
        if (string.IsNullOrWhiteSpace(key))
            return Empty;

        // If key is not empty, trim the whitespace
        // This must produce a valid non-empty key, otherwise we'd
        // have already returned on the whitespace check
        key = key.Trim();

        return new(key);
    }
}
