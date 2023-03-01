namespace Vouchers.Primitives;

public class AggregateRoot<TKey> : Entity<TKey>
{
    private readonly List<IEvent> _events = new();

    public void RaiseEvent(IEvent @event) => _events.Add(@event);

    public IEnumerable<IEvent> Events => _events.ToList();

    public void ClearEvents() => _events.Clear();
}