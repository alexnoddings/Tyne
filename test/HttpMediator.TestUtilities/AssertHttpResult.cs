using System;
using System.Net;
using Xunit;
namespace Tyne.HttpMediator;

public static class AssertHttpResult
{
    public static void HasStatusCode<T>(HttpStatusCode expectedStatusCode, HttpResult<T> actual)
    {
        ArgumentNullException.ThrowIfNull(actual);
        Assert.Equal(expectedStatusCode, actual.StatusCode);
    }

    public static T IsOk<T>(HttpStatusCode expectedStatusCode, HttpResult<T> actual)
    {
        var actualValue = AssertResult.IsOk(actual);
        HasStatusCode(expectedStatusCode, actual);
        return actualValue;
    }

    public static T IsOk<T>(HttpStatusCode expectedStatusCode, T expected, HttpResult<T> actual)
    {
        var actualValue = AssertResult.IsOk(expected, actual);
        HasStatusCode(expectedStatusCode, actual);
        return actualValue;
    }

    public static Error IsError<T>(HttpStatusCode expectedStatusCode, in HttpResult<T> actual)
    {
        var actualError = AssertResult.IsError(actual);
        HasStatusCode(expectedStatusCode, actual);
        return actualError;
    }

    public static Error IsError<T>(HttpStatusCode expectedStatusCode, in Error expected, in HttpResult<T> actual)
    {
        var actualError = AssertResult.IsError(expected, actual);
        HasStatusCode(expectedStatusCode, actual);
        return actualError;
    }

    public static Error IsError<T>(HttpStatusCode expectedStatusCode, string expectedErrorMessage, in HttpResult<T> actual) =>
        IsError(expectedStatusCode, Error.Default.Code, expectedErrorMessage, null, actual);

    public static Error IsError<T>(HttpStatusCode expectedStatusCode, string expectedErrorCode, string expectedErrorMessage, in HttpResult<T> actual) =>
        IsError(expectedStatusCode, expectedErrorCode, expectedErrorMessage, null, actual);

    public static Error IsError<T>(HttpStatusCode expectedStatusCode, string expectedErrorCode, string expectedErrorMessage, Exception? expectedException, in HttpResult<T> actual) =>
        IsError(expectedStatusCode, Error.From(expectedErrorCode, expectedErrorMessage, expectedException), actual);

    public static void AreEqual<T>(in HttpResult<T> expected, in HttpResult<T> actual)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(actual);

        AssertResult.AreEqual(expected, actual);

        Assert.Equal(expected, actual);

        Assert.True(expected == actual, "Equality operator should return true.");
        Assert.True(actual == expected, "Equality operator should return true.");

        Assert.False(expected != actual, "Inequality operator should return false.");
        Assert.False(actual != expected, "Inequality operator should return false.");

        Assert.True(expected.Equals(actual), "Equals method should return true.");
        Assert.True(actual.Equals(expected), "Equals method  should return true.");

        Assert.True(((IEquatable<HttpResult<T>>)expected).Equals(actual), "Equatable.Equals method should return true.");
        Assert.True(((IEquatable<HttpResult<T>>)actual).Equals(expected), "Equatable.Equals method should return true.");

        Assert.True(expected.Equals(actual as object), "Equals (as object) method should return true.");
        Assert.True(actual.Equals(expected as object), "Equals (as object) method should return true.");

        Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    public static void AreNotEqual<T>(in HttpResult<T> expected, in HttpResult<T> actual)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(actual);

        AssertResult.AreNotEqual(expected, actual);

        Assert.NotEqual(expected, actual);

        Assert.False(expected == actual);
        Assert.False(actual == expected);

        Assert.True(expected != actual);
        Assert.True(actual != expected);

        Assert.False(expected.Equals(actual));
        Assert.False(actual.Equals(expected));

        Assert.False(((IEquatable<HttpResult<T>>)expected).Equals(actual));
        Assert.False(((IEquatable<HttpResult<T>>)actual).Equals(expected));

        Assert.False(expected.Equals(actual as object));
        Assert.False(actual.Equals(expected as object));

        Assert.NotEqual(expected.GetHashCode(), actual.GetHashCode());
    }
}
