using Vouchers.Entities;

namespace Vouchers.InterCommunication;

public class OutboxMessage: Entity<Guid>
{
    public string Type { get; }
    public string Data { get; }
    public OutboxMessageState State { get; private set; }
    public DateTime CreatedDateTime { get; }
    public DateTime ProcessedDateTime { get; private set; }


    public static OutboxMessage Create(string type, string data) => new OutboxMessage(Guid.NewGuid(), type, data);

    private OutboxMessage(Guid id, string type, string data) : base(id)
    {
        Type = type;
        Data = data;
        State = OutboxMessageState.Ready;
        CreatedDateTime = DateTime.Now;
    }

    private OutboxMessage()
    {}
    
    public void Process()
    {
        State = OutboxMessageState.Processed;
        ProcessedDateTime = DateTime.Now;
    }
}