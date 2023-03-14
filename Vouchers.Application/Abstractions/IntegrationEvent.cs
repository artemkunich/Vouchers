using System;

namespace Vouchers.Application.Abstractions;

public class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; set; }
}