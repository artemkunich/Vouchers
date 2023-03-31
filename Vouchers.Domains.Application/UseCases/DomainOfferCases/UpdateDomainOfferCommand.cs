using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Permissions;

namespace Vouchers.Domains.Application.UseCases.DomainOfferCases;

[DomainOfferPermission]
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