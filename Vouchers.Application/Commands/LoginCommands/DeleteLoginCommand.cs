using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands.LoginCommands;

public sealed class DeleteLoginCommand
{
    [Required]
    public string LoginName { get; }
}