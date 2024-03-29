﻿using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;
using Vouchers.Identities.Domain;

namespace Vouchers.Application.Commands.DomainOfferCommands;

[Permission(IdentityRole.Manager)]
public sealed class CreateDomainOfferCommand : IRequest<IdDto<Guid>>
{
    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public int MaxMembersCount { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public string Currency { get; set; }
    [Required]
    public string InvoicePeriod { get; set; }
    [Required]
    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public int? MaxContractsPerIdentity { get; set; }
}