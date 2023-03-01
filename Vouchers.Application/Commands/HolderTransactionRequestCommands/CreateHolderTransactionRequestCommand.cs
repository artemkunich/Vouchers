using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.HolderTransactionRequestCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class CreateHolderTransactionRequestCommand : IRequest<IdDto<Guid>>
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