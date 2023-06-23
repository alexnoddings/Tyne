namespace Tyne.EntityFramework;

public interface IUpdatable<TUser> : IUpdatable
{
    public TUser? LastUpdatedBy { get; set; }
}
