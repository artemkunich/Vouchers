using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands.LoginCommands;

public sealed class UpdateLoginCommand
{
    [Required]
    public string CurrentLoginName { get; }

    [Required]
    public string NewLoginName { get; }
}