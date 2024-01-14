namespace Tyne;

public static class AssertResult
{
    public static void IsOk<T>(T expected, Result<T> actual)
    {
        ArgumentNullException.ThrowIfNull(actual);

        Assert.True(actual.IsOk);
        Assert.Equal(expected, actual.Value);

        var exception = Assert.Throws<BadResultException>(() => actual.Error);
        Assert.Equal(exception.Message, ExceptionMessages.Result_OkHasNoError);
    }

    public static void IsError<T>(in Error expected, in Result<T> actual)
    {
        ArgumentNullException.ThrowIfNull(actual);

        Assert.False(actual.IsOk);
        Assert.Equal(expected, actual.Error);

        ErrorThrowsExceptionOnValueAccessor(actual);
    }

    public static void IsError<T>(string expectedErrorMessage, in Result<T> actual) =>
        IsError(Error.DefaultCode, expectedErrorMessage, null, actual);

    public static void IsError<T>(int expectedErrorCode, string expectedErrorMessage, in Result<T> actual) =>
        IsError(expectedErrorCode, expectedErrorMessage, null, actual);

    public static void IsError<T>(int expectedErrorCode, string expectedErrorMessage, Exception? expectedException, in Result<T> actual)
    {
        ArgumentNullException.ThrowIfNull(actual);

        Assert.False(actual.IsOk);
        Assert.Equal(expectedErrorCode, actual.Error.Code);
        Assert.Equal(expectedErrorMessage, actual.Error.Message);
        Assert.Equal(expectedException, actual.Error.CausedBy);

        ErrorThrowsExceptionOnValueAccessor(actual);
    }

    private static void ErrorThrowsExceptionOnValueAccessor<T>(Result<T> result)
    {
        var exception = Assert.Throws<BadResultException>(() => result.Value);
        Assert.Equal(exception.Message, ExceptionMessages.Result_ErrorHasNoValue);
    }

    private static IEquatable<Result<T>> AsEquatable<T>(in Result<T> result) => result;

    public static void AreEqual<T>(in Result<T> expected, in Result<T> actual)
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

        Assert.True(AsEquatable(expected).Equals(actual));
        Assert.True(AsEquatable(actual).Equals(expected));

        Assert.True(expected.Equals(actual as object));
        Assert.True(actual.Equals(expected as object));

        Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    public static void AreNotEqual<T>(in Result<T> expected, in Result<T> actual)
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

        Assert.False(AsEquatable(expected).Equals(actual));
        Assert.False(AsEquatable(actual).Equals(expected));

        Assert.False(expected.Equals(actual as object));
        Assert.False(actual.Equals(expected as object));

        Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
    }
}
