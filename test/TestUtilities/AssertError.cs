namespace Tyne;

public static class AssertError
{
    public static void IsDefault(in Error actual) =>
        AreEqual(Error.Default, actual);

    public static void AreEqual(in Error expected, in Error actual)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(actual);

        Assert.Equal(expected, actual);

        Assert.True(expected == actual);
        Assert.True(actual == expected);

        Assert.False(expected != actual);
        Assert.False(actual != expected);

        Assert.True(expected.Equals(actual));
        Assert.True(actual.Equals(expected));

        Assert.True(((IEquatable<Error>)expected).Equals(actual));
        Assert.True(((IEquatable<Error>)actual).Equals(expected));

        Assert.True(expected.Equals(actual as object));
        Assert.True(actual.Equals(expected as object));

        Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    public static void AreNotEqual(in Error expected, in Error actual)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(actual);

        Assert.NotEqual(expected, actual);

        Assert.False(expected == actual);
        Assert.False(actual == expected);

        Assert.True(expected != actual);
        Assert.True(actual != expected);

        Assert.False(expected.Equals(actual));
        Assert.False(actual.Equals(expected));

        Assert.False(((IEquatable<Error>)expected).Equals(actual));
        Assert.False(((IEquatable<Error>)actual).Equals(expected));

        Assert.False(expected.Equals(actual as object));
        Assert.False(actual.Equals(expected as object));

        Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
    }
}
