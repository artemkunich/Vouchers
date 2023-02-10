using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class UpdateVoucherCommand
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid VoucherValueId { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public bool? CanBeExchanged { get; set; }
  
}