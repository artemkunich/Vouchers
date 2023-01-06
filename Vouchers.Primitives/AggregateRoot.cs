namespace Vouchers.Primitives;

public class AggregateRoot<TKey> : Entity<TKey>
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    protected AggregateRoot(TKey id) : base(id)
    {
        
    }
    protected AggregateRoot() { }
    
    public void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public IEnumerable<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();
}