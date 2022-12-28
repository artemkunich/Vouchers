namespace Vouchers.Entities;

public abstract class Entity<TKey>
{
    public TKey Id { get; }
    public ICollection<OutboxEvent> OutboxEvents { get; } = new List<OutboxEvent>();

    protected Entity(TKey id)
    {
        Id = id;
    }
    protected Entity() { }

    public bool Equals(Entity<TKey> entity)
    {
        if (ReferenceEquals(this, entity))
            return true;

        if (Id.Equals(default) || entity.Id.Equals(default))
            return false;

        return Id.Equals(entity.Id);
    }

    public bool NotEquals(Entity<TKey> entity) => !Equals(entity);

    public static implicit operator TKey(Entity<TKey> entity) => entity.Id;
}
