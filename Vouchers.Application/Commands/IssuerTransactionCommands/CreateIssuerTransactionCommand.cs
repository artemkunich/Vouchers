using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Commands.IssuerTransactionCommands;

public sealed class CreateIssuerTransactionCommand
{
    [Required]
    public Guid IssuerAccountId { get; set; }

    [Required]
    public Guid VoucherId { get; set; }

    [Required]
    public decimal Quantity { get; set; }
}