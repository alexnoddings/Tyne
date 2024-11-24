namespace Tyne.HttpMediator;

public abstract class TestHttpResponseBase
{
    public const int DefaultNewCount = -1;
    public int NewCount { get; set; } = DefaultNewCount;
}
