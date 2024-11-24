namespace Tyne.EntityFramework;

public interface ICreatable
{
    public Guid? CreatedById { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
