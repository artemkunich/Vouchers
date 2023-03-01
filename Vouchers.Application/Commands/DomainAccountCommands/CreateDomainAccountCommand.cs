using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainAccountCommands;

[IdentityRoles(IdentityRole.User)]
public sealed class CreateDomainAccountCommand : IRequest<IdDto<Guid>>
{      
    [Required]
    public Guid DomainId { get; set; }
}