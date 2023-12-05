using System.Diagnostics.CodeAnalysis;

namespace Tyne;

internal static class AssertOption
{
    public static void IsSome<T>(T expected, in Option<T> actual)
    {
        Assert.True(actual.HasValue);
        Assert.Equal(expected, actual.Value);
    }

    public static void IsNone<T>(Option<T> actual)
    {
        Assert.False(actual.HasValue);
    }

    private static IEquatable<Option<T>> AsEquatable<T>(in Option<T> option) => option;

    public static void AreEqual<T>(in Option<T> expected, in Option<T> actual)
    {
        Assert.Equal(expected, actual);

        Assert.True(expected == actual);
        Assert.True(actual == expected);

        Assert.False(expected != actual);
        Assert.False(actual != expected);

        Assert.True(expected.Equals(actual));
        Assert.True(actual.Equals(expected));

        Assert.True(AsEquatable(expected).Equals(actual));
        Assert.True(AsEquatable(actual).Equals(expected));

        Assert.True(expected.Equals(actual as object));
        Assert.True(actual.Equals(expected as object));

        Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    public static void AreNotEqual<T>(in Option<T> expected, in Option<T> actual, bool includeHashCode = true)
    {
        Assert.NotEqual(expected, actual);

        Assert.False(expected == actual);
        Assert.False(actual == expected);

        Assert.True(expected != actual);
        Assert.True(actual != expected);

        Assert.False(expected.Equals(actual));
        Assert.False(actual.Equals(expected));

        Assert.False(AsEquatable(expected).Equals(actual));
        Assert.False(AsEquatable(actual).Equals(expected));

        Assert.False(expected.Equals(actual as object));
        Assert.False(actual.Equals(expected as object));

        if (includeHashCode)
            Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    [SuppressMessage("Major Bug", "S2583: Conditionally executed code should be reachable", Justification = "False positive.")]
    [SuppressMessage("Major Code Smell", "S2589: Boolean expressions should not be gratuitous", Justification = "False positive.")]
    public static void Assert_AreEqual<T>(T? expected, in Option<T> actual)
    {
        Assert.Equal(expected, actual);

        Assert.True(actual == expected);
        Assert.True(expected == actual);

        Assert.False(actual != expected);
        Assert.False(expected != actual);

        Assert.True(actual.Equals(expected));

        Assert.True(AsEquatable(actual).Equals(expected));

        Assert.True(actual.Equals(expected as object));

        Assert.Equal(actual.GetHashCode(), expected?.GetHashCode() ?? 0);
    }

    public static void Assert_AreNotEqual<T>(T? expected, in Option<T> actual)
    {
        Assert.NotEqual(expected, actual);

        Assert.False(actual == expected);
        Assert.False(expected == actual);

        Assert.True(actual != expected);
        Assert.True(expected != actual);

        Assert.False(actual.Equals(expected));

        Assert.False(AsEquatable(actual).Equals(expected));

        Assert.False(actual.Equals(expected as object));
    }
}
