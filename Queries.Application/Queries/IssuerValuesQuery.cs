using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class IssuerValuesQuery : ListQuery, IRequest<IReadOnlyList<VoucherValueDto>>
{
    [Required]
    public Guid IssuerAccountId { get; set; }

    public string Ticker { get; set; }
}