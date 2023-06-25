using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class IdentityDomainAccountsQuery : ListQuery, IRequest<IReadOnlyList<DomainAccountDto>>
{
    public string DomainName { get; set; }
}