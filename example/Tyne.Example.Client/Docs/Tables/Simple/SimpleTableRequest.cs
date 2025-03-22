using Tyne.Example.Client.Common.Tables;

namespace Tyne.Example.Client.Docs.Tables.Intro.Simple;

public class SimpleTableRequest : TyneExampleRequest
{
    public string PlanetName { get; set; } = string.Empty;
}
