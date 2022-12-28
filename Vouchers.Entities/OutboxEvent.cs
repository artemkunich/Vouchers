namespace Vouchers.Entities;

public class OutboxEvent: Entity<Guid>
{
    public string Type { get; }
    public string Data { get; }
    public string EntityId { get; private set; }
    public string EntityName { get; private set;}
    public OutboxEventState State { get; private set; }
    public DateTime CreatedDateTime { get; }
    public DateTime ProcessedDateTime { get; private set; }


    public static OutboxEvent Create<TEntityKey>(string type, string data, Entity<TEntityKey> entity)
    {
        var newOutbox = new OutboxEvent(Guid.NewGuid(), type, data);
        newOutbox.EntityId = entity.Id.ToString();
        newOutbox.EntityName = entity.GetType().Name;
        entity.OutboxEvents.Add(newOutbox);
        return newOutbox;
    }

    private OutboxEvent(Guid id, string type, string data) : base(id)
    {
        Type = type;
        Data = data;
        State = OutboxEventState.Ready;
        CreatedDateTime = DateTime.Now;
    }

    private OutboxEvent()
    {}
    
    public void Process()
    {
        State = OutboxEventState.Processed;
        ProcessedDateTime = DateTime.Now;
    }
}