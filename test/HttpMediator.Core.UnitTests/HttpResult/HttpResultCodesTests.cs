using System.Net;

namespace Tyne.HttpMediator;

public class HttpResultCodesTests
{
    [Fact]
    public void OK() =>
        AssertStatusCodeCore(HttpStatusCode.OK, HttpResult.Codes.OK(42));

    [Fact]
    public void BadRequest_Message() =>
        AssertStatusCode_Message(HttpStatusCode.BadRequest, HttpResult.Codes.BadRequest<int>);

    [Fact]
    public void BadRequest_CodeAndMessage() =>
        AssertStatusCode_CodeAndMessage(HttpStatusCode.BadRequest, HttpResult.Codes.BadRequest<int>);

    [Fact]
    public void BadRequest_Error() =>
        AssertStatusCode_Error(HttpStatusCode.BadRequest, e => HttpResult.Codes.BadRequest<int>(e));

    [Fact]
    public void Forbidden_Message() =>
        AssertStatusCode_Message(HttpStatusCode.Forbidden, HttpResult.Codes.Forbidden<int>);

    [Fact]
    public void Forbidden_CodeAndMessage() =>
        AssertStatusCode_CodeAndMessage(HttpStatusCode.Forbidden, HttpResult.Codes.Forbidden<int>);

    [Fact]
    public void Forbidden_Error() =>
        AssertStatusCode_Error(HttpStatusCode.Forbidden, e => HttpResult.Codes.Forbidden<int>(e));

    [Fact]
    public void InternalServerError_Message() =>
        AssertStatusCode_Message(HttpStatusCode.InternalServerError, HttpResult.Codes.InternalServerError<int>);

    [Fact]
    public void InternalServerError_CodeAndMessage() =>
        AssertStatusCode_CodeAndMessage(HttpStatusCode.InternalServerError, HttpResult.Codes.InternalServerError<int>);

    [Fact]
    public void InternalServerError_Error() =>
        AssertStatusCode_Error(HttpStatusCode.InternalServerError, e => HttpResult.Codes.InternalServerError<int>(e));

    [Fact]
    public void NotFound_Message() =>
        AssertStatusCode_Message(HttpStatusCode.NotFound, HttpResult.Codes.NotFound<int>);

    [Fact]
    public void NotFound_CodeAndMessage() =>
        AssertStatusCode_CodeAndMessage(HttpStatusCode.NotFound, HttpResult.Codes.NotFound<int>);

    [Fact]
    public void NotFound_Error() =>
        AssertStatusCode_Error(HttpStatusCode.NotFound, e => HttpResult.Codes.NotFound<int>(e));

    [Fact]
    public void Unauthorized_Message() =>
        AssertStatusCode_Message(HttpStatusCode.Unauthorized, HttpResult.Codes.Unauthorized<int>);

    [Fact]
    public void Unauthorized_CodeAndMessage() =>
        AssertStatusCode_CodeAndMessage(HttpStatusCode.Unauthorized, HttpResult.Codes.Unauthorized<int>);

    [Fact]
    public void Unauthorized_Error() =>
        AssertStatusCode_Error(HttpStatusCode.Unauthorized, e => HttpResult.Codes.Unauthorized<int>(e));

    private static void AssertStatusCode_Message(HttpStatusCode expectedStatusCode, Func<string, HttpResult<int>> resultFunc) =>
        AssertStatusCodeCore(expectedStatusCode, resultFunc(TestError.Message));

    private static void AssertStatusCode_CodeAndMessage(HttpStatusCode expectedStatusCode, Func<int, string, HttpResult<int>> resultFunc) =>
        AssertStatusCodeCore(expectedStatusCode, resultFunc(TestError.Code, TestError.Message));

    private static void AssertStatusCode_Error(HttpStatusCode expectedStatusCode, Func<Error, HttpResult<int>> resultFunc) =>
        AssertStatusCodeCore(expectedStatusCode, resultFunc(TestError.Instance));

    private static void AssertStatusCodeCore(HttpStatusCode expectedStatusCode, HttpResult<int> result)
    {
        var actualStatusCode = result.StatusCode;
        Assert.Equal(expectedStatusCode, actualStatusCode);
    }
}
