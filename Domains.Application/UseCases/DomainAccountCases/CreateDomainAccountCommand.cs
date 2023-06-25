using System;
using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.UseCases.DomainAccountCases;

public sealed class CreateDomainAccountCommand : IRequest<IdDto<Guid>>
{      
    [Required]
    public Guid DomainId { get; set; }
}