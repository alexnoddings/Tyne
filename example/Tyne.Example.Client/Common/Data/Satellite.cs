namespace Tyne.Example.Client.Common.Data;

public class Satellite
{
    public string Name { get; set; } = string.Empty;
    public int ApogeeKm { get; set; }
    public int PerigeeKm { get; set; }
    public bool IsAlive { get; set; }
}
