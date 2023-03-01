using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Primitives;

public interface IEvent
{
    public Guid Id { get; set; }
}