namespace Tyne.EntityFramework;

public interface IUpdatable
{
    public Guid? LastUpdatedById { get; set; }
    public DateTime LastUpdatedAtUtc { get; set; }
}
