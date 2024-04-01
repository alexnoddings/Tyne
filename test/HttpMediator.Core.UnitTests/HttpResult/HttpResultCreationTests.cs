using System.Net;

namespace Tyne.HttpMediator;

public class HttpResultCreationTests
{
    public static IEnumerable<object?[]> MakeStatusCodeRangeData(int start, int range = 100) =>
        Enumerable.Range(start, range)
        .Select(i => (HttpStatusCode)i)
        .Select(c => new object?[] { c })
        .ToList();

    public static IEnumerable<object?[]> BelowStatusCodesData => MakeStatusCodeRangeData(99, 1);
    public static IEnumerable<object?[]> InformationStatusCodesData => MakeStatusCodeRangeData(100);
    public static IEnumerable<object?[]> SuccessfulStatusCodesData => MakeStatusCodeRangeData(200);
    public static IEnumerable<object?[]> RedirectionStatusCodesData => MakeStatusCodeRangeData(300);
    public static IEnumerable<object?[]> ClientErrorStatusCodesData => MakeStatusCodeRangeData(400);
    public static IEnumerable<object?[]> ServerErrorStatusCodesData => MakeStatusCodeRangeData(500);
    public static IEnumerable<object?[]> AboveErrorStatusCodesData => MakeStatusCodeRangeData(600, 1);

    [Theory]
    [MemberData(nameof(SuccessfulStatusCodesData))]
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
    [MemberData(nameof(RedirectionStatusCodesData))]
    [MemberData(nameof(ClientErrorStatusCodesData))]
    [MemberData(nameof(ServerErrorStatusCodesData))]
    [MemberData(nameof(AboveErrorStatusCodesData))]
    public void Ok_RejectsOtherStatusCodes(HttpStatusCode statusCode)
    {
        // Act and assert
        Assert.Throws<BadResultException>(() => HttpResult.Ok(42, statusCode));
    }

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
    public void Error_RejectsOtherStatusCodes(HttpStatusCode statusCode)
    {
        // Act and assert
        Assert.Throws<BadResultException>(() => HttpResult.Error<int>(TestError.Message, statusCode));
    }
}
