namespace Vouchers.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; }

        protected Entity(Guid id)
        {
            Id = id;
        }
        protected Entity() { }

        public bool Equals(Entity entity)
        {
            if (ReferenceEquals(this, entity))
                return true;

            if (Id.Equals(default) || entity.Id.Equals(default))
                return false;

            return Id.Equals(entity.Id);
        }

        public bool NotEquals(Entity entity) => !Equals(entity);

        public static implicit operator Guid(Entity entity) => entity.Id;
    }
}