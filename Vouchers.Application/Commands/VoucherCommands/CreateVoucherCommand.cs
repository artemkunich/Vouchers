﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class CreateVoucherCommand
{
    [Required]
    public Guid VoucherValueId { get; set; }

    [Required]
    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public bool CanBeExchanged { get; set; }
}