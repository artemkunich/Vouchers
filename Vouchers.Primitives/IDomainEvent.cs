using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Primitives;

public interface IDomainEvent
{
    public Guid Id { get; set; }
}