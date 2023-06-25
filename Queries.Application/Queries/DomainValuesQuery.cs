using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class DomainValuesQuery : ListQuery, IRequest<IReadOnlyList<VoucherValueDto>>
{
    [Required]
    public Guid DomainId { get; set; }

    public string Ticker { get; set; }

    public string IssuerName { get; set; }
}