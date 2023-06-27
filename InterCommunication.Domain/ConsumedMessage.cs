using System;
using Akunich.Domain.Abstractions;

namespace Vouchers.Persistence.InterCommunication;

public class ConsumedMessage: Entity<Guid>
{
    public Guid MessageId { get; init; }
    public string Consumer { get; init; }
    public DateTime ConsumedDate { get; init; }


    public static ConsumedMessage Create(Guid id, Guid messageId, string consumer, DateTime consumedDate) => new()
    {
        Id = id,
        MessageId = messageId,
        Consumer = consumer,
        ConsumedDate = consumedDate,
    };
}