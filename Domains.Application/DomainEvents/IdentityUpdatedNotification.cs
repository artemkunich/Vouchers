using System;
using Akunich.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record IdentityUpdatedNotification : INotification
{
    public string NewFirstName { get; set; }

    public string NewLastName { get; set; }

    public string NewEmail { get; set; }

    public Guid? NewImageId { get; set; }

}