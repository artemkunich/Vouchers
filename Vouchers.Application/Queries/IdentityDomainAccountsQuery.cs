using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

[IdentityRoles(IdentityRole.User)]
public sealed class IdentityDomainAccountsQuery : ListQuery, IRequest<IReadOnlyList<DomainAccountDto>>
{
    public string DomainName { get; set; }
}