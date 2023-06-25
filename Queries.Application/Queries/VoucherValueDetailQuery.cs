using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class VoucherValueDetailQuery : IRequest<VoucherValueDetailDto>
{
    [Required]
    public Guid Id { get; init; }
}