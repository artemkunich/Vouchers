using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public class HolderTransactionRequestQuery : IRequest<HolderTransactionRequestDto>
{
    [Required]
    public Guid Id { get; init; }
}