namespace Tyne.Aerospace.Client.Features.Systems.Tables.Intro.Simple;

public class SimpleTableResponse
{
    public string Name { get; set; }
    public int DiameterKm { get; set; }
    public long DistanceFromSunKm { get; set; }

    public SimpleTableResponse(string name, int diameterKm, long distanceFromSunKm)
    {
        Name = name;
        DiameterKm = diameterKm;
        DistanceFromSunKm = distanceFromSunKm;
    }
}
