using System;
using Vouchers.Primitives;

namespace Vouchers.Persistence.InterCommunication;

public class ConsumedMessage: Entity<Guid>
{
    public Guid MessageId { get; }
    public string Consumer { get; }
    public DateTime ConsumedDate { get; }


    public static ConsumedMessage Create(Guid messageId, string handler) => new (Guid.NewGuid(), messageId, handler);

    private ConsumedMessage(Guid id, Guid messageId, string consumer) : base(id)
    {
        MessageId = messageId;
        Consumer = consumer;
        ConsumedDate = DateTime.Now;
    }

    private ConsumedMessage()
    {}
}