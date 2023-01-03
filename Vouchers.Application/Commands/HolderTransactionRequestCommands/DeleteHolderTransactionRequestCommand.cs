using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands.HolderTransactionRequestCommands;

public sealed class DeleteHolderTransactionRequestCommand
{
    [Required]
    public Guid HolderTransactionRequestId { get; set; }
}