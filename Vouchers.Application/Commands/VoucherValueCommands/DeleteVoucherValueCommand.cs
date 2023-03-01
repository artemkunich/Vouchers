using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherValueCommands;

[IdentityRoles(IdentityRole.User)]
public sealed class DeleteVoucherValueCommand : IRequest<Unit>
{
    [Required]
    public Guid VoucherValueId { get; }
}