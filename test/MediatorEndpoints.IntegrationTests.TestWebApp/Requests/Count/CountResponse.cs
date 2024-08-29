namespace Tyne.MediatorEndpoints;

public class CountResponse
{
    public const int DefaultNewCount = -1;

    public int NewCount { get; set; } = DefaultNewCount;
}
