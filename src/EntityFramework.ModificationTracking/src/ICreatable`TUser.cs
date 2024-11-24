namespace Tyne.EntityFramework;

public interface ICreatable<TUser> : ICreatable
{
    public TUser? CreatedBy { get; set; }
}
