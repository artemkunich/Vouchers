using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class IssuerVouchersQuery : ListQuery, IRequest<IReadOnlyList<VoucherDto>>
{
    public Guid ValueId { get; set; }
}