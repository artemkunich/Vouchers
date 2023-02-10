using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands.HolderTransactionRequestCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DeleteHolderTransactionRequestCommand
{
    [Required]
    public Guid HolderTransactionRequestId { get; set; }
}