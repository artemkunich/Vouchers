using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands.HolderTransactionRequestCommands;

public sealed class CreateHolderTransactionRequestCommand
{
    public Guid? CreditorAccountId { get; set; }
    [Required]
    public Guid DebtorAccountId { get; set; }

    [Required]
    public decimal Amount { get; set; }
    [Required]
    public Guid UnitTypeId { get; set; }

    public int? MaxDaysBeforeValidityStart { get; set; }
    public int? MinDaysBeforeValidityEnd { get; set; }

    public bool MustBeExchangeable { get; set; }

    public DateTime DueDate { get; set;}

    [MaxLength(1024)]
    public string Message { get; set; }
}