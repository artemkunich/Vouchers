using Vouchers.Entities;

namespace Vouchers.InterCommunication;

[AggregateRoot]
public class InboxMessage: Entity<Guid>
{
    public Guid OriginalId { get; }
    public string Handler { get; }
    public string Data { get; }
    public InboxMessageState State { get; private set; }
    public DateTime ReceivedDateTime { get; }
    public DateTime ProcessedDateTime { get; private set; }


    public static InboxMessage Create(Guid originalId, string handler, string data) => new InboxMessage(Guid.NewGuid(), originalId, handler, data);

    private InboxMessage(Guid id, Guid originalId, string handler, string data) : base(id)
    {
        OriginalId = originalId;
        Handler = handler;
        Data = data;
        State = InboxMessageState.Received;
        ReceivedDateTime = DateTime.Now;
    }

    private InboxMessage()
    {}
    
    public void Process()
    {
        State = InboxMessageState.Processed;
        ProcessedDateTime = DateTime.Now;
    }
}