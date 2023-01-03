using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands.LoginCommands;

public sealed class CreateLoginCommand
{
    [Required]
    public string LoginName { get; }
}