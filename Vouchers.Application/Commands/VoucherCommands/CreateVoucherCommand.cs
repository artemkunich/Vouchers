using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherCommands;

public sealed class CreateVoucherCommand : IRequest<IdDto<Guid>>
{
    [Required]
    public Guid VoucherValueId { get; set; }

    [Required]
    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public bool CanBeExchanged { get; set; }
}