namespace Vouchers.Primitives;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; set; }
}