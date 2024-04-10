namespace Tyne;

public static class AssertOption
{
    public static void IsSome<T>(in Option<T> actual)
    {
        Assert.True(actual.HasValue);
    }

    public static void IsSome<T>(T expected, in Option<T> actual)
    {
        IsSome(actual);
        Assert.Equal(expected, actual.Value);
    }

    public static void IsNone<T>(Option<T> actual)
    {
        Assert.False(actual.HasValue);
    }

    public static void AreEqual<T>(in Option<T> expected, in Option<T> actual)
    {
        Assert.Equal(expected, actual);

        Assert.True(expected == actual);
        Assert.True(actual == expected);

        Assert.False(expected != actual);
        Assert.False(actual != expected);

        Assert.True(expected.Equals(actual));
        Assert.True(actual.Equals(expected));

        Assert.True(((IEquatable<Option<T>>)expected).Equals(actual));
        Assert.True(((IEquatable<Option<T>>)actual).Equals(expected));

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

        Assert.False(((IEquatable<Option<T>>)expected).Equals(actual));
        Assert.False(((IEquatable<Option<T>>)actual).Equals(expected));

        Assert.False(expected.Equals(actual as object));
        Assert.False(actual.Equals(expected as object));

        if (includeHashCode)
            Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
    }
}
