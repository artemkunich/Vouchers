using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainAccountCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class CreateDomainAccountCommand
{      
    [Required]
    public Guid DomainId { get; set; }
}