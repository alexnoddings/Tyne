namespace Tyne.Example.Client.Common.Tables;

public class TyneExampleRequest
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public string? OrderBy { get; init; }
    public bool OrderByDescending { get; init; }
}
