using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;

namespace Vouchers.Application.Commands.HolderTransactionRequestCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DeleteHolderTransactionRequestCommand : IRequest<Unit>
{
    [Required]
    public Guid HolderTransactionRequestId { get; set; }
}