using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainOfferCommands;

[IdentityRoles(IdentityRole.Manager)]
public sealed class UpdateDomainOfferCommand : IRequest<Unit>
{
    [Required]
    public Guid Id { get; set; }

    public bool? Terminate { get; set; }

    public string Description { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public int? MaxContractsPerIdentity { get; set; }
}