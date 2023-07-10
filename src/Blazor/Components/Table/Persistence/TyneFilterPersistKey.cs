using System.Diagnostics;
using System.Reflection;

namespace Tyne.Blazor;

[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
public readonly struct TyneFilterPersistKey : IEquatable<TyneFilterPersistKey>
{
    public string Key { get; } = string.Empty;
    public bool IsEmpty => Key.Length is 0;

    public static TyneFilterPersistKey Empty { get; } = new(string.Empty);

    private TyneFilterPersistKey(string key)
    {
        Key = key;
    }

    public override bool Equals(object? obj) =>
        obj is TyneFilterPersistKey key
        && Equals(key);

    public bool Equals(TyneFilterPersistKey other) =>
        Key.Equals(other.Key, StringComparison.Ordinal);

    public override int GetHashCode() =>
        Key.GetHashCode(StringComparison.Ordinal);

    public override string? ToString() =>
        IsEmpty ?
        "(empty)"
        : Key;

    public static bool operator ==(TyneFilterPersistKey left, TyneFilterPersistKey right) =>
        left.Equals(right);

    public static bool operator !=(TyneFilterPersistKey left, TyneFilterPersistKey right) =>
        !(left == right);

    public static implicit operator string(TyneFilterPersistKey key) =>
        key.Key;

    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = $"{nameof(From)} exists.")]
    public static implicit operator TyneFilterPersistKey(string persistAs) =>
        From(persistAs);

    public static TyneFilterPersistKey From(string? persistAs) =>
        From(persistAs, null);

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
