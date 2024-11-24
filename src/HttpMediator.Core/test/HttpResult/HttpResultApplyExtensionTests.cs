using System.Net;

namespace Tyne.HttpMediator;

public class HttpResultApplyExtensionTests
{
    [Fact]
    public void Apply1_NullSome_Throws()
    {
        // Arrange
        var okResult = HttpResult.Ok(42, HttpStatusCode.OK);
        var errorResult = HttpResult.Error<int>(TestError.Instance, TestError.StatusCode);

        Action<int> ok = null!;

        // Act and assert
        _ = AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok));
        _ = AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok));
    }

    [Fact]
    public void Apply2_NullSome_Throws()
    {
        // Arrange
        var okResult = HttpResult.Ok(42, HttpStatusCode.OK);
        var errorResult = HttpResult.Error<int>(TestError.Instance, TestError.StatusCode);

        Action<int> ok = null!;
        static void err(Error _)
        {
            // Just a filler method
        }

        // Act and assert
        _ = AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok, err));
        _ = AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok, err));
    }

    [Fact]
    public void Apply2_NullNone_Throws()
    {
        // Arrange
        var okResult = HttpResult.Ok(42, HttpStatusCode.OK);
        var errorResult = HttpResult.Error<int>(TestError.Instance, TestError.StatusCode);

        static void ok(int _)
        {
            // Just a filler method
        }
        Action<Error> err = null!;

        // Act and assert
        _ = AssertExt.ThrowsArgumentNullException(() => okResult.Apply(ok, err));
        _ = AssertExt.ThrowsArgumentNullException(() => errorResult.Apply(ok, err));
    }
}
