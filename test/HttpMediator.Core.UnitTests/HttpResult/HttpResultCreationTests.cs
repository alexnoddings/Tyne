using System.Net;

namespace Tyne.HttpMediator;

public class HttpResultCreationTests
{
    private static TheoryData<HttpStatusCode> MakeStatusCodeRangeData(int start, int range = 100)
    {
        var theoryData = new TheoryData<HttpStatusCode>();
        foreach (var i in Enumerable.Range(start, range))
            theoryData.Add((HttpStatusCode)i);
        return theoryData;
    }

    public static TheoryData<HttpStatusCode> BelowStatusCodesData => MakeStatusCodeRangeData(99, 1);
    public static TheoryData<HttpStatusCode> InformationStatusCodesData => MakeStatusCodeRangeData(100);
    public static TheoryData<HttpStatusCode> SuccessfulStatusCodesData => MakeStatusCodeRangeData(200);
    public static TheoryData<HttpStatusCode> RedirectionStatusCodesData => MakeStatusCodeRangeData(300);
    public static TheoryData<HttpStatusCode> ClientErrorStatusCodesData => MakeStatusCodeRangeData(400);
    public static TheoryData<HttpStatusCode> ServerErrorStatusCodesData => MakeStatusCodeRangeData(500);
    public static TheoryData<HttpStatusCode> AboveErrorStatusCodesData => MakeStatusCodeRangeData(600, 1);

    [Theory]
    [MemberData(nameof(SuccessfulStatusCodesData))]
    [MemberData(nameof(RedirectionStatusCodesData))]
    public void Ok_AcceptsSuccessfulStatusCodes(HttpStatusCode statusCode)
    {
        // Act
        var result = HttpResult.Ok(42, statusCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(BelowStatusCodesData))]
    [MemberData(nameof(InformationStatusCodesData))]
    [MemberData(nameof(ClientErrorStatusCodesData))]
    [MemberData(nameof(ServerErrorStatusCodesData))]
    [MemberData(nameof(AboveErrorStatusCodesData))]
    public void Ok_RejectsOtherStatusCodes(HttpStatusCode statusCode) =>
        Assert.Throws<BadResultException>(() => HttpResult.Ok(42, statusCode));

    [Theory]
    [MemberData(nameof(ClientErrorStatusCodesData))]
    [MemberData(nameof(ServerErrorStatusCodesData))]
    public void Error_AcceptsErrorStatusCodes(HttpStatusCode statusCode)
    {
        // Act
        var result = HttpResult.Error<int>(TestError.Message, statusCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(BelowStatusCodesData))]
    [MemberData(nameof(InformationStatusCodesData))]
    [MemberData(nameof(SuccessfulStatusCodesData))]
    [MemberData(nameof(RedirectionStatusCodesData))]
    [MemberData(nameof(AboveErrorStatusCodesData))]
    public void Error_RejectsOtherStatusCodes(HttpStatusCode statusCode) =>
        Assert.Throws<BadResultException>(() => HttpResult.Error<int>(TestError.Message, statusCode));

    [Theory]
    [MemberData(nameof(SuccessfulStatusCodesData))]
    [MemberData(nameof(RedirectionStatusCodesData))]
    public void ToHttpResult_OkResult_AcceptsSuccessfulStatusCodes(HttpStatusCode statusCode)
    {
        // Arrange
        var result = Result.Ok(42);

        // Act
        var httpResult = result.ToHttpResult(statusCode);

        // Assert
        Assert.NotNull(httpResult);
        var httpResultValue = AssertHttpResult.IsOk(statusCode, httpResult);
        Assert.Equal(42, httpResultValue);
    }

    [Theory]
    [MemberData(nameof(BelowStatusCodesData))]
    [MemberData(nameof(InformationStatusCodesData))]
    [MemberData(nameof(ClientErrorStatusCodesData))]
    [MemberData(nameof(ServerErrorStatusCodesData))]
    [MemberData(nameof(AboveErrorStatusCodesData))]
    public void ToHttpResult_OkResult_RejectsOtherStatusCodes(HttpStatusCode statusCode)
    {
        // Assert
        var result = Result.Ok(42);

        // Act
        HttpResult<int> act() =>
            result.ToHttpResult(statusCode);

        // Assert
        _ = Assert.Throws<BadResultException>(act);
    }

    [Theory]
    [MemberData(nameof(ClientErrorStatusCodesData))]
    [MemberData(nameof(ServerErrorStatusCodesData))]
    public void ToHttpResult_ErrorResult_AcceptsErrorStatusCodes(HttpStatusCode statusCode)
    {
        // Arrange
        var result = Result.Error<int>(TestError.Instance);

        // Act
        var httpResult = result.ToHttpResult(statusCode);

        // Assert
        Assert.NotNull(httpResult);
        var httpResultError = AssertHttpResult.IsError(statusCode, httpResult);
        Assert.Equal(TestError.Instance, httpResultError);
    }

    [Theory]
    [MemberData(nameof(BelowStatusCodesData))]
    [MemberData(nameof(InformationStatusCodesData))]
    [MemberData(nameof(SuccessfulStatusCodesData))]
    [MemberData(nameof(RedirectionStatusCodesData))]
    [MemberData(nameof(AboveErrorStatusCodesData))]
    public void ToHttpResult_ErrorResult_RejectsOtherStatusCodes(HttpStatusCode statusCode)
    {
        // Assert
        var result = Result.Error<int>(TestError.Instance);

        // Act
        HttpResult<int> act() =>
            result.ToHttpResult(statusCode);

        // Assert
        _ = Assert.Throws<BadResultException>(act);
    }
}
