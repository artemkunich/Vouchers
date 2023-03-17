using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Abstractions;

public interface IIntegrationEvent
{
    public Guid Id { get; }
}