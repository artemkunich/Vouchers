using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherValueCommands;

public sealed class DeleteVoucherValueCommand
{
    [Required]
    public Guid VoucherValueId { get; }
}