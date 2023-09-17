namespace Tyne.Aerospace.Data;

public interface IEntity<TKey>
{
    public TKey Id { get; set; }
}

public interface IEntity : IEntity<Guid>
{
}
