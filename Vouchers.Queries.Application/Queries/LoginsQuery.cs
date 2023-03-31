using System;
using System.Collections.Generic;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Identities.Domain;

namespace Vouchers.Domains.Application.Queries;

[Permission(IdentityRole.Admin)]
public sealed class LoginsQuery : ListQuery, IRequest<IReadOnlyList<LoginDto>>
{
    public string LoginName { get; set; }

    public string IdentityName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}