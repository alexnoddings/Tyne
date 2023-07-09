using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tyne.Blazor;

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
        other.Key == Key;

    public override int GetHashCode() =>
        Key.GetHashCode(StringComparison.Ordinal);

    public override string? ToString() => Key;

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
        new(persistAs ?? string.Empty);

    public static TyneFilterPersistKey From(string? persistAs, PropertyInfo? propertyInfo)
    {
        var key =
            persistAs == "*"
            ? propertyInfo?.Name
            : persistAs;

        return new(key ?? string.Empty);
    }
}
