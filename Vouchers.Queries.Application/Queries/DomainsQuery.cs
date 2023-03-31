using System.Collections.Generic;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class DomainsQuery : ListQuery, IRequest<IReadOnlyList<DomainDto>>
{
    public string Name { get; set; }

    public string OwnerName { get; set; }
}