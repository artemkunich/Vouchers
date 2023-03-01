using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.IssuerTransactionCommands;

[IdentityRoles(IdentityRole.User)]
public sealed class CreateIssuerTransactionCommand : IRequest<IdDto<Guid>>
{
    [Required]
    public Guid IssuerAccountId { get; set; }

    [Required]
    public Guid VoucherId { get; set; }

    [Required]
    public decimal Quantity { get; set; }
}