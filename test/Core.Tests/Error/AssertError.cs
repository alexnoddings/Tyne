namespace Tyne;

internal static class AssertError
{
    public static void IsDefault(in Error actual) =>
        AreEqual(Error.Default, actual);

    private static IEquatable<Error> AsEquatable(in Error error) => error;

    public static void AreEqual(in Error expected, in Error actual)
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

    public static void AreNotEqual(in Error expected, in Error actual)
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

        Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
    }
}
