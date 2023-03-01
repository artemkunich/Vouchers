using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DeleteVoucherCommand : IRequest<Unit>
{
    [Required]
    public Guid VoucherValueId { get; }

    [Required]
    public Guid VoucherId { get; }
}