using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Identities.Domain;

namespace Vouchers.Application.Commands.DomainOfferCommands;

[Permission(IdentityRole.Manager)]
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