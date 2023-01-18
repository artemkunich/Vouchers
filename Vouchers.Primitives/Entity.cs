namespace Vouchers.Primitives;

public abstract class Entity<TKey>: IEntity<TKey>
{
    public TKey Id { get; init; }

    public bool Equals(Entity<TKey> entity)
    {
        if (ReferenceEquals(this, entity))
            return true;

        if (Id is null || entity.Id is null || Id.Equals(default) || entity.Id.Equals(default))
            return false;

        return Id.Equals(entity.Id);
    }

    public bool NotEquals(Entity<TKey> entity) => !Equals(entity);

    public static implicit operator TKey(Entity<TKey> entity) => entity.Id;
}
