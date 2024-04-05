namespace Tyne;

public static class AssertResult
{
    public static T IsOk<T>(Result<T> actual)
    {
        ArgumentNullException.ThrowIfNull(actual);

        Assert.True(actual.IsOk, $"Result should be Ok, was {actual}.");

        var exception = Assert.Throws<BadResultException>(() => actual.Error);
        Assert.Equal(exception.Message, ExceptionMessages.Result_OkHasNoError);

        return actual.Value;
    }

    public static T IsOk<T>(T expected, Result<T> actual)
    {
        var actualValue = IsOk(actual);
        Assert.Equal(expected, actualValue);

        return actualValue;
    }

    public static Error IsError<T>(Result<T> actual)
    {
        ArgumentNullException.ThrowIfNull(actual);

        Assert.False(actual.IsOk, $"Result should be Error, was {actual}.");
        var exception = Assert.Throws<BadResultException>(() => actual.Value);
        Assert.Equal(exception.Message, ExceptionMessages.Result_ErrorHasNoValue);

        return actual.Error;
    }

    public static Error IsError<T>(in Error expected, in Result<T> actual)
    {
        ArgumentNullException.ThrowIfNull(actual);

        var actualError = IsError(actual);
        Assert.Equal(expected, actual.Error);
        return actualError;
    }

    public static Error IsError<T>(string expectedErrorMessage, in Result<T> actual) =>
        IsError(Error.DefaultCode, expectedErrorMessage, null, actual);

    public static Error IsError<T>(int expectedErrorCode, string expectedErrorMessage, in Result<T> actual) =>
        IsError(expectedErrorCode, expectedErrorMessage, null, actual);

    public static Error IsError<T>(int expectedErrorCode, string expectedErrorMessage, Exception? expectedException, in Result<T> actual) =>
        IsError(Error.From(expectedErrorCode, expectedErrorMessage, expectedException), actual);

    public static void AreEqual<T>(in Result<T> expected, in Result<T> actual)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(actual);

        Assert.Equal(expected, actual);

        Assert.True(expected == actual, "Equality operator should return true.");
        Assert.True(actual == expected, "Equality operator should return true.");

        Assert.False(expected != actual, "Inequality operator should return false.");
        Assert.False(actual != expected, "Inequality operator should return false.");

        Assert.True(expected.Equals(actual), "Equals method should return true.");
        Assert.True(actual.Equals(expected), "Equals method  should return true.");

        Assert.True(((IEquatable<Result<T>>)expected).Equals(actual), "Equatable.Equals method should return true.");
        Assert.True(((IEquatable<Result<T>>)actual).Equals(expected), "Equatable.Equals method should return true.");

        Assert.True(expected.Equals(actual as object), "Equals (as object) method should return true.");
        Assert.True(actual.Equals(expected as object), "Equals (as object) method should return true.");

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

        Assert.False(((IEquatable<Result<T>>)expected).Equals(actual));
        Assert.False(((IEquatable<Result<T>>)actual).Equals(expected));

        Assert.False(expected.Equals(actual as object));
        Assert.False(actual.Equals(expected as object));

        Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
    }
}
