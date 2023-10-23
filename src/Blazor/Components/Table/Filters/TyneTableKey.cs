using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tyne.Blazor;

/// <summary>
///     Represents a key for use with Tyne tables.
/// </summary>
/// <remarks>
///     <para>
///         <b><see cref="TyneTableKey"/> is deprecated in favour of <see cref="TyneKey"/>.</b>
///     </para>
///     <para>
///         A key is either <see cref="string.Empty"/> (if no key is known or desired),
///         or a valid series of characters. These may not start, end with, or be exclusively, whitespace.
///         A key is never <see langword="null"/>.
///     </para>
///     <para>
///         Any keys created are trimmed for whitespace,
///         and invalid keys (e.g. <see langword="null"/> or <c>"      "</c>)
///         are replaced with <see cref="string.Empty"/>.
///     </para>
///     <para>
///         Keys should be constructed using <see cref="From(string?)"/> or <see cref="From(string?, PropertyInfo?)"/>.
///     </para>
///     <para>
///         Keys are case-sensitive. See <see cref="Equals(TyneTableKey)"/> for equality rules.
///     </para>
/// </remarks>
[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
// S1133: Deprecated code should be removed.
//        It will be eventually. This is here to transition between versions.
#pragma warning disable S1133
[Obsolete($"Use {nameof(TyneKey)} instead.", DiagnosticId = "TYNE_OLD_TABLEKEY")]
#pragma warning restore S1133
public readonly struct TyneTableKey : IEquatable<TyneTableKey>
{
    // When Key is used as an auto-implemented getter, it can end up as
    // null when default(TyneTableKey) is used as no initialisation code is executed.
    // This backing field allows the Key property to never return null, even if un-initialised.
    private readonly string? _key;

    /// <summary>
    ///     The value of the key.
    /// </summary>
    /// <remarks>
    ///     See <see cref="TyneTableKey"/> for more details on what a valid key may be.
    /// </remarks>
    public string Key => _key ?? string.Empty;

    /// <summary>
    ///     <see langword="true"/> when <see cref="Key"/> is <see cref="string.Empty"/>, otherwise <see langword="false"/>.
    /// </summary>
    public bool IsEmpty => Key.Length is 0;

    /// <summary>
    ///     A static instance of <see cref="TyneTableKey"/> whose <see cref="Key"/> is <see cref="string.Empty"/>.
    /// </summary>
    public static readonly TyneTableKey Empty = new(string.Empty);

    private TyneTableKey(string key)
    {
        _key = key;
    }

    /// <summary>
    ///     Initialises a new <see cref="TyneTableKey"/> with an empty <see cref="Key"/>.
    /// </summary>
    /// <remarks>
    ///     You should prefer using <see cref="Empty"/> to reference an empty key as it avoids an unnecessary allocation.
    /// </remarks>
    public TyneTableKey()
    {
    }

    /// <summary>
    ///     Determines whether <paramref name="obj"/> is a <see cref="TyneTableKey"/>,
    ///     and if so whether it equals this object.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/>? to compare with this instance.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="obj"/> is a <see cref="TyneTableKey"/>
    ///     and is equal to this instance.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(TyneTableKey)"/> for how equality is determined.
    /// </remarks>
    public override bool Equals(object? obj) =>
        obj is TyneTableKey key
        && Equals(key);

    /// <summary>
    ///     Determines whether <paramref name="other"/> represents the same key as this instance.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="TyneTableKey"/> to compare with this instance.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="other"/>'s <see cref="Key"/> is equal to this instance's.
    ///     Equality is compared with <see cref="StringComparison.Ordinal"/>.
    /// </returns>
    public bool Equals(TyneTableKey other) =>
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
    ///     The left <see cref="TyneTableKey"/>.
    /// </param>
    /// <param name="right">
    ///     The right <see cref="TyneTableKey"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/>
    ///     are equal, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(TyneTableKey)"/> for how equality is determined.
    /// </remarks>
    public static bool operator ==(TyneTableKey left, TyneTableKey right) =>
        left.Equals(right);

    /// <summary>
    ///     Determines whether <paramref name="left"/> is not equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left">
    ///     The left <see cref="TyneTableKey"/>.
    /// </param>
    /// <param name="right">
    ///     The right <see cref="TyneTableKey"/>.
    /// </param>
    /// <returns>
    ///     <see langword="false"/> if <paramref name="left"/> and <paramref name="right"/>
    ///     are equal, otherwise <see langword="true"/>.
    /// </returns>
    /// <remarks>
    ///     See <see cref="Equals(TyneTableKey)"/> for how equality is determined.
    /// </remarks>
    public static bool operator !=(TyneTableKey left, TyneTableKey right) =>
        !(left == right);

    /// <summary>
    ///     Implicitly converts this instance to a <see cref="string"/>.
    /// </summary>
    /// <param name="key">
    ///     The <see cref="TyneTableKey"/> to convert.
    /// </param>
    /// <remarks>
    ///     This simply returns <paramref name="key"/>'s <see cref="Key"/>.
    /// </remarks>
    public static implicit operator string(TyneTableKey key) =>
        key.Key;

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Transition method not intended for direct use.")]
    public static implicit operator TyneKey(TyneTableKey key) => TyneKey.From(key._key);

    /// <summary>
    ///     Creates a <see cref="TyneTableKey"/> derived from <paramref name="persistAs"/>.
    /// </summary>
    /// <param name="persistAs">
    ///     A <see cref="string"/> to use as the key.
    /// </param>
    /// <returns>
    ///     A <see cref="TyneTableKey"/>.
    /// </returns>
    /// <remarks>
    ///     This is equivalent to calling <see cref="From(string?, PropertyInfo?)"/> with <c>(<paramref name="persistAs"/>, <see langword="null"/>)</c>.
    /// </remarks>
    public static TyneTableKey From(string? persistAs) =>
        From(persistAs, null);

    /// <summary>
    ///     Creates a <see cref="TyneTableKey"/> derived from
    ///     <paramref name="persistAs"/> and <paramref name="propertyInfo"/>.
    /// </summary>
    /// <param name="persistAs">
    ///     A <see cref="string"/> to use as the key.
    /// </param>
    /// <param name="propertyInfo">
    ///     Optionally, a <see cref="PropertyInfo"/> to use as the key.
    ///     This is only used in the special case that <paramref name="persistAs"/> is <c>"*"</c>.
    ///     See remarks for more info about this.
    /// </param>
    /// <returns>
    ///     A <see cref="TyneTableKey"/>.
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
    ///         In any other case, a new <see cref="TyneTableKey" /> will be created.
    ///         It's <see cref="Key"/> will be equivalent to <paramref name="persistAs"/> after it has been trimmed.
    ///     </para>
    /// </remarks>
    public static TyneTableKey From(string? persistAs, PropertyInfo? propertyInfo)
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
