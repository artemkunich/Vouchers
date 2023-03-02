using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainAccountCommands;

public sealed class CreateDomainAccountCommand : IRequest<IdDto<Guid>>
{      
    [Required]
    public Guid DomainId { get; set; }
}