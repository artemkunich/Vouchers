namespace Vouchers.Primitives;

public abstract class Event : IEvent
{
    public Guid Id { get; set; }
}