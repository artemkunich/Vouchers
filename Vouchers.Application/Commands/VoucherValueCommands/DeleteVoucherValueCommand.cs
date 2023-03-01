using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherValueCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DeleteVoucherValueCommand : IRequest<Unit>
{
    [Required]
    public Guid VoucherValueId { get; }
}