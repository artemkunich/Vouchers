﻿using System.Collections.Generic;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class DomainsQuery : ListQuery, IRequest<IReadOnlyList<DomainDto>>
{
    public string Name { get; set; }

    public string OwnerName { get; set; }
}