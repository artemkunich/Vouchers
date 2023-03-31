using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.UseCases.DomainAccountCases;

public sealed class CreateDomainAccountCommand : IRequest<Dtos.IdDto<Guid>>
{      
    [Required]
    public Guid DomainId { get; set; }
}