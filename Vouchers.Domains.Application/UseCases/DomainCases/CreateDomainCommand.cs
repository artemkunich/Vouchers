using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.UseCases.DomainCases;

public sealed class CreateDomainCommand : IRequest<Dtos.IdDto<Guid>>
{
    [Required]
    public Guid OfferId { get; set; }

    [Required]
    public string DomainName { get; set; }
}