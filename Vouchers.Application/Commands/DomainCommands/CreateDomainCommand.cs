using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainCommands;

[IdentityRoles(IdentityRole.User)]
public sealed class CreateDomainCommand : IRequest<IdDto<Guid>>
{
    [Required]
    public Guid OfferId { get; set; }

    [Required]
    public string DomainName { get; set; }
}