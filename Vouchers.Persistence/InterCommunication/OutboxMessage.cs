using System;
using Vouchers.Primitives;
using Vouchers.InterCommunication;

namespace Vouchers.Persistence.InterCommunication;

public class OutboxMessage: Entity<Guid>
{
    public string Type { get; init; }
    public string Data { get; init; }
    public OutboxMessageState State { get; private set; }
    public DateTime CreatedDateTime { get; init; }
    public DateTime ProcessedDateTime { get; private set; }


    public static OutboxMessage Create(Guid id, string type, string data) => new()
    {
        Id = id,
        Type = type,
        Data = data,
    };

    public void MarkAsProcessed()
    {
        State = OutboxMessageState.Processed;
        ProcessedDateTime = DateTime.UtcNow;
    }
}