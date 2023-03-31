using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Core.Application.Dtos;

namespace Vouchers.Core.Application.UseCases.IssuerTransactionCases;

public sealed class CreateIssuerTransactionCommand : IRequest<IdDto<Guid>>
{
    [Required]
    public Guid IssuerAccountId { get; set; }

    [Required]
    public Guid VoucherId { get; set; }

    [Required]
    public decimal Quantity { get; set; }
}