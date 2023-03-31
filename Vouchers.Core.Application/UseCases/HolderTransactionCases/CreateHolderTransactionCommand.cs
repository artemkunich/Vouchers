using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Core.Application.Dtos;

namespace Vouchers.Core.Application.UseCases.HolderTransactionCases;

public sealed class CreateHolderTransactionCommand : IRequest<IdDto<Guid>>
{
    public Guid? HolderTransactionRequestId { get; set; }

    [Required]
    public Guid CreditorAccountId { get; set; }
    [Required]
    public Guid DebtorAccountId { get; set; }

    [Required]
    public decimal Amount { get; set; }
    [Required]
    public Guid UnitTypeId { get; set; }

    [Required]
    public ICollection<Tuple<Guid, decimal>> Items { get; set; }

    [MaxLength(1024)]
    public string Message { get; set; }
}