using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Events.IdentityEvents;

public abstract class Event
{
    [Required]
    public Guid EventId { get; set; }
}