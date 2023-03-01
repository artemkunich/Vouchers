using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DomainsQuery : ListQuery, IRequest<IReadOnlyList<DomainDto>>
{
    public string Name { get; set; }

    public string OwnerName { get; set; }
}